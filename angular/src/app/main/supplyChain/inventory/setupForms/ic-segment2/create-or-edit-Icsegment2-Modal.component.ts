import { Component, OnInit, Injector, ViewChild, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IcSegment2ServiceProxy, CreateOrEditICSegment2Dto } from '../../shared/services/ic-segment2-service';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { IcSegmentFinderModalComponent } from '../../FinderModals/ic-segment-finder-modal.component';
import * as  moment from'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';

@Component({
    selector: 'createOrEditIcSegment2Modal',
    templateUrl: './create-or-edit-Icsegment2-Modal.component.html'
})
export class CreateOrEditIcSegment2ModalComponent extends AppComponentBase implements OnInit {

    active = false;
    saving = false;
    flag = false;

    //seg1ID = '';
    seg1Name = '';
    filterText: '';
    seg1IdFilter: '';
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

  seg1Setup: string;
  seg2Setup: string;

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('ICSegment1FinderModal', {static: true}) icSegment1FinderModal: IcSegmentFinderModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    iCSegment2: CreateOrEditICSegment2Dto = new CreateOrEditICSegment2Dto();
    constructor(
        injector: Injector,
        private _IcSegment2ServiceProxy: IcSegment2ServiceProxy,
        private _icSetupsService: ICSetupsService
    ) { super(injector) }

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
        });
    }

    show(update: boolean = false, Segment2ID?: number): void {
        debugger;
        if (!update) {
            this.iCSegment2 = new CreateOrEditICSegment2Dto();
            this.iCSegment2.id = Segment2ID;
            this.flag = update;

            this.active = true;
            this.modal.show();
        } else {
            this._IcSegment2ServiceProxy.GetICSegment2ForEdit(Segment2ID).subscribe(result => {
                this.seg1Name = result.seg1Name;
                this.iCSegment2 = result.icSegment;
                this.flag = update;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        this.iCSegment2.flag = this.flag;
        this._IcSegment2ServiceProxy.createOrEdit(this.iCSegment2)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for New Segment",this.l('SavedSuccessfully'),( (isConfirmed) =>  {
                    if (isConfirmed) {

                        let seg1ID = this.iCSegment2.seg1ID;
                        this.iCSegment2 = new CreateOrEditICSegment2Dto();
                        this.flag = false;
                        this.iCSegment2.seg1ID = seg1ID;
                        this._IcSegment2ServiceProxy.GetSegment2MaxId(this.iCSegment2.seg1ID).subscribe(result => {
                            this.iCSegment2.seg2ID = result.toString();
                        });

                        this.modalSave.emit(null);
                    }
                    else {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    }
                }));

                
            });
    }

    openSelectICSegment1Modal() {
        debugger;
         this.icSegment1FinderModal.id = this.iCSegment2.seg1ID;
         this.icSegment1FinderModal.displayName = this.seg1Name;
         this.icSegment1FinderModal.show();
    }


    setICSegment1IdNull() {
        debugger;
         this.iCSegment2.seg1ID = null;
         this.seg1Name = '';
         this.iCSegment2.seg2ID  = null;
    }


    getNewICSegment1Id() {
        debugger;
         this.iCSegment2.seg1ID = this.icSegment1FinderModal.id;
         this.seg1Name = this.icSegment1FinderModal.displayName;

         if (this.iCSegment2.seg1ID !== '' && this.iCSegment2.seg1ID != null) {
             this.iCSegment2.seg2ID = null;
             this._IcSegment2ServiceProxy.GetSegment2MaxId(this.iCSegment2.seg1ID).subscribe(result => {
                 this.iCSegment2.seg2ID = result.toString();
             });
         }
         else {
             this.iCSegment2.seg2ID = null;
         }
    }

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
