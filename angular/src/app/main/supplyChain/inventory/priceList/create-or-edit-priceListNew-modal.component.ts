import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { InventoryGlLinkDto } from '../shared/dto/inventory-glLink-dto';
import { InventoryGlLinkService } from '../shared/services/inventory-gl-link.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { PriceListDto } from '../shared/dto/priceList-dto';
import { PriceListService } from '../shared/services/priceList.service';



@Component({
    selector: 'PriceListNewModal',
    templateUrl: './create-or-edit-priceListNew-modal.component.html'
})
export class CreateOrEditPriceListNewModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    priceListChk: boolean = false;
    priceList: PriceListDto = new PriceListDto();



    constructor(
        injector: Injector,
        private _priceListervice: PriceListService
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        if (!id) {
            this.priceList.priceList = '';
            this.priceList.priceListName = '';
            this.priceList.active = true;
        }
        else {
            this._priceListervice.getDataForEdit(id).subscribe(
                data => {
                    this.priceList.priceList = data["result"]["priceList"]["priceList"];
                    this.priceList.priceListName = data["result"]["priceList"]["priceListName"];
                    this.priceList.id = data["result"]["priceList"]["id"];
                    this.priceList.active = data["result"]["priceList"]["active"];
                }
            );

        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._priceListervice.create(this.priceList)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }


    close(): void {
        this.active = false;
        this.modal.hide();
    }

    priceListCheckIfExists() {
        this._priceListervice.priceListCheckIfExists(this.priceList.priceList).subscribe(
            data => {
                if (data["result"] == true) {
                    this.notify.info(this.l('Price List Already Exists'));
                    this.priceListChk = true;
                }
                else {
                    this.priceListChk = false;
                }
            }
        );
    }
}
