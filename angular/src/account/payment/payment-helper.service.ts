import { Injectable } from '@angular/core';
import { EditionPaymentType, SubscriptionPaymentGatewayType } from '@shared/service-proxies/service-proxies';

@Injectable()
export class PaymentHelperService {

    getPaymentGatewayType(gatewayType) {
        if (parseInt(gatewayType) === SubscriptionPaymentGatewayType.Paypal) {
            return 'Paypal';
        }

        return 'Stripe';
    }

    getEditionPaymentType(editionPaymentType) {
        if (parseInt(editionPaymentType) === EditionPaymentType.BuyNow) {
            return 'BuyNow';
        } else if (parseInt(editionPaymentType) === EditionPaymentType.Extend) {
            return 'Extend';
        } else if (parseInt(editionPaymentType) === EditionPaymentType.NewRegistration) {
            return 'NewRegistration';
        }

        return 'Upgrade';
    }
}
