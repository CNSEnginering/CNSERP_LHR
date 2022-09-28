import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetAccountsPostingForViewDto, AccountsPostingDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GlChequesDto } from '@app/main/finance/shared/dto/GlCheques-dto';

@Component({
    selector: 'viewGlChequesModal',
    templateUrl: './view-glCheques-modal.component.html'
})
export class ViewGlChequesModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GlChequesDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GlChequesDto();
    }

    show(item: GlChequesDto): void {
        this.item = item["glCheque"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
