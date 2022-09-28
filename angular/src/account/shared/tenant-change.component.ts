import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AccountServiceProxy, CompanyProfilesServiceProxy } from '@shared/service-proxies/service-proxies';
import { TenantChangeModalComponent } from './tenant-change-modal.component';

@Component({
    selector: 'tenant-change',
    template:
        `<span *ngIf="isMultiTenancyEnabled" style="color:black">
    {{"CurrentCompany" | localize}}:
        <span *ngIf="tenancyName" title="{{name}}"><strong>{{tenancyName}}</strong></span> <span *ngIf="!tenancyName">{{"NotSelected" | localize}}</span> (<a href="javascript:;" (click)="showChangeModal()">{{l("Select")}}</a>)
        <div class="companyName" style="font-weight:bold ">{{companyName}}</div>
        <tenantChangeModal #tenantChangeModal></tenantChangeModal>
    </span>`
})
export class TenantChangeComponent extends AppComponentBase implements OnInit {

    @ViewChild('tenantChangeModal', { static: false }) tenantChangeModal: TenantChangeModalComponent;

    tenancyName: string;
    name: string;
    companyName: string = '';

    constructor(
        injector: Injector,
        private _accountService: AccountServiceProxy
    ) {
        super(injector);
    }

    ngOnInit() {
        if (this.appSession.tenant) {
            this.tenancyName = this.appSession.tenant.tenancyName;
            this.name = this.appSession.tenant.name;
            this._accountService.getCompanyName().subscribe(result => {
                this.companyName = result;
                localStorage.setItem('companyName', this.companyName);
            });
        }
    }

    get isMultiTenancyEnabled(): boolean {
        return abp.multiTenancy.isEnabled;
    }

    showChangeModal(): void {
        this.tenantChangeModal.show(this.tenancyName);
    }
}
