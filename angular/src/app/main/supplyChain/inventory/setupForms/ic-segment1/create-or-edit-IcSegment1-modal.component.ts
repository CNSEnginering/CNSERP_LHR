import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditICSegment1Dto, IcSegment1ServiceProxy } from '../../shared/services/ic-segment1-service';
import { ICSetupsService } from '../../shared/services/ic-setup.service';
import * as moment from 'moment';
import { RegisterTenantResultComponent } from '@account/register/register-tenant-result.component';


@Component({
    selector: 'createOrEditIcSegment1selector',
    templateUrl: './create-or-edit-IcSegment1-modal.component.html'
})
export class CreateOrEditIcSegment1ModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag = false;

    segSetup: string;

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

    disableCreateButton = false;

    iCSegment1: CreateOrEditICSegment1Dto = new CreateOrEditICSegment1Dto();

    constructor(
        injector: Injector,
        private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
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
            this.segSetup = result.items[0].segment1;
        });
    }

    show(update: boolean = false, Segment1ID?: number): void {
        debugger;
        this.init();
        if (!update) {
            this.iCSegment1 = new CreateOrEditICSegment1Dto();
            this.iCSegment1.id = Segment1ID;
            this.flag = update;

            this.active = true;
            this.modal.show();
        } else {
            this._IcSegment1ServiceProxy.GetICSegment1ForEdit(Segment1ID).subscribe(result => {
                this.iCSegment1 = result.icSegment;
                this.flag = update;
                this.active = true;
                this.modal.show();
            });
        }
    }

    init(): void {
        this._IcSegment1ServiceProxy.GetSegment1MaxId().subscribe((result) => {
            this.iCSegment1.seg1ID = result.toString();
        });
    }

    save(): void {
        debugger;
        this.saving = true;
        this.iCSegment1.flag = this.flag;
        this._IcSegment1ServiceProxy.createOrEdit(this.iCSegment1)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {

                this.message.confirm("Press 'Yes' for New Segment",this.l('SavedSuccessfully'),( (isConfirmed) =>  {
                    if (isConfirmed) {
                        let seg1ID = this.iCSegment1.seg1ID;
                        this.iCSegment1 = new CreateOrEditICSegment1Dto();

                        this.init();
                        this.flag = false;
                        this.modalSave.emit(null);
                    }
                    else
                    {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this.close();
                        this.modalSave.emit(null);
                    }
                }));
               
            });
    }

    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
