import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetBankForViewDto } from '@shared/service-proxies/service-proxies';
import { GetGLReconHeaderForViewDto, GLReconHeaderDto } from '@app/main/finance/shared/dto/glReconHeader-dto';


@Component({
    selector: 'viewBankReconcileModal',
    templateUrl: './view-bankReconcile-modal.component.html'
})
export class ViewBankReconcileModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    gLReconHeader: GetGLReconHeaderForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.gLReconHeader = new GetGLReconHeaderForViewDto();
        this.gLReconHeader.gLReconHeader = new GLReconHeaderDto();
    }

    show(gLReconHeader: GetGLReconHeaderForViewDto): void {
        
        this.gLReconHeader = gLReconHeader;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
