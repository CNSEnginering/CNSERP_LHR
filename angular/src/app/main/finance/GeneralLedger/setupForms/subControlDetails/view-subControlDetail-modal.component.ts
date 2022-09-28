import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetSubControlDetailForViewDto, SubControlDetailDto, SubControlDetailsServiceProxy, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { RegisterTenantResultComponent } from '@account/register/register-tenant-result.component';
import * as moment from 'moment';

@Component({
    selector: 'viewSubControlDetailModal',
    templateUrl: './view-subControlDetail-modal.component.html'
})
export class ViewSubControlDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSubControlDetailForViewDto;


    constructor(
        injector: Injector,
        private _subControlDetailsServiceProxy: SubControlDetailsServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy
    ) {
        super(injector);
        this.item = new GetSubControlDetailForViewDto();
        this.item.subControlDetail = new SubControlDetailDto();
    }

    ngOnInit(): void {
        this.GetGloptionList();
    }

    defaultclaccFilter = '';
    stockctrlaccFilter = '';
    seg1NameFilter = '';
    seg2NameFilter = '';
    seg3NameFilter = '';
    directPostFilter = -1;
    autoSeg3Filter = -1;
    maxAUDTDATEFilter : moment.Moment;
		minAUDTDATEFilter : moment.Moment;
    audtuserFilter = '';
        chartofControlIdFilter = '';

        GloptionSetup : string;
        Gloptionlevel1 : string;

    GetGloptionList() {
        this._gLOptionsServiceProxy.getAll(
            "",
            this.defaultclaccFilter,
            this.stockctrlaccFilter,
            this.seg1NameFilter,
            this.seg2NameFilter,
            this.seg3NameFilter,
            this.directPostFilter,
            this.autoSeg3Filter,
            this.maxAUDTDATEFilter,
            this.minAUDTDATEFilter,
            this.audtuserFilter,
            this.chartofControlIdFilter,
            null,
            0,
            2147483646
            ).subscribe( res => {
            debugger;
            this.GloptionSetup = res.items[0].glOption.seg2Name;
            this.Gloptionlevel1 = res.items[0].glOption.seg1Name;
        })
    }

    show(item: GetSubControlDetailForViewDto): void {

        this._subControlDetailsServiceProxy.getSubControlDetailForView(item.subControlDetail.id).subscribe(result => {
            debugger;
            this.item = result;
        });
        // this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
