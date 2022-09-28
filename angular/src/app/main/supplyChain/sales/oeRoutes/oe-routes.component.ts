import { Component, Injector, ViewEncapsulation, ViewChild} from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/components/table/table';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { oeroutesdto } from '../shared/dtos/oeRoutes-dto';
import { OeroutesService } from 'app/main/supplyChain/sales/shared/services/oeroutes.service';
import { CreateOrEditOeRoutesComponent } from './create-or-edit-oe-routes.component';


@Component({
    templateUrl: './oe-routes.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OeroutesComponent extends AppComponentBase {

    @ViewChild('createOrEditOERoutesComponent', { static: true }) createOrEditOERoutesComponent: CreateOrEditOeRoutesComponent;
    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    OeroutesDto : oeroutesdto = new oeroutesdto();
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
        private _OeroutesService: OeroutesService
    ) {
        super(injector);
    }

    getAll(event?: LazyLoadEvent) {
        debugger;
    this.sorting = this.primengTableHelper.getSorting(this.dataTable);
    this.skipCount = this.primengTableHelper.getSkipCount(this.paginator, event);
    this.MaxResultCount = this.primengTableHelper.getMaxResultCount(this.paginator, event);
    
    this.primengTableHelper.showLoadingIndicator();

    this._OeroutesService.getAll(
      
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

    createOeroutes(): void {
         debugger;

            this._OeroutesService.getDocNo().subscribe(result => {
             debugger; 
            if(result!=0){
                this.maxID=result;
            }
              this.createOrEditOERoutesComponent.show(null,this.maxID);
          });
    }

    delete(oeRoutesDto: oeroutesdto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._OeroutesService.delete(oeRoutesDto.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

}
