import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme4ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-montserrat.css'];
    }
}
