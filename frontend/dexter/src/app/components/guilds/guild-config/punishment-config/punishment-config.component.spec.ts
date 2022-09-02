import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PunishmentConfigComponent } from './punishment-config.component';

describe('PunishmentConfigComponent', () => {
  let component: PunishmentConfigComponent;
  let fixture: ComponentFixture<PunishmentConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PunishmentConfigComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PunishmentConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
