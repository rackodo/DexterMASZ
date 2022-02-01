import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardGuildInfoComponent } from './dashboard-guildinfo.component';

describe('DashboardGuildInfoComponent', () => {
  let component: DashboardGuildInfoComponent;
  let fixture: ComponentFixture<DashboardGuildInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardGuildInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardGuildInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
