using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Editions;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;

namespace ERP.MultiTenancy.Payments
{
    [Table("AppSubscriptionPayments")]
    [MultiTenancySide(MultiTenancySides.Host)]
    public class SubscriptionPayment : FullAuditedEntity<long>
    {
        public string Description { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionPaymentStatus Status { get; protected set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public string ExternalPaymentId { get; set; }

        public Edition Edition { get; set; }

        public string InvoiceNo { get; set; }

        public bool IsRecurring { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }

        public void SetAsCancelled()
        {
            if (Status == SubscriptionPaymentStatus.NotPaid)
            {
                Status = SubscriptionPaymentStatus.Cancelled;
            }
        }

        public void SetAsFailed()
        {
            Status = SubscriptionPaymentStatus.Failed;
        }

        public void SetAsPaid()
        {
            if (Status == SubscriptionPaymentStatus.NotPaid)
            {
                Status = SubscriptionPaymentStatus.Paid;
            }
        }

        public void SetAsCompleted()
        {
            if (Status == SubscriptionPaymentStatus.Paid)
            {
                Status = SubscriptionPaymentStatus.Completed;
            }
        }

        public SubscriptionPayment()
        {
            Status = SubscriptionPaymentStatus.NotPaid;
        }

        public PaymentPeriodType GetPaymentPeriodType()
        {
            return DayCount == 30 ? Payments.PaymentPeriodType.Monthly : Payments.PaymentPeriodType.Annual;
        }
    }
}
