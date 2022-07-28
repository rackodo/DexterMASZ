import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RankcardPreviewComponent } from './rankcard-preview.component';

describe('RankcardPreviewComponent', () => {
  let component: RankcardPreviewComponent;
  let fixture: ComponentFixture<RankcardPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RankcardPreviewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RankcardPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
