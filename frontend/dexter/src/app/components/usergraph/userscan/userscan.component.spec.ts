import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserScanComponent } from './userscan.component';

describe('UserScanComponent', () => {
  let component: UserScanComponent;
  let fixture: ComponentFixture<UserScanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserScanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserScanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
