import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivateVcsConfigComponent } from './private-vcs-config.component';

describe('PrivateVcsConfigComponent', () => {
  let component: PrivateVcsConfigComponent;
  let fixture: ComponentFixture<PrivateVcsConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivateVcsConfigComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivateVcsConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
