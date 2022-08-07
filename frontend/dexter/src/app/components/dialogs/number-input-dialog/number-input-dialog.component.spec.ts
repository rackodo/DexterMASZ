import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NumberInputDialogComponent } from './number-input-dialog.component';

describe('NumberInputDialogComponent', () => {
  let component: NumberInputDialogComponent;
  let fixture: ComponentFixture<NumberInputDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NumberInputDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NumberInputDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
