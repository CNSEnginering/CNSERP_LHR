import { Component, Input, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ScriptLoaderService } from '@shared/utils/script-loader.service';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { XmlHttpRequestHelper } from '@shared/helpers/XmlHttpRequestHelper';
import { TenantRegistrationHelperService } from '@account/register/tenant-registration-helper.service';

import {
    StripePaymentServiceProxy,
    StripeCreateSubscriptionInput,
    PaymentServiceProxy,
    SubscriptionPaymentDto,
    StripeConfigurationDto,
    PaymentPeriodType,
    SubscriptionPaymentGatewayType,
    SubscriptionStartType,
    EditionPaymentType
} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'stripe-subscribe-component',
    templateUrl: './stripe-subscribe.component.html',
    animations: [accountModuleAnimation()]
})

export class StripeSubscribeComponent extends AppComponentBase implements OnInit {
    @Input() selectedPaymentPeriodType: PaymentPeriodType = PaymentPeriodType.Monthly;
    @Input() editionPaymentType: EditionPaymentType;

    amount = 0;
    description = '';

    subscriptionPayment: SubscriptionPaymentDto;
    stripeIsLoading = true;
    subscriptionPaymentGateway = SubscriptionPaymentGatewayType;
    subscriptionStartType = SubscriptionStartType;

    paymentId;
    successCallbackUrl;
    errorCallbackUrl;
    redirectUrl = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _stripePaymentAppService: StripePaymentServiceProxy,
        private _paymentAppService: PaymentServiceProxy,
        private _router: Router,
        private _tenantRegistrationHelper: TenantRegistrationHelperService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.paymentId = this._activatedRoute.snapshot.queryParams['paymentId'];
        this.redirectUrl = this._activatedRoute.snapshot.queryParams['redirectUrl'];

        new ScriptLoaderService().load('https://checkout.stripe.com/checkout.js').then(() => {
            this._paymentAppService.getPayment(this.paymentId)
                .subscribe((result: SubscriptionPaymentDto) => {
                    this.amount = result.amount;
                    this.description = result.description;
                    this.successCallbackUrl = result.successUrl;
                    this.errorCallbackUrl = result.errorUrl;

                    this._stripePaymentAppService.getConfiguration()
                        .subscribe((config: StripeConfigurationDto) => {
                            this.prepareStripeButton(config.publishableKey);
                            this.stripeIsLoading = false;
                        });
                });
        });
    }

    prepareStripeButton(stripeKey: string): void {
        let handler = StripeCheckout.configure({
            key: stripeKey,
            locale: 'auto',
            currency: this.appSession.application.currency,
            token: token => {
                abp.ui.setBusy();

                let input = new StripeCreateSubscriptionInput();
                input.paymentId = this.paymentId;
                input.stripeToken = token.id;

                this._stripePaymentAppService.createSubscription(input).subscribe(() => {
                    XmlHttpRequestHelper.ajax('POST',
                        this.successCallbackUrl + (this.successCallbackUrl.indexOf('?') >= 0 ? '&' : '?') + 'paymentId=' + this.paymentId,
                        null,
                        null,
                        (result) => {
                            this._tenantRegistrationHelper.registrationResult.isActive = true;
                            abp.ui.clearBusy();
                            this._router.navigate([this.redirectUrl]);
                        });
                });
            }
        });

        document.getElementById('stripe-checkout').addEventListener('click', e => {
            handler.open({
                name: 'ERP',
                description: this.description,
                amount: this.amount * 100
            });
            e.preventDefault();
        });

        window.addEventListener('popstate', () => {
            handler.close();
        });
    }
}
