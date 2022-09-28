import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetCurrencyRateForViewDto, CurrencyRateDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCurrencyRateModal',
    templateUrl: './view-currencyRate-modal.component.html'
})
export class ViewCurrencyRateModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCurrencyRateForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetCurrencyRateForViewDto();
        this.item.currencyRate = new CurrencyRateDto();
    }

    show(item: GetCurrencyRateForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
