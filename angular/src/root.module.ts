import { AbpModule } from '@abp/abp.module';
import { PlatformLocation, registerLocaleData } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, Injector, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppAuthService } from '@app/shared/common/auth/app-auth.service';
import { AppConsts } from '@shared/AppConsts';
import { CommonModule } from '@shared/common/common.module';
import { AppSessionService } from '@shared/common/session/app-session.service';
import { AppUiCustomizationService } from '@shared/common/ui/app-ui-customization.service';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import {
    API_BASE_URL,
    UiCustomizationSettingsDto,
    ThemeSettingsDto,
    ThemeMenuSettingsDto,
    ThemeLayoutSettingsDto,
    ThemeHeaderSettingsDto,
    ThemeSubHeaderSettingsDto,
    ThemeFooterSettingsDto,
    ApplicationInfoDto
} from '@shared/service-proxies/service-proxies';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import * as localForage from 'localforage';
import * as _ from 'lodash';
import * as moment from 'moment';
import { AppPreBootstrap } from './AppPreBootstrap';
import { AppModule } from './app/app.module';
import { RootRoutingModule } from './root-routing.module';
import { RootComponent } from './root.component';
import { DomHelper } from '@shared/helpers/DomHelper';
import { CookieConsentService } from '@shared/common/session/cookie-consent.service';
import { NgxBootstrapDatePickerConfigService } from 'assets/ngx-bootstrap/ngx-bootstrap-datepicker-config.service';
import { LocaleMappingService } from '@shared/locale-mapping.service';

export function appInitializerFactory(
    injector: Injector,
    platformLocation: PlatformLocation) {
    return () => {
        abp.ui.setBusy();

        return new Promise<boolean>((resolve, reject) => {
            AppConsts.appBaseHref = getBaseHref(platformLocation);
            let appBaseUrl = getDocumentOrigin() + AppConsts.appBaseHref;

            AppPreBootstrap.run(appBaseUrl, () => {
                handleLogoutRequest(injector.get(AppAuthService));
                initializeLocalForage();

                if (UrlHelper.isInstallUrl(location.href)) {
                    doConfigurationForInstallPage(injector);
                    abp.ui.clearBusy();
                    resolve(true);
                } else {
                    let appSessionService: AppSessionService = injector.get(AppSessionService);
                    appSessionService.init().then((result) => {
                        initializeAppCssClasses(injector, result);
                        initializeTenantResources(injector);
                        initializeCookieConsent(injector);
                        registerLocales(resolve, reject);
                    }, (err) => {
                        abp.ui.clearBusy();
                        reject(err);
                    });
                }

            }, resolve, reject);
        });
    };
}

function initializeLocalForage() {
    localForage.config({
        driver: localForage.LOCALSTORAGE,
        name: 'ERP',
        version: 1.0,
        storeName: 'abpzerotemplate_local_storage',
        description: 'Cached data for ERP'
    });
}

function getDefaultThemeForInstallPage(): UiCustomizationSettingsDto {
    let theme = new UiCustomizationSettingsDto();
    theme.baseSettings = new ThemeSettingsDto();
    theme.baseSettings.theme = 'default';
    theme.baseSettings.menu = new ThemeMenuSettingsDto();
    theme.baseSettings.menu.asideSkin = 'light';

    theme.baseSettings.header = new ThemeHeaderSettingsDto();
    theme.baseSettings.header.headerSkin = 'light';

    theme.baseSettings.subHeader = new ThemeSubHeaderSettingsDto();

    theme.baseSettings.layout = new ThemeLayoutSettingsDto();
    theme.baseSettings.layout.layoutType = 'fluid';
    theme.baseSettings.header = new ThemeHeaderSettingsDto();
    theme.baseSettings.footer = new ThemeFooterSettingsDto();
    return theme;
}

function setApplicationInfoForInstallPage(injector, theme: UiCustomizationSettingsDto) {
    let appSessionService: AppSessionService = injector.get(AppSessionService);
    appSessionService.theme = theme;
    appSessionService.application = new ApplicationInfoDto();
    appSessionService.application.releaseDate = moment().startOf('day');

}

function doConfigurationForInstallPage(injector) {
    let theme = getDefaultThemeForInstallPage();
    setApplicationInfoForInstallPage(injector, theme);

    initializeAppCssClasses(injector, theme);
}

function initializeAppCssClasses(injector: Injector, theme: UiCustomizationSettingsDto) {
    let appUiCustomizationService = injector.get(AppUiCustomizationService);
    appUiCustomizationService.init(theme);

    //Css classes based on the layout
    if (abp.session.userId) {
        document.body.className = appUiCustomizationService.getAppModuleBodyClass();
    } else {
        document.body.className = appUiCustomizationService.getAccountModuleBodyClass();
    }
}

function initializeTenantResources(injector: Injector) {
    let appSessionService: AppSessionService = injector.get(AppSessionService);

    if (appSessionService.tenant && appSessionService.tenant.customCssId) {
        document.head.appendChild(
            DomHelper.createElement('link', [
                {
                    key: 'id',
                    value: 'TenantCustomCss'
                },
                {
                    key: 'rel',
                    value: 'stylesheet'
                },
                {
                    key: 'href',
                    value: AppConsts.remoteServiceBaseUrl + '/TenantCustomization/GetCustomCss?id=' + appSessionService.tenant.customCssId
                }])
        );
    }

    let metaImage = DomHelper.getElementByAttributeValue('meta', 'property', 'og:image');
    if (metaImage) {
        //set og share image meta tag
        if (!appSessionService.tenant || !appSessionService.tenant.logoId) {
            let ui: AppUiCustomizationService = injector.get(AppUiCustomizationService);
            metaImage.setAttribute('content', window.location.origin + '/assets/common/images/app-logo-on-' + abp.setting.get(appSessionService.theme.baseSettings.theme + '.' + 'App.UiManagement.Left.AsideSkin') + '.svg');
        } else {
            metaImage.setAttribute('content', AppConsts.remoteServiceBaseUrl + '/TenantCustomization/GetLogo?id=' + appSessionService.tenant.logoId);
        }
    }
}

function initializeCookieConsent(injector: Injector) {
    let cookieConsentService: CookieConsentService = injector.get(CookieConsentService);
    cookieConsentService.init();
}

function getDocumentOrigin() {
    if (!document.location.origin) {
        return document.location.protocol + '//' + document.location.hostname + (document.location.port ? ':' + document.location.port : '');
    }

    return document.location.origin;
}

function registerLocales(resolve: (value?: boolean | Promise<boolean>) => void, reject: any) {
    if (shouldLoadLocale()) {
        let angularLocale = convertAbpLocaleToAngularLocale(abp.localization.currentLanguage.name);
        import(`@angular/common/locales/${angularLocale}.js`)
            .then(module => {
                registerLocaleData(module.default);
                NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales().then(_ => {
                    resolve(true);
                    abp.ui.clearBusy();
                });
            }, reject);
    } else {
        NgxBootstrapDatePickerConfigService.registerNgxBootstrapDatePickerLocales().then(_ => {
            resolve(true);
            abp.ui.clearBusy();
        });
    }
}

export function shouldLoadLocale(): boolean {
    return abp.localization.currentLanguage.name && abp.localization.currentLanguage.name !== 'en-US';
}

export function convertAbpLocaleToAngularLocale(locale: string): string {
    return new LocaleMappingService().map('angular', locale);
}

export function getRemoteServiceBaseUrl(): string {
    return AppConsts.remoteServiceBaseUrl;
}

export function getCurrentLanguage(): string {
    return abp.localization.currentLanguage.name;
}

export function getBaseHref(platformLocation: PlatformLocation): string {
    let baseUrl = platformLocation.getBaseHrefFromDOM();
    if (baseUrl) {
        return baseUrl;
    }

    return '/';
}

function handleLogoutRequest(authService: AppAuthService) {
    let currentUrl = UrlHelper.initialUrl;
    let returnUrl = UrlHelper.getReturnUrl();
    if (currentUrl.indexOf(('account/logout')) >= 0 && returnUrl) {
        authService.logout(true, returnUrl);
    }
}

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppModule,
        CommonModule.forRoot(),
        AbpModule,
        ServiceProxyModule,
        HttpClientModule,
        RootRoutingModule
    ],
    declarations: [
        RootComponent
    ],
    providers: [
        { provide: API_BASE_URL, useFactory: getRemoteServiceBaseUrl },
        {
            provide: APP_INITIALIZER,
            useFactory: appInitializerFactory,
            deps: [Injector, PlatformLocation],
            multi: true
        },
        {
            provide: LOCALE_ID,
            useFactory: getCurrentLanguage
        }
    ],
    bootstrap: [RootComponent]
})
export class RootModule {

}
