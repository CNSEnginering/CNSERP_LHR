import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    EditionSelectDto,
    PaymentInfoDto,
    PaymentServiceProxy,
    CreatePaymentDto,
    PaymentGatewayModel,
    EditionPaymentType,
    PaymentPeriodType,
    SubscriptionPaymentGatewayType,
    SubscriptionPaymentType,
    SubscriptionStartType
} from '@shared/service-proxies/service-proxies';

import { AppConsts } from '@shared/AppConsts';
import { PaymentHelperService } from './payment-helper.service';

@Component({
    templateUrl: './upgrade.component.html',
    animations: [accountModuleAnimation()]
})

export class UpgradeEditionComponent extends AppComponentBase implements OnInit {

    subscriptionPaymentType: typeof SubscriptionPaymentType = SubscriptionPaymentType;
    subscriptionStartType: typeof SubscriptionStartType = SubscriptionStartType;

    editionPaymentType: EditionPaymentType;
    edition: EditionSelectDto = new EditionSelectDto();
    tenantId: number = abp.session.tenantId;
    subscriptionPaymentGateway = SubscriptionPaymentGatewayType;

    paymentPeriodType: PaymentPeriodType;
    additionalPrice: number;
    upgradeEditionId;
    editionPaymentTypeCheck: typeof EditionPaymentType = EditionPaymentType;
    paymentGateways: PaymentGatewayModel[];

    constructor(
        injector: Injector,
        private _router: Router,
        private _paymnetHelperService: PaymentHelperService,
        private _activatedRoute: ActivatedRoute,
        private _paymentAppService: PaymentServiceProxy) {
        super(injector);
    }

    ngOnInit(): void {
        this.editionPaymentType = parseInt(this._activatedRoute.snapshot.queryParams['editionPaymentType']);
        this.upgradeEditionId = this._activatedRoute.snapshot.queryParams['upgradeEditionId'];

        if (this.appSession.tenant.edition.isFree) {
            this._router.navigate(['account/buy'],
                {
                    queryParams: {
                        tenantId: this.appSession.tenant.id,
                        editionPaymentType: this.editionPaymentType,
                        editionId: this.upgradeEditionId,
                        subscriptionStartType: this.subscriptionStartType.Paid
                    }
                });
        }

        this._paymentAppService.getPaymentInfo(this.upgradeEditionId)
            .subscribe((result: PaymentInfoDto) => {
                this.edition = result.edition;
                this.additionalPrice = Number(result.additionalPrice.toFixed(2));
            });

        this._paymentAppService.getLastCompletedPayment().subscribe(result => {
            let gateway = new PaymentGatewayModel();
            gateway.gatewayType = (result.gateway as any);
            gateway.supportsRecurringPayments = true;

            this.paymentGateways = [gateway];
            this.paymentPeriodType = result.paymentPeriodType;

            if (this.appSession.tenant.subscriptionPaymentType === this.subscriptionPaymentType.Manual) {
                this._paymentAppService.getActiveGateways(undefined)
                    .subscribe((result: PaymentGatewayModel[]) => {
                        this.paymentGateways = result;
                    });
            }
        });
    }

    checkout(gatewayType) {
        let input = new CreatePaymentDto();
        input.editionId = this.edition.id;
        input.editionPaymentType = ((this.editionPaymentType) as any);
        input.paymentPeriodType = ((this.paymentPeriodType) as any);
        input.recurringPaymentEnabled = this.hasRecurringSubscription();
        input.subscriptionPaymentGatewayType = gatewayType;
        input.successUrl = AppConsts.remoteServiceBaseUrl + '/api/services/app/payment/' + this._paymnetHelperService.getEditionPaymentType(this.editionPaymentType) + 'Succeed';
        input.errorUrl = AppConsts.remoteServiceBaseUrl + '/api/services/app/payment/PaymentFailed';

        this._paymentAppService.createPayment(input)
            .subscribe((paymentId: number) => {
                this._router.navigate(['account/' + this.getPaymentGatewayType(gatewayType).toLocaleLowerCase() + '-purchase'],
                    {
                        queryParams: {
                            paymentId: paymentId,
                            isUpgrade: true,
                            redirectUrl: 'app/admin/subscription-management'
                        }
                    });
            });
    }

    getPaymentGatewayType(gatewayType): string {
        return this._paymnetHelperService.getPaymentGatewayType(gatewayType);
    }

    hasRecurringSubscription(): boolean {
        return this.appSession.tenant.subscriptionPaymentType !== this.subscriptionPaymentType.Manual;
    }
}
