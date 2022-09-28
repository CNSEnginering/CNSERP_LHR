import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CreateOrEditChequeBookDto } from '../shared/dto/chequeBooks-dto';
import { ChequeBooksServiceProxy } from '../shared/services/chequeBooks.service';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { CreateOrEditChequeBookDetailDto } from '../shared/dto/chequeBookDetails-dto';

@Component({
    selector: 'createOrEditChequeBookModal',
    templateUrl: './create-or-edit-chequeBook-modal.component.html',
    styleUrls: ['./create-or-edit-chequeBook-modal.component.css']
})
export class CreateOrEditChequeBookModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild("bankfinderModal", { static: true }) bankfinderModal: CommonServiceLookupTableModalComponent;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    chequeBook: CreateOrEditChequeBookDto = new CreateOrEditChequeBookDto();

    docDate: Date;


    isUpdate: boolean;

    numPartOfCheck: number;
    lengthOfNumPartOfCheck: number;
    newData: any;
    newData2 = [];

    tempCheque: String;
    lengthOfChString: number;

    constructor(
        injector: Injector,
        private _chequeBooksServiceProxy: ChequeBooksServiceProxy
    ) {
        super(injector);
    }

    show(flag: boolean | undefined, chequeBookId?: number): void {
        this.docDate = null;
        if (!flag) {
            this.chequeBook = new CreateOrEditChequeBookDto();
            this.chequeBook.id = chequeBookId;
            this.chequeBook.flag = flag;
            this.docDate = new Date();

            this._chequeBooksServiceProxy.getMaxDocId().subscribe(result => {
                this.chequeBook.docNo = result;
            });
            this.chequeBook.active = true;

            this.active = true;
            this.modal.show();
        } else {
            this._chequeBooksServiceProxy.getChequeBookForEdit(chequeBookId).subscribe(result => {
                this.chequeBook = result.chequeBook;

                this.rowData = result.chequeBook.chequeBookDetail;
                this.chequeBook.flag = flag;
                this.isUpdate = flag;
                this.docDate = moment(this.chequeBook.docDate).toDate();
                this.active = true;
                this.modal.show();
            });
        }

    }

    save(): void {
        debugger;

        var count = this.gridApi.getDisplayedRowCount();
        if (count == 0) {
            this.notify.error(this.l('Please Process to Enter Grid Data'));
            return;
        }

        this.saving = true;

        var rowData = [];
        this.gridApi.forEachNode(node => {
            rowData.push(node.data);
        });

        this.chequeBook.docDate = moment(this.docDate);

        this.chequeBook.audtDate = moment();
        this.chequeBook.audtUser = this.appSession.user.userName;

        if (!this.chequeBook.id) {
            this.chequeBook.createDate = moment();
            this.chequeBook.createdBy = this.appSession.user.userName;
        }

        this.chequeBook.chequeBookDetail = rowData;
        this._chequeBooksServiceProxy.createOrEdit(this.chequeBook)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    //=====================Bank================
    openBankModal() {
        this.bankfinderModal.id = null;
        this.bankfinderModal.displayName = "";
        this.bankfinderModal.show("Bank");
    }

    setBankIdNull() {
        this.chequeBook.bankid = null;
        this.chequeBook.bankName = "";
        this.chequeBook.bankAccNo = "";
    }

    getNewbank() {
        this.chequeBook.bankid = this.bankfinderModal.id;
        this.chequeBook.bankName = this.bankfinderModal.displayName;
        this.chequeBook.bankAccNo = this.bankfinderModal.accountId;

    }

    onProcess() {
        debugger;
        this.rowData = [];
        var matches = this.chequeBook.fromChNo.match(/\d+$/)[0];
        // var matches = this.chequeBook.fromChNo.match(/(\d+)/)[0];
        this.numPartOfCheck = Number(matches);

        this.newData = [];
        this.newData2 = [];

        this.tempCheque;
        this.lengthOfChString = this.chequeBook.fromChNo.length;
        this.lengthOfNumPartOfCheck = this.numPartOfCheck.toString().length;

        var temp = (this.lengthOfChString - this.lengthOfNumPartOfCheck);

        for (let i = 0; i < this.chequeBook.noofCh; i++) {
            this.tempCheque = this.chequeBook.fromChNo.slice(0, temp);
            this.tempCheque = this.tempCheque.concat(String(this.numPartOfCheck + i));
            this.newData = {
                "bankid": this.chequeBook.bankid,
                "fromChNo": this.tempCheque,
                "toChNo": this.tempCheque
            };
            this.newData2.push(this.newData);
        }

        this.chequeBook.toChNo = String(this.tempCheque);

        this.rowData = this.newData2;

    }

    //==================================Grid=================================
    public gridApi;
    public gridColumnApi;
    public rowData;
    columnDefs = [

        { headerName: this.l('DetID'), field: 'detID', sortable: true, width: 60, valueGetter: 'node.rowIndex+1' },
        { headerName: this.l('BankID'), field: 'bankid', sortable: true, width: 120, resizable: true },
        { headerName: this.l('ChequeNo'), field: 'fromChNo', sortable: true, editable: true, filter: true, width: 120, resizable: true },
        // { headerName: this.l('ToChNo'), field: 'toChNo', sortable: true, editable: true, filter: true, width: 120, resizable: true }

    ];

    onGridReady(params) {
        debugger;
        this.rowData = [];
        if (this.isUpdate) {
            this.rowData = this.chequeBook.chequeBookDetail;
        }
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        params.api.sizeColumnsToFit();
    }

}
