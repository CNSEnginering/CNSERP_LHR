import { Component, Injector, ViewChild, Output, EventEmitter, OnInit } from '@angular/core';
import { AppCommonModule } from '@app/shared/common/app-common.module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';

import { CreateOrEditICSegment3Dto, IcSegment3ServiceProxy, ICSegment3Dto } from '../../shared/services/ic-segment3-service';
import { ModalDirective } from 'ngx-bootstrap';
import { IcSegmentFinderModalComponent } from '../../FinderModals/ic-segment-finder-modal.component';
import { IcSegment2FinderModalComponent } from '../../FinderModals/ic-segment2-finder-modal.component';
import * as  moment from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';
@Component({
    selector: 'createOrEditIcSegment3Modal',
    templateUrl: './create-or-edit-Icsegment3-Modal.component.html'
})
export class CreateOrEditIcSegment3ModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('ICSegment1FinderModal', { static: true }) icSegment1FinderModal: IcSegmentFinderModalComponent;
    @ViewChild('ICSegment2FinderModal', { static: true }) icSegment2FinderModal: IcSegment2FinderModalComponent;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag = false;

    seg1Name: string;
    seg2Name: string;


    seg1Setup: string;
    seg2Setup: string;
    seg3Setup: string;

    filterText: '';
    seg1IdFilter: '';
    seg1NameFilter: '';
    segment2Filter = '';
    segment3Filter = '';
    maxAllowNegativeFilter: number;
    maxAllowNegativeFilterEmpty: number;
    minAllowNegativeFilter: number;
    minAllowNegativeFilterEmpty: number;
    maxErrSrNoFilter: number;
    maxErrSrNoFilterEmpty: number;
    minErrSrNoFilter: number;
    minErrSrNoFilterEmpty: number;
    maxCostingMethodFilter: number;
    maxCostingMethodFilterEmpty: number;
    minCostingMethodFilter: number;
    minCostingMethodFilterEmpty: number;
    prBookIDFilter = '';
    rtBookIDFilter = '';
    cnsBookIDFilter = '';
    slBookIDFilter = '';
    srBookIDFilter = '';
    trBookIDFilter = '';
    prdBookIDFilter = '';
    pyRecBookIDFilter = '';
    adjBookIDFilter = '';
    asmBookIDFilter = '';
    wsBookIDFilter = '';
    dsBookIDFilter = '';
    maxCurrentLocIDFilter: number;
    maxCurrentLocIDFilterEmpty: number;
    minCurrentLocIDFilter: number;
    minCurrentLocIDFilterEmpty: number;
    opt4Filter = '';
    opt5Filter = '';
    createdByFilter = '';
    maxCreateadOnFilter: moment.Moment;
    minCreateadOnFilter: moment.Moment;


    iCSegment3: CreateOrEditICSegment3Dto = new CreateOrEditICSegment3Dto();

    constructor(
        injector: Injector,
        private _IcSegment3ServiceProxy: IcSegment3ServiceProxy,
        private _icSetupsService: ICSetupsService
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this._icSetupsService.getAll(
            this.filterText,
            this.seg1IdFilter,
            this.segment2Filter,
            this.segment3Filter,
            this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty : this.maxErrSrNoFilter,
            this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty : this.minErrSrNoFilter,
            this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty : this.maxCostingMethodFilter,
            this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty : this.minCostingMethodFilter,
            this.prBookIDFilter,
            this.rtBookIDFilter,
            this.cnsBookIDFilter,
            this.slBookIDFilter,
            this.srBookIDFilter,
            this.trBookIDFilter,
            this.prdBookIDFilter,
            this.pyRecBookIDFilter,
            this.adjBookIDFilter,
            this.asmBookIDFilter,
            this.wsBookIDFilter,
            this.dsBookIDFilter,
            this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty : this.maxCurrentLocIDFilter,
            this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty : this.minCurrentLocIDFilter,
            this.opt4Filter,
            this.opt5Filter,
            this.createdByFilter,
            this.maxCreateadOnFilter,
            this.minCreateadOnFilter,
            null,
            0,
            2147483646
        ).subscribe(result => {
            debugger;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.seg1Setup = result.items[0].segment1;
            this.seg2Setup = result.items[0].segment2;
            this.seg3Setup = result.items[0].segment3;
        });
    }


    show(update: boolean = false, Segment3ID?: number): void {
        debugger;

        if (!update) {
            this.iCSegment3 = new CreateOrEditICSegment3Dto();
            this.iCSegment3.id = Segment3ID;
            this.flag = update;

            this.active = true;
            this.modal.show();
        } else {
            this._IcSegment3ServiceProxy.GetICSegment3ForEdit(Segment3ID).subscribe(result => {
                this.iCSegment3 = result.icSegment;
                this.seg1Name = result.seg1Name;
                this.seg2Name = result.seg2Name
                this.flag = update;
                this.active = true;
                this.modal.show();
            });
        }
    }



    openSelectICSegment1Modal() {
        this.icSegment1FinderModal.id = this.iCSegment3.seg1ID;
        this.icSegment1FinderModal.displayName = this.seg1Name;
        this.icSegment1FinderModal.show();
    }
    openSelectICSegment2Modal() {
        debugger;
        this.icSegment2FinderModal.id = this.iCSegment3.seg2ID;
        this.icSegment2FinderModal.displayName = this.seg2Name;
        this.icSegment2FinderModal.show(this.iCSegment3.seg1ID);

    }


    setICSegment1IdNull() {
        this.iCSegment3.seg1ID = '';
        this.seg1Name = '';
        this.iCSegment3.seg2ID = '';
        this.seg2Name = '';
        this.iCSegment3.seg3ID = null;

    }
    setICSegment2IdNull() {
        debugger;
        this.iCSegment3.seg2ID = '';
        this.seg2Name = '';
        this.iCSegment3.seg3ID = null;
    }


    getNewICSegment1Id() {
        debugger;
        this.iCSegment3.seg1ID = this.icSegment1FinderModal.id;
        this.seg1Name = this.icSegment1FinderModal.displayName;
        this.iCSegment3.seg2ID = null;
        this.seg2Name = '';
        this.iCSegment3.seg3ID = null;

    }
    getNewICSegment2Id() {
        this.iCSegment3.seg2ID = this.icSegment2FinderModal.id;
        this.seg2Name = this.icSegment2FinderModal.displayName;
        debugger;
        if (this.iCSegment3.seg2ID != '' && this.iCSegment3.seg2ID != null) {

            this._IcSegment3ServiceProxy.GetSegment3MaxId(this.iCSegment3.seg2ID).subscribe((result) => {
                this.iCSegment3.seg3ID = result.toString();
            });
        } else {
            this.iCSegment3.seg2ID = null;
        }


    }


    save(): void {
        debugger;
        this.saving = true;
        this.iCSegment3.flag = this.flag;
        this._IcSegment3ServiceProxy.createOrEdit(this.iCSegment3)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for New Segment", this.l('SavedSuccessfully'), ((isConfirmed) => {
                    if (isConfirmed) {
                        let seg1ID = this.iCSegment3.seg1ID;
                        let seg2ID = this.iCSegment3.seg2ID;
                        this.iCSegment3 = new CreateOrEditICSegment3Dto();
                        this.flag = false;
                        this.iCSegment3.seg1ID = seg1ID;
                        this.iCSegment3.seg2ID = seg2ID;

                        this._IcSegment3ServiceProxy.GetSegment3MaxId(this.iCSegment3.seg2ID).subscribe((result) => {
                            this.iCSegment3.seg3ID = result.toString();
                        });
                        this.modalSave.emit(null);
                    }
                    else {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    }
                }));
                // this.notify.info(this.l('SavedSuccessfully'));
                // this.close();
                // this.modalSave.emit(null);
            });
    }

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
