import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { APTransactionListServiceProxy,GetBookViewModeldto,UserDto} from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './aptransactionlist.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class APTransactionListComponent extends AppComponentBase {


    fromDate : moment.Moment;
    toDate : moment.Moment;
    bookId = '';
    userId  ='';
    directPost = false;
    status= ''
    
    constructor( 
        injector: Injector,
        private _apTransactionListServiceProxy: APTransactionListServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        
    ) {
        super(injector);
    }
  
   
    grgcat: GetBookViewModeldto[] = [];
    grguser: UserDto[] = [];

   
    ngOnInit(){
        this.init();

        this.init1();
    }

    init(): void {

        this._apTransactionListServiceProxy.getBookList()
            .subscribe((result) => {
                this.grgcat = result.items;
            });
//
        }

        init1(): void {      
this._apTransactionListServiceProxy.getUserList()
.subscribe((result) => {
    debugger;
    this.grguser = result.items;
});
    } 

    getReport(){
        

        debugger;
        this._apTransactionListServiceProxy.getReportParm(  
            this.fromDate,
            this.toDate,
            this.bookId,
            this.userId,
          
            this.directPost,
            this.status)
        .subscribe(result=> {
          debugger;
        });
    }
   

   
}

