import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ICOPT4ServiceProxy } from '../../shared/services/ic-opt4-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditICOPT4Dto } from '../../shared/dto/ic-opt4-dto';
import * as moment from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';

@Component({
    selector: 'createOrEditICOPT4Modal',
    templateUrl: './create-or-edit-icopT4-modal.component.html'
})
export class CreateOrEditICOPT4ModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

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

    maxCreateadOnFilter: moment.Moment;
    minCreateadOnFilter: moment.Moment;

    optSetup: string;

    icopT4: CreateOrEditICOPT4Dto = new CreateOrEditICOPT4Dto();

    audtDate: Date;
    createDate: Date;
    createdByFilter: any;
    filterText: string;


    constructor(
        injector: Injector,
        private _icopT4ServiceProxy: ICOPT4ServiceProxy,
        private _icSetupsService: ICSetupsService,
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
            this.optSetup = result.items[0].opt4;
        });
    }


    show(icopT4?: number): void {
        debugger;
        this.createDate = null;
        this.audtDate = null;

        if (!icopT4) {
            this.icopT4 = new CreateOrEditICOPT4Dto();
            this.icopT4.id = icopT4;

            this.icopT4.active = true;
            this._icopT4ServiceProxy.getMaxLocId().subscribe(result => {
                this.icopT4.optID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            this._icopT4ServiceProxy.getICOPT4ForEdit(icopT4).subscribe(result => {
                this.icopT4 = result.iCOPT4;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;
        this.icopT4.audtDate = moment();
        this.icopT4.audtUser = this.appSession.user.userName;

        if (!this.icopT4.id) {
            this.icopT4.createDate = moment();
            this.icopT4.createdBy = this.appSession.user.userName;
        }
        this._icopT4ServiceProxy.createOrEdit(this.icopT4)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
