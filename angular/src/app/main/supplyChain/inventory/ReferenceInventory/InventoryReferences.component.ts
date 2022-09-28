import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';

@Component({
    template: `<salesReference [refType]="'FA'" ></salesReference>`,
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class InventoryReferencesComponent extends AppComponentBase {

    constructor(
        injector: Injector,

    ) {
        super(injector);
    }


}
