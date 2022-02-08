import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMapEditDialogComponent } from './usermap-edit-dialog.component';

describe('UserMapEditDialogComponent', () => {
  let component: UserMapEditDialogComponent;
  let fixture: ComponentFixture<UserMapEditDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserMapEditDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserMapEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
