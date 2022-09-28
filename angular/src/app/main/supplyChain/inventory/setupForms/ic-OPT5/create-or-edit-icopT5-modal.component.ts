import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ICOPT5ServiceProxy } from '../../shared/services/ic-opt5-service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditICOPT5Dto } from '../../shared/dto/ic-opt5-dto';
import * as moment from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';

@Component({
    selector: 'createOrEditICOPT5Modal',
    templateUrl: './create-or-edit-icopT5-modal.component.html'
})
export class CreateOrEditICOPT5ModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    icopT5: CreateOrEditICOPT5Dto = new CreateOrEditICOPT5Dto();

    audtDate: Date;
    createDate: Date;


    seg1IdFilter: '';
        segment2Filter = '';
        segment3Filter = '';
        maxAllowNegativeFilter : number;
            maxAllowNegativeFilterEmpty : number;
            minAllowNegativeFilter : number;
            minAllowNegativeFilterEmpty : number;
        maxErrSrNoFilter : number;
            maxErrSrNoFilterEmpty : number;
            minErrSrNoFilter : number;
            minErrSrNoFilterEmpty : number;
        maxCostingMethodFilter : number;
            maxCostingMethodFilterEmpty : number;
            minCostingMethodFilter : number;
            minCostingMethodFilterEmpty : number;
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
        maxCurrentLocIDFilter : number;
            maxCurrentLocIDFilterEmpty : number;
            minCurrentLocIDFilter : number;
            minCurrentLocIDFilterEmpty : number;
        opt4Filter = '';
        opt5Filter = '';
        
        maxCreateadOnFilter : moment.Moment;
            minCreateadOnFilter : moment.Moment;

            optSetup: string;
    filterText: any;
    createdByFilter: string;

    


    constructor(
        injector: Injector,
        private _icopT5ServiceProxy: ICOPT5ServiceProxy,
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
          this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty: this.maxErrSrNoFilter,
          this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty: this.minErrSrNoFilter,
          this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty: this.maxCostingMethodFilter,
          this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty: this.minCostingMethodFilter,
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
          this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty: this.maxCurrentLocIDFilter,
          this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty: this.minCurrentLocIDFilter,
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
          this.optSetup = result.items[0].opt5;
      });
      }

    show(icuomId?: number): void {
        this.createDate = null;
        this.audtDate = null;

        if (!icuomId) {
            this.icopT5 = new CreateOrEditICOPT5Dto();
            this.icopT5.id = icuomId;
            this.icopT5.active = true;
            this._icopT5ServiceProxy.getMaxLocId().subscribe(result => {
                this.icopT5.optID = result;
            });
            this.active = true;
            this.modal.show();
        } else {
            this._icopT5ServiceProxy.getICOPT5ForEdit(icuomId).subscribe(result => {
                this.icopT5 = result.ICOPT5;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.icopT5.audtDate = moment();
        this.icopT5.audtUser = this.appSession.user.userName;

        if (!this.icopT5.id) {
            this.icopT5.createDate = moment();
            this.icopT5.createdBy = this.appSession.user.userName;
        }

        this._icopT5ServiceProxy.createOrEdit(this.icopT5)
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