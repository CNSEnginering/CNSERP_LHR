import { Component, Injector, OnInit,ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { Oedriversdto } from '../shared/dtos/oedriversdto';
import { OedriversService } from '../shared/services/oedrivers.service';
import { CreatOrEditOedriversComponent } from './creat-or-edit-oedrivers.component';

@Component({
  selector: 'app-oedrivers',
  templateUrl: './oedrivers.component.html',
 
})
export class OedriversComponent extends AppComponentBase {
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;
  @ViewChild('creatOrEditOedriversComponent', { static: true }) creatOrEditOedriversComponent:CreatOrEditOedriversComponent;

 OedriversDto : Oedriversdto = new Oedriversdto();
 filterText = '';
 maxDesignationIDFilter : number;
 maxDesignationIDFilterEmpty : number;
 minDesignationIDFilter : number;
 minDesignationIDFilterEmpty : number;
 maxID: number;
 listData: any;
 sorting: any;
 skipCount: any;
 MaxResultCount: any;
  constructor(
    injector: Injector,
    private _OedriversService : OedriversService
    )
  {
    super(injector);
  }

  ngOnInit() {
  }
  getAll(event?: LazyLoadEvent) {
    debugger;
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);
    
    this.primengTableHelper.showLoadingIndicator();

    this._OedriversService.getAll(
      
      this.filterText,
   
      this.sorting,
      this.skipCount,
      this.MaxResultCount,
    ).subscribe(result => {
      debugger;
      console.log(result);

      this.listData = result["result"]["items"];
      this.primengTableHelper.totalRecordsCount = result["result"]["totalCount"];
      this.primengTableHelper.records = this.listData;
      this.primengTableHelper.hideLoadingIndicator();

      
    });
  }
  reloadPage(): void {
    this.paginator.changePage(this.paginator.getPage());
  }
  createOedrivers(): void {
    debugger
  
    this._OedriversService.getDocNo().subscribe(result => {
      debugger; 
      if(result!=0){
          this.maxID=result;
      }
        this.creatOrEditOedriversComponent.show(null,this.maxID);
    });
  
  }
  delete(OedriversDto: Oedriversdto): void {
    debugger;
    this.message.confirm(
      '',
      (isConfirmed) => {
        if (isConfirmed) {
          this._OedriversService.delete(OedriversDto.id)
            .subscribe(() => {
              debugger;
              this.reloadPage();
              this.notify.success(this.l('SuccessfullyDeleted'));
            });
        }
      }
    );
  }
}
