import { Component, Input, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ScriptLoaderService } from '@shared/utils/script-loader.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { XmlHttpRequestHelper } from '@shared/helpers/XmlHttpRequestHelper';
import { TenantRegistrationHelperService } from '@account/register/tenant-registration-helper.service';

import {
    CancelPaymentDto,
    PayPalPaymentServiceProxy,
    SubscriptionPaymentDto,
    PaymentServiceProxy,
    PayPalConfigurationDto,
    PaymentPeriodType,
    SubscriptionPaymentGatewayType,
    EditionPaymentType
} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'paypal-purchase-component',
    templateUrl: './paypal-purchase.component.html',
    animations: [accountModuleAnimation()]
})

export class PayPalPurchaseComponent extends AppComponentBase implements OnInit {
    @Input() selectedPaymentPeriodType: PaymentPeriodType = PaymentPeriodType.Monthly;
    @Input() editionPaymentType: EditionPaymentType;

    config: PayPalConfigurationDto;

    paypalIsLoading = true;
    subscriptionPaymentGateway = SubscriptionPaymentGatewayType;

    totalAmount = 0;
    description = '';
    paymentId;
    redirectUrl;
    successCallbackUrl;
    errorCallbackUrl;

    constructor(
        private injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _payPalPaymentAppService: PayPalPaymentServiceProxy,
        private _paymentAppService: PaymentServiceProxy,
        private _router: Router,
        private _tenantRegistrationHelper: TenantRegistrationHelperService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.paymentId = this._activatedRoute.snapshot.queryParams['paymentId'];
        this.redirectUrl = this._activatedRoute.snapshot.queryParams['redirectUrl'];

        new ScriptLoaderService().load('https://www.paypalobjects.com/api/checkout.js').then(() => {

            this._paymentAppService.getPayment(this.paymentId)
                .subscribe((result: SubscriptionPaymentDto) => {
                    this.description = result.description;
                    this.totalAmount = result.amount;
                    this.successCallbackUrl = result.successUrl;
                    this.errorCallbackUrl = result.errorUrl;

                    this.subscriptionPaymentGateway = ((result.gateway) as any);

                    this._payPalPaymentAppService.getConfiguration()
                        .subscribe((config: PayPalConfigurationDto) => {
                            this.config = config;

                            this.paypalIsLoading = false;
                            this.preparePaypalButton();
                        });
                });
        });
    }

    preparePaypalButton(): void {
        const self = this;
        (<any>window).paypal.Button.render({
            style: { size: 'responsive' },
            env: self.config.environment,
            client: {
                sandbox: self.config.clientId,
                production: ''
            },
            commit: true,
            payment(data, actions) {
                return actions.payment.create({
                    transactions: [{
                        amount: {
                            total: self.totalAmount,
                            currency: self.appSession.application.currency
                        }
                    }]
                });
            },
            onAuthorize(data) {
                self._payPalPaymentAppService.confirmPayment(self.paymentId, data.paymentID, data.payerID)
                    .subscribe(() => {
                        XmlHttpRequestHelper.ajax('post',
                            self.successCallbackUrl + (self.successCallbackUrl.includes('?') ? '&' : '?') + 'paymentId=' + self.paymentId,
                            null,
                            null,
                            (result) => {
                                this._tenantRegistrationHelper.registrationResult.isActive = true;
                                self._router.navigate([self.redirectUrl]);
                            });
                    });
            },
            onCancel(data) {
                const input = new CancelPaymentDto();
                input.gateway = self.subscriptionPaymentGateway.Paypal;
                input.paymentId = data.paymentID;
                self._payPalPaymentAppService.cancelPayment(input).toPromise();
            }
        }, '#paypal-button');

    }
}
