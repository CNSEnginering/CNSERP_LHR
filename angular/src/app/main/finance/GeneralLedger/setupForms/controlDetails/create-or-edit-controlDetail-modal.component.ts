import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ControlDetailsServiceProxy, CreateOrEditControlDetailDto, GroupCategoryForComboboxDto, GroupCodesServiceProxy, GLOptionsServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditControlDetailModal',
    templateUrl: './create-or-edit-controlDetail-modal.component.html'
})
export class CreateOrEditControlDetailModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag = false;

    controlDetail: CreateOrEditControlDetailDto = new CreateOrEditControlDetailDto();
    grgcat: GroupCategoryForComboboxDto[] = [];
    GroupCat: number;

    constructor(
        injector: Injector,
        private _groupCodesServiceProxy: GroupCodesServiceProxy,
        private _controlDetailsServiceProxy: ControlDetailsServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy
    ) {
        super(injector);
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


    ngOnInit(): void {
        debugger
        this.GetGloptionList();
    }

    GloptionSetup : string;

    segment1des = '';

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
            this.GloptionSetup = res.items[0].glOption.seg1Name;
        })
    }


    show(update: boolean = false, controlDetailId?:number): void {
        debugger;
        this.init();
        if ( !update) {
            this.controlDetail = new CreateOrEditControlDetailDto();
            this.controlDetail.id = controlDetailId;
            this.controlDetail.family = 1
            this.flag = update;
            
            this.active = true;
            this.modal.show();
        } else {
            this._controlDetailsServiceProxy.getControlDetailForEdit(controlDetailId).subscribe(result => {
                this.controlDetail = result.controlDetail;
                this.flag = update;
                this.GroupCat = result.family;
                this.active = true;
                this.modal.show();
            });
        }
    }

    init(): void {

        debugger;

        this._controlDetailsServiceProxy.conDetailMaxid().subscribe((result) => {
            this.controlDetail.seg1ID = result
        });
        this._groupCodesServiceProxy.getGroupCategoryForCombobox()
            .subscribe((result) => {
                this.grgcat = result.items;
            });
    }

    save(): void {
        debugger;
        this.saving = true;
       this.controlDetail.flag = this.flag;
        this._controlDetailsServiceProxy.createOrEdit(this.controlDetail)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new group", this.l('SavedSuccessfully'), (isConfirmed) => {
                    if (isConfirmed) {
                        this.show();
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

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
