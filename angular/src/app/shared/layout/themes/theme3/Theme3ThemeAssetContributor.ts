import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme3ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-montserrat.css'];
    }
}
