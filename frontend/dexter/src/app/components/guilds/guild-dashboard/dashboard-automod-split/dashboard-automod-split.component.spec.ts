import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardAutoModSplitComponent } from './dashboard-automod-split.component';

describe('DashboardAutoModSplitComponent', () => {
  let component: DashboardAutoModSplitComponent;
  let fixture: ComponentFixture<DashboardAutoModSplitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardAutoModSplitComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardAutoModSplitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
