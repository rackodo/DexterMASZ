import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduledMessageEditDialogComponent } from './scheduled-message-edit-dialog.component';

describe('ScheduledMessageExtendedEditDialogComponent', () => {
  let component: ScheduledMessageExtendedEditDialogComponent;
  let fixture: ComponentFixture<ScheduledMessageExtendedEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduledMessageEditDialogComponent ]
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
