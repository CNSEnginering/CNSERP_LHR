import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { Segmentlevel3sServiceProxy, CreateOrEditSegmentlevel3Dto, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { Segmentlevel3ControlDetailLookupTableModalComponent } from './segmentlevel3-controlDetail-lookup-table-modal.component';
import { Segmentlevel3SubControlDetailLookupTableModalComponent } from './segmentlevel3-subControlDetail-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
    selector: 'createOrEditSegmentlevel3Modal',
    templateUrl: './create-or-edit-segmentlevel3-modal.component.html'
})
export class CreateOrEditSegmentlevel3ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('segmentlevel3ControlDetailLookupTableModal', { static: true }) segmentlevel3ControlDetailLookupTableModal: Segmentlevel3ControlDetailLookupTableModalComponent;
    @ViewChild('segmentlevel3SubControlDetailLookupTableModal', { static: true }) segmentlevel3SubControlDetailLookupTableModal: Segmentlevel3SubControlDetailLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    Isupdate = false;
    flag  = false;

    segmentlevel3: CreateOrEditSegmentlevel3Dto = new CreateOrEditSegmentlevel3Dto();

    controlDetailId = '';
    controlDetailDesc = ''
    subControlDetailId = '';
    subControlDetailDesc = '';
    target: string;

    constructor(
        injector: Injector,
        private _segmentlevel3sServiceProxy: Segmentlevel3sServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy
    ) {
        super(injector);
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

    show(update:boolean = false, segmentlevel3Id?: number): void {
        debugger;
        if (!update) {
            this.segmentlevel3 = new CreateOrEditSegmentlevel3Dto();
            this.segmentlevel3.id = segmentlevel3Id;
            this.controlDetailId = '';
            this.Isupdate = update;
            this.subControlDetailId = '';

            this.active = true;
            this.modal.show();
        } else {
            this._segmentlevel3sServiceProxy.getSegmentlevel3ForEdit(segmentlevel3Id).subscribe(result => {
                this.segmentlevel3 = result.segmentlevel3;
                this.flag = update;
                this.controlDetailId = result.controlDetailId;
                this.controlDetailDesc = result.controlDetailDesc
                this.subControlDetailId = result.subControlDetailId;
                this.subControlDetailDesc = result.subControlDetailDesc;
                this.Isupdate = update;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        this.segmentlevel3.flag = this.flag;
        debugger;
        this.segmentlevel3.controlDetailId = this.controlDetailId;
        this.segmentlevel3.subControlDetailId = this.subControlDetailId;
        this._segmentlevel3sServiceProxy.createOrEdit(this.segmentlevel3)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new main", this.l('SavedSuccessfully'), (isConfirmed) => {
                    if (isConfirmed) {
                        this.getNewSubControlDetailId();
                        this.controlDetailDesc = "";   
                        this.subControlDetailDesc = "";      
                        this.modalSave.emit(null);    
                    }
                    else
                    {
                        this.close();
                        this.modalSave.emit(null);
                    }
                }); 
            });
    }
    
    getNewFinanceModal()
    {
        debugger;
        switch (this.target) {
            case "Level1":
                this.getNewControlDetailId();
                break;
            case "Level2":
                this.getNewSubControlDetailId();
                break;

            default:
                break;
        }
    }

    openSelectControlDetailModal() {
        this.target="Level1";
        this.FinanceLookupTableModal.id = this.controlDetailId;
        this.FinanceLookupTableModal.displayName = this.controlDetailDesc;
        this.FinanceLookupTableModal.show(this.target);
    }

    openSelectSubControlDetailModal() {
        debugger;
        this.target="Level2";
        this.FinanceLookupTableModal.id = this.subControlDetailId;
        this.FinanceLookupTableModal.displayName = this.subControlDetailDesc;
        this.FinanceLookupTableModal.show(this.target,this.controlDetailId);

    }


    setControlDetailIdNull() {
        this.controlDetailId = '';
        this.controlDetailDesc = '';
        this.subControlDetailId = '';
        this.subControlDetailDesc = '';
        this.segmentlevel3.seg3ID = null;

    }

    setSubControlDetailIdNull() {
        debugger;
        this.subControlDetailId = '';
        this.subControlDetailDesc = '';
        this.segmentlevel3.seg3ID = null;
    }


    getNewControlDetailId() {
        debugger;
        this.controlDetailId = this.FinanceLookupTableModal.id;
        this.controlDetailDesc = this.FinanceLookupTableModal.displayName;
        this.subControlDetailId = null;
        this.subControlDetailDesc = '';
        this.segmentlevel3.seg3ID = null;

    }

    getNewSubControlDetailId() {
        this.subControlDetailId = this.FinanceLookupTableModal.id;
        this.subControlDetailDesc = this.FinanceLookupTableModal.displayName;
debugger;
        if (this.subControlDetailId != '' &&  this.subControlDetailId != null ) {

            this._segmentlevel3sServiceProxy.getMaxSeg3ID(this.subControlDetailId).subscribe(result => {
                this.segmentlevel3.seg3ID = result;
            });
        } else {
            this.subControlDetailId = null;
        }


    }

    close(): void {

        this.flag = false;
        this.active = false;
        this.modal.hide();
    }
}
