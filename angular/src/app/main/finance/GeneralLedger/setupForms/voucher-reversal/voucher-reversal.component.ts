import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CreateOrEditvoucherReversalComponent } from './create-or-edit-voucher-reversal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Paginator, LazyLoadEvent } from 'primeng/primeng';
import { voRevservices } from '@app/main/finance/shared/services/voRev.service';
import { Table } from 'primeng/table';
import { voRevdto } from '@app/main/finance/shared/dto/voRev-dto';

@Component({
  selector: 'app-voucher-reversal',
  templateUrl: './voucher-reversal.component.html',
  animations: [appModuleAnimation()]
})
export class VoucherReversalComponent extends AppComponentBase {
  @ViewChild('createOrEditVoucherReversal', { static: true })
  createOrEditVoucherReversal :CreateOrEditvoucherReversalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  filterText = '';
  maxDesignationIDFilter : number;
	maxDesignationIDFilterEmpty : number;
  minDesignationIDFilter : number;
	minDesignationIDFilterEmpty : number;

  constructor(injector: Injector,
    private _vouRevservices : voRevservices,)
  {
    super(injector); 
  }

  getAll(event?: LazyLoadEvent)
  {
    debugger;
    if (this.primengTableHelper.shouldResetPaging(event)) {
        this.paginator.changePage(0);
        return;
    }
    this.primengTableHelper.showLoadingIndicator();

    this._vouRevservices.getAll(
      this.filterText,

      this.primengTableHelper.getSorting(this.dataTable),
      this.primengTableHelper.getSkipCount(this.paginator, event),
      this.primengTableHelper.getMaxResultCount(this.paginator, event)

      //this.primengTableHelper.getSorting(this.dataTable),
      //this.primengTableHelper.getSkipCount(this.paginator, event),
      //this.primengTableHelper.getMaxResultCount(this.paginator, event)Empty : this.minDesignationIDFilter,

    ).subscribe(result => {
      console.log(result)
       debugger;
       console.log(result.items)
        this.primengTableHelper.totalRecordsCount = result.totalCount;
        this.primengTableHelper.records = result.items;
        this.primengTableHelper.hideLoadingIndicator();
    });

  }
 

  createVoucherReversal() : void
    {
      debugger
      this.createOrEditVoucherReversal.show(null);
    };

    reloadPage(): void {
      debugger;
      this.paginator.changePage(this.paginator.getPage());
  }
  
    delete(revdto: voRevdto){
      debugger;
      this.message.confirm(
          '',
          this.l('AreYouSure'),
          (isConfirmed) => {
              if (isConfirmed) {
                  this._vouRevservices.delete(revdto.id)
                      .subscribe(() => {
                          this.reloadPage();
                          this.notify.success(this.l('SuccessfullyDeleted'));
                      });
              }
          }
      );
    }

}
