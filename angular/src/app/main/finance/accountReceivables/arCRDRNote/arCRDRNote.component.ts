import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { CRDRNoteComponent } from '../../accountPayables/crdrNote/crdrNote.component';

@Component({
    templateUrl: './arCRDRNote.component.html',
    encapsulation: ViewEncapsulation.None,
})
export class ARCRDRComponent extends AppComponentBase {

    @ViewChild('crdrNoteComponent', { static: true }) CRDRNoteModal: CRDRNoteComponent;

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
        this.CRDRNoteModal.setTypeID(1);
        
    }
}
