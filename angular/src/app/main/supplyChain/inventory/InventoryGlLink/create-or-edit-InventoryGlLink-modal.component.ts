import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';
import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { InventoryGlLinkLookupTableModalComponent } from '../FinderModals/InventoryGlLink-lookup-table-modal.component';
import { IcSegment1ServiceProxy } from '../shared/services/ic-segment1-service';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { InventoryLookupTableModalComponent } from '@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component';
import { debug } from 'console';


@Component({
    selector: 'InventoryGlLinkModal',
    templateUrl: './create-or-edit-InventoryGlLink-modal.component.html'
})
export class CreateOrEditInventoryGlLinkModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
    @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;
    @ViewChild('InventoryLookupTableModal', { static: true }) InventoryLookupTableModal: InventoryLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    segmentCheck: boolean = false;
    inventoryGlLink: InventoryGlLinkDto = new InventoryGlLinkDto();
    target: any;


    constructor(
        injector: Injector,
        private _inventoryGlLinkservice: InventoryGlLinkService,
        private _IcSegment1ServiceProxy: IcSegment1ServiceProxy,
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        this.segmentCheck = false;
        if (!id) {
            this.inventoryGlLink = new InventoryGlLinkDto();
        }
        else {
            this.primengTableHelper.showLoadingIndicator();
            this._inventoryGlLinkservice.GetInventoryGlLinkForEdit(id).subscribe(data => {
                this.inventoryGlLink.id = data["result"]["inventoryGlLink"]["id"];
                this.inventoryGlLink.locId = data["result"]["inventoryGlLink"]["locID"];
                this.inventoryGlLink.locDesc = data["result"]["inventoryGlLink"]["locDesc"];
                this.inventoryGlLink.segId = data["result"]["inventoryGlLink"]["segID"];
                this.inventoryGlLink.segDesc = data["result"]["inventoryGlLink"]["segDesc"];
                this.inventoryGlLink.accRec = data["result"]["inventoryGlLink"]["accRec"];
                this.inventoryGlLink.accRecDesc = data["result"]["inventoryGlLink"]["accRecDesc"];
                this.inventoryGlLink.accRet = data["result"]["inventoryGlLink"]["accRet"];
                this.inventoryGlLink.accRetDesc = data["result"]["inventoryGlLink"]["accRetDesc"];
                this.inventoryGlLink.accAdj = data["result"]["inventoryGlLink"]["accAdj"];
                this.inventoryGlLink.accAdjDesc = data["result"]["inventoryGlLink"]["accAdjDesc"];
                this.inventoryGlLink.accCgs = data["result"]["inventoryGlLink"]["accCGS"];
                this.inventoryGlLink.accCgsDesc = data["result"]["inventoryGlLink"]["accCGSDesc"];
                this.inventoryGlLink.accWip = data["result"]["inventoryGlLink"]["accWIP"];
                this.inventoryGlLink.accWipDesc =  data["result"]["inventoryGlLink"]["accWIPDesc"];
                this.inventoryGlLink.glLocId = data["result"]["inventoryGlLink"]["glLocID"];
                this.inventoryGlLink.glLocDesc = data["result"]["inventoryGlLink"]["glLocDesc"];
                console.log( data["result"]);
            });
        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._inventoryGlLinkservice.create(this.inventoryGlLink)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }

    getNewFinanceModal() {
        this.getNewChartOfAC(this.target);
    }
    getNewInventoryModal() {
        switch (this.target) {
            case "Location":
                this.getNewLocation();
                break;
            case "Segment1":
            case "Segment2":
            case "Segment3":
                this.getNewSegment1();
                break;

            default:
                break;
        }
    }
    //////////////////////////////////////Chart of Account///////////////////////////////////////////////
    openSelectChartofACModal(ac) {
        debugger;
        this.target = "ChartOfAccount";
        if (ac == "accRec") {
            this.FinanceLookupTableModal.id = this.inventoryGlLink.accRec;
            this.FinanceLookupTableModal.displayName = this.inventoryGlLink.accRecDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "accRet") {
            this.FinanceLookupTableModal.id = this.inventoryGlLink.accRet;
            this.FinanceLookupTableModal.displayName = this.inventoryGlLink.accRetDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "accAdj") {
            this.FinanceLookupTableModal.id = this.inventoryGlLink.accAdj;
            this.FinanceLookupTableModal.displayName = this.inventoryGlLink.accAdjDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "accCgs") {
            this.FinanceLookupTableModal.id = this.inventoryGlLink.accCgs;
            this.FinanceLookupTableModal.displayName = this.inventoryGlLink.accCgsDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        else if (ac == "accWip") {
            this.FinanceLookupTableModal.id = this.inventoryGlLink.accWip;
            this.FinanceLookupTableModal.displayName = this.inventoryGlLink.accWipDesc;
            this.FinanceLookupTableModal.show(this.target);
        }
        this.target = ac;
    }

    openGlLocation() {
        debugger;
        this.target = "GLLocation"
        this.FinanceLookupTableModal.id = String(this.inventoryGlLink.glLocId);
        this.FinanceLookupTableModal.displayName = this.inventoryGlLink.glLocDesc;
        this.FinanceLookupTableModal.show(this.target);

    }

    getNewChartOfAC(ac) {
        debugger;
        if (ac == "accRec") {
            this.inventoryGlLink.accRec = this.FinanceLookupTableModal.id;
            this.inventoryGlLink.accRecDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "accRet") {
            this.inventoryGlLink.accRet = this.FinanceLookupTableModal.id;
            this.inventoryGlLink.accRetDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "accAdj") {
            this.inventoryGlLink.accAdj = this.FinanceLookupTableModal.id;
            this.inventoryGlLink.accAdjDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "accCgs") {
            this.inventoryGlLink.accCgs = this.FinanceLookupTableModal.id;
            this.inventoryGlLink.accCgsDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "accWip") {
            this.inventoryGlLink.accWip = this.FinanceLookupTableModal.id;
            this.inventoryGlLink.accWipDesc = this.FinanceLookupTableModal.displayName;
        }
        else if (ac == "GLLocation") {
            this.inventoryGlLink.glLocId = Number(this.FinanceLookupTableModal.id);
            this.inventoryGlLink.glLocDesc = this.FinanceLookupTableModal.displayName;
        }
    }
    //////////////////////////////////////Chart of Account///////////////////////////////////////////////
    //////////////////////////////////////Location///////////////////////////////////////////////
    openLocationModal() {
        this.target = "Location";
        this.InventoryLookupTableModal.id = String(this.inventoryGlLink.locId);
        this.InventoryLookupTableModal.displayName = this.inventoryGlLink.locDesc;
        this.InventoryLookupTableModal.show(this.target);

    }
    getNewLocation() {
        this.inventoryGlLink.locId = Number(this.InventoryLookupTableModal.id);
        this.inventoryGlLink.locDesc = this.InventoryLookupTableModal.displayName;
        this.checkSegmentExists();
    }
    //////////////////////////////////////Location///////////////////////////////////////////////
    //////////////////////////////////////Segment 1///////////////////////////////////////////////
    openSegment1Modal() {
        this._inventoryGlLinkservice.GetGLlinkSeg().subscribe(
            data => {
                if (data["result"] == "1") {
                    this.target = "Segment1";
                }
                else if (data["result"] == "2") {
                    this.target = "Segment2";
                }
                else if (data["result"] == "3") {
                    this.target = "Segment3";
                }
                this.InventoryLookupTableModal.id = this.inventoryGlLink.segId;
                this.InventoryLookupTableModal.displayName = this.inventoryGlLink.segDesc;
                this.InventoryLookupTableModal.show(this.target);
            }
        )


    }
    getNewSegment1() {
        this.inventoryGlLink.segId = this.InventoryLookupTableModal.id;
        this.inventoryGlLink.segDesc = this.InventoryLookupTableModal.displayName;
        this.checkSegmentExists();
    }
    setPickerNull(type){
        switch (type) {
            case "GLLocation":
                this.inventoryGlLink.glLocId = null;
                this.inventoryGlLink.glLocDesc = "";
                break;
            case "Loc":
                this.inventoryGlLink.locId = null;
                this.inventoryGlLink.locDesc = "";
                break;
            case "SEGID":
                this.inventoryGlLink.segId = null;
                this.inventoryGlLink.segDesc = "";
                break;
            case "AccRec":
                this.inventoryGlLink.accRec = null;
                this.inventoryGlLink.accRecDesc = "";
                break;
            case "AccRet":
                this.inventoryGlLink.accRet = null;
                this.inventoryGlLink.accRetDesc = "";
                break;
            case "AccAdj":
                this.inventoryGlLink.accAdj = null;
                this.inventoryGlLink.accAdjDesc = "";
                break;
            case "AccCGS":
                this.inventoryGlLink.accCgs = null;
                this.inventoryGlLink.accCgsDesc = "";
                break;
            case "AccWIP":
                this.inventoryGlLink.accWip = null;
                this.inventoryGlLink.accWipDesc = "";
                break;
            default:
                break;
        }
    }
    //////////////////////////////////////Segment1///////////////////////////////////////////////
    // getLookUpData() {
    //     // if (this.InventoryGlLinkLookupTableModal.type == "accRec") {
    //     //     this.inventoryGlLink.accRec = this.InventoryGlLinkLookupTableModal.data.id;
    //     //     this.inventoryGlLink.accRecDesc = this.InventoryGlLinkLookupTableModal.data.accountName;
    //     // }
    //     // else if (this.InventoryGlLinkLookupTableModal.type == "accRet") {
    //     //     this.inventoryGlLink.accRet = this.InventoryGlLinkLookupTableModal.data.id;
    //     //     this.inventoryGlLink.accRetDesc = this.InventoryGlLinkLookupTableModal.data.accountName;
    //     // }
    //     // else if (this.InventoryGlLinkLookupTableModal.type == "accAdj") {
    //     //     this.inventoryGlLink.accAdj = this.InventoryGlLinkLookupTableModal.data.id;
    //     //     this.inventoryGlLink.accAdjDesc = this.InventoryGlLinkLookupTableModal.data.accountName;
    //     // }
    //     // else if (this.InventoryGlLinkLookupTableModal.type == "accCgs") {
    //     //     this.inventoryGlLink.accCgs = this.InventoryGlLinkLookupTableModal.data.id;
    //     //     this.inventoryGlLink.accCgsDesc = this.InventoryGlLinkLookupTableModal.data.accountName;
    //     // }
    //     // else if (this.InventoryGlLinkLookupTableModal.type == "accWip") {
    //     //     this.inventoryGlLink.accWip = this.InventoryGlLinkLookupTableModal.data.id;
    //     //     this.inventoryGlLink.accWipDesc = this.InventoryGlLinkLookupTableModal.data.accountName;
    //     // }
    //     if (this.InventoryGlLinkLookupTableModal.type == "locId") {
    //         this.inventoryGlLink.locId = this.InventoryGlLinkLookupTableModal.data.locID;
    //         this.inventoryGlLink.locDesc = this.InventoryGlLinkLookupTableModal.data.locName;
    //         this.checkSegmentExists();
    //     }
    //     else if (this.InventoryGlLinkLookupTableModal.type == "segId") {
    //         this.inventoryGlLink.segId = this.InventoryGlLinkLookupTableModal.data.seg1ID;
    //         this.inventoryGlLink.segDesc = this.InventoryGlLinkLookupTableModal.data.seg1Name;
    //         this.checkSegmentExists();
    //     }
    // }



    close(): void {
        this.active = false;
        this.modal.hide();
    }


    checkSegmentExists() {
        if (this.inventoryGlLink.locId > 0 && this.inventoryGlLink.segId != undefined)
            this._inventoryGlLinkservice.GetSegIdAgainstLoc(this.inventoryGlLink.locId, this.inventoryGlLink.segId)
                .subscribe((res: any) => {
                    this.segmentCheck = res.result;
                    if (this.segmentCheck == true)
                        this.notify.info(this.l('Segment Against This Location Already Exists'));
                });
    }
}
