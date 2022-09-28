import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    OnInit,
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import {
    SubControlDetailsServiceProxy,
    CreateOrEditSubControlDetailDto,
    GLOptionsServiceProxy,
    ChartofControlsServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { SubControlDetailControlDetailLookupTableModalComponent } from "./subControlDetail-controlDetail-lookup-table-modal.component";
import { ResourceLoader } from "@angular/compiler";
import { FinanceLookupTableModalComponent } from "@app/finders/finance/finance-lookup-table-modal.component";
import {
    IPLCategoryComboboxItemDto,
    PLCategoriesServiceProxy,
} from "@app/main/finance/shared/services/plCategories.service";

@Component({
    selector: "createOrEditSubControlDetailModal",
    templateUrl: "./create-or-edit-subControlDetail-modal.component.html",
})
export class CreateOrEditSubControlDetailModalComponent
    extends AppComponentBase
    implements OnInit {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    //@ViewChild('subControlDetailControlDetailLookupTableModal', { static: true }) subControlDetailControlDetailLookupTableModal: SubControlDetailControlDetailLookupTableModalComponent;
    @ViewChild("FinanceLookupTableModal", { static: true })
    FinanceLookupTableModal: FinanceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    IsUpdate = false;
    flag = false;

    subControlDetail: CreateOrEditSubControlDetailDto = new CreateOrEditSubControlDetailDto();
    plCategoryList: IPLCategoryComboboxItemDto[];
    plBSCategoryList: IPLCategoryComboboxItemDto[];
    plCFCategoryList: IPLCategoryComboboxItemDto[];
    controlDetail = "";
    ControlDID = "";
    target: any;
    plSortOrder: number;
    bsSortOrder: number;

    constructor(
        injector: Injector,
        private _subControlDetailsServiceProxy: SubControlDetailsServiceProxy,
        private _gLOptionsServiceProxy: GLOptionsServiceProxy,
        private _chartofControlsServiceProxy: ChartofControlsServiceProxy,
        private _PLCategoriesServiceProxy: PLCategoriesServiceProxy
    ) {
        super(injector);
    }

    defaultclaccFilter = "";
    stockctrlaccFilter = "";
    seg1NameFilter = "";
    seg2NameFilter = "";
    seg3NameFilter = "";
    directPostFilter = -1;
    autoSeg3Filter = -1;
    maxAUDTDATEFilter: moment.Moment;
    minAUDTDATEFilter: moment.Moment;
    audtuserFilter = "";
    chartofControlIdFilter = "";

    ngOnInit(): void {
        debugger;
        this.GetGloptionList();
    }

    GloptionSetup: string;

    Gloptionlevel1: string;

    segment1des = "";

    GetGloptionList() {
        this._gLOptionsServiceProxy
            .getAll(
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
            )
            .subscribe((res) => {
                debugger;
                this.GloptionSetup = res.items[0].glOption.seg2Name;
                this.Gloptionlevel1 = res.items[0].glOption.seg1Name;
            });
    }
    setCFSortOrder(e): void {
        debugger;
        var selectedTxt = e.target.options[e.target.options.selectedIndex].text;
        this.subControlDetail.sortCFOrder = this.plCFCategoryList.find(
            (x) => x.displayText == selectedTxt
        ).sortOrder;
    }
    resetCFVals()
    {
        debugger;
       this.subControlDetail.accountCFHeader=null;
       this.subControlDetail.accountCFType=null;
       this.subControlDetail.sortCFOrder=null;
    }
    show(Update: boolean = false, subControlDetailId?: number): void {
        if (!Update) {
            this.subControlDetail = new CreateOrEditSubControlDetailDto();
            this.subControlDetail.id = subControlDetailId;
            this.controlDetail = "";
            this.ControlDID = "";
            this.IsUpdate = Update;
            this.active = true;
            this.modal.show();
        } else {
            this._subControlDetailsServiceProxy
                .getSubControlDetailForEdit(subControlDetailId)
                .subscribe((result) => {
                    this.subControlDetail = result.subControlDetail;
                    this.controlDetail = result.controlDetailDesc;
                    this.ControlDID = result.controlDetailId;
                    this.IsUpdate = Update;
                    this.active = true;

                    this.getHeaderList(this.subControlDetail.accountType);
                    this.getBSHeaderList(this.subControlDetail.accountBSType);
                    this.getCFHeaderList(this.subControlDetail.accountCFType);
                    this.flag = Update;
                    this.modal.show();
                });
        }
    }

    save(): void {
        debugger;
        this.saving = true;
        this.subControlDetail.flag = this.flag;
        this.subControlDetail.segmantID1 = this.ControlDID;
        this._subControlDetailsServiceProxy
            .createOrEdit(this.subControlDetail)

            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.message.confirm(
                    "Press 'Yes' for create new control",
                    this.l("SavedSuccessfully"),
                    (isConfirmed) => {
                        if (isConfirmed) {
                            this.getNewControlDetailId();
                            this.subControlDetail.segmentName = "";
                        } else {
                            this.close();
                            this.modalSave.emit(null);
                        }
                    }
                );
            });
    }
    getNewFinanceModal() {
        debugger;
        switch (this.target) {
            case "Level1":
                this.getNewControlDetailId();
                break;

            default:
                break;
        }
    }
    getCFHeaderList(value): void {
       
        this._chartofControlsServiceProxy.getCFCategoryList(value).subscribe(res => {
            this.plCFCategoryList = res["result"]["items"];
        })
    }

    openSelectControlDetailModal() {
        debugger;
        this.target = "Level1";
        this.FinanceLookupTableModal.id = this.ControlDID;
        this.FinanceLookupTableModal.displayName = this.controlDetail;
        this.FinanceLookupTableModal.show(this.target);
    }

    setControlDetailIdNull() {
        debugger;
        this.ControlDID = null;
        this.controlDetail = "";
        this.subControlDetail.seg2ID = null;
    }

    getNewControlDetailId() {
        debugger;
        this.ControlDID = this.FinanceLookupTableModal.id;
        this.controlDetail = this.FinanceLookupTableModal.displayName;

        if (this.ControlDID !== "" && this.ControlDID != null) {
            this.subControlDetail.seg2ID = null;
            this._subControlDetailsServiceProxy
                .getSubControlID(this.ControlDID)
                .subscribe((result) => {
                    this.subControlDetail.seg2ID = result;
                });
        } else {
            this.subControlDetail.seg2ID = null;
        }
    }
    close(): void {
        this.plCategoryList = [];
        this.plBSCategoryList = [];
        this.active = false;
        this.modal.hide();
    }

    getHeaderList(value): void {
        this._PLCategoriesServiceProxy
            .getCategoryList(value)
            .subscribe((res) => {
                this.plCategoryList = res.items;                
            });
    }

    setSortOrder(e): void {
        debugger;
        this.subControlDetail.sortOrder = this.plCategoryList.find(
            (x) => x.id == e
        ).sortOrder;
    }

    setBSSortOrder(e): void {
        debugger;
       
        this.subControlDetail.sortBSOrder = this.plBSCategoryList.find(
            (x) => x.id == e
        ).sortOrder;
    }
    getBSHeaderList(value): void {
        this._chartofControlsServiceProxy
            .getBSCategoryList(value)
            .subscribe((res) => {
                this.plBSCategoryList = res["result"]["items"];                
            });
    }

    resetBSVals() {
        debugger;
        this.subControlDetail.accountBSHeader = null;
        this.subControlDetail.accountBSType = null;
        this.subControlDetail.sortBSOrder = null;
    }

    resetPLVals() {
        this.subControlDetail.accountHeader = null;
        this.subControlDetail.accountType = null;
        this.subControlDetail.sortOrder = null;
    }
    UpdateAccount(value): void {
        debugger
        this.subControlDetail.acctype=value;
        this.subControlDetail.segmantID1 = this.ControlDID;
        this._chartofControlsServiceProxy
            .updateAccountList(this.subControlDetail)
            .subscribe((res) => {
                //this.plBSCategoryList = res["result"]["items"];
                this.message.info("Account Updated.");                
            });
    }
}
