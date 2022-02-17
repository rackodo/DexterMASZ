import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMapCardComponent } from './usermap-card.component';

describe('UserMapCardComponent', () => {
  let component: UserMapCardComponent;
  let fixture: ComponentFixture<UserMapCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserMapCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserMapCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
