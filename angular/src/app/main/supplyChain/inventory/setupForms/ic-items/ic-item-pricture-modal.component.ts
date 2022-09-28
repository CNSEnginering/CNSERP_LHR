import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { ProfileServiceProxy, UpdateProfilePictureInput } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'icItemPrictureMmodal',
  templateUrl: './ic-item-pricture-modal.component.html'
})
export class IcItemPrictureModalComponent extends AppComponentBase {

  @ViewChild('changeProfilePictureModal', {static: true}) modal: ModalDirective;

    public active = false;
    public uploader: FileUploader;
    public temporaryPictureUrl: string;
    public saving = false;

    private maxProfilPictureBytesUserFriendlyValue = 5;
    private temporaryPictureFileName: string;
    private _uploaderOptions: FileUploaderOptions = {};

    imageChangedEvent: any = '';

    constructor(
        injector: Injector,
        private _profileService: ProfileServiceProxy,
        private _tokenService: TokenService
    ) {
        super(injector);
    }

    initializeModal(): void {
        this.active = true;
        this.temporaryPictureUrl = '';
        this.temporaryPictureFileName = '';
        this.initFileUploader();
    }

    show(): void {
      debugger;
        this.initializeModal();
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.imageChangedEvent = '';
        this.uploader.clearQueue();
        this.modal.hide();
    }

    fileChangeEvent(event: any): void {
        if (event.target.files[0].size > 5242880) { //5MB
            this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
            return;
        }

        this.imageChangedEvent = event;
    }

    imageCroppedFile(file: File) {
        let files: File[] = [file];
        this.uploader.clearQueue();
        this.uploader.addToQueue(files);
    }

    initFileUploader(): void {
      debugger
        this.uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + '/Profile/UploadProfilePicture' });
        this._uploaderOptions.autoUpload = false;
        this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        this._uploaderOptions.removeAfterUpload = true;
        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

         this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
             form.append('FileType', fileItem.file.type);
             form.append('FileName', 'ProfilePicture');
             form.append('FileToken', this.guid());
         };

         this.uploader.onSuccessItem = (item, response, status) => {
             const resp = <IAjaxResponse>JSON.parse(response);
             if (resp.success) {
                 this.updateProfilePicture(resp.result.fileToken);
             } else {
                 this.message.error(resp.error.message);
             }
         };

        // this.uploader.setOptions(this._uploaderOptions);
    }

    updateProfilePicture(fileToken: string): void {
      debugger
        const input = new UpdateProfilePictureInput();
        input.fileToken = fileToken;
        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;

        this.saving = true;
        this._profileService.updateProfilePicture(input)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
             //   abp.event.trigger('profilePictureChanged');
                this.close();
            });
    }

    guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    // save(): void {
    //     this.uploader.uploadAll();
    // }

}
