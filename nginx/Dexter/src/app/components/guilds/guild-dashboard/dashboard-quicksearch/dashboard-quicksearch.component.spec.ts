import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardQuickSearchComponent } from './dashboard-quicksearch.component';

describe('DashboardQuickSearchComponent', () => {
  let component: DashboardQuickSearchComponent;
  let fixture: ComponentFixture<DashboardQuickSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardQuickSearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardQuickSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
