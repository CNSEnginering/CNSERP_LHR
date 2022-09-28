/* tslint:disable:no-unused-variable */

import { APP_BASE_HREF } from '@angular/common';
import { TestBed, async } from '@angular/core/testing';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { RootModule } from '../root.module';
import { AppComponent } from './app.component';

export function getRemoteServiceBaseUrl(): string {
    return 'http://bd-test-vm3.cnse.com.pk:91';
}

describe('App: ERP', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [
                RootModule
            ],
            providers: [
                { provide: API_BASE_URL, useValue: getRemoteServiceBaseUrl() },
                { provide: APP_BASE_HREF, useValue: '/' }
            ]
        }).compileComponents();
    }));

    it('should create the app', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));

});
