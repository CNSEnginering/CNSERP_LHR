import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { CommonServiceFindersService } from '@app/finders/shared/services/commonServicesFinders.service';
import { VoucherprintDto } from '../dto/voucherprint-dto';

@Component({
  selector: 'app-alert-log',
  templateUrl: './alert-log.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class AlertLogComponent extends AppComponentBase{

  @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;
  @ViewChild("CommonServiceLookupTableModal", { static: true })CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
  voucherDto : VoucherprintDto = new VoucherprintDto();
   target : string;
   result : string;
   formName : string;
   constructor(
    injector: Injector,
    private _commonAppServices : CommonServiceFindersService
  ) { 
    super(injector)
  }

  ngOnInit(){
    debugger
    this.getFormName("GetFormIDList")
  } 

  getFormName(target: string){
    this._commonAppServices.getAllForm(target).subscribe(result => {
      debugger
      this.formName = result;
  });
  }


  getReport() {
    debugger
    let repParams = "";
    repParams += this.voucherDto.formName + "$";
    repParams = repParams.replace(/[?$]&/, "");
    this.reportView.show("DeleteLog", repParams);
  }
}
