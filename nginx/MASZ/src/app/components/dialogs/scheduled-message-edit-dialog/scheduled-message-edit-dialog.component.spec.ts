import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledMessageExtendedEditDialogComponent } from './scheduled-message-edit-dialog.component';

describe('ScheduledMessageExtendedEditDialogComponent', () => {
  let component: ScheduledMessageExtendedEditDialogComponent;
  let fixture: ComponentFixture<ScheduledMessageExtendedEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduledMessageExtendedEditDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduledMessageExtendedEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
