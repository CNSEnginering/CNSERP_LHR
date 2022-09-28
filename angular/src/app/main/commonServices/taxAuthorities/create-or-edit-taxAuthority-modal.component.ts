import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TaxAuthoritiesServiceProxy, CreateOrEditTaxAuthorityDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditTaxAuthorityModal',
    templateUrl: './create-or-edit-taxAuthority-modal.component.html'
})
export class CreateOrEditTaxAuthorityModalComponent extends AppComponentBase {


    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;



    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    flag = false;

    taxAuthority: CreateOrEditTaxAuthorityDto = new CreateOrEditTaxAuthorityDto();

    companyProfileId = '';


    constructor(
        injector: Injector,
        private _taxAuthoritiesServiceProxy: TaxAuthoritiesServiceProxy
    ) {
        super(injector);
    }

    show(isCreate: boolean, taxAuthorityId?: string): void {
//;
        if (isCreate) {
            //;
            this.taxAuthority = new CreateOrEditTaxAuthorityDto();
            this.taxAuthority.id = taxAuthorityId;
            //   this.taxAuthority.audtdate = moment().startOf('day');
            this.companyProfileId = '';
            this.flag = isCreate;
            this.active = true;
            this.modal.show();
        } else {
            this._taxAuthoritiesServiceProxy.getTaxAuthorityForEdit(taxAuthorityId).subscribe(result => {
                this.taxAuthority = result.taxAuthority;



                this.companyProfileId = result.taxAuthority.id;
                this.flag = isCreate;
                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {


        this.saving = true;

        this.taxAuthority.flag = this.flag;
        this._taxAuthoritiesServiceProxy.createOrEdit(this.taxAuthority)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                //;
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
    close(): void {

        this.active = false;
        this.modal.hide();
    }

    // CheckifExists(event): void{
    //     this._taxAuthoritiesServiceProxy.getTaxAuthorityForView(event).subscribe(res => {
    //         if (res != null) {
    //             //;
    //             //this.notify.error('Already Exist');
    //             this.taxAuthority.id = null;
    //         }
    //     })

    // };

    onSearchChange(searchValue: string): void {
        if(this.taxAuthority.id.length < 3)
        {
            //this.isCodeLength = false;
            this.message.warn("","Tax Authority must be 3 digit");
            
        }
        this._taxAuthoritiesServiceProxy.checkTaxAuthExist(searchValue).subscribe(
            res => {
                if(res == true)
                {
                   // this.isExists = true;
                    this.notify.warn(this.l('Tax Authority Already Exists'));
                    this.taxAuthority.id = null;
                }
                else
                {
                   // this.isExists = false;
                }
            }
        );
    };
}
