import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuDirective } from './_base/layout/directives/menu.directive';
import { OffcanvasDirective } from './_base/layout/directives/offcanvas.directive';
import { HeaderDirective } from './_base/layout/directives/header.directive';
import { ToggleDirective } from './_base/layout/directives/toggle.directive';
import { LayoutRefService } from './_base/layout/services/layout-ref.service';

@NgModule({
    imports: [CommonModule],
    declarations: [
        // directives
        MenuDirective,
        OffcanvasDirective,
        HeaderDirective,
        ToggleDirective
    ],
    exports: [
        // directives
        MenuDirective,
        OffcanvasDirective,
        HeaderDirective,
        ToggleDirective
    ],
    providers: [
        LayoutRefService
    ]
})
export class CoreModule { }
