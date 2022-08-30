import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OffsetEditorComponent } from './offset-editor.component';

describe('OffsetEditorComponent', () => {
  let component: OffsetEditorComponent;
  let fixture: ComponentFixture<OffsetEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OffsetEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OffsetEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
