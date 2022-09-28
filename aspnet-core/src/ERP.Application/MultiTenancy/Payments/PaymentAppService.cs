using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using ERP.Authorization;
using ERP.Editions;
using ERP.Editions.Dto;
using ERP.MultiTenancy.Dto;
using ERP.MultiTenancy.Payments.Dto;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;

namespace ERP.MultiTenancy.Payments
{
    public class PaymentAppService : ERPAppServiceBase, IPaymentAppService
    {
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly EditionManager _editionManager;
        private readonly IPaymentGatewayStore _paymentGatewayStore;
        private readonly TenantManager _tenantManager;

        public PaymentAppService(
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            EditionManager editionManager,
            IPaymentGatewayStore paymentGatewayStore,
            TenantManager tenantManager)
        {
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _editionManager = editionManager;
            _paymentGatewayStore = paymentGatewayStore;
            _tenantManager = tenantManager;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement)]
        public async Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input)
        {
            var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("TenantEditionIsNotAssigned"));
            }

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            var targetEdition = input.UpgradeEditionId == null ? currentEdition : (SubscribableEdition)await _editionManager.GetByIdAsync(input.UpgradeEditionId.Value);

            decimal additionalPrice = 0;

            if (input.UpgradeEditionId.HasValue)
            {
                var remainingDaysCount = tenant.CalculateRemainingDayCount();
                if (remainingDaysCount > 0)
                {
                    additionalPrice = TenantManager
                        .GetUpgradePrice(
                            currentEdition,
                            targetEdition,
                            remainingDaysCount
                        );
                }
            }

            var edition = ObjectMapper.Map<EditionSelectDto>(input.UpgradeEditionId == null ? currentEdition : targetEdition);

            return new PaymentInfoDto
            {
                Edition = edition,
                AdditionalPrice = additionalPrice
            };
        }

        public async Task<long> CreatePayment(CreatePaymentDto input)
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new ApplicationException("A payment only can be created for a tenant. TenantId is not set in the IAbpSession!");
            }

            decimal amount;
            string targetEditionName;

            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                var targetEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(input.EditionId);
                targetEditionName = targetEdition.DisplayName;

                var tenant = await TenantManager.GetByIdAsync(AbpSession.GetTenantId());
                amount = await CalculateAmountForPaymentAsync(targetEdition, input.PaymentPeriodType, input.EditionPaymentType, tenant);

                if (tenant != null && input.RecurringPaymentEnabled)
                {
                    tenant.SubscriptionPaymentType = SubscriptionPaymentType.RecurringAutomatic;
                    await _tenantManager.UpdateAsync(tenant);
                }
            }

            var payment = new SubscriptionPayment
            {
                Description = GetPaymentDescription(input.EditionPaymentType, input.PaymentPeriodType, targetEditionName),
                PaymentPeriodType = input.PaymentPeriodType,
                EditionId = input.EditionId,
                TenantId = AbpSession.GetTenantId(),
                Gateway = input.SubscriptionPaymentGatewayType,
                Amount = amount,
                DayCount = input.PaymentPeriodType.HasValue ? (int)input.PaymentPeriodType.Value : 0,
                IsRecurring = input.RecurringPaymentEnabled,
                SuccessUrl = input.SuccessUrl,
                ErrorUrl = input.ErrorUrl
            };

            return await _subscriptionPaymentRepository.InsertAndGetIdAsync(payment);
        }

        public async Task CancelPayment(CancelPaymentDto input)
        {
            var payment = await _subscriptionPaymentRepository.GetByGatewayAndPaymentIdAsync(
                    input.Gateway,
                    input.PaymentId
                );

            payment.SetAsCancelled();
        }

        public async Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input)
        {
            var query = _subscriptionPaymentRepository.GetAll()
                .Include(sp => sp.Edition)
                .Where(sp => sp.TenantId == AbpSession.GetTenantId())
                .OrderBy(input.Sorting);

            var payments = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            var paymentsCount = query.Count();

            return new PagedResultDto<SubscriptionPaymentListDto>(paymentsCount, ObjectMapper.Map<List<SubscriptionPaymentListDto>>(payments));
        }

        public List<PaymentGatewayModel> GetActiveGateways(GetActiveGatewaysInput input)
        {
            return _paymentGatewayStore.GetActiveGateways()
                .WhereIf(input.RecurringPaymentsEnabled.HasValue, gateway => gateway.SupportsRecurringPayments == input.RecurringPaymentsEnabled.Value)
                .ToList();
        }

        public async Task<SubscriptionPaymentDto> GetPaymentAsync(long paymentId)
        {
            return ObjectMapper.Map<SubscriptionPaymentDto>(await _subscriptionPaymentRepository.GetAsync(paymentId));
        }

        public async Task<SubscriptionPaymentDto> GetLastCompletedPayment()
        {
            var payment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
                tenantId: AbpSession.GetTenantId(), 
                gateway: null, 
                isRecurring: null);

            return ObjectMapper.Map<SubscriptionPaymentDto>(payment);
        }

        public async Task BuyNowSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.Paid)
            {
                throw new ApplicationException("Your payment is not completed !");
            }

            payment.SetAsCompleted();

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                false,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.BuyNow
            );
        }

        public async Task NewRegistrationSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.Paid)
            {
                throw new ApplicationException("Your payment is not completed !");
            }

            payment.SetAsCompleted();

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.NewRegistration
            );
        }

        public async Task UpgradeSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.Paid)
            {
                throw new ApplicationException("Your payment is not completed !");
            }

            payment.SetAsCompleted();

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.Upgrade
            );
        }

        public async Task ExtendSucceed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            if (payment.Status != SubscriptionPaymentStatus.Paid)
            {
                throw new ApplicationException("Your payment is not completed !");
            }

            payment.SetAsCompleted();

            await _tenantManager.UpdateTenantAsync(
                payment.TenantId,
                true,
                null,
                payment.PaymentPeriodType,
                payment.EditionId,
                EditionPaymentType.Extend
            );
        }

        public async Task PaymentFailed(long paymentId)
        {
            var payment = await _subscriptionPaymentRepository.GetAsync(paymentId);
            payment.SetAsFailed();
        }

        private async Task<decimal> CalculateAmountForPaymentAsync(SubscribableEdition targetEdition, PaymentPeriodType? periodType, EditionPaymentType editionPaymentType, Tenant tenant)
        {
            if (editionPaymentType != EditionPaymentType.Upgrade)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            if (tenant.EditionId == null)
            {
                throw new UserFriendlyException(L("CanNotUpgradeSubscriptionSinceTenantHasNoEditionAssigned"));
            }

            var remainingDaysCount = tenant.CalculateRemainingDayCount();

            if (remainingDaysCount <= 0)
            {
                return targetEdition.GetPaymentAmount(periodType);
            }

            Debug.Assert(tenant.EditionId != null, "tenant.EditionId != null");

            var currentEdition = (SubscribableEdition)await _editionManager.GetByIdAsync(tenant.EditionId.Value);

            return TenantManager.GetUpgradePrice(currentEdition, targetEdition, remainingDaysCount);
        }

        private string GetPaymentDescription(EditionPaymentType editionPaymentType, PaymentPeriodType? paymentPeriodType, string targetEditionName)
        {
            var description = L(editionPaymentType + "_Edition_Description", targetEditionName);
            if (!paymentPeriodType.HasValue)
            {
                return description;
            }

            if (editionPaymentType == EditionPaymentType.NewRegistration || editionPaymentType == EditionPaymentType.BuyNow)
            {
                description += " (" + L(paymentPeriodType.Value.ToString()) + ")";
            }

            return description;
        }
    }
}