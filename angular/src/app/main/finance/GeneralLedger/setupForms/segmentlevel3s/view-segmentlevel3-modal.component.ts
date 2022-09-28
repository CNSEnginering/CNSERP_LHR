import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetSegmentlevel3ForViewDto, Segmentlevel3Dto, Segmentlevel3sServiceProxy, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'viewSegmentlevel3Modal',
    templateUrl: './view-segmentlevel3-modal.component.html'
})
export class ViewSegmentlevel3ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetSegmentlevel3ForViewDto;


    constructor(
        injector: Injector,
        private _segmentlevel3sServiceProxy: Segmentlevel3sServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy

    ) {
        super(injector);
        this.item = new GetSegmentlevel3ForViewDto();
        this.item.segmentlevel3 = new Segmentlevel3Dto();
    }

    ngOnInit(): void {
        debugger
        this.GetGloptionList();
    }

    GloptionSetup : string;

    segment1Des = '';
    segment2Des = '';

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

        Gloptionlevel1: string;
        Gloptionlevel2: string;

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
            this.GloptionSetup = res.items[0].glOption.seg3Name;
            this.Gloptionlevel1 = res.items[0].glOption.seg1Name;
            this.Gloptionlevel2 = res.items[0].glOption.seg2Name
        })
    }

    show(item: GetSegmentlevel3ForViewDto): void {
        debugger;

        this._segmentlevel3sServiceProxy.getSegmentlevel3ForView(item.segmentlevel3.id).subscribe(result => {
            this.item = result;

        });

        //this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
