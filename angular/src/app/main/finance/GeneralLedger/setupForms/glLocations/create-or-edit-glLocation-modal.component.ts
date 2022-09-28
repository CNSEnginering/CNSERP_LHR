import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { GLLocationsServiceProxy, CreateOrEditGLLocationDto, CityServiceProxy, GetCityForViewDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditGLLocationModal',
    templateUrl: './create-or-edit-glLocation-modal.component.html'
})
export class CreateOrEditGLLocationModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    locMaxId = 0;

    cities: GetCityForViewDto[];
    prefixAppend: boolean = false;

    glLocation: CreateOrEditGLLocationDto = new CreateOrEditGLLocationDto();

    auditDate: Date;


    constructor(
        injector: Injector,
        private _glLocationsServiceProxy: GLLocationsServiceProxy,
        private _cityServiceProxy: CityServiceProxy
    ) {
        super(injector);
    }

    show(glLocationId?: number, maxId?: number): void {
        debugger;
        this._cityServiceProxy.getAll().subscribe(result => {
            this.cities = result.items;
        });

        this.auditDate = null;

        if (!glLocationId) {
            this.glLocation = new CreateOrEditGLLocationDto();
            this.glLocation.id = glLocationId;
            this.glLocation.cityID = 0;
            this.locMaxId = maxId;

            this.active = true;
            this.modal.show();
        } else {
            this._glLocationsServiceProxy.getGLLocationForEdit(glLocationId).subscribe(result => {
                this.glLocation = result.glLocation;

                if (this.glLocation.auditDate) {
                    this.auditDate = this.glLocation.auditDate.toDate();
                }

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;


        if (this.auditDate) {
            if (!this.glLocation.auditDate) {
                this.glLocation.auditDate = moment(this.auditDate).startOf('day');
            }
            else {
                this.glLocation.auditDate = moment(this.auditDate);
            }
        }
        else {
            this.glLocation.auditDate = null;
        }

        this.glLocation.auditDate = moment();
        this.glLocation.auditUser = this.appSession.user.userName;
        this._glLocationsServiceProxy.createOrEdit(this.glLocation)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.message.confirm("Press 'Yes' for create new location", this.l('SavedSuccessfully'), (isConfirmed) => {
                    if (isConfirmed) {
                        this._glLocationsServiceProxy.getMaxLocId().subscribe(result => {
                            if (result != 0) {
                                this.show(null, result);
                                this.modalSave.emit(null);
                            }
                            else {
                                this.show();
                                this.modalSave.emit(null);
                            }
                        });
                    }
                    else {
                        this.close();
                        this.modalSave.emit(null);
                    }
                });
            });
    }



    updatePrefix(cityID): void {

        for (let city of this.cities) {
            if (city.city.cityID == cityID) {
                this.glLocation.preFix = city.city.preFix;
                this.prefixAppend = false;
                this.glLocation.locDesc = "";
                break;
            }
        }

    }



    updateDesc(val): void {
        debugger;
        if (val == "") {
            this.prefixAppend = false;
            return;
        }

        if (this.glLocation.preFix == null) {
            this.prefixAppend = true;
            return;
        }

        if (!this.prefixAppend) {
            this.glLocation.locDesc = this.glLocation.preFix + "-" + val;
            this.prefixAppend = true;
        }
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
