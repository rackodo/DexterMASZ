import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RankcardLevelDisplayComponent } from './rankcard-level-display.component';

describe('RankcardLevelDisplayComponent', () => {
  let component: RankcardLevelDisplayComponent;
  let fixture: ComponentFixture<RankcardLevelDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RankcardLevelDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RankcardLevelDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
