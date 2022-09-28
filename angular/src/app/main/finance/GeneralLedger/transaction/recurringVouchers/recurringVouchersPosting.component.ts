import { Component, Injector, ViewEncapsulation, ViewChild, OnInit, ViewChildren, Output, EventEmitter, QueryList, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsPostingsServiceProxy, AccountsPostingDto, CreateOrEditAccountsPostingDto, GetUsersForAccountPostingDto, GetBooksForAccountPostingDto, AccountsPostingListDto, VoucherEntryServiceProxy, CreateOrEditGLTRHeaderDto, CreateOrEditGLTRDetailDto, VoucherEntryDto, GLTRHeadersServiceProxy, GLTRDetailsServiceProxy, GLTRDetailDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
import { ModalDirective } from 'ngx-bootstrap';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';

@Component({
  templateUrl: './recurringVouchersPosting.component.html',
  encapsulation: ViewEncapsulation.None,
  animations: [appModuleAnimation()]
})
export class RecurringVouchersPostingComponent extends AppComponentBase implements OnInit
{

  @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChildren("checkboxes") checkboxes: QueryList<ElementRef>;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('CommonServiceLookupTableModal', { static: true }) CommonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
    mode: string;
    active = false;
    saving = false;
    fromDoc: number;
    toDoc: number;
    detId: number;
    desc:  String;
    gltrHeader: CreateOrEditGLTRHeaderDto = new CreateOrEditGLTRHeaderDto();
    gltrDetail: CreateOrEditGLTRDetailDto = new CreateOrEditGLTRDetailDto();
    voucherEntry: VoucherEntryDto=new VoucherEntryDto();

    constructor(
        injector: Injector,
        private _accountsPostingsServiceProxy: AccountsPostingsServiceProxy,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy ,
        private _gltrHeadersServiceProxy: GLTRHeadersServiceProxy,
        private _gltrDetailsServiceProxy: GLTRDetailsServiceProxy,
    ) {
        super(injector);
        // this.getUsersForAccountPostingDto = new Array<GetUsersForAccountPostingDto>();
        // this.getBooksForAccountPostingDto = new Array<GetBooksForAccountPostingDto>();
    }

    ngOnInit() {
      //this.show("AccountsPosting");
    }

    openVoucher(){
       this.CommonServiceLookupTableModal.show("RecurringVouchers",""," Recurring Vouchers");
    }

    save(): void {  
        this.voucherEntry = new VoucherEntryDto();
        this.voucherEntry.gltrDetail = new Array<CreateOrEditGLTRDetailDto>();
        this._gltrHeadersServiceProxy.getGLTRHeaderForEdit(this.detId).subscribe(result => {
            this.voucherEntry.gltrHeader = result.gltrHeader;    
            this.voucherEntry.gltrHeader.id = undefined;  
            this.voucherEntry.gltrHeader.docDate = moment().endOf('day');
            this.voucherEntry.gltrHeader.docMonth =  moment(this.voucherEntry.gltrHeader.docDate).month() + 1
                this._voucherEntryServiceProxy
                .getMaxDocId(this.voucherEntry.gltrHeader.bookID, false, moment(this.gltrHeader.docDate).format("LLLL"))
                .subscribe(result => {
                    this.gltrHeader.docNo = Number(result);
                    
                    this._voucherEntryServiceProxy
                    .getMaxDocId(this.voucherEntry.gltrHeader.bookID, true, moment().format("LLLL"))
                    .subscribe(result => {
                        this.voucherEntry.gltrHeader.fmtDocNo = undefined;
                        this.voucherEntry.gltrHeader.fmtDocNo = result;
        
                this._gltrDetailsServiceProxy.filterGLTRDData(this.detId).subscribe(resultD => {
                    resultD["items"].forEach(element => {
                        debugger;
                        element["gltrDetail"]["id"] = undefined;
                        element["gltrDetail"]["amount"] = 0;
                        this.voucherEntry.gltrDetail.push(element["gltrDetail"])
                    });  
                    this.saving = true;
                    this._voucherEntryServiceProxy.createOrEditVoucherEntry(this.voucherEntry)
                     .pipe(finalize(() => { this.saving = false;}))
                     .subscribe(() => {
                           this.notify.info("Voucher No "+this.gltrHeader.docNo +" has been created");
                        });      
                }); 
            });   
            
        });   

        });
    
            
    }


    close(): void {

    }

    getNewCommonServiceModal()
    {
        if(!isNaN(Number(this.CommonServiceLookupTableModal.id)))
        {
            this.fromDoc = Number(this.CommonServiceLookupTableModal.id);
            this.detId = Number(this.CommonServiceLookupTableModal.detId);
            this.desc = this.CommonServiceLookupTableModal.narration;
        }
    }
}
