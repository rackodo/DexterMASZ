import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LevelsConfigComponent } from './levels-config.component';

describe('LevelsConfigComponent', () => {
  let component: LevelsConfigComponent;
  let fixture: ComponentFixture<LevelsConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LevelsConfigComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LevelsConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
