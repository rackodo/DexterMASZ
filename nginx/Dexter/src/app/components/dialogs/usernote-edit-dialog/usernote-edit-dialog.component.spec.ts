import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNoteEditDialogComponent } from './usernote-edit-dialog.component';

describe('UserNoteEditDialogComponent', () => {
  let component: UserNoteEditDialogComponent;
  let fixture: ComponentFixture<UserNoteEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserNoteEditDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserNoteEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
