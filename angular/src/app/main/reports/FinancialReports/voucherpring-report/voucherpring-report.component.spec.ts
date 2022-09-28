import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoucherpringReportComponent } from './voucherpring-report.component';

describe('VoucherpringReportComponent', () => {
  let component: VoucherpringReportComponent;
  let fixture: ComponentFixture<VoucherpringReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VoucherpringReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoucherpringReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
