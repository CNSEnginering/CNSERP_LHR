import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditLocationDto } from '../shared/dto/location-dto';
import { LocationServiceProxy } from '../shared/services/location-service';

@Component({
    selector: 'createOrEditLocationModal',
    templateUrl: './create-or-edit-location-modal.component.html'
})
export class CreateOrEditLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    location: CreateOrEditLocationDto = new CreateOrEditLocationDto();

    createDate: Date;
    audtDate: Date;



    constructor(
        injector: Injector,
        private _locationServiceProxy: LocationServiceProxy
    ) {
        super(injector);
    }

    show(locationId?: number): void {

        debugger;
        if (!locationId) {
            this.location = new CreateOrEditLocationDto();
            this.location.id = locationId;
            this.location.active = true;

            this._locationServiceProxy.getMaxLocationId().subscribe(result => {
                this.location.locID = result;
            });

            this.active = true;
            this.modal.show();
        } else {
            debugger;
            this._locationServiceProxy.getLocationForEdit(locationId).subscribe(result => {
                debugger;
                this.location = result.location;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        debugger;


        this.location.audtDate = moment();
        this.location.audtUser = this.appSession.user.userName;

        if (!this.location.id) {
            this.location.createDate = moment();
            this.location.createdBy = this.appSession.user.userName;
        }

        this._locationServiceProxy.createOrEdit(this.location)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                debugger;
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
