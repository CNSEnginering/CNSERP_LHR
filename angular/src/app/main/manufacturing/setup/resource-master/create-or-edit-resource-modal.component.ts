import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { MFRESMASDto } from '../../shared/dto/resource.dto';
import { ResourceServiceProxy } from '../../shared/service/resource.service';


@Component({
    selector: 'resourceModal',
    templateUrl: './create-or-edit-resource-modal.component.html'
})
export class CreateOrEditResourceMasterModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
   

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    transCheck: boolean = false;
    dto: MFRESMASDto = new MFRESMASDto();
   
    editMode: boolean = false;


    constructor(
        injector: Injector,
        private _resourceService: ResourceServiceProxy,
    ) {
        super(injector);
    }

    show(id?: number): void {
        this.active = true;
        this.transCheck = false;
        if (!id) {
            this.editMode = false;

            this.dto = new MFRESMASDto();
        }
        else {
            this.editMode = true;

            this.primengTableHelper.showLoadingIndicator();
            this._resourceService.getDataForEdit(id).subscribe(data => {
                console.log(data);
                debugger;
                this.dto = data.mfresmas
            });
        }
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this._resourceService.create(this.dto)
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

    setunit(e)
    {
        debugger
        this.dto.unit = e.target.options[e.target.options.selectedIndex].text
    }
}

