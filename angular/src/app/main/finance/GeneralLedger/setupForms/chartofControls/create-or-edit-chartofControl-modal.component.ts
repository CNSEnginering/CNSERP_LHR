import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { ChartofControlsServiceProxy, CreateOrEditChartofControlDto, SegmentCodeDtoView, ComboboxItemDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ChartofControlControlDetailLookupTableModalComponent } from './chartofControl-controlDetail-lookup-table-modal.component';
import { ChartofControlSubControlDetailLookupTableModalComponent } from './chartofControl-subControlDetail-lookup-table-modal.component';
import { ChartofControlSegmentlevel3LookupTableModalComponent } from './chartofControl-segmentlevel3-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { LegderTypeComboboxService } from '@app/shared/common/legdertype-combobox/legdertype-combobox.service';
import { PLCategoriesServiceProxy, IPLCategoryComboboxItemDto } from '@app/main/finance/shared/services/plCategories.service';
import { FriendProfilePictureComponent } from '@shared/utils/friend-profile-picture.component';


@Component({
    selector: 'createOrEditChartofControlModal',
    templateUrl: './create-or-edit-chartofControl-modal.component.html'
})
export class CreateOrEditChartofControlModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('chartofControlControlDetailLookupTableModal', { static: true }) chartofControlControlDetailLookupTableModal: ChartofControlControlDetailLookupTableModalComponent;
    @ViewChild('chartofControlSubControlDetailLookupTableModal', { static: true }) chartofControlSubControlDetailLookupTableModal: ChartofControlSubControlDetailLookupTableModalComponent;
    @ViewChild('chartofControlSegmentlevel3LookupTableModal', { static: true }) chartofControlSegmentlevel3LookupTableModal: ChartofControlSegmentlevel3LookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag = false;
    accode: SegmentCodeDtoView[] = [];
    chartofControl: CreateOrEditChartofControlDto = new CreateOrEditChartofControlDto();
    plCategoryList: IPLCategoryComboboxItemDto[];
    plBSCategoryList: IPLCategoryComboboxItemDto[];
    plCFCategoryList: IPLCategoryComboboxItemDto[];
    creationDate: Date;
    auditTime: Date;
    controlDetailId = '';
    controlDetailSegmentName = '';
    subControlDetailId = '';
    subControlDetailSegmentName = '';
    segmentlevel3ID = '';
    segmentlevel3SegmentName = '';
    editMode: boolean = false;
    ControlDID = "";
    segment1 = '';
    segment2 = '';
    segment3 = '';
    target: string;
    ledgerTypes: ComboboxItemDto[];
    constructor(
        injector: Injector,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy,
        private _LegderTypeComboboxService: LegderTypeComboboxService,
        private _PLCategoriesServiceProxy: PLCategoriesServiceProxy
    ) {
        super(injector);
    }



    show(update: boolean = false, chartofControlId?: string): void {
        this.creationDate = null;
        this.init();
        this.auditTime = null;
        if (!chartofControlId) {
            this.chartofControl = new CreateOrEditChartofControlDto();
            this.chartofControl.id = chartofControlId;
            this.chartofControl.inactive = false;
            this.chartofControl.subLedger = false;
            this.controlDetailId = '';
            this.controlDetailSegmentName = '';
            this.subControlDetailId = '';
            this.subControlDetailSegmentName = '';
            this.segmentlevel3ID = '';
            this.segmentlevel3SegmentName = '';
            this.flag = update;
            this.active = true;
            this.editMode = false;
            this.modal.show();

        } else {
            this.reloadLedherTypes(true);
            this._chartofControlsServiceProxy.getChartofControlForEdit(chartofControlId).subscribe(result => {
                this.editMode = true;
                this.chartofControl = result.chartofControl;
                console.log(result);

                if (this.chartofControl.creationDate) {
                    this.creationDate = this.chartofControl.creationDate.toDate();
                }
                if (this.chartofControl.auditTime) {
                    this.auditTime = this.chartofControl.auditTime.toDate();
                }


                this.controlDetailId = result.chartofControl.controlDetailId;
                this.subControlDetailId = result.chartofControl.subControlDetailId;
                this.segmentlevel3ID = result.chartofControl.segmentlevel3Id
                this.controlDetailSegmentName = result.controlDetailSegmentName;
                this.subControlDetailSegmentName = result.subControlDetailSegmentName;
                this.segmentlevel3SegmentName = result.segmentlevel3SegmentName;
                

                if (this.chartofControl.controlDetailId != null && this.chartofControl.controlDetailId != '') {
                    this._chartofControlsServiceProxy.getListAccountCode(this.chartofControl.controlDetailId)
                        .subscribe(result => {

                            this.accode = result.items;
                        });
                } else {
                    this.chartofControl.groupCode = null;
                    this.accode = null;
                }

                this.getHeaderList(this.chartofControl.accountType);
                this.getBSHeaderList(this.chartofControl.accountBSType);
                this.getCFHeaderList(this.chartofControl.accountCFType);


                this.flag = update;
                this.active = true;
                this.modal.show();
            });
        }
    }

    init(): void {
        this._chartofControlsServiceProxy.getSegmentName().subscribe(res => {
            this.segment1 = res.items[0].glOption.seg1Name;
            this.segment2 = res.items[0].glOption.seg2Name;
            this.segment3 = res.items[0].glOption.seg3Name;
        });

    }
    resetCFVals()
    {
        debugger;
       this.chartofControl.accountCFHeader=null;
       this.chartofControl.accountCFType=null;
       this.chartofControl.sortCFOrder=null;
    }
   
    getCFHeaderList(value): void {
       debugger
        this._chartofControlsServiceProxy.getCFCategoryList(value).subscribe(res => {
            this.plCFCategoryList = res["result"]["items"];
        })
    }
    setCFSortOrder(e): void {
        debugger;
        var selectedTxt = e.target.options[e.target.options.selectedIndex].text;
        this.chartofControl.sortCFOrder = this.plCFCategoryList.find(
            (x) => x.displayText == selectedTxt
        ).sortOrder;
    }
    save(): void {
        this.saving = true;
        this.chartofControl.flag = this.flag;
        this.chartofControl.segmantID1 = this.ControlDID;
        if (this.creationDate) {
            if (!this.chartofControl.creationDate) {
                this.chartofControl.creationDate = moment(this.creationDate).startOf('day');
            }
            else {
                this.chartofControl.creationDate = moment(this.creationDate);
            }
        }
        else {
            this.chartofControl.creationDate = null;
        }
        if (this.auditTime) {
            if (!this.chartofControl.auditTime) {
                this.chartofControl.auditTime = moment(this.auditTime).startOf('day');
            }
            else {
                this.chartofControl.auditTime = moment(this.auditTime);
            }
        }
        else {
            this.chartofControl.auditTime = null;
        }

        this.chartofControl.controlDetailId = this.controlDetailId;
        this.chartofControl.subControlDetailId = this.subControlDetailId;
        this.chartofControl.segmentlevel3Id = this.segmentlevel3ID;

        this._chartofControlsServiceProxy.createOrEdit(this.chartofControl)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                //this.notify.info(this.l('SavedSuccessfully'));
                if (this.editMode != true) {
                    this.message.confirm("Press 'Yes' for create new Account", this.l('SavedSuccessfully'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.getMaxAcccountID(this.segmentlevel3ID);
                            this.chartofControl.inactive = false;
                            this.chartofControl.subLedger = false;
                            this.chartofControl.accountName = "";
                            this.chartofControl.slType = null;
                            this.chartofControl.oldCode = "";
                            this.modalSave.emit(null);
                        } else {
                            this.close();
                            this.modalSave.emit(null);
                        }
                    });
                }
                else {
                    this.close();
                    this.modalSave.emit(null);
                }
            });
    }

    getNewFinanceModal() {
        switch (this.target) {
            case "Level1":
                this.getNewControlDetailId();
                break;
            case "Level2":
                this.getNewSubControlDetailId();
                break;
            case "Level3":
                this.getNewSegmentlevel3Id();
                break;

            default:
                break;
        }
    }

    openSelectControlDetailModal() {
        this.target = "Level1";
        this.FinanceLookupTableModal.id = this.controlDetailId;
        this.FinanceLookupTableModal.displayName = this.controlDetailSegmentName;
        this.FinanceLookupTableModal.show(this.target);
    }

    openSelectSubControlDetailModal() {
        this.target = "Level2";
        this.FinanceLookupTableModal.id = this.subControlDetailId;
        this.FinanceLookupTableModal.displayName = this.subControlDetailSegmentName;
        this.FinanceLookupTableModal.show(this.target, this.controlDetailId);
    }

    openSelectSegmentlevel3Modal() {
        this.target = "Level3";
        this.FinanceLookupTableModal.id = this.segmentlevel3ID;
        this.FinanceLookupTableModal.displayName = this.segmentlevel3SegmentName;
        this.FinanceLookupTableModal.show(this.target, this.subControlDetailId);
    }

    setControlDetailIdNull() {
        this.controlDetailId = '';
        this.controlDetailSegmentName = '';
        this.chartofControl.id = '';
        this.subControlDetailId = '';
        this.subControlDetailSegmentName = '';
        this.segmentlevel3ID = '';
        this.segmentlevel3SegmentName = '';
        this.chartofControl.groupCode = null;
        this.accode = null;

    }

    setSubControlDetailIdNull() {
        this.subControlDetailId = '';
        this.subControlDetailSegmentName = '';
        this.segmentlevel3ID = '';
        this.segmentlevel3SegmentName = '';
        this.chartofControl.id = '';
    }

    setSegmentlevel3IdNull() {
        this.segmentlevel3ID = '';
        this.segmentlevel3SegmentName = '';
        this.chartofControl.id = '';

    }

    getNewControlDetailId() {
        this.controlDetailId = this.FinanceLookupTableModal.id;
        this.controlDetailSegmentName = this.FinanceLookupTableModal.displayName;

        if (this.controlDetailId != null && this.controlDetailId != '') {
            this._chartofControlsServiceProxy.getListAccountCode(this.controlDetailId)
                .subscribe(result => {
                    this.accode = result.items;
                    this.chartofControl.groupCode = result.items[0].groupId;
                });
        } else {
            this.chartofControl.groupCode = null;
            this.accode = null;
        }

        this.setSubControlDetailIdNull();

    }

    getNewSubControlDetailId() {
        this.subControlDetailId = this.FinanceLookupTableModal.id;
        this.subControlDetailSegmentName = this.FinanceLookupTableModal.displayName;

        this.setSegmentlevel3IdNull();
    }

    getNewSegmentlevel3Id() {
        this.segmentlevel3ID = this.FinanceLookupTableModal.id;
        this.segmentlevel3SegmentName = this.FinanceLookupTableModal.displayName;

        // Assign Chart of Account id
        this.getMaxAcccountID(this.segmentlevel3ID);
    }

    getMaxAcccountID(seg3Id) {
        this._chartofControlsServiceProxy.getMaxAcccountID(seg3Id).subscribe(result => {
            this.chartofControl.id = result;
        });
    }

    close(): void {

        this.flag = false;
        this.active = false;
        this.modal.hide();
    }


    accountHeader: string;
    accountType: string;

    onTypeChange(value): void {
        this.getHeaderList(value);
        // this._PLCategoriesServiceProxy.getCategoryList(value).subscribe(res => {
        //     ;
        //     this.plCategoryList = res.items;
        // })
       // console.log(value);
    }

    onBSTypeChange(value): void {
        this.getBSHeaderList(value);
        // this._PLCategoriesServiceProxy.getCategoryList(value).subscribe(res => {
        //     ;
        //     this.plCategoryList = res.items;
        // })
        //console.log(value);
    }


    getHeaderList(value): void {
        this._PLCategoriesServiceProxy.getCategoryList(value).subscribe(res => {
            this.plCategoryList = res.items;
        })
    }

    getBSHeaderList(value): void {
       
        this._chartofControlsServiceProxy.getBSCategoryList(value).subscribe(res => {
            this.plBSCategoryList = res["result"]["items"];
        })
    }


    SelectedType: boolean;

    getLedgerTypes() {
        this._LegderTypeComboboxService.getLedgerTypesForCombobox('').subscribe(res => {
            this.ledgerTypes = res.items
            for (let index = 0; index < 1; index++) {
                this.SelectedType = this.ledgerTypes[index].isSelected = true;

            }

        })
    }

    ResetBsSetting(){
        debugger
        this.chartofControl.accountBSType=null;
        this.chartofControl.accountBSHeader=null;
        this.chartofControl.sortBSOrder=null;
       
    }
    reloadLedherTypes(params) {
        if (params) {
            this.getLedgerTypes();
        }
        else {
            this.ledgerTypes = null;
            this.chartofControl.slType = null;
        }
    }

    setSortOrder(e): void {
        debugger;
        var selectedTxt = e.target.options[e.target.options.selectedIndex].text;
        this.chartofControl.sortOrder = this.plCategoryList.find(
            (x) => x.displayText == selectedTxt
        ).sortOrder;
    }

    setBSSortOrder(e): void {
        debugger;
        var selectedTxt = e.target.options[e.target.options.selectedIndex].text;
        this.chartofControl.sortBSOrder = this.plBSCategoryList.find(
            (x) => x.displayText == selectedTxt
        ).sortOrder;
    }

    resetBSVals()
    {
        debugger;
       this.chartofControl.accountBSHeader=null;
       this.chartofControl.accountBSType=null;
       this.chartofControl.sortBSOrder=null;
    }

    resetPLVals()
    {
        this.chartofControl.accountHeader=null;
        this.chartofControl.accountType=null;
        this.chartofControl.sortOrder=null;
    }
    UpdateAccount(value): void {
        debugger
        this.chartofControl.acctype=value;
        this.chartofControl.controlDetailId = this.controlDetailId;
        this.chartofControl.subControlDetailId = this.subControlDetailId;
        this._chartofControlsServiceProxy
            .updateAccount(this.chartofControl)
            .subscribe((res) => {
                //this.plBSCategoryList = res["result"]["items"];
                this.message.info("Account Updated.");                
            });
    }

}
