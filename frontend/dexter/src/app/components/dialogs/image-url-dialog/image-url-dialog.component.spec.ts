import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageUrlDialogComponent } from './image-url-dialog.component';

describe('ImageUrlDialogComponent', () => {
  let component: ImageUrlDialogComponent;
  let fixture: ComponentFixture<ImageUrlDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImageUrlDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageUrlDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
