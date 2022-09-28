import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    DebugElement
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";

import { RegionsDto } from "../shared/dto/regions-dto";
import { RegionService } from "../shared/services/regions.service";

@Component({
    selector: "regionsModal",
    templateUrl: "./create-or-edit-regions-modal.component.html"
})
export class CreateOrEditRegionsModalComponent extends AppComponentBase {
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    edit: boolean = false;
    active = false;
    saving = false;
    validForm: boolean = false;
    regionsDto: RegionsDto = new RegionsDto();
    locationList;
    type: string = "";
    constructor(injector: Injector, private _regionsService: RegionService) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        this.regionsDto = new RegionsDto();
        this._regionsService.GetParentLocationList().subscribe(data => {
            debugger;
            this.locationList = data["result"];
        });
        if (!id) {
            this.edit = false;
            this.regionsDto.id = undefined;
            this.regionsDto.locationTitle = "";
        } else {
            this.edit = true;
            debugger;
            this._regionsService.getDataForEdit(id).subscribe(data => {
                this.regionsDto.id = data["result"]["iceLocation"]["id"];
                this.regionsDto.parentId =
                    data["result"]["iceLocation"]["parentId"];
                this.regionsDto.locationTitle =
                    data["result"]["iceLocation"]["locationTitle"];
            });
        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._regionsService.create(this.regionsDto).subscribe(() => {
            this.saving = false;
            this.notify.info(this.l("SavedSuccessfully"));
            this.close();
            this.modalSave.emit(null);
        });
        this.close();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    // openlookUpModal(type: string) {
    //     this.type = type;
    //     this.CostCenterLookupTableModal.data = null;
    //     if (this.type != "Account") {
    //         this.CostCenterLookupTableModal.accId = this.costCenterDto.accountID;
    //         if (this.CostCenterLookupTableModal.accId != "") {
    //             this.CostCenterLookupTableModal.show(type);
    //         }
    //     } else {
    //         this.CostCenterLookupTableModal.show(type);
    //     }
    // }

    // getLookUpData() {
    //     if (this.type == "Account") {
    //         if (this.CostCenterLookupTableModal.data != null) {
    //             this.costCenterDto.accountID = this.CostCenterLookupTableModal.data.id;
    //             this.costCenterDto.accountName = this.CostCenterLookupTableModal.data.accountName;
    //         }
    //         this.costCenterDto.subAccId = 0;
    //         this.costCenterDto.subAccName = "";
    //     } else {
    //         this.costCenterDto.subAccId = this.CostCenterLookupTableModal.data.id;
    //         this.costCenterDto.subAccName = this.CostCenterLookupTableModal.data.subAccName;
    //     }
    // }
}
