import { Injector, Pipe, PipeTransform } from '@angular/core';
import { FeatureCheckerService } from '@abp/features/feature-checker.service';

@Pipe({
    name: 'checkFeature'
})
export class FeatureCheckerPipe implements PipeTransform {

    featureCheckerService: FeatureCheckerService;

    constructor(injector: Injector) {
        this.featureCheckerService = injector.get(FeatureCheckerService);
    }

    transform(feature: string): boolean {
        return this.featureCheckerService.isEnabled(feature);
    }
}
