import { IThemeAssetContributor } from '../ThemeAssetContributor';
import { ThemeHelper } from '@app/shared/layout/themes/ThemeHelper';
import * as rtlDetect from 'rtl-detect';

export class DefaultThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        let asideSkin = ThemeHelper.getAsideSkin();
        let headerSkin = ThemeHelper.getHeaderSkin();
        const isRtl = rtlDetect.isRtlLang(abp.localization.currentLanguage.name);

        return [
            '/assets/metronic/themes/default/css/skins/header/base/' + headerSkin + (isRtl ? '.rtl' : '') + '.css',
            '/assets//metronic/themes/default/css/skins/brand/' + asideSkin + (isRtl ? '.rtl' : '') + '.css',
            '/assets/metronic/themes/default/css/skins/aside/' + asideSkin + (isRtl ? '.rtl' : '') + '.css'
        ];
    }
}
