import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAccountsPostingForViewDto, AccountsPostingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewAccountsPostingModal',
    templateUrl: './view-accountsPosting-modal.component.html'
})
export class ViewAccountsPostingModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetAccountsPostingForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetAccountsPostingForViewDto();
        this.item.accountsPosting = new AccountsPostingDto();
    }

    show(item: GetAccountsPostingForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
