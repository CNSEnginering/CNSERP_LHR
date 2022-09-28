import { TestBed } from '@angular/core/testing';

import { SideBarColorService } from './side-bar-color.service';

describe('SideBarColorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SideBarColorService = TestBed.get(SideBarColorService);
    expect(service).toBeTruthy();
  });
});
