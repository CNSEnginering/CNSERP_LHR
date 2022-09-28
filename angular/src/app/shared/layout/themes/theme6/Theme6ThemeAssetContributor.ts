import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme6ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-montserrat.css'];
    }
}
