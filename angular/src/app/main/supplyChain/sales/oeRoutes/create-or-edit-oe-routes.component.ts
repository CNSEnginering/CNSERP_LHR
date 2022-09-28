import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';


import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { oeroutesdto } from '../shared/dtos/oeRoutes-dto';
import { OeroutesService } from '../shared/services/oeroutes.service';

@Component({
  selector: 'app-create-or-edit-oe-routes',
  templateUrl: './create-or-edit-oe-routes.component.html',
})
export class CreateOrEditOeRoutesComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    OeroutesDto: oeroutesdto = new oeroutesdto();

    active = false;
    saving = false;
    editMode = false;
    routID:number;
    routDesc:string;


    constructor(
        injector: Injector,
        private _OeroutesService: OeroutesService
    ) {
        super(injector);
    }

    createDate: Date;
    audtDate: Date;

    show(id?: any, maxId?: number): void {
        debugger;

        if (!id) {

            this.OeroutesDto = new oeroutesdto();
            this.OeroutesDto.routID = maxId;
            this.OeroutesDto.routDesc = "";
            this.OeroutesDto.active = true;
            
            this.active = true;
            this.modal.show();
          } else {
            debugger;
      
            this._OeroutesService.getDataForEdit(id).subscribe(result => {
              debugger;
      
              this.OeroutesDto = result["oeRoutes"];     
      
      
              this.active = true;
              this.modal.show();
            });
      
          };
        
    }

    save() {
        this.saving = true;
        debugger;
        //this.OeroutesDto.audtDate = new Date;
        //this.OeroutesDto.audtUser = this.appSession.user.userName;
        //if (this.editMode == true) {
          //this.OeroutesDto.createDate = new Date;
          //this.OeroutesDto.createdBy = this.appSession.user.userName;
       // }
        //debugger
        this._OeroutesService.create(this.OeroutesDto)
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
