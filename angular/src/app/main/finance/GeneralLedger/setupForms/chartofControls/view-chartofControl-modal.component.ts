import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetChartofControlForViewDto, ChartofControlDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewChartofControlModal',
    templateUrl: './view-chartofControl-modal.component.html'
})
export class ViewChartofControlModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    segment1 = '';
    segment2 = '';
    segment3 = '';

    item: GetChartofControlForViewDto;
    _chartofControlsServiceProxy: any;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetChartofControlForViewDto();
        this.item.chartofControl = new ChartofControlDto();
    }

    show(item: GetChartofControlForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show(); 
        this._chartofControlsServiceProxy.getSegmentName().subscribe(res => {
            this.segment1 = res.items[0].glOption.seg1Name;
            this.segment2 = res.items[0].glOption.seg2Name;
            this.segment3 = res.items[0].glOption.seg3Name;
        });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
