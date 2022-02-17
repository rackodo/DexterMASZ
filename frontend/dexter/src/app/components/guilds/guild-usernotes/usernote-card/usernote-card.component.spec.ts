import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserNoteCardComponent } from './usernote-card.component';

describe('UserNoteCardComponent', () => {
  let component: UserNoteCardComponent;
  let fixture: ComponentFixture<UserNoteCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserNoteCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserNoteCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
