import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RankcardCustomizerComponent } from './rankcard-customizer.component';

describe('RankcardCustomizerComponent', () => {
  let component: RankcardCustomizerComponent;
  let fixture: ComponentFixture<RankcardCustomizerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RankcardCustomizerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RankcardCustomizerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
