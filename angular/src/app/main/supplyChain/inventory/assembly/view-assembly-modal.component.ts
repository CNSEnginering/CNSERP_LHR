import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AssemblyDto } from '../shared/dto/assembly-dto';



@Component({
    selector: 'viewAssemblyModal',
    templateUrl: './view-assembly-modal.component.html'
})
export class ViewAssemblyComponent extends AppComponentBase {

    @ViewChild('viewAssemblyModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    active = false;
    saving = false;

    item: AssemblyDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new AssemblyDto();
    }

    show(item: AssemblyDto): void {
        debugger
        this.item = item["assembly"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
