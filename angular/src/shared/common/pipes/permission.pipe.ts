import { Injector, Pipe, PipeTransform } from '@angular/core';
import { PermissionCheckerService } from '@abp/auth/permission-checker.service';

@Pipe({
    name: 'permission'
})
export class PermissionPipe implements PipeTransform {

    permission: PermissionCheckerService;

    constructor(injector: Injector) {
        this.permission = injector.get(PermissionCheckerService);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }

    isGrantedAny(...permissions: string[]): boolean {
        if (!permissions) {
            return false;
        }

        for (const permission of permissions) {
            if (this.isGranted(permission)) {
                return true;
            }
        }

        return false;
    }

    transform(key: string): boolean {
        return this.isGranted(key);
    }
}
