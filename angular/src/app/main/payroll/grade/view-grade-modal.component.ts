import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetGradeForViewDto, GradeDto } from '../shared/dto/grade-dto';

@Component({
    selector: 'viewGradeModal',
    templateUrl: './view-grade-modal.component.html'
})
export class ViewGradeModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetGradeForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetGradeForViewDto();
        this.item.grade = new GradeDto();
    }

    show(item: GetGradeForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
