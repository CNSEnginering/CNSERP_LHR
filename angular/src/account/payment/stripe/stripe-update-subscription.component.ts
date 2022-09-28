import { Component, Injector, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { XmlHttpRequestHelper } from '@shared/helpers/XmlHttpRequestHelper';

import {
    StripePaymentServiceProxy,
    PaymentServiceProxy,
    StripeUpdateSubscriptionInput,
    SubscriptionPaymentDto
} from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'stripe-update-subscirption-component',
    templateUrl: './stripe-update-subscription.component.html',
    animations: [accountModuleAnimation()]
})

export class StripeUpdateSubscriptionComponent extends AppComponentBase implements OnInit {

    description = '';
    paymentId;
    successCallbackUrl;
    errorCallbackUrl;
    redirectUrl = '';

    constructor(
        injector: Injector,
        private _activatedRoute: ActivatedRoute,
        private _stripePaymentAppService: StripePaymentServiceProxy,
        private _paymentAppService: PaymentServiceProxy,
        private _router: Router
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.paymentId = this._activatedRoute.snapshot.queryParams['paymentId'];
        this.redirectUrl = this._activatedRoute.snapshot.queryParams['redirectUrl'];

        this._paymentAppService.getPayment(this.paymentId)
            .subscribe((result: SubscriptionPaymentDto) => {
                this.description = result.description;
                this.successCallbackUrl = result.successUrl;
                this.errorCallbackUrl = result.errorUrl;
            });
    }

    upgradeSubscription(): void {
        abp.ui.setBusy();

        let input = new StripeUpdateSubscriptionInput();
        input.paymentId = this.paymentId;

        this._stripePaymentAppService.updateSubscription(input).subscribe(() => {
            XmlHttpRequestHelper.ajax('POST',
                this.successCallbackUrl + (this.successCallbackUrl.indexOf('?') >= 0 ? '&' : '?') + 'paymentId=' + this.paymentId,
                null,
                null,
                (result) => {
                    abp.ui.clearBusy();
                    this._router.navigate([this.redirectUrl]);
                });
        });
    }
}
