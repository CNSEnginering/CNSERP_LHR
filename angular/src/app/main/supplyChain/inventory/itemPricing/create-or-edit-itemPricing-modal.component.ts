import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter,
    DebugElement,
    OnInit,
} from "@angular/core";

import { BsDatepickerConfig, ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { ItemPricingLookupTableModalComponent } from "../FinderModals/itemPricing-lookup-table-modal.component";
import { ItemPricingDto } from "../shared/dto/itemPricing-dto";
import { ItemPricingService } from "../shared/services/itemPricing.service";
import { InventoryLookupTableModalComponent } from "@app/finders/supplyChain/inventory/inventory-lookup-table-modal.component";
import { datepicker } from "jquery";
import { split } from "lodash";


@Component({
    selector: "itemPricingModal",
    templateUrl: "./create-or-edit-itemPricing-modal.component.html",
})




export class CreateOrEditItemPricingModalComponent
    extends AppComponentBase
    implements OnInit {
    type: string;
    editState: boolean = false;
    gridApi;
    gridColumnApi;
    rowData: any[];
    rowSelection;
    formValid: boolean;
    paramsData: any;
    priceList: string;
    active: boolean | undefined = true;
    activeNew: any;
    docDate: Date |string;
    id: any;
    itemDate: Date |string;
    editMode: boolean = false;
    priceListExists: boolean = false;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @ViewChild("ItemPricingLookupTableModal", { static: true })
    ItemPricingLookupTableModal: ItemPricingLookupTableModalComponent;
    @ViewChild("inventoryLookupTableModal", { static: true })
    inventoryLookupTableModal: InventoryLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    checkItemId: boolean = false;
    saving = false;
    itemPricingDto: ItemPricingDto = new ItemPricingDto();
    itemPricingDetailData: ItemPricingDto[] = new Array<ItemPricingDto>();
    
    columnDefs = [
        
        {
            headerName: this.l("SrNo"),
            editable: false,
            field: "srNo",
            sortable: true,
            width: 100,
            valueGetter: "node.rowIndex+1",
        },
        {
            headerName: this.l("Item"),
            editable: false,
            field: "item",
            sortable: true,
            filter: true,
            width: 200,
            resizable: true,
        },
        {
            headerName: this.l("Description"),
            editable: false,
            field: "description",
            sortable: true,
            filter: true,
            width: 227,
            resizable: true,
        },
        {
            headerName: this.l("Unit"),
            editable: false,
            field: "unit",
            sortable: true,
            filter: true,
            width: 150,
            resizable: true,
        },
        {
            headerName: this.l("Conver"),
            editable: false,
            field: "conver",
            sortable: true,
            filter: true,
            width: 100,
            resizable: true,
        },
        // { headerName: this.l('PriceList'), editable: false, field: 'priceList', sortable: true, filter: true, width: 150, resizable: true },
        // { headerName: this.l('Description'), editable: false, field: 'priceListDesc', sortable: true, filter: true, width: 200, resizable: true },
        // {
        //     headerName: this.l("Price"),
        //     editable: true,
        //     field: "price",
        //     width: 100,
        //     resizable: true,
        // },
        // {
        //     headerName: this.l("DiscValue"),
        //     editable: true,
        //     field: "discValue",
        //     sortable: true,
        //     width: 100,
        //     resizable: true,
        // },
        // {
        //     headerName: this.l("NetPrice"),
        //     editable: false,
        //     field: "netPrice",
        //     sortable: true,
        //     width: 100,
        //     resizable: true,
        // },
////////////////////////////////////////////////////////////////mz
{
    headerName: this.l("Purchase Price"),
    editable: true,
    field: "purchasePrice",
    sortable: true,
    width: 150,
    resizable: true,
},
{
    headerName: this.l("Sale Price"),
    editable: true,
    field: "salePrice",
    sortable: true,
    width: 150,
    resizable: true,
},

{
  
   
    headerName: this.l("Date"),
    editable: true,
    field: "itemDate",
    type:Date,
    // this.input.type = 'number';
    sortable: true,
    filter: true,
    width: 200,
    resizable: true,
    
  
}


    ];
    constructor(
        injector: Injector,
        private _itemPricingService: ItemPricingService
    ) {
        super(injector);
    }

    show(id?: string): void {
        debugger;
        this.rowData = [];
        this.activeNew = true;
        this.editMode = false;
        this.priceList = "";
        this.itemPricingDto = new ItemPricingDto();
        this.itemPricingDetailData = new Array<ItemPricingDto>();
        //this.itemPricingDto.itemID = '';

        this.active = true;
        if (!id) {
            this.id = undefined;
            this.docDate = new Date;
            this.checkFormValid();
        } else {
            this._itemPricingService.getDataForEdit(id).subscribe((data) => {
            
                debugger;
                if(this.docDate==null){
                    this.docDate="";
                }
                else{
                    this.docDate=new Date(this.docDate);
                    
                }
                this.editMode = true;
                // this.itemPricingDto.priceList = data["result"]["itemPricing"]["priceList"];
                // this.itemPricingDto.priceListDesc = data["result"]["itemPricing"]["priceListDesc"];
                // this.itemPricingDto.itemID = data["result"]["itemPricing"]["itemID"];
                // this.itemPricingDto.price = data["result"]["itemPricing"]["price"];
                // this.itemPricingDto.discValue = data["result"]["itemPricing"]["discValue"];
                // this.itemPricingDto.netPrice = data["result"]["itemPricing"]["netPrice"];
                // this.itemPricingDto.active = data["result"]["itemPricing"]["active"];
                // this.itemPricingDto.id = data["result"]["itemPricing"]["id"];
                //  this.itemPricingDto.itemDate = data["result"]["ItemDate"]["ItemDate"];
                this.activeNew = data["result"][0]["itemPricing"]["active"];
                this.priceList = data["result"][0]["itemPricing"]["priceList"];
                this.docDate = data["result"][0]["itemPricing"]["docDate"];
                this.id = data["result"][0]["itemPricing"]["id"];
debugger;
                 for(var i =0; i<= data["result"].length-1;i++ )
                 {
                     data["result"][i]["itemPricing"]["itemDate"] = new Date(data["result"][i]["itemPricing"]["itemDate"]).toLocaleString("en-GB").split(',')[0];
                 }


                this.itemDate = data["result"][0]["itemPricing"]["itemDate"]; // data["result"][0]["itemPricing"]["itemDate"];
                
                this.editState = false;
                this.addRecordToGrid(data["result"]);
                //this.checkFormValid();
            });
        }
        this.modal.show();
    }
    removeRecordFromGrid() {
        var selectedData = this.gridApi.getSelectedRows();
        var filteredDataIndex = this.itemPricingDetailData.findIndex(
            (x) => x.srNo == selectedData[0].srNo
        );
        this.itemPricingDetailData.splice(filteredDataIndex, 1);
        this.gridApi.updateRowData({ remove: selectedData });
        this.gridApi.refreshCells();
        //this.totalItems = this.itemPricingDetailData.length;
        //this.calculateTotalQty();
        this.editState = false;
        this.checkFormValid();
    }
    save(): void {
    debugger;
        this.saving = true;
        this.itemPricingDetailData[0].id = this.id;

        for (var i = 0; i<= this.itemPricingDetailData.length-1; i++)
        {
            this.itemPricingDetailData[i].itemDate = new Date (split(this.itemPricingDetailData[i].itemDate.toString(),'/')[1] + '/' + split(this.itemPricingDetailData[i].itemDate.toString(),'/')[0] + '/' + split(this.itemPricingDetailData[i].itemDate.toString(),'/')[2]); 
        }
        

        this._itemPricingService
            .create(this.itemPricingDetailData)
            .subscribe(() => {
                this.saving = false;
                this.notify.info(this.l("SavedSuccessfully"));
                this.close();
                this.modalSave.emit(null);
            });
        this.close();
    }

    close(): void {
        //this.active = false;
        this.modal.hide();
    }

    openlookUpModal(type: string) {
        debugger;
        this.ItemPricingLookupTableModal.data = null;
        this.ItemPricingLookupTableModal.show(type);
    }
    getItemLookUpData() {
        debugger;

        let rowNode = this.gridApi.getRenderedNodes();
        let validItem = true;
        for (let node of rowNode) {
            if (node.data.item == this.inventoryLookupTableModal.id) {
                this.notify.info(
                    this.l("Item Id is already added in the grid")
                );
                validItem = false;
                break;
            }
        }

        if (!validItem) return;

        this._itemPricingService
            .checkItemIdAgainstPriceList(
                this.itemPricingDto.priceList,
                this.inventoryLookupTableModal.id
            )
            .subscribe((data) => {
                this.checkItemId = data["result"];
                if (this.checkItemId == true) {
                    this.notify.info(
                        this.l("Item Id is already against this price list")
                    );
                } else {
                    this.paramsData.data.item = this.inventoryLookupTableModal.id;
                    this.paramsData.data.description = this.inventoryLookupTableModal.displayName;
                    this.paramsData.data.unit = this.inventoryLookupTableModal.unit;
                    this.paramsData.data.conver = this.inventoryLookupTableModal.conver;
                    
                    this.gridApi.refreshCells();
                    this.addOrUpdateRecordToDetailData(
                        this.paramsData.data,
                        ""
                    );
                    if (
                        this.paramsData.data.item == undefined ||
                        this.paramsData.data.item == ""
                    ) {
                        // this.notify.info("Qty Should Be Greater Than Zero");
                        this.editState = true;
                    } else {
                        this.editState = false;
                    }
                    this.checkFormValid();
                }
            });
    }
    getLookUpData() {
        if (this.ItemPricingLookupTableModal.type == "PriceList") {
            // if (typeof this.ItemPricingLookupTableModal.data != "object") {
            this.itemPricingDto.priceList = this.ItemPricingLookupTableModal.data.priceList;
            this.itemPricingDto.priceListDesc = this.ItemPricingLookupTableModal.data.priceListName;
            this.priceList = this.ItemPricingLookupTableModal.data.priceList;
            // }
        }
        // else {
        //     //if (typeof this.ItemPricingLookupTableModal.data != "object") {
        //     this.itemPricingDto.itemID = this.ItemPricingLookupTableModal.data.itemId;
        //     this.itemPricingDto.itemDesc = this.ItemPricingLookupTableModal.data.descp;
        //     // }
        // }

        // this.checkItemIdAgainstPriceList();
        this.CheckPriceListExists();
    }

    calculateNetPrice(params) {
        debugger;
        if (Number(params.data.price) >= Number(params.data.discValue)) {
            params.data.netPrice =
                Number(params.data.price) - Number(params.data.discValue);
            this.gridApi.refreshCells();
            this.addOrUpdateRecordToDetailData(params.data, "");
            this.editState = false;
        } else {
            this.notify.info(
                "Discount Should Be Less Than Or Equal Than Price Value"
            );
            this.editState = true;
        }
        this.checkFormValid();
    }
    ngOnInit(): void {
        debugger;
        this.rowData = [];
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
      
        this.type = "item";
        // this.ItemPricingLookupTableModal.show("GatePassItem");
        this.inventoryLookupTableModal.id = "";
        this.inventoryLookupTableModal.displayName = "";
        this.inventoryLookupTableModal.show("Items");
        this.paramsData = params;
    }
    activeNewChange() {
        this.itemPricingDetailData[0].active = this.activeNew;
    }
    cellValueChanged(params) {
        this.addOrUpdateRecordToDetailData(params.data, "");
        //this.calculateNetPrice(params);
        // this.checkEditState();
        if (
            params.data.item == undefined ||
            params.data.item == "" ||
            params.data.price == 0
        ) {
            // this.notify.info("Qty Should Be Greater Than Zero");
            this.editState = true;
        }
        this.checkFormValid();
    }
    cellEditingStarted(params) {
        //  if(params.colDef.headerName="Date"){
        // //  var test=params.data.itemDate;
        // // this.ItemDatechange(test);
        //  }
        
         this.formValid = false;
    }
    addRecordToGrid(record: any) {
        debugger;
        if (this.editState == false) {
            if (this.priceList != "" && this.priceList != undefined) {
                this.editState = true;
                if (record != undefined) {
                    record.forEach((val, index) => {
                        var str = val.itemPricing.itemID.split("*");
                        // var str = val.itemPricing.itemID;
                        var newData = {
                            srNo: index,
                            item: str[0],
                            // item:str,
                            // description: undefined,
                            // unit:undefined,
                            // conver: undefined,
                            description: str[1],
                            unit: str[2],
                            conver: str[3],
                            price: val.itemPricing.price,
                            // discValue: val.itemPricing.discValue,
                            // netPrice: val.itemPricing.netPrice,
                            purchasePrice:val.itemPricing.purchasePrice,
                            salePrice:val.itemPricing.salePrice,
                            // itemDate:val.itemPricing.itemDate,


                            itemDate:val.itemPricing.itemDate,
                            //itemdate:new Date(val.itemPricing.itemDate).toLocaleString("en-GB").split(',')[0],
                            
                            
                        };
                        this.addOrUpdateRecordToDetailData(newData, "record");

                        this.gridApi.updateRowData({ add: [newData] });
                    });
                    this.editState = false;
                    this.checkFormValid();
                } else {
                    let length = this.itemPricingDetailData.length;
                  
                    
                    var newData = {
                        srNo: ++length,
                        item: undefined,
                        description: undefined,
                        unit: undefined,
                        conver: undefined,
                        // price: 1,
                        // discValue: 0,
                        // netPrice: 0,
                        purchasePrice:0,
                        salePrice:0,
                        itemDate: new Date(this.docDate).toLocaleString("en-GB").split(',')[0],
               
                        // item: undefined,
                        // description: undefined,
                        // uni: undefined,
                        // conver: undefined,
                        // qty: undefined,
                        // lotNo: undefined,
                        // bundle: undefined,
                        // remarks: undefined,
                        // maxQty: undefined
                    };
                    this.addOrUpdateRecordToDetailData(newData, "record");

                    this.gridApi.updateRowData({ add: [newData] });
                }
                this.checkFormValid();
            } else {
                this.notify.info("Please Select Price List First");
            }
        }
    }

    addOrUpdateRecordToDetailData(data: any, type: string) {
        debugger;
        //var item_date=new Date (split(data.itemDate.toString(),'/')[1] + '/' + split(data.itemDate.toString(),'/')[0] + '/' + split(data.itemDate.toString(),'/')[2]); 
        if (type == "record") {
            this.itemPricingDto = new ItemPricingDto();
            this.itemPricingDto.docDate =new Date (this.docDate);
            this.itemPricingDto.active = this.activeNew;
            this.itemPricingDto.priceList = this.priceList;
            this.itemPricingDto.srNo = data.srNo;
            this.itemPricingDto.itemID = data.item;
             this.itemPricingDto.price = data.price;
            // this.itemPricingDto.netPrice = data.netPrice;
            // this.itemPricingDto.discValue = data.discValue;
            this.itemPricingDto.purchasePrice=data.purchasePrice;
            this.itemPricingDto.salePrice=data.salePrice;
             this.itemPricingDto.itemDate= data.itemDate;
          
            //this.itemPricingDto.itemDate =item_date;

             this.itemPricingDetailData.push(this.itemPricingDto);
        } else {
            var filteredData = new ItemPricingDto();
            filteredData = this.itemPricingDetailData.find(
                (x) => x.srNo == data.srNo
            );
            if (filteredData.srNo != undefined) {
                filteredData.docDate = new Date (this.docDate);
                filteredData.active = this.activeNew;
                filteredData.itemID = data.item;
                filteredData.description = data.description;
                filteredData.price = data.price;
                filteredData.itemDate = data.itemDate;

                filteredData.salePrice = data.salePrice;
                filteredData.purchasePrice = data.purchasePrice;
                // filteredData.netPrice = data.netPrice;
                // filteredData.discValue = data.discValue;
                filteredData.priceList = this.priceList;
                // if (Number(data.qty) == 0) {
                //     this.notify.info("Qty Cannot Be Zero");
                // }
                // else if (Number(data.maxQty) < Number(data.qty)) {
                //     if (Number(data.maxQty) != 0)
                //         this.notify.info("Qty Cannot Be Greater Than Remaining " + data.maxQty + " Qty");
                // }
                // else {

                // }
            }
        }
    }
    checkFormValid() {
        debugger;
        if (
            this.itemPricingDto.docDate == null ||
            this.itemPricingDetailData.length == 0 ||
            this.editState == true ||
            this.priceListExists == true
        ) {
            this.formValid = false;
        } else {
            this.formValid = true;
            this.docDate=new Date(this.docDate);
          
          
        }
    }

    dateChange(event) {
        
        debugger;
        this.docDate = event;
        this.itemPricingDetailData[0].docDate =  new Date(this.docDate);
        
    }
    // ItemDatechange(perm){
    //     debugger;
    //     this.itemDate = perm;
    //     this.itemDate =  new Date(this.itemDate);
    //     // { docDate | date: 'dd/MM/yyyy' }
     
    //     return this.itemDate;
    // }

    CheckPriceListExists() {
        debugger;
        this._itemPricingService
            .CheckPriceListExists(this.priceList)
            .subscribe((data) => {
                if (data["result"] == true) {
                    this.notify.info("This Price List Already Exists");
                    this.priceListExists = true;
                } else {
                    this.priceListExists = false;
                }
                this.checkFormValid();
            });
    }

   
}
