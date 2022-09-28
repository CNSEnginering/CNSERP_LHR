import { Component, ViewChild, Injector, Output, EventEmitter  } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GLSLGroupsDto } from '@app/main/finance/shared/dto/glslgroup-dto';
import { GLSlGroupServiceProxy } from '@app/main/finance/shared/services/glslgroup.service';

@Component({
  selector: 'app-create-or-editglslgroup',
  templateUrl: './create-or-editglslgroup.component.html'
})
export class CreateOrEditglslgroupComponent  extends AppComponentBase {

 
  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  
  active = false;
  saving = false;

  GlDto: GLSLGroupsDto = new GLSLGroupsDto();

  audtDate: Date;
  createDate: Date;
  target: any;
  constructor(
      injector: Injector,
      private _glGroupServiceProxy: GLSlGroupServiceProxy
  ) {
      super(injector);
  }

  close(): void {
    this.active = false;
    this.modal.hide();
}

show(id?: any): void {
  debugger
  
  this.audtDate = null;
  this.createDate = null;
  
  if (!id) {
      this.GlDto = new GLSLGroupsDto();
      this._glGroupServiceProxy.getMaxDocId().subscribe(res => {
           this.GlDto.sLGrpID=res["result"];
      });
      this.active = true;
      this.modal.show();
  } else {
    debugger
      this._glGroupServiceProxy.getDataForEdit(id).subscribe(result => {
        debugger
          //this.GlDto=result["result"]["glslGroups"];
          this.GlDto.id=result["result"]["glslGroups"]["id"];
          this.GlDto.sLGrpID=result["result"]["glslGroups"]["slGrpID"];
          this.GlDto.sLGRPDESC=result["result"]["glslGroups"]["slgrpdesc"];
          this.GlDto.active=result["result"]["glslGroups"]["active"];
         //console.log(this.GlDto);
         ///this.GlDto.sLGRPDESC=result["glslGroups"]["slgrpdesc"];
          this.active=true;
          this.modal.show();
      });
  }
}


save(): void {
  this.saving = true;

  this._glGroupServiceProxy.create(this.GlDto)
      .pipe(finalize(() => { this.saving = false; }))
      .subscribe(() => {
        debugger
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
}


}
