import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OffsetEditorSettingsDialog } from './settings-dialog.component';

describe('SettingsDialogComponent', () => {
  let component: OffsetEditorSettingsDialog;
  let fixture: ComponentFixture<OffsetEditorSettingsDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OffsetEditorSettingsDialog ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OffsetEditorSettingsDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
