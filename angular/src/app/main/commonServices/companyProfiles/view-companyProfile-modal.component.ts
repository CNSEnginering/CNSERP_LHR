import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetCompanyProfileForViewDto, CompanyProfileDto, CompanyProfilesServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCompanyProfileModal',
    templateUrl: './view-companyProfile-modal.component.html'
})
export class ViewCompanyProfileModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCompanyProfileForViewDto;


    constructor(
        injector: Injector,
        private _companyProfilesServiceProxy: CompanyProfilesServiceProxy
    ) {
        super(injector);
        this.item = new GetCompanyProfileForViewDto();
        this.item.companyProfile = new CompanyProfileDto();
    }

    show(item: GetCompanyProfileForViewDto): void {

        this._companyProfilesServiceProxy.getCompanyProfileForView(item.companyProfile.id).subscribe(result=>{
            this.item = result;
        })

     //   this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
