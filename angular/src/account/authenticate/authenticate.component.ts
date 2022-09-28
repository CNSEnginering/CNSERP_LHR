import { AbpSessionService } from '@abp/session/abp-session.service';
import { AccountServiceProxy, IsTenantAvailableInput, IsTenantAvailableOutput, TenantAvailabilityState } from '@shared/service-proxies/service-proxies';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/common/app-component-base';
import { SessionServiceProxy, UpdateUserSignInTokenOutput } from '@shared/service-proxies/service-proxies';
import { UrlHelper } from 'shared/helpers/UrlHelper';
import { ExternalLoginProvider, LoginService } from '../login/login.service';
import { ActivatedRoute } from '@angular/router';
import { finalize } from 'rxjs/operators';
@Component({
  //animations: [accountModuleAnimation()],
  templateUrl: './authenticate.component.html'
})
export class AuthenticateComponent  extends AppComponentBase implements OnInit {
  submitting = false;
    isMultiTenancyEnabled: boolean = this.multiTenancy.isEnabled;
    saving = false;
    tenancyName:string;
    urlRefresh:boolean=false;
    constructor(
        injector: Injector,
        public loginService: LoginService,
        private _router: Router,
        private _sessionService: AbpSessionService,
        private _sessionAppService: SessionServiceProxy,
        private route: ActivatedRoute,
        private _accountService: AccountServiceProxy,
    ) {
        super(injector);
    }

    get multiTenancySideIsTeanant(): boolean {
        return this._sessionService.tenantId > 0;
    }

    get isTenantSelfRegistrationAllowed(): boolean {
        return this.setting.getBoolean('App.TenantManagement.AllowSelfRegistration');
    }

    get isSelfRegistrationAllowed(): boolean {
        if (!this._sessionService.tenantId) {
            return false;
        }

        return this.setting.getBoolean('App.UserManagement.AllowSelfRegistration');
    }

    ngOnInit(): void {
      debugger
      this.loginService.authenticateModel.userNameOrEmailAddress = this.route.snapshot.queryParamMap.get('username');
      this.loginService.authenticateModel.password = this.route.snapshot.queryParamMap.get('pass');
       this.tenancyName = this.route.snapshot.queryParamMap.get('tenant');
      let input = new IsTenantAvailableInput();
      input.tenancyName = this.tenancyName;

      this.saving = true;
      this._accountService.isTenantAvailable(input)
          .pipe(finalize(() => { this.saving = false; }))
          .subscribe((result: IsTenantAvailableOutput) => {
              switch (result.state) {
                  case TenantAvailabilityState.Available:
                      abp.multiTenancy.setTenantIdCookie(result.tenantId);
                      debugger
                    
                     if (!localStorage.getItem('foo')) { 
                      localStorage.setItem('foo', 'no reload') 
                      location.reload() 
                    } 
                    else {
                      localStorage.removeItem('foo') 
                    }
                      return;
                  case TenantAvailabilityState.InActive:
                      this.message.warn(this.l('CompanyIsNotActive', this.tenancyName));
                 
                      break;
                  case TenantAvailabilityState.NotFound: //NotFound
                      this.message.warn(this.l('ThereIsNoCompanyDefinedWithName{0}', this.tenancyName));
                    
                      break;
              }

            

          });

          // this.submitting = true;
          // this.loginService.authenticate(
          //     () => {
                  
          //         this.submitting = false;
          //         abp.ui.clearBusy();
          //     }
          // );
        
          this.loginService.authenticate(
            () => {
                
                this.submitting = false;
                abp.ui.clearBusy();
            }
        );

          // this._sessionAppService.updateUserSignInToken()
          //   .subscribe((result: UpdateUserSignInTokenOutput) => {
          //       const initialReturnUrl = UrlHelper.getReturnUrl();
          //       const returnUrl = initialReturnUrl + (initialReturnUrl.indexOf('?') >= 0 ? '&' : '?') +
          //           'accessToken=' + result.signInToken +
          //           '&userId=' + result.encodedUserId +
          //           '&tenantId=' + result.encodedTenantId;

          //       location.href = returnUrl;
          //   });
            
        

          let state = UrlHelper.getQueryParametersUsingHash().state;
          if (state && state.indexOf('openIdConnect') >= 0) {
              this.loginService.openIdConnectLoginCallback({});
          }


    }
  
  

   
}
