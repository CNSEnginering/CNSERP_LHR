import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { CompanyProfilesServiceProxy, CreateOrEditCompanyProfileDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { AbpSessionService } from 'abp-ng2-module/dist/src/session/abp-session.service';
import { AppConsts } from '@shared/AppConsts';


@Component({
    selector: 'createOrEditCompanyProfileModal',
    templateUrl: './create-or-edit-companyProfile-modal.component.html',
    styleUrls: ['./create-or-edit-companyProfile-modal.component.css']
})
export class CreateOrEditCompanyProfileModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    IsEdit = false;

    companyProfile: CreateOrEditCompanyProfileDto = new CreateOrEditCompanyProfileDto();

    fiscalStartDate: Date;
    fiscalEndDate: Date;
    locDate: Date;
    appDate: Date;


    constructor(
        injector: Injector,
        private _companyProfilesServiceProxy: CompanyProfilesServiceProxy
    ) {
        super(injector);
    }

    show(companyProfileId?: string): void {
        this.fiscalStartDate = null;
        this.fiscalEndDate = null;
        this.locDate = null;
        this.appDate = null;
        if (!companyProfileId) {
            this.companyProfile = new CreateOrEditCompanyProfileDto();
            this.companyProfile.id = companyProfileId;
            this.IsEdit = false;
            this.active = true;
            this.modal.show();
        } else {
            this._companyProfilesServiceProxy.getCompanyProfileForEdit(companyProfileId).subscribe(result => {
                this.companyProfile = result.companyProfile;

                this._companyProfilesServiceProxy.getImage(this.companyProfileAppId, 1).subscribe(imageResult => {
                    debugger;
                    if (imageResult != null) {
                        this.profileUploadUrl = 'data:image/jpg;base64,' + imageResult;
                    }
                });

                this._companyProfilesServiceProxy.getImage(this.companyProfileAppId, 1).subscribe(fileResult => {
                    debugger;
                    if (fileResult != null) {
                        this.url = 'data:image/jpg;base64,' + fileResult;
                        const album = {
                            src: this.url
                        };
                        this.image.push(album);
                    }
                });

                this.IsEdit = true;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
         debugger;
        this._companyProfilesServiceProxy.createOrEdit(this.companyProfile)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }


    myStyle(): object {
        return {"height": "32px !important"}
      }


    close(): void {

        this.active = false;
        this.modal.hide();
    }

    //===========================Profile Picture=============================

    profileUploadUrl: string = "";
    readonly companyProfileAppId = 3;
    readonly appName = "Company";
    url: string;
    image: any[] = [];

    onBeforeProfileUpload(): void {
        debugger;
        this.profileUploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles?';
        if (this.companyProfileAppId !== undefined)
            this.profileUploadUrl += "APPID=" + encodeURIComponent("" + this.companyProfileAppId) + "&";
        if (this.appName !== undefined)
            this.profileUploadUrl += "AppName=" + encodeURIComponent("" + this.appName) + "&";
        if (this.companyProfile.id !== undefined)
            this.profileUploadUrl += "DocID=" + encodeURIComponent("" + 1) + "&";
        this.profileUploadUrl = this.profileUploadUrl.replace(/[?&]$/, "");
    }
    onProfileUpload(): void {
        debugger;
        this._companyProfilesServiceProxy.getImage(this.companyProfileAppId, 1).subscribe(imageResult => {
            debugger;
            if (imageResult != null) {
                this.profileUploadUrl = 'data:image/jpg;base64,' + imageResult;
            }
        });

    }
}
