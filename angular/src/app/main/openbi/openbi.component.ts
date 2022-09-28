import { Component, ViewEncapsulation, Injector, OnInit } from "@angular/core";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { AppComponentBase } from "@shared/common/app-component-base";
import { AbpSessionService } from "abp-ng2-module/dist/src/session/abp-session.service";


@Component({
    templateUrl: './openbi.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class OpenBiComponent extends AppComponentBase implements OnInit {

    constructor(injector: Injector,
        private sessionService: AbpSessionService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        window.open("http://192.168.223.149/Ice.Ios/Default.aspx?tenant=" + this.sessionService.tenantId, "_blank");
        var _url = window.location.hostname + ":" + window.location.port;
        window.open("http://" + _url + "/app/main/dashboard", "_self");
    }

}