import { AppConsts } from '@shared/AppConsts';
import * as rtlDetect from 'rtl-detect';
import { StyleLoaderService } from '@shared/utils/style-loader.service';
import { Theme8ThemeAssetContributor } from '@app/shared/layout/themes/theme8/Theme8ThemeAssetContributor';
import { Theme2ThemeAssetContributor } from '@app/shared/layout/themes/theme2/Theme2ThemeAssetContributor';
import { Theme3ThemeAssetContributor } from '@app/shared/layout/themes/theme3/Theme3ThemeAssetContributor';
import { Theme4ThemeAssetContributor } from '@app/shared/layout/themes/theme4/Theme4ThemeAssetContributor';
import { Theme5ThemeAssetContributor } from '@app/shared/layout/themes/theme5/Theme5ThemeAssetContributor';
import { Theme6ThemeAssetContributor } from '@app/shared/layout/themes/theme6/Theme6ThemeAssetContributor';
import { Theme9ThemeAssetContributor } from '@app/shared/layout/themes/theme9/Theme9ThemeAssetContributor';
import { Theme7ThemeAssetContributor } from '@app/shared/layout/themes/theme7/Theme7ThemeAssetContributor';
import { Theme10ThemeAssetContributor } from '@app/shared/layout/themes/theme10/Theme10ThemeAssetContributor';
import { Theme11ThemeAssetContributor } from '@app/shared/layout/themes/theme11/Theme11ThemeAssetContributor';
import { Theme12ThemeAssetContributor } from '@app/shared/layout/themes/theme12/Theme12ThemeAssetContributor';
import { DefaultThemeAssetContributor } from '@app/shared/layout/themes/default/DefaultThemeAssetContributor';
import { ThemeHelper } from '@app/shared/layout/themes/ThemeHelper';
import * as _ from 'lodash';

export class DynamicResourcesHelper {

    static loadResources(callback: () => void): void {
        Promise.all([DynamicResourcesHelper.loadStlyes()])
            .then(() => {
                callback();
            });
    }

    static loadStlyes(): Promise<any> {
        let theme = ThemeHelper.getTheme();

        const isRtl = rtlDetect.isRtlLang(abp.localization.currentLanguage.name);

        if (isRtl) {
            document.documentElement.setAttribute('dir', 'rtl');
        }

        const cssPostfix = isRtl ? '-rtl' : '';
        const styleLoaderService = new StyleLoaderService();

        let styleUrls = [
            AppConsts.appBaseUrl + '/assets/metronic/themes/' + theme + '/css/style.bundle' + cssPostfix.replace('-', '.') + '.css',
            AppConsts.appBaseUrl + '/assets/primeng/datatable/css/primeng.datatable' + cssPostfix + '.css',
            //AppConsts.appBaseUrl + '/assets/common/styles/themes/' + theme + '/primeng.datatable' + cssPostfix + '.css',
            AppConsts.appBaseUrl + '/assets/common/styles/metronic-customize.css',
            AppConsts.appBaseUrl + '/assets/common/styles/themes/' + theme + '/metronic-customize.css',
            AppConsts.appBaseUrl + '/assets/common/styles/metronic-customize-angular.css'
        ].concat(DynamicResourcesHelper.getAdditionalThemeAssets());

        styleLoaderService.loadArray(styleUrls);

        if (isRtl) {
            styleLoaderService.load(
                AppConsts.appBaseUrl + '/assets/common/styles/abp-zero-template-rtl.css'
            );
        }

        return Promise.resolve(true);
    }

    static getAdditionalThemeAssets(): string[] {
        let theme = ThemeHelper.getTheme();

        if (theme === 'default') {
            return new DefaultThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme8') {
            return new Theme8ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme2') {
            return new Theme2ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme3') {
            return new Theme3ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme4') {
            return new Theme4ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme5') {
            return new Theme5ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme6') {
            return new Theme6ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme7') {
            return new Theme7ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme9') {
            return new Theme9ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme10') {
            return new Theme10ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme11') {
            return new Theme11ThemeAssetContributor().getAssetUrls();
        }

        if (theme === 'theme12') {
            return new Theme12ThemeAssetContributor().getAssetUrls();
        }

        return [];
    }
}
