import { Component, Injector, ViewEncapsulation, ViewChild, OnInit, ViewChildren, Output, EventEmitter, QueryList, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsPostingsServiceProxy, AccountsPostingDto, CreateOrEditAccountsPostingDto, GetUsersForAccountPostingDto, GetBooksForAccountPostingDto, AccountsPostingListDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditAccountsPostingModalComponent } from './create-or-edit-accountsPosting-modal.component';
import { ViewAccountsPostingModalComponent } from './view-accountsPosting-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
import { ModalDirective } from 'ngx-bootstrap';

@Component({
  templateUrl: './Posted.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class PostedComponent extends AppComponentBase implements OnInit
{

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    mode: string;
    active = false;
    saving = false;
    fromDate: moment.Moment;
    toDate: moment.Moment;
    fromDoc: number;
    toDoc: number;
    users: string[] = [];
    books: string[] = [];
    posting: number[] = [];
    accountsPosting: CreateOrEditAccountsPostingDto = new CreateOrEditAccountsPostingDto();
    getUsersForAccountPostingDto: GetUsersForAccountPostingDto[];
    getBooksForAccountPostingDto: GetBooksForAccountPostingDto[];
    auditTime: Date;
    accountsPostingListDto: AccountsPostingListDto[];
    dataFoundFlag: boolean = false;
    constructor(
        injector: Injector,
        private _accountsPostingsServiceProxy: AccountsPostingsServiceProxy
    ) {
        super(injector);
        this.getUsersForAccountPostingDto = new Array<GetUsersForAccountPostingDto>();
        this.getBooksForAccountPostingDto = new Array<GetBooksForAccountPostingDto>();
    }
  ngAfterViewInit(): void {
    throw new Error("Method not implemented.");
  }

    ngOnInit() {
      this.show("AccountsPosting");
    }

    show(mode: string): void {    
        this.mode = mode;
        this.auditTime = null;
        this.books = [];
        this.users = [];
        this.accountsPostingListDto = [];
        this.getUsersForAccountPostingDto = [];
        this.getBooksForAccountPostingDto = [];
        this.posting = [];
        this.dataFoundFlag = false;
        //if (!accountsPostingId) {
        this.accountsPosting = new CreateOrEditAccountsPostingDto();
        //this.accountsPosting.id = accountsPostingId;
        this.accountsPosting.docDate = moment().startOf('day');
        this.fromDate = null;
        this.toDate = null;
        this.fromDoc = 0;
        this.toDoc = 999999;
        this.active = true;
      //  this.modal.show();
        //   } else {
        // this._accountsPostingsServiceProxy.getAccountsPostingForEdit(accountsPostingId).subscribe(result => {
        //     this.accountsPosting = result.accountsPosting;

        //     if (this.accountsPosting.auditTime) {
        //         this.auditTime = this.accountsPosting.auditTime.toDate();
        //     }

        //     this.active = true;
        //     this.modal.show();
        // });
        // }
    }

    save(): void {
      
        this.saving = true;
        if (this.auditTime) {
            if (!this.accountsPosting.auditTime) {
                this.accountsPosting.auditTime = moment(this.auditTime).startOf('day');
            }
            else {
                this.accountsPosting.auditTime = moment(this.auditTime);
            }
        }
        else {
            this.accountsPosting.auditTime = null;
        }
        this._accountsPostingsServiceProxy.createOrEdit(this.accountsPosting)
            .pipe(finalize(() => {
               this.saving = false; 
              }))
            .subscribe(() => {
              this.notify.info(this.l('SavedSuccessfully'));
               // this.close();
               // this.modalSave.emit(null);
              
            });
            this.accountsPostingListDto.length = 0;
            // this.getUsersForAccountPostingDto.length = 0;
            // this.getBooksForAccountPostingDto.length = 0;
    }

    findData() {
        this.accountsPostingListDto.length = 0;
        if (this.fromDate != undefined && this.toDate != undefined && this.fromDoc != undefined && this.toDoc != undefined) {
            this._accountsPostingsServiceProxy.getDetailForAccountsPosting(
                this.fromDate,
                this.toDate,
                this.mode,
                this.fromDoc,
                this.toDoc
            ).subscribe(
                res => {
                    this.getUsersForAccountPostingDto = res.getUsersForAccountPostingDto;
                    this.getBooksForAccountPostingDto = res.getBooksForAccountPostingDto;
                    if (this.getUsersForAccountPostingDto.length == 0 && this.getBooksForAccountPostingDto.length == 0) {
                        this.dataFoundFlag = true;
                    }
                    else {
                        this.dataFoundFlag = false;
                    }
                }
            )
        }
    }

    getBookChkBoxData(event: HTMLInputElement) {
        if (event.checked == true) {
            this.books.push(event.value);
        }
        else {
            this.books.forEach((el, index) => {
                if (el == event.value) {
                    this.books.splice(index, 1);
                }
            });
        }
    }

    getUserChkBoxData(event: HTMLInputElement) {
        if (event.checked == true) {
            this.users.push(event.value);
        }
        else {
            this.users.forEach((el, index) => {
                if (el == event.value) {
                    this.users.splice(index, 1);
                }
            });
        }
    }

    findDataForAccountList() {
        this._accountsPostingsServiceProxy.getAccountsPostingList(this.users, this.books, this.fromDate, this.toDate, this.mode, this.fromDoc, this.toDoc).subscribe(
            res => {
                this.accountsPostingListDto = res;
                this.accountsPostingListDto.length == 0 ? this.dataFoundFlag = true : this.dataFoundFlag = false;
                //console.log(this.accountsPostingListDto);
            }
        )
    }

    getDataForPosting(event: HTMLInputElement) {
        if (event.checked == true) {
            this.posting.push(parseInt(event.value));
        } else {
            this.posting.forEach((el, index) => {
                if (el == parseInt(event.value)) {
                    this.posting.splice(index, 1);
                }
            });
        }
    }

    updateData() {
        this._accountsPostingsServiceProxy.postingData(this.posting.slice(), this.mode, true).subscribe();
        this.notify.info(this.l('SavedSuccessfully'))
        this.accountsPostingListDto.length = 0;
        this.getUsersForAccountPostingDto.length = 0;
        this.getBooksForAccountPostingDto.length = 0;
        this.posting.length = 0;
        //this.close();
        //this.modalSave.emit(null);
    }

    selectAll() {
        this.checkboxes.forEach((element) => {
            element.nativeElement.checked = true;
            this.posting.push(element.nativeElement.value);
        });
    }

    unSelectAll() {
        this.checkboxes.forEach((element) => {
            element.nativeElement.checked = false;
            this.posting.push(element.nativeElement.value);
        });
    }

    close(): void {

        // this.active = false;
        // this.modal.hide();
    }
}
