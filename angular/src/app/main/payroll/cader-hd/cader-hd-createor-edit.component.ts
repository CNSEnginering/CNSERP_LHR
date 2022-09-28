import {
  Component,
  ViewChild,
  Injector,
  Output,
  EventEmitter,
  OnInit,
  ElementRef,
  AfterViewInit
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";

import { Cader_link_DDto } from "../shared/dto/CaderD-dto";
import { CaderHead } from "../shared/dto/CaderH-dto";
import { CaderHDService } from "../shared/services/CaderHD.service";
import { throwIfEmpty } from "rxjs/operators";

import { AppConsts } from "@shared/AppConsts";
import { caderDTo } from "../shared/dto/cader-dto";
import { PayRollLookupTableModalComponent } from '@app/finders/payRoll/payRoll-lookup-table-modal.component';
import { isLength } from "lodash";
@Component({
  selector: 'app-cader-hd-createor-edit',
  templateUrl: './cader-hd-createor-edit.component.html'
})
export class CreateOrEditCaderHDComponent  extends AppComponentBase
implements OnInit{

  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  @ViewChild('PayRollLookupTableModal', { static: true }) PayRollLookupTableModal: PayRollLookupTableModalComponent;
  // @ViewChild('ItemPricingLookupTableModal', { static: true }) ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
  // @ViewChild('CostCenterLookupTableModal', { static: true }) CostCenterLookupTableModal: CostCenterLookupTableModalComponent;
  // @ViewChild('InventoryGlLinkLookupTableModal', { static: true }) InventoryGlLinkLookupTableModal: InventoryGlLinkLookupTableModalComponent;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  totalItems: number;
  editMode: boolean = false;
  totalQty: number;
  active = false;
  saving = false;
  priceListChk: boolean = false;
  Caders: CaderHead;
  CaderDetailData: Cader_link_DDto[] = new Array<Cader_link_DDto>();
  tabMode: any;
  gridApi;
  CaderList:any;
  PayTypeList:any;
  gridColumnApi;
  rowData;
  rowSelection;
  checkedval:boolean;
  paramsData;
  
  PFW;
  Cader1;
  CaderName:string;
  AccID;
  AccName:string;
  desc;
  TypeDC1;

  target:any;

  LocCheckVal:boolean;
  paramsData2;
  fgIndex: number = 0;
  rmIndex: number = 0;
  bpIndex: number = 0;
  type: string;
  editState: Boolean = false;
  editState1: Boolean = false;
  editState2: Boolean = false;
  appId = 11;
  appName = "AssemblyEntry";
  uploadedFiles = [];
  checkImage = true;
  image = [];
  url: string;
  uploadUrl: string;
  //private _lightbox: Lightbox;
  columnDefs = [
      {
          headerName: this.l("SrNo"),
          editable: false,
          field: "srNo",
          sortable: true,
          width: 100,
          valueGetter: "node.rowIndex+1"
      },
      {
          headerName: this.l("Group Of Department ID"),
          editable: false,
          field: "caderID",
          sortable: true,
          filter: true,
          width: 200,
          hide: true,
          resizable: true
      },
      {
        headerName: this.l("Group Of Department Name"),
        editable: false,
        field: "caderName",
        sortable: true,
        filter: true,
        width: 200,
        resizable: true
    },
      {
          headerName: this.l("Chart Of Account"),
          editable: false,
          field: "accountID",
          sortable: true,
          filter: true,
          width: 220,
          hide:true,
          resizable: true
      },
      {
        headerName: this.l("Chart Of Account"),
        editable: false,
        field: "accountName",
        sortable: true,
        filter: true,
        width: 220,
        resizable: true
    },
      {
          headerName: this.l("Type"),
          editable: false,
          field: "type",
          sortable: true,
          filter: true,
          width: 150,
          resizable: true
      },
      {
          headerName: this.l("Pay For Work"),
          editable: false,
          field: "payType",
          sortable: true,
          filter: true,
          width: 200,
          resizable: true
      },
      {
          headerName: this.l("Narration"),
          editable: true,
          field: "narration",
          sortable: true,
          filter: true,
          width: 200,
          resizable: true
      }
  ];
  formValid: boolean = false;
  constructor(
      injector: Injector,
      private _caderService: CaderHDService
  ) {
      super(injector);
      // this.gatePassDetailData.length = 0;
      // this.gatePassDetailDataTemp.length = 0;
  }

  ngOnInit(): void {
      this.rowData = [];
    
      this.tabMode = 7;
  }
  ngAfterViewInit() { }

   GetCaderList(){
    this._caderService.getList().subscribe(result => {
        this.CaderList = result["result"];
    });
   }
   GetPayTypeList(){
    this._caderService.getPayTypeList().subscribe(result => {
        this.PayTypeList = result["result"];
    });
   }
CaderChange($event){
debugger
this.Cader1 = $event.target.options[$event.target.options.selectedIndex].value;
this.CaderName=$event.target.options[$event.target.options.selectedIndex].text;
}
DrCr(ids){
this.TypeDC1=ids;
}
payChange(ids){
this.PFW=ids;
}
openCaderModal() {
    this.target = "ChartOfAccount";
    this.PayRollLookupTableModal.id = String(this.AccID);
    this.PayRollLookupTableModal.displayName = this.desc;
    this.PayRollLookupTableModal.show(this.target);
}
setCaderNull() {
    this.AccID = "";
    this.desc = "";
}
getNewCaderModal() {
    this.AccID = this.PayRollLookupTableModal.id;
     this.desc = this.PayRollLookupTableModal.displayName;
}

show(id?: number): void {
      this.active = true;
      this.Caders = new CaderHead();
      this.CaderDetailData = new Array<Cader_link_DDto>();
      this.GetCaderList();
      this.GetPayTypeList();
      //this.gatePassDetail = new GatePassDetailDto();
      //this.gatePassDetailData = new Array<GatePassDetailDto>();
      this.editMode = false;
      this.totalQty = 0;
      this.totalItems = 0;
      this.tabMode = 7;
      this.formValid = false;

      this.url = null;
      this.image = [];
      this.uploadedFiles = [];
      this.uploadUrl = null;
      this.checkImage = true;

      if (!id) {
        //   this._assemblyService.GetDocId().subscribe(res => {
        //       this.assembly.docNo = res["result"];
        //       this.assembly.docDate = new Date();
        //   });
      } else {
          this.editMode = true;
          this._caderService.getDataForEdit(id).subscribe((data: any) => {
            this.formValid = true;
            this.Caders.id = id;
            data["result"]["cader_link_H"]["caderDetailDto"].forEach(element => {
                debugger
                this.gridApi.updateRowData({ add: [element] });
            });
            
            this.checkFormValid();
          });
      }
      this.modal.show();
  }
  save() {
    this.message.confirm("Save", isConfirmed => {
        if (isConfirmed) {
            debugger
            this.saving = true;
          

            this.gridApi.forEachNode(node=>
            {
                    this.Caders.caderDetail.push(node.data);
            });
           
            this._caderService.create(this.Caders).subscribe(() => {
                this.saving = false;
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
            this.close();
        }
    });
}
  close(): void {
      this.active = false;
      this.modal.hide();
  }
  SetDefaultRecord(result:any){
      console.log(result);
       
        this.checkedval=result.cDateOnly;
        if(result.allowLocID==false){
            this.LocCheckVal=false;
        }else{
          this.LocCheckVal=true;
        }
        //this.typeDesc=result.transTypeName;
    }
  getIndex(transType) {
      if (transType == 7) return ++this.fgIndex;
      else if (transType == 9) return ++this.rmIndex;
      else if (transType == 8) return ++this.bpIndex;
  }

  addRecordToGrid() {
    debugger
    if (this.AccID !=undefined && this.Cader1 !=undefined && this.TypeDC1!=undefined && this.PFW!=undefined) {
      if (this.checkICader() != true) {
              let length = this.CaderDetailData.length;
              var newData = {
                  srNo: ++length,
                  caderID: this.Cader1,
                  caderName: this.CaderName,
                  accountID: this.AccID,
                  accountName:this.desc,
                  type: this.TypeDC1,
                  payType: this.PFW,
                  narration:''
              };
              this.gridApi.updateRowData({ add: [newData] });
      } else {
          this.notify.info("Already Exist!!");
      }
      this.checkFormValid();
    }else{
        this.notify.info("Plz Select ");
    }
  }

  checkICader() {
      var checkItem = false;
      debugger
      if (this.AccID !=undefined && this.Cader1!=undefined && this.TypeDC1!=undefined && this.PFW!=undefined) {
        this.gridApi.forEachNode(node=>
            {
                if(node.data.caderID==this.Cader1 && node.data.accountID == this.AccID && node.data.payType == this.PFW){
                    checkItem=true;
                }
            });
      }
      return checkItem;
  }
  removeRecordFromGrid() {
      debugger
    var selectedData;
    selectedData = this.gridApi.getSelectedRows();
    this.gridApi.updateRowData({ remove: selectedData });
    this.gridApi.refreshCells();
    this.editState = false;
    this.checkFormValid();
}
checkFormValid() {
  var length = 0;
  debugger
  this.gridApi.forEachNode(node=>
    {
        debugger
        if(node.data != null)
        {
            length++;
        }
    });
  if (length > 0) {
        this.formValid = true;
  } else {
    this.formValid = false;
  }
}
  enterDate() {
      this.formValid = false;
  }
  leaveDate() {
      this.checkFormValid();
  }
 

  

  onGridReady(params) {
      this.rowData = [];
      this.gridApi = params.api;
      this.gridColumnApi = params.columnApi;
      params.api.sizeColumnsToFit();
      this.rowSelection = "multiple";
  }

 


  onRowDoubleClicked(params) {
      this.type = "item";
      this.paramsData = params;
  }

  

  
  setTabMode(event) {
      if (event.srcElement.innerText == "Finished Goods") {
          this.tabMode = 7;
      } else if (event.srcElement.innerText == "Raw Materials") {
          this.tabMode = 9;
      } else if (event.srcElement.innerText == "Buy Products") {
          this.tabMode = 8;
      }
  }


  cellValueChanged() {
 
    this.gridApi.refreshCells();
    this.editState = false;
    this.checkFormValid();
  }
 


  cellEditingStarted(params) {
      this.formValid = false;
  }



}

