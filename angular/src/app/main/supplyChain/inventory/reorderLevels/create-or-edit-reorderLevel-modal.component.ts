import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { ReorderLevelDto } from '../shared/dto/reorder-levels-dto';
import { ReorderLevelsService } from '../shared/services/reorder-levels.service';
import { GetDataService } from '../shared/services/get-data.service';
import { ItemPricingLookupTableModalComponent } from '../FinderModals/itemPricing-lookup-table-modal.component';

@Component({
    selector: 'createOrEditReorderLevelModal',
    templateUrl: './create-or-edit-reorderLevel-modal.component.html'
})
export class CreateOrEditReorderLevelModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('ItemPricingLookupTableModal', { static: true }) ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    reorderLevel: ReorderLevelDto = new ReorderLevelDto();

    createDate: Date;
    audtDate: Date;
    locations:any;
    itemName:string;


    constructor(
        injector: Injector,
        private _reorderLevelsService: ReorderLevelsService,
        private _getDataService: GetDataService
    ) {
        super(injector);
    }

    show(reorderLevelId?: number): void {
    this.createDate = null;
    this.audtDate = null;

    this.getLocations("ICLocations");

        if (!reorderLevelId) {
            this.reorderLevel = new ReorderLevelDto();
            this.reorderLevel.id = reorderLevelId;
            this.reorderLevel.locId=0;
            this.reorderLevel.minLevel=0;
            this.reorderLevel.maxLevel=99999;
            this.reorderLevel.ordLevel=0;
            this.itemName="";
            this.active = true;
            this.modal.show();
        } else {
            this._reorderLevelsService.getReorderLevelForEdit(reorderLevelId).subscribe(result => {
                this.reorderLevel = result;

                if (this.reorderLevel.createDate) {
					this.createDate = this.reorderLevel.createDate.toDate();
                }
                if (this.reorderLevel.audtDate) {
					this.audtDate = this.reorderLevel.audtDate.toDate();
                }
                this.itemName=result.itemName;
                this.active = true;
                this.modal.show();
            });
        }
    }

    getLocations(target:string):void{
        debugger;
        this._getDataService.getList(target).subscribe(result => {
            this.locations = result;
        }); 
    }

    //=====================Item Model================
    openSelectItemModal() {
        debugger;
        this.ItemPricingLookupTableModal.show('Item');
    }


    setItemIdNull() {
        this.reorderLevel.itemId = null;
        this.itemName='';
    }


    getNewItemId() {
        debugger;
        this.reorderLevel.itemId = this.ItemPricingLookupTableModal.data.itemId;
        this.itemName=this.ItemPricingLookupTableModal.data.descp;
    }
    //================Item Model===============
 
    save(): void {

        if(this.reorderLevel.locId==null || this.reorderLevel.locId==0){
            this.notify.error(this.l('Please select location'));
            return;
        }

        this.saving = true;

			
        if (this.createDate) {
            if (!this.reorderLevel.createDate) {
                this.reorderLevel.createDate = moment(this.createDate).startOf('day');
            }
            else {
                this.reorderLevel.createDate = moment(this.createDate);
            }
        }
        else {
            this.reorderLevel.createDate = null;
        }
        if (this.audtDate) {
            if (!this.reorderLevel.audtDate) {
                this.reorderLevel.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.reorderLevel.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.reorderLevel.audtDate = null;
        }

        this.reorderLevel.audtDate=moment();
        this.reorderLevel.audtUser = this.appSession.user.userName;

        if(!this.reorderLevel.id){
            this.reorderLevel.createDate=moment();
            this.reorderLevel.createdBy = this.appSession.user.userName;
        }
            this._reorderLevelsService.createOrEdit(this.reorderLevel)
             .pipe(finalize(() => { this.saving = false;}))
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
