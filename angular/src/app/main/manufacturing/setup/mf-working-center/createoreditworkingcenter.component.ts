import {
  Component,
  ViewChild,
  Injector,
  Output,
  EventEmitter,
  OnInit
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";

import { mfworkingCenterService } from "../../shared/service/mfworkingCenter.service";
import { CreateOrEditMFWCMDto } from "../../shared/dto/mfwcm.dto";
import { CreateOrEditMFWCRESDto } from "../../shared/dto/mfwcres.dto";
import { CreateOrEditMFWCTOLDto } from "../../shared/dto/mfwctol.dto";
// import { CostCenterLookupTableModalComponent } from "../FinderModals/costCenter-lookup-table-modal.component";
 //import { ItemPricingLookupTableModalComponent } from "@app/finders/Manufacturing/resource-lookup-table-modal.component";
import { getUnpackedSettings } from "http2";
//import { Lightbox } from "ngx-lightbox";
import { AppConsts } from "@shared/AppConsts";
import { GLTRHeadersServiceProxy } from "@shared/service-proxies/service-proxies";
import { AgGridExtend } from "@app/shared/common/ag-grid-extend/ag-grid-extend";
import { finalize } from "rxjs/operators";
import { ResourceLookupTableModalComponent } from "@app/finders/manufacturing/resource-lookup-table-modal/resource-lookup-table-modal.component";


@Component({
  selector: 'appcreateoreditworkingcenter',
  templateUrl: './createoreditworkingcenter.component.html'
 
})
export class CreateoreditworkingcenterComponent extends AppComponentBase implements OnInit {

    @ViewChild("ResourceLookupTablemodal", { static: true })
   
    ResourceLookupTablemodal:ResourceLookupTableModalComponent;
  @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
  
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  totalRes: number;
  editMode: boolean = false;
  totalTool: number;
  active = false;
  tabMode:any;
  rsIndex: number = 0;
tlIndex: number = 0;
validfield:boolean=false;
  saving = false;
  processing = false;
  priceListChk: boolean = false;
  MasterWorkingDto: CreateOrEditMFWCMDto;
  ResourceDetailDto: CreateOrEditMFWCRESDto;
  ToolDetailDto:CreateOrEditMFWCTOLDto;
  MasterWorkingService:CreateOrEditMFWCMDto = new CreateOrEditMFWCMDto();
  ResWorkingDetailData: CreateOrEditMFWCRESDto[] = new Array<CreateOrEditMFWCRESDto>();
  ToolWorkingDetailData: CreateOrEditMFWCTOLDto[] = new Array<CreateOrEditMFWCTOLDto>();
  agGridExtend: AgGridExtend = new AgGridExtend();
  approving;
  //For Grid One
  gridApi;
  gridColumnApi;
  LocValCheck:boolean;
  rowData;
  rowSelection;
  paramsData;
  //For Grid Two
    gridApi1;
    gridColumnApi1;
    rowData1;
    rowSelection1;
    paramsData1;

    CheckDate:boolean;
    type: string;
    editState: Boolean = false;
  appId = 10;
  tabcheck:boolean=false;
  appName = "mfWorkingCenter";
  uploadedFiles = [];
  checkImage = true;
  image = [];
  url: string;
  uploadUrl: string;
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
          headerName: this.l("ResourceID"),
          editable: false,
          field: "ResourceID",
          sortable: true,
          filter: true,
          width: 200,
          resizable: true
      },
      {
          headerName: this.l("Description"),
          editable: false,
          field: "description",
          sortable: true,
          filter: true,
          width: 220,
          resizable: true
      },
      {
          headerName: this.l("UOM"),
          editable: false,
          field: "unit",
          sortable: true,
          filter: true,
          width: 150,
          resizable: true
      },
      {
          headerName: this.l("Cost Rate"),
          editable: false,
          field: "Rate",
          sortable: true,
          filter: true,
          width: 200,
          resizable: true
      },
      {
          headerName: this.l("Qty"),
          editable: true,
          field: "qty",
          width: 100,
          resizable: true,
          valueFormatter: this.agGridExtend.formatNumber
      },
      {
          headerName: this.l("Total"),
          editable: false,
          field: "Total",
          sortable: true,
          width: 150,
          resizable: true
      },
     
      {
          headerName: this.l("Comments"),
          editable: true,
          field: "remarks",
          sortable: true,
          width: 200,
          resizable: true
      }
  ];
  columnDefs1 = [
    {
        headerName: this.l("SrNo"),
        editable: false,
        field: "srNo",
        sortable: true,
        width: 100,
        valueGetter: "node.rowIndex+1"
    },
    {
        headerName: this.l("Tool ID"),
        editable: false,
        field: "ToolID",
        sortable: true,
        filter: true,
        width: 200,
        resizable: true
    },
    {
        headerName: this.l("Description"),
        editable: false,
        field: "description",
        sortable: true,
        filter: true,
        width: 220,
        resizable: true
    },
    {
        headerName: this.l("UOM"),
        editable: false,
        field: "unit",
        sortable: true,
        filter: true,
        width: 150,
        resizable: true
    },
    {
        headerName: this.l("Cost Rate"),
        editable: false,
        field: "Rate",
        sortable: true,
        filter: true,
        width: 200,
        resizable: true
    },
    {
        headerName: this.l("Qty"),
        editable: true,
        field: "qty",
        width: 100,
        resizable: true,
        valueFormatter: this.agGridExtend.formatNumber
    },
    {
        headerName: this.l("Total"),
        editable: false,
        field: "Total",
        sortable: true,
        width: 150,
        resizable: true
    },
   
    {
        headerName: this.l("Comments"),
        editable: true,
        field: "remarks",
        sortable: true,
        width: 200,
        resizable: true
    }
];
  formValid: boolean = false;
  constructor(
      injector: Injector,
      private _mfWorkingCenterService: mfworkingCenterService,
     
      private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
     // private _lightbox: Lightbox
  ) {
      super(injector);
      // this.gatePassDetailData.length = 0;
      // this.gatePassDetailDataTemp.length = 0;
  }
 
  show(id?: number, type?: string): void {
      this.active = true;
      this.MasterWorkingDto = new CreateOrEditMFWCMDto();
      this.ResWorkingDetailData = new Array<CreateOrEditMFWCRESDto>();
      this.ToolWorkingDetailData = new Array<CreateOrEditMFWCTOLDto>();
      //this.gatePassDetail = new GatePassDetailDto();
      //this.gatePassDetailData = new Array<GatePassDetailDto>();

      this.editMode = false;
      this.totalTool = 0;
      this.totalRes = 0;

      this.url = null;
      this.image = [];
      this.uploadedFiles = [];
      this.uploadUrl = null;
      this.checkImage = true;

      this.formValid = false;
      if (!id) {
        //   this._mfWorkingCenterService.GetDocId().subscribe(res => {
        //       this.transfer.docNo = res["result"];
        //       this.transfer.docDate = new Date();
        //   });
        this.validfield=false;
      } else {
          this.editMode = true;
          this._mfWorkingCenterService
              .getDataForEdit(id, type)
              .subscribe((data: any) => {
                  debugger
                  this.MasterWorkingDto.id = data["result"]["mfwcm"]["id"];
                  this.MasterWorkingDto.wCID = data["result"]["mfwcm"]["wcid"];
                  this.validfield=true;
                  this.MasterWorkingDto.wCESC = data["result"]["mfwcm"]["wcesc"];
                 
                  this.MasterWorkingDto.tOTRSCCOST =
                      data["result"]["mfwcm"]["totrsccost"];
                  this.MasterWorkingDto.tOTTLCOST =
                      data["result"]["mfwcm"]["tottlcost"];
               
                debugger
                  this.AddRecordFromDBTab7(
                      data["result"]["mfwcm"]["resDetailDto"]
                  );
                
                  this.AddRecordFromDBTab9(
                    data["result"]["mfwcm"]["toolDetailDto"]
                );
                  this.checkFormValid();

                 
              });
      }
      this.modal.show();
  }

  close(): void {
      this.active = false;
      this.modal.hide();
   //   this._lightbox.close();
  }

  CheckvalidDoc(){
    debugger
  var docNO=this.MasterWorkingDto.wCID;
  this._mfWorkingCenterService
  .getDataForValidation(docNO)
  .subscribe((data: any) => {
      debugger
      if(data.result>0){
        this.message.warn('This Document No is Already Exist!', "Document No.");
        return;
      }
     
     
  });
}
AddRecordFromDBTab7(record:any){
    if (record != undefined) {
        record.forEach((val, index) => {
            if (this.editMode == false) {
                var newData = {
                    srNo: index,
                   
                    ResourceID: val.resid,
                    description: val.resdesc,
                    unit: val.uom,
                    Rate: val.unitcost,
                    qty: val.reqqty,
                    remarks: val.remarks,
                    //  maxQty: val.qty
                };
                this.addOrUpdateRecordToDetailData(newData, "record",7);
                this.gridApi.updateRowData({ add: [newData] });
               
            } else {
              
                    index = this.getIndex(7);
                    var newData = {
                        srNo: index,
                        ResourceID: val.resid,
                        description: val.resdesc,
                        unit: val.uom,
                        Rate: val.unitcost,
                        qty: val.reqqty,
                        remarks: val.remarks,
                        // wcid:val.wcid,
                        // detID:val.detID
                    };
                    
                
                
                this.addOrUpdateRecordToDetailData(newData, "record",7);
                this.gridApi.updateRowData({ add: [newData] });
            }
        });
       
       
    }
}

AddRecordFromDBTab9(record:any){
    if (record != undefined) {
        record.forEach((val, index) => {
            if (this.editMode == false) {
                debugger
                
                var newData = {
                    srNo: index,
                    ToolID:val.tooltyid,
                    description: val.tooltydesc,
                    unit: val.uom,
                    Rate: val.unitcost,
                    qty: val.reqqty,
                    remarks: val.remarks,
                    //  maxQty: val.qty
                };
                this.addOrUpdateRecordToDetailData(newData, "record",9);
                this.gridApi1.updateRowData({ add: [newData] });
               
            } else {
                debugger
                    index = this.getIndex(9);
                    var newData = {
                        srNo: index,
                       
                        ToolID:val.tooltyid,
                    description: val.tooltydesc,
                    unit: val.uom,
                    Rate: val.unitcost,
                    qty: val.reqqty,
                    remarks: val.remarks,
                    };
                    
                
                this.addOrUpdateRecordToDetailData(newData, "record",9);
                this.gridApi1.updateRowData({ add: [newData] });
                
            }
        });
       
        this.checkFormValid();
    }
}

  addRecordToGrid(record: any) {
  debugger
        if (this.tabMode == 7) this.editState = true;
        else if (this.tabMode == 9) this.editState = true;

        if (record != undefined) {
            record.forEach((val, index) => {
                if (this.editMode == false) {
                    debugger
                    var str = val.itemId.split("*");
                    var newData = {
                        srNo: index,
                        ResourceID: str[0],
                        description: str[1],
                        unit: str[2],
                        Rate: str[3],
                        qty: val.qty,
                        
                        remarks: val.remarks
                        //  maxQty: val.qty
                    };
                    this.addOrUpdateRecordToDetailData(newData, "record",this.tabMode);
                    if (this.tabMode == 7)
                        this.gridApi.updateRowData({ add: [newData] });
                    else if (this.tabMode == 9) this.gridApi1.updateRowData({ add: [newData] });
                   
                } else {
                    debugger

                   
                    if (this.tabMode = 7) {
                        index = this.getIndex(this.tabMode);
                        var newData = {
                            srNo: index,
                            ResourceID: val.resid,
                            description: val.resdesc,
                            unit: val.uom,
                            Rate: val.unitcost,
                            qty: val.reqqty,
                            remarks: val.remarks,
                            // wcid:val.wcid,
                            // detID:val.detID
                        };
                        this.gridApi.updateRowData({ add: [newData] });
                    } else if (this.tabMode = 9) {
                        index = this.getIndex(this.tabMode);
                        var newData1 = {
                            srNo: index,
                            ToolID: val.resid,
                            description: val.resdesc,
                            unit: val.uom,
                            Rate: val.unitcost,
                            qty: val.reqqty,
                            remarks: val.remarks,
                            // wcid:val.wcid,
                            // detID:val.detID
                        };
                        setTimeout(() => {
                            this.gridApi1.updateRowData({ add: [newData] });
                        }, 2000);
                        // this.gridApi1.updateRowData({ add: [newData] });
                    }
                    
                    this.addOrUpdateRecordToDetailData(newData, "record",this.tabMode);
                }
            });
            if (this.tabMode == 7) this.editState = false;
            else if (this.tabMode == 9) this.editState = false;
           
        } else {
            if(this.tabMode == 7){
                let length = this.ResWorkingDetailData.length;
                var newData = {
                    srNo: ++length,
                    ResourceID: undefined,
                    description: undefined,
                    unit: undefined,
                    Rate: undefined,
                    qty: 0,
                    //  lotNo: undefined,
                    //   bundle: undefined,
                    remarks: undefined
                    //  maxQty: undefined
                };

                this.addOrUpdateRecordToDetailData(newData, "record",this.tabMode);
                if (this.tabMode == 7)
                    this.gridApi.updateRowData({ add: [newData] });
                else if (this.tabMode == 9) this.gridApi1.updateRowData({ add: [newData] });

            }else if(this.tabMode == 9){
                let length = this.ToolWorkingDetailData.length;
                var newData1 = {
                    srNo: ++length,
                    ToolID: undefined,
                    description: undefined,
                    unit: undefined,
                    Rate: undefined,
                    qty: 0,
                    //  lotNo: undefined,
                    //   bundle: undefined,
                    remarks: undefined
                    //  maxQty: undefined
                };
                this.addOrUpdateRecordToDetailData(newData1, "record",this.tabMode);
                if (this.tabMode == 7)
                    this.gridApi.updateRowData({ add: [newData1] });
                else if (this.tabMode == 9) this.gridApi1.updateRowData({ add: [newData1] });
            }
           
           
            
        }
     
    this.checkFormValid();
}

getIndex(transType) {
    if (transType == 7) return ++this.rsIndex;
    else if (transType == 9) return ++this.tlIndex;
    
}

  setTabMode(event) {
     if (event.srcElement.innerText == "Resources") {
         this.tabMode = 7;
     } else if (event.srcElement.innerText == "Tools") {
         this.tabMode = 9;
     } 
}
  addOrUpdateRecordToDetailData(data: any, type: string,Modes:number) {
  debugger
  if(Modes==7){
    if (type == "record") {
        this.ResourceDetailDto = new CreateOrEditMFWCRESDto();
        this.ResourceDetailDto.srNo = data.srNo;
        this.ResourceDetailDto.wCID=this.MasterWorkingDto.wCID;
        this.ResourceDetailDto.rESID = data.ResourceID;
        this.ResourceDetailDto.rESDESC = data.description;
        this.ResourceDetailDto.uOM = data.unit;
        this.ResourceDetailDto.rEQQTY = data.qty;
        this.ResourceDetailDto.uNITCOST = data.Rate;
        this.ResourceDetailDto.tOTALCOST=data.qty*data.Rate;
        data.Total=data.qty*data.Rate;
        this.ResWorkingDetailData.push(this.ResourceDetailDto);
      
   } 
    else {
        var filteredData = this.ResWorkingDetailData.find(
            x => x.srNo == data.srNo
        );
        if (filteredData.srNo != undefined) {
            filteredData.rESID = data.ResourceID;
            filteredData.wCID=this.MasterWorkingDto.wCID;
            filteredData.rESDESC = data.description;
            filteredData.uOM = data.unit;
            filteredData.rEQQTY = data.qty;
            filteredData.uNITCOST = data.Rate; 
            filteredData.rEQQTY = data.qty;
            filteredData.tOTALCOST=data.Rate*data.qty;
            data.Total=data.qty*data.Rate;
            filteredData.uOM = data.unit;
       }
    }

  }else if(Modes==9)
  {
    if (type == "record") {
        this.ToolDetailDto = new CreateOrEditMFWCTOLDto();
        this.ToolDetailDto.srNo = data.srNo;
        this.ToolDetailDto.wCID=this.MasterWorkingDto.wCID;
        this.ToolDetailDto.tOOLTYID = data.ToolID;
        this.ToolDetailDto.tOOLTYDESC = data.description;
        this.ToolDetailDto.uNITCOST = data.Rate;
        this.ToolDetailDto.rEQQTY = data.qty;
        this.ToolDetailDto.uOM=data.unit;
        this.ToolDetailDto.tOTALCOST=data.Rate*data.qty;
        data.Total=data.qty*data.Rate;
        this.ToolWorkingDetailData.push(this.ToolDetailDto);
   } 
    else {
        var filteredData1 = this.ToolWorkingDetailData.find(
            x => x.srNo == data.srNo
        );
        if (filteredData1.srNo != undefined) {
            filteredData1.tOOLTYID = data.ToolID;
            filteredData1.srNo = data.srNo;
            filteredData1.wCID=this.MasterWorkingDto.wCID;
            filteredData1.tOOLTYDESC = data.description;
            filteredData1.uOM=data.unit;
            filteredData1.uNITCOST = data.Rate;
            filteredData1.rEQQTY = data.qty;
            filteredData1.tOTALCOST=data.Rate*data.qty;
            data.Total=data.qty*data.Rate;
           // filteredData1.unit = data.unit;
       }
    }


  }
 
     // this.totalItems = this.transferDetailData.length;
      this.calculateTotalQty();
  }

  ngOnInit(): void {
      this.rowData = [];
      this.tabMode = 7;
  }
  
  onGridReady(params) {
      this.rowData = [];
      this.gridApi = params.api;
      this.gridColumnApi = params.columnApi;
      params.api.sizeColumnsToFit();
      this.rowSelection = "multiple";
  }

  onRowDoubleClicked(params) {

      debugger;
      if (params.colDef.headerName == "ResourceID") {
          this.type = "Resources";
           this.ResourceLookupTablemodal.show(this.type);
          this.ResourceLookupTablemodal.id = "";
          this.ResourceLookupTablemodal.displayName = "";
          // this.inventoryLookupTableModal.show("Items");
          this.paramsData = params;
      }
  }

  onGridReady1(params) {
    this.rowData1 = [];
    this.gridApi1 = params.api;
    this.gridColumnApi1 = params.columnApi;
    params.api.sizeColumnsToFit();
    this.rowSelection1 = "multiple";
    
   
}

onRowDoubleClicked1(params) {

    debugger;
    if (params.colDef.headerName == "Tool ID") {
        this.type = "";
         this.ResourceLookupTablemodal.show(this.type);
        this.ResourceLookupTablemodal.id = "";
        this.ResourceLookupTablemodal.displayName = "";
        this.ResourceLookupTablemodal.unitCost="";
        this.ResourceLookupTablemodal.uom="";
        this.paramsData = params;
    }
}

  SetDefaultRecord(result:any){
      
        this.CheckDate=result.cDateOnly;
      //  this.transfer.fromLocId=result.currentLocID;
       // this.transfer.fromLocDesc=result.currentLocName;
      //  this.transfer.toLocId=result.currentLocID;
      //  this.transfer.toLocDesc=result.currentLocName;
        if(result.allowLocID==true){
          this.LocValCheck=true;
        }else{
          this.LocValCheck=false;
        }
        
    }
    
  getLookUpData() {
      debugger
      if (this.tabMode == 7) {
          this.paramsData.data.ResourceID = this.ResourceLookupTablemodal.id;
          this.paramsData.data.description = this.ResourceLookupTablemodal.displayName;
          this.paramsData.data.Rate = this.ResourceLookupTablemodal.unitCost;
          this.paramsData.data.unit = this.ResourceLookupTablemodal.uom;
          this.paramsData.data.Total =parseFloat(this.ResourceLookupTablemodal.unitCost)*0;
         /// this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
          // this.paramsData.data.item = this.ItemPricingLookupTableModal.data.itemId;
          // this.paramsData.data.description = this.ItemPricingLookupTableModal.data.descp;
          // this.paramsData.data.conver = this.ItemPricingLookupTableModal.data.conver;
       
             this.checkEditState();
             this.checkFormValid();
             
      }else if(this.tabMode ==9){
        this.paramsData.data.ToolID = this.ResourceLookupTablemodal.id;
        this.paramsData.data.description = this.ResourceLookupTablemodal.displayName;
        this.paramsData.data.Rate = this.ResourceLookupTablemodal.unitCost;
        this.paramsData.data.unit = this.ResourceLookupTablemodal.uom;
        this.paramsData.data.Total =parseFloat(this.ResourceLookupTablemodal.unitCost)*0;
      }
    //   else if (this.type == "CostCenter")
    //       this.getNewCostCenter();
    this.gridApi1.refreshCells();
      this.gridApi.refreshCells();
      if (this.paramsData != undefined) {
          this.addOrUpdateRecordToDetailData(this.paramsData.data, "",this.tabMode);
          this.checkEditState();
          this.checkFormValid();
      }

  }
  checkEditState() {
      if (
          this.paramsData.data.item != "" &&
          this.paramsData.data.qty > 0 &&
          this.paramsData.data.qty <= this.paramsData.data.maxQty
      ) {
          this.editState = false;
      } else {
          this.editState = true;
      }
  }
  cellValueChanged(params) {
      debugger
      this.addOrUpdateRecordToDetailData(params.data, "",this.tabMode);
      this.calculateTotalQty();
      this.checkEditState();
      if (params.data.qty <= 0) {
          this.notify.info("Qty Should Be Greater Than Zero");
      }
      this.checkFormValid();
  }

  cellEditingStarted(params) {
   
     this.formValid = false;
  }
  cellValueChanged1(params) {
    debugger
    this.addOrUpdateRecordToDetailData(params.data, "",this.tabMode);
    this.calculateTotalQty();
    this.checkEditState();
    if (params.data.qty <= 0) {
        this.notify.info("Qty Should Be Greater Than Zero");
    }
    this.checkFormValid();
}

cellEditingStarted1(params) {
    debugger
    this.formValid = false;
}

 


  save() {
      debugger
     
      this.saving = true;
                this.MasterWorkingDto.resDetailDto.push(
                    ...this.ResWorkingDetailData.slice()
                );
                this.MasterWorkingDto.toolDetailDto.push(
                    ...this.ToolWorkingDetailData.slice()
                );
                debugger
                this._mfWorkingCenterService.create(this.MasterWorkingDto).subscribe(() => {
                    this.saving = false;
                    this.notify.info(this.l("SavedSuccessfully"));
                    this.close();
                    this.modalSave.emit(null);
                });
                this.close();
    
  }

  calculateTotalQty() {
      debugger
    this.totalTool = 0;let qty1=0;
    this.ToolWorkingDetailData.forEach((val, index) => {
      qty1 = qty1 + Number(val.rEQQTY);
      if (!isNaN(qty1)) this.MasterWorkingDto.tOTTLCOST = qty1;
  });
  this.ToolWorkingDetailData.length == 0 ? (this.totalTool = 0) : "";
    
  let qty = 0;
      this.totalRes = 0;
      this.ResWorkingDetailData.forEach((val, index) => {
          qty = qty + Number(val.rEQQTY);
          if (!isNaN(qty)) this.MasterWorkingDto.tOTRSCCOST = qty;
      });
      this.ResWorkingDetailData.length == 0 ? (this.totalRes = 0) : "";
     
      this.checkFormValid();
  }

  removeRecordFromGrid() {
    var selectedData;
    if (this.tabMode == 7) selectedData = this.gridApi.getSelectedRows();
    else if (this.tabMode == 9) selectedData = this.gridApi1.getSelectedRows();
    

   
    if (this.tabMode == 7) {
        var filteredDataIndex = this.ResWorkingDetailData.findIndex(
            x => x.srNo == selectedData[0].srNo 
        );
        this.ResWorkingDetailData.splice(filteredDataIndex, 1);

        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
    } else if (this.tabMode == 9) {
        var filteredDataIndex = this.ToolWorkingDetailData.findIndex(
            x => x.srNo == selectedData[0].srNo 
        );
        this.ToolWorkingDetailData.splice(filteredDataIndex, 1);
        this.gridApi1.updateRowData({ remove: selectedData });
        this.gridApi1.refreshCells();
    }
    //this.totalItems = this.ResWorkingDetailData.length;
    this.calculateTotalQty();
    this.editState = false;
    this.checkFormValid();
  }

  dateChange(event: any) {
      // this.transfer.docDate = event;
      // var currDate = new Date();
      // if (this.transfer.docDate > currDate) {
      //     this.notify.info("You cannot enter the date after today");
      // }
      this.checkFormValid();
  }

  checkFormValid() {
   
    this.ResWorkingDetailData.forEach((val, index) => {
       if(val.rEQQTY>0 && val.rEQQTY !=undefined){
        this.formValid = true;
       }else{
        this.formValid = false;
       }
    });
    this.ToolWorkingDetailData.forEach((val, index) => {
        if(val.rEQQTY>0 && val.rEQQTY !=undefined){
            this.formValid = true;
           }else{
            this.formValid = false;
           }
    });

      // if (
      //     // this.transfer.docDate == null ||
      //     // this.transfer.docDate > new Date() ||
      //     // this.transfer.fromLocId == undefined ||
      //     // this.transfer.toLocId == undefined ||
      //     // this.transfer.docNo == null ||
      //     // this.transfer.docNo == undefined ||
      //     // this.transferDetailData.length == 0 ||
      //     // this.totalQty == 0 ||
      //     // this.transfer.fromLocId === this.transfer.toLocId ||
      //     // this.editState == true
      // ) {
      //     this.formValid = false;
      // } else {
      //     this.formValid = true;
      // }
  }

  openlookUpModal(type: string) {
      if(this.LocValCheck==true){
          if (type == "fromLoc") {
              // this.inventoryLookupTableModal.id = "";
              // this.inventoryLookupTableModal.displayName = "";
              // this.inventoryLookupTableModal.show("Location");
              // this.type = type;
              // this.rowData = [];
              // this.totalQty = 0;
              // this.totalItems = 0;
              // this.transferDetailData.length = 0;
          }
      }else{
          if (type == "toLoc") {
              // this.inventoryLookupTableModal.id = "";
              // this.inventoryLookupTableModal.displayName = "";
              // this.inventoryLookupTableModal.show("Location");
              // this.type = type;
              // this.rowData = [];
              // this.totalQty = 0;
              // this.totalItems = 0;
              // this.transferDetailData.length = 0;
          }
      }
     
  }

  checkLocValidTrasnfer() {
      // if (this.transfer.fromLocId == this.transfer.toLocId) {
      //     this.notify.info(
      //         this.l("From Location And To Location Cannot Be Equal")
      //     );
      //     this.transfer.fromLocId = null;
      //     this.transfer.toLocId = null;
      // }
  }

  checkQtyBeforeSave() {
      var checkQtyZero: boolean = false;
      // this.transfer.transferDetailDto.forEach((val, index) => {
      //     if (val.qty == 0) {
      //         checkQtyZero = true;
      //     }
      // });
      return checkQtyZero;
  }

  enterDate() {
      this.formValid = false;
  }
  leaveDate() {
      this.checkFormValid();
  }

}
