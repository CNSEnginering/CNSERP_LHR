import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme5ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-asap-condensed.css'];
    }
}
