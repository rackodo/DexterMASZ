import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoModConfigComponent } from './automod-config.component';

describe('AutoModConfigComponent', () => {
  let component: AutoModConfigComponent;
  let fixture: ComponentFixture<AutoModConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoModConfigComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoModConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
