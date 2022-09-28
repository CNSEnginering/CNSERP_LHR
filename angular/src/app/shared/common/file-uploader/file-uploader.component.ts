import { Component, Injector } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DemoUiComponentsServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.css'],
  animations: [appModuleAnimation()]
})
export class FileUploaderComponent extends AppComponentBase {


  uploadUrl: string;
  uploadedFiles: any[] = [];

  constructor(
    injector: Injector,
    private demoUiComponentsService: DemoUiComponentsServiceProxy
  ) {
    super(injector);
    this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/DemoUiComponents/UploadFiles';
  }

  // upload completed event
  onUpload(event): void {
    for (const file of event.files) {
      this.uploadedFiles.push(file);
    }
  }

  onBeforeSend(event): void {
    event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
  }

}