import { AfterViewInit, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
     EditionSelectDto,
    PasswordComplexitySetting,
    ProfileServiceProxy,
    RegisterTenantOutput,
    TenantRegistrationServiceProxy,
    PaymentPeriodType,
    SubscriptionPaymentGatewayType,
    SubscriptionStartType,
    EditionPaymentType
} from '@shared/service-proxies/service-proxies';
import { RegisterTenantModel } from './register-tenant.model';
import { TenantRegistrationHelperService } from './tenant-registration-helper.service';
import { finalize, catchError } from 'rxjs/operators';
import { RecaptchaComponent } from 'ng-recaptcha';

@Component({
    templateUrl: './register-tenant.component.html',
    animations: [accountModuleAnimation()]
})
export class RegisterTenantComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @ViewChild('recaptchaRef', {static: true}) recaptchaRef: RecaptchaComponent;
    model: RegisterTenantModel = new RegisterTenantModel();
    passwordComplexitySetting: PasswordComplexitySetting = new PasswordComplexitySetting();
    subscriptionStartType = SubscriptionStartType;
    editionPaymentType: EditionPaymentType;
    paymentPeriodType = PaymentPeriodType;
    selectedPaymentPeriodType: PaymentPeriodType = PaymentPeriodType.Monthly;
    subscriptionPaymentGateway = SubscriptionPaymentGatewayType;
    paymentId = '';
    recaptchaSiteKey: string = AppConsts.recaptchaSiteKey;

    saving = false;

    constructor(
        injector: Injector,
        private _tenantRegistrationService: TenantRegistrationServiceProxy,
        private _router: Router,
        private _profileService: ProfileServiceProxy,
        private _tenantRegistrationHelper: TenantRegistrationHelperService,
        private _activatedRoute: ActivatedRoute
    ) {
        super(injector);
    }

    ngOnInit() {
        this.model.editionId = this._activatedRoute.snapshot.queryParams['editionId'];
        this.editionPaymentType = this._activatedRoute.snapshot.queryParams['editionPaymentType'];

        if (this.model.editionId) {
            this.model.subscriptionStartType = this._activatedRoute.snapshot.queryParams['subscriptionStartType'];
        }

        //Prevent to create tenant in a tenant context
        if (this.appSession.tenant != null) {
            this._router.navigate(['account/login']);
            return;
        }

        this._profileService.getPasswordComplexitySetting().subscribe(result => {
            this.passwordComplexitySetting = result.setting;
        });
    }

    ngAfterViewInit() {
        if (this.model.editionId) {
            this._tenantRegistrationService.getEdition(this.model.editionId)
                .subscribe((result: EditionSelectDto) => {
                    this.model.edition = result;
                });
        }
    }

    get useCaptcha(): boolean {
        return this.setting.getBoolean('App.TenantManagement.UseCaptchaOnRegistration');
    }

    save(): void {
        if (this.useCaptcha && !this.model.captchaResponse) {
            this.message.warn(this.l('CaptchaCanNotBeEmpty'));
            return;
        }

        this.saving = true;
        this._tenantRegistrationService.registerTenant(this.model)
            .pipe(finalize(() => { this.saving = false; }))
            .pipe(catchError((err, caught): any => {
                this.recaptchaRef.reset();
            }))
            .subscribe((result: RegisterTenantOutput) => {
                this.notify.success(this.l('SuccessfullyRegistered'));
                this._tenantRegistrationHelper.registrationResult = result;

                if (parseInt(this.model.subscriptionStartType.toString()) === SubscriptionStartType.Paid) {
                    this._router.navigate(['account/buy'],
                        {
                            queryParams: {
                                tenantId: result.tenantId,
                                editionId: this.model.editionId,
                                subscriptionStartType: this.model.subscriptionStartType,
                                editionPaymentType: this.editionPaymentType
                            }
                        });
                } else {
                    this._router.navigate(['account/register-tenant-result']);
                }
            });
    }

    captchaResolved(captchaResponse: string): void {
        this.model.captchaResponse = captchaResponse;
    }

    onPaymentPeriodChangeChange(selectedPaymentPeriodType) {
        this.selectedPaymentPeriodType = selectedPaymentPeriodType;
    }
}
