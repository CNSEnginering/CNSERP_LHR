import { Component, OnInit, Injector, ViewChild, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PriceListService } from '../shared/services/priceList.service';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { Paginator } from 'primeng/components/paginator/paginator';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { CreateOrEditGetPassModalComponent } from './create-or-edit-gatePass-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { GatePassService } from '../shared/services/gatePass.service';
import { ViewGatePassComponent } from './view-gatePass-modal.component';
import { GetDataService } from '@app/main/supplyChain/inventory/shared/services/get-data.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
import { GatePassHeaderDto } from '../shared/dto/gatePassHeader-dto';
@Component({
  templateUrl: './gatePass.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class GatePassComponent extends AppComponentBase implements OnInit {
  filterText = '';
  priceList: any;
  sorting: any;
  skipCount: any;
  MaxResultCount: any;
  fromDoc:number;
  toDoc:number;

  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('GetPassModal', { static: true }) GetPassModal: CreateOrEditGetPassModalComponent;
  @ViewChild('viewGatePassModal', { static: true }) viewGatePassModal: ViewGatePassComponent;
  @ViewChild("reportviewrModalComponent", { static: false }) reportView: ReportviewrModalComponent;
  constructor(injector: Injector,
    private _gatePassService: GatePassService,
    private _fileDownloadService: FileDownloadService,
    private _getDataService: GetDataService
  ) {
    super(injector);
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);

    this.primengTableHelper.showLoadingIndicator();
    if (this.permission.isGranted('Inventory.InwardGatePasses.View')
      && this.permission.isGranted('Inventory.OutwardGatePasses.View')
    ) {
      this._gatePassService.getAll(
        this.filterText,
        this.sorting,
        this.skipCount,
        this.MaxResultCount
      ).subscribe(data => {
        this.priceList = data["result"]["items"]
        this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
        this.primengTableHelper.records = this.priceList;
        this.primengTableHelper.hideLoadingIndicator();
      });
    }
    else if (this.permission.isGranted('Inventory.InwardGatePasses.View')) {
      this._gatePassService.getAllInward(
        this.filterText,
        this.sorting,
        this.skipCount,
        this.MaxResultCount
      ).subscribe(data => {
        this.priceList = data["result"]["items"]
        this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
        this.primengTableHelper.records = this.priceList;
        this.primengTableHelper.hideLoadingIndicator();
      });
    }
    else if (this.permission.isGranted('Inventory.OutwardGatePasses.View')) {
      this._gatePassService.getAllOutward(
        this.filterText,
        this.sorting,
        this.skipCount,
        this.MaxResultCount
      ).subscribe(data => {
        this.priceList = data["result"]["items"]
        this.primengTableHelper.totalRecordsCount = data["result"]["totalCount"];
        this.primengTableHelper.records = this.priceList;
        this.primengTableHelper.hideLoadingIndicator();
      });
    }
  }
  delete(id: number) {
    this.message.confirm(
      '',
      this.l('AreYouSure'),
      (isConfirmed) => {
        if (isConfirmed) {
          this._gatePassService.delete(id).subscribe(() => {
            this.reloadPage()
            this.notify.success(this.l('SuccessfullyDeleted'))
          });
        }
      }
    );
  }
  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }
  createOrEdit(id: number) {
    this.GetPassModal.show(id, "");
  }

  createOrEditGatePass(type: string) {
    this.GetSetUpDetail();
    this.GetPassModal.show(null, type);
  }
  GetSetUpDetail(): void {
    this._getDataService.GetSetUpDetail().subscribe(result => {
      debugger; 
      if(result!=null){
         // this.=result;
         
      }
        this.GetPassModal.SetDefaultRecord(result);
    });
  }

  view(data: any) {
    this.viewGatePassModal.show(data);
  }
  exportToExcel() {
    this._gatePassService.GetDataToExcel(
      this.filterText,
      this.sorting,
      this.skipCount,
      this.MaxResultCount
    ).subscribe((result: any) => {
      this._fileDownloadService.downloadTempFile(result["result"]);
    });
  }

  // getReport(gatedoc:any, gatetype:any) {
  //   debugger
  //   let rptParams = "";

  //   rptParams += encodeURIComponent("" + gatepass.docNo) + "$";
  //   rptParams += encodeURIComponent("" + gatepass.docNo) + "$";
  //   rptParams = rptParams.replace(/[?$]&/, "");
  //   if(gatepass.typeId=1){
  //   this.reportView.show("InwardGatePass", rptParams);
  //   }else{
  //     this.reportView.show("OutwardGatePass", rptParams);
  //   }
  // }

  getReport(gatepass:any) {
    debugger
    let rptParams = "";

    rptParams += encodeURIComponent("" + gatepass.docNo) + "$";
    rptParams += encodeURIComponent("" + gatepass.docNo) + "$";
    rptParams = rptParams.replace(/[?$]&/, "");
    if(gatepass.typeID==1){
    this.reportView.show("InwardGatePass", rptParams);
    }else{
      this.reportView.show("OutwardGatePass", rptParams);
    }
  }

}
