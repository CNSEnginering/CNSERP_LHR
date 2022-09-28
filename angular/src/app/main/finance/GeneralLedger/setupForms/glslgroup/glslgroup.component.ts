import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GLSlGroupServiceProxy  } from '@app/main/finance/shared/services/glslgroup.service';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditglslgroupComponent } from './create-or-editglslgroup.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-glslgroup',
  templateUrl: './glslgroup.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class GlslgroupComponent  extends AppComponentBase {

  @ViewChild('createOrEditSLGroupModal', { static: true }) createOrEditGroupCategoryModal: CreateOrEditglslgroupComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  advancedFiltersAreShown = false;
  filterText = '';
  grpctdescFilter = '';
  maxID:any;




  constructor(
      injector: Injector,
      private _groupCategoriesServiceProxy: GLSlGroupServiceProxy,
      private _notifyService: NotifyService,
      private _tokenAuth: TokenAuthServiceProxy,
      private _activatedRoute: ActivatedRoute,
      private _fileDownloadService: FileDownloadService
  ) {
      super(injector);
  }

  getGroupCategories(event?: LazyLoadEvent) {
      if (this.primengTableHelper.shouldResetPaging(event)) {
          this.paginator.changePage(0);
          return;
      }

      this.primengTableHelper.showLoadingIndicator();
debugger;
      this._groupCategoriesServiceProxy.getAll(
         
          this.filterText,
          this.grpctdescFilter,
          this.primengTableHelper.getSkipCount(this.paginator, event),
          this.primengTableHelper.getMaxResultCount(this.paginator, event)
      ).subscribe(result => {
          debugger;
          this.primengTableHelper.totalRecordsCount = result["result"]["totalCount"];
          this.primengTableHelper.records = result["result"]["items"];
          // if (result.items.length == 0) {
          //     this.maxID = 1
          // }
          // else {
          //     this.maxID = result.items.slice(-1)[0].groupCategory.id + 1;
          // }


          this.primengTableHelper.hideLoadingIndicator();
      });
  }

  reloadPage(): void {
      this.paginator.changePage(this.paginator.getPage());
  }

  createGroupCategory(): void {
      this.createOrEditGroupCategoryModal.show(this.maxID);
  }

  deleteGroupCategory(groupCategory: any): void {
      this.message.confirm(
          '',
          (isConfirmed) => {
              if (isConfirmed) {
                  this._groupCategoriesServiceProxy.delete(groupCategory.id)
                      .subscribe(() => {
                          this.reloadPage();
                          this.notify.success(this.l('SuccessfullyDeleted'));
                      });
              }
          }
      );
  }

 

}
