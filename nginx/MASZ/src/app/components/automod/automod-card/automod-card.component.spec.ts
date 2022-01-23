import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoModCardComponent } from './automod-card.component';

describe('AutoModCardComponent', () => {
  let component: AutoModCardComponent;
  let fixture: ComponentFixture<AutoModCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoModCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoModCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
