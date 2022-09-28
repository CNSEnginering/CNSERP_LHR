import { IThemeAssetContributor } from '../ThemeAssetContributor';

export class Theme9ThemeAssetContributor implements IThemeAssetContributor {
    getAssetUrls(): string[] {
        return ['/assets/fonts/fonts-montserrat.css'];
    }
}
