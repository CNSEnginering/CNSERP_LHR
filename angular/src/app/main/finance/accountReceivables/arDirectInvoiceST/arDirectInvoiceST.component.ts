import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DirectinvoiceComponent } from '../../accountPayables/directInvoices/directInvoice.component';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './arDirectInvoiceST.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ARDirectinvoiceSTComponent extends AppComponentBase {

    @ViewChild('directInvoiceModal', { static: true }) directInvoiceModal: DirectinvoiceComponent;

    constructor(
        injector: Injector,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
    ) {
        super(injector);
    }

    ngOnInit(){
        debugger;
        this.directInvoiceModal.getInvType('ST');
        
    }
}
