import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme7ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-montserrat.css'];
    }
}
