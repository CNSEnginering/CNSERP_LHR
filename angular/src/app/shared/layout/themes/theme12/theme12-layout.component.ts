import { Injector, Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ThemesLayoutBaseComponent } from '@app/shared/layout/themes/themes-layout-base.component';
import { UrlHelper } from '@shared/helpers/UrlHelper';
import { OffcanvasOptions } from '@metronic/app/core/_base/layout/directives/offcanvas.directive';
import { LayoutRefService } from '@metronic/app/core/_base/layout/services/layout-ref.service';
import { AppConsts } from '@shared/AppConsts';

@Component({
    templateUrl: './theme12-layout.component.html',
    selector: 'theme12-layout',
    animations: [appModuleAnimation()]
})
export class Theme12LayoutComponent extends ThemesLayoutBaseComponent implements OnInit, AfterViewInit {

    @ViewChild('ktHeader', {static: true}) ktHeader: ElementRef;
    remoteServiceBaseUrl: string = AppConsts.remoteServiceBaseUrl;
    toggle: any;
    menuCanvasOptions: OffcanvasOptions = {
        baseClass: 'kt-aside',
        overlay: true,
        closeBy: 'kt_aside_close_btn',
        toggleBy: [{
            target: 'kt_aside_mobile_toggler',
            state: 'kt-header-mobile__toolbar-toggler--active'
        }]
    };
companyName:string;
    constructor(
        injector: Injector,
        private layoutRefService: LayoutRefService
    ) {
        super(injector);
        this.companyName=localStorage.getItem('companyName');
    }

    ngOnInit() {
        this.installationMode = UrlHelper.isInstallUrl(location.href);
    }

    ngAfterViewInit(): void {
        this.layoutRefService.addElement('header', this.ktHeader.nativeElement);
        this.initAsideToggler();
    }

    initAsideToggler(): void {
        this.toggle = new KTToggle('kt_aside_toggler', {
            target: 'body',
            targetState: 'kt-aside--minimize',
            togglerState: 'kt-aside__brand-aside-toggler--active'
        });
    }
}
