import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { GridOptions } from 'ag-grid-community';
import { CreateOrEditCSUserLocHDto,CreateOrEditCSUserLocDDto } from '@app/main/commonServices/shared/dto/UserLoc-dto';
import { userLocService } from '@app/main/commonServices/shared/services/userLoc.service';
// import { CheckboxCellComponent } from '@app/shared/common/checkbox-cell/checkbox-cell.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';
import { CheckBoxCellComponent } from './checkBoxCell/checkbox-cell.component';

@Component({
  selector: 'app-create-or-edit-userloc',
  templateUrl: './create-or-edit-userloc.component.html'
})
export class CreateOrEditUserlocComponent extends AppComponentBase  {

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
  // @ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  active = false;
  saving = false;
  processing = false;
  private setParms;
  checkalltxt:any=false;
  userLocHeader: CreateOrEditCSUserLocHDto = new CreateOrEditCSUserLocHDto();
  userLocDetail: CreateOrEditCSUserLocDDto = new CreateOrEditCSUserLocDDto();


  public gridOptions: GridOptions;

  target: string;
  isUpdate: boolean;
  check: boolean;


  constructor(
      injector: Injector,
      private _userLocService: userLocService
  ) {
      super(injector);
  }


  show(id:any): void {
      debugger;

      if (!id) {
          debugger;
          this.userLocHeader = new CreateOrEditCSUserLocHDto();
          //this.userLocHeader.id = gLSecurityHeaderId;
          //this.userLocHeader.typeID = ;
          this.rowData = [];
          this.active = true;
          this.modal.show();
      } else {
             this._userLocService.getDataForEdit(id).subscribe(res=>{
              
               this.userLocHeader.typeID=res["result"]["csUserLocH"]["typeID"];
               this.userLocHeader.userId=res["result"]["csUserLocH"]["userId"];
               this.userLocHeader.id=res["result"]["csUserLocH"]["id"];
               this.getUserName();
               this._userLocService.getUserLocation(this.userLocHeader.userId,this.userLocHeader.typeID)
               .subscribe(res=>{
                 console.log(res);
                 this.rowData = res["result"]["userLocD"];
                 this.gridApi.refreshCells();
               })

             })
       
        this.active = true;
        this.modal.show();
      
      }
  }
  CheckAll(){
    debugger
    if(this.checkalltxt==false){
        this.gridApi.forEachNode(element => {
          debugger
         
            this.checkalltxt=true;
            element.data.status=true;
         
         
        });
      }else{
        this.gridApi.forEachNode(element => {
          debugger
         
            this.checkalltxt=false;
            element.data.status=false;
         
         
        });
      }
        this.gridApi.refreshCells();
  }
  //==================================Grid=================================
  private gridApi;
  private gridColumnApi;
  private rowData;
  private rowSelection;
  columnDefs = [

      { headerName: this.l('SrNo'), field: 'srNo', sortable: true, width: 60, valueGetter: 'node.rowIndex+1' },
      { headerName: this.l('LocId'), field: 'locId', sortable: true, width: 120, editable: false, visible: true },
      { headerName: this.l('Location'), field: 'locdesc', sortable: true, width: 120, editable: false, resizable: true },
      // { headerName: this.l('Allow'), field: 'status', sortable: true, filter: true, width: 70, resizable: true ,
      // cellRendererFramework: CheckBoxCellComponent},
      { headerName: 'Allow', 
    field: 'status', 
    editable:false,width: 70,
    cellRenderer: params => {
        return `<input type='checkbox' ${params.value ? 'checked' : ''} />`;
    }
}
      // { headerName: this.l('Allow All'), field: 'allowAll', sortable: true, filter: true, width: 70, resizable: true ,
      // cellRendererFramework: CheckBoxCellComponent},
      
  ];

  onGridReady(params) {
      debugger;
      this.rowData = [];
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
        this.rowSelection = "multiple";
  }

 
  FetchLocation(){
    if(this.userLocHeader.userName!=null && this.userLocHeader.userName!="" && this.userLocHeader.typeID!=undefined){
      this._userLocService.getUserLocation(this.userLocHeader.userId,this.userLocHeader.typeID)
      .subscribe(res=>{
        console.log(res);
        this.rowData = res["result"]["userLocD"];
        this.gridApi.refreshCells();
      })
    }else{
      this.message.error("Enter Correct User Id and Type","User Id ,Type")
    }
      
  }
  onCellClicked(params) {
      debugger;
     var status=params.data.status;
     if(status==false){
      params.data.status=true;
     }else if(status==true){
      params.data.status=false;
     }
     //this.gridApi.refreshCells();
  }

  addIconCellRendererFunc(params) {
      debugger;
      return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
  }

  onBtStartEditing(index, col) {
      debugger;
      this.gridApi.setFocusedCell(index, col);
      this.gridApi.startEditingCell({
          rowIndex: index,
          colKey: col
      });
  }


  


  onCellValueChanged(params) {
      debugger;
      this.gridApi.refreshCells();
  }
  onCellEditingStarted(params) {
      debugger;

  }

  //==================================Grid=================================

  save(): void {

      this.message.confirm(
          'Save User Location',
          (isConfirmed) => {
              if (isConfirmed) {
                  debugger;
                  var count = this.gridApi.getDisplayedRowCount();

                  if (count == 0) {
                      this.notify.error(this.l('Please Enter Grid Data'));
                      return;
                  }

                  
                  
                  this.saving = true;

                  var rowData = [];
                  this.gridApi.forEachNode(node => {
                    debugger
                      if(node.data.status=="")
                      {
                          node.data.status=false;
                      }
                      rowData.push(node.data);
                  });

                 
                   this.userLocHeader.userLocD = rowData;
                   this.userLocHeader.userLocD.forEach(element => {
                       debugger;
                      element.userID = this.userLocHeader.userId;
                      element.typeID=this.userLocHeader.typeID;
                  });
                  this._userLocService.create(this.userLocHeader)
                      .pipe(finalize(() => { this.saving = false; }))
                      .subscribe(() => {
                          this.notify.info(this.l('SavedSuccessfully'));
                          this.close();
                          this.modalSave.emit(null);
                      });
              }
          }
      );
  }

  getUserName()
  {
      debugger;
      this.userLocHeader.userId=this.userLocHeader.userId.trim();
      if(this.userLocHeader.userId!="" && this.userLocHeader.userId!=null)
      {
          this._userLocService.getUserInfo(this.userLocHeader.userId).subscribe(res=>{
                  debugger;
                  //console.log(res["result"]);
                  this.userLocHeader.userName=res["result"];
          });
      }
      
  }

  // processGLSecurity():void{
  //     debugger;
  //     this.processing = true;

  //     this._gLSecurityDetailService.processGLSecurity(this.glSecurityHeader.userID, this.glSecurityHeader.id).subscribe(result => {
  //         debugger
  //         if (result == "done") {
  //             this.processing = false;
  //             this.notify.info(this.l('Processed Successfully'));
  //         } else {
  //             this.processing = false;
  //             this.notify.error(this.l('Process Failed'));
  //         }
  //     });
  // }
 
  /////////////////////////////////////////////////////////ChartOfAccount/////////////////////////////////////////////////////
  
 


 
  /////////////////////////////////////////////////////////ChartOfAccount/////////////////////////////////////////////////////
  

  close(): void {
      this.active = false;
      this.modal.hide();
  }

}
