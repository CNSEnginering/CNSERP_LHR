import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme8ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-asap-condensed.css'];
    }
}
