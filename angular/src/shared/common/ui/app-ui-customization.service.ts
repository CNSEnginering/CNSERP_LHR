import { Injectable } from '@angular/core';
import { UiCustomizationSettingsDto } from '@shared/service-proxies/service-proxies';
import * as rtlDetect from 'rtl-detect';

@Injectable()
export class AppUiCustomizationService {
    private _theme: UiCustomizationSettingsDto;

    init(theme: UiCustomizationSettingsDto): void {
        this._theme = theme;
    }

    getAppModuleBodyClass(): string {
        let topMenuUsed = this._theme.baseSettings.menu.position === 'top';
        const isRtl = rtlDetect.isRtlLang(
            abp.localization.currentLanguage.name
        );

        let cssClass =
            'kt-page--' +
            this._theme.baseSettings.layout.layoutType +
            ' kt-subheader--enabled kt-aside-left--offcanvas';

        if (this._theme.baseSettings.header.desktopFixedHeader) {
            cssClass += ' kt-header--fixed';
        } else {
            cssClass += ' kt-header--static';
        }

        if (this._theme.baseSettings.header.mobileFixedHeader) {
            cssClass += ' kt-header-mobile--fixed';
        }

        if (this._theme.baseSettings.menu.fixedAside && !topMenuUsed) {
            cssClass += ' kt-aside--fixed';
        }

        if (this._theme.baseSettings.menu.defaultMinimizedAside) {
            cssClass += ' kt-aside--minimize';
        }

        if (isRtl) {
            cssClass += ' kt-quick-panel--left kt-demo-panel--left';
        } else {
            cssClass += ' kt-quick-panel--right kt-demo-panel--right';
        }

        if (this._theme.baseSettings.menu.position === 'left') {
            cssClass += ' kt-aside-left--enabled kt-aside--enabled';
            cssClass +=
                ' kt-subheader--' +
                this._theme.baseSettings.subHeader.subheaderStyle;

            if (this._theme.baseSettings.menu.fixedAside) {
                cssClass += ' kt-aside--fixed';
            } else {
                cssClass += ' kt-aside--static';
            }
        } else {
            cssClass += ' kt-subheader--transparent';
        }

        if (topMenuUsed) {
            cssClass +=
                ' kt-header--minimize-' +
                this._theme.baseSettings.header.minimizeDesktopHeaderType;
        }

        if (this._theme.baseSettings.subHeader.fixedSubHeader) {
            cssClass += ' kt-subheader--fixed';
        }

        if (
            this._theme.baseSettings.footer.fixedFooter &&
            this._theme.baseSettings.layout.layoutType !== 'fixed'
        ) {
            cssClass += ' kt-footer--fixed';
        }

        return cssClass;
    }

    getAccountModuleBodyClass() {
        return 'kt-header--fixed kt-header-mobile--fixed kt-subheader--fixed kt-subheader--enabled kt-subheader--solid kt-aside--enabled kt-aside--fixed kt-page--loading';
    }

    getSelectEditionBodyClass() {
        return 'm--skin-';
    }

    getLeftAsideClass(): string {
        let cssClass = 'kt-aside-menu';

        if (this._theme.baseSettings.menu.submenuToggle === 'true') {
            cssClass += ' kt-aside-menu--dropdown';
        }

        if (this._theme.baseSettings.menu.fixedAside && this._theme.baseSettings.menu.submenuToggle !== 'true') {
            cssClass += ' ps';
        }

        return cssClass;
    }

    isSubmenuToggleDropdown(): boolean {
        return this._theme.baseSettings.menu.submenuToggle === 'true';
    }

    getTopBarMenuContainerClass(): string {
        let menuCssClass =
            'm-header__bottom m-header-menu--skin-' +
            this._theme.baseSettings.menu.asideSkin +
            ' m-container m-container--full-height m-container--responsive';
        if (this._theme.baseSettings.layout.layoutType === 'boxed') {
            return menuCssClass + ' m-container--xxl';
        }

        return menuCssClass;
    }

    getIsMenuScrollable(): boolean {
        return (
            this._theme.allowMenuScroll &&
            this._theme.baseSettings.menu.fixedAside
        );
    }

    getSideBarMenuItemClass(item, isMenuActive) {
        let menuCssClass = 'kt-menu__item';

        if (item.items.length) {
            menuCssClass += ' kt-menu__item--submenu';
        }

        if (isMenuActive) {
            menuCssClass += ' kt-menu__item--open kt-menu__item--active';
        }

        return menuCssClass;
    }
}
