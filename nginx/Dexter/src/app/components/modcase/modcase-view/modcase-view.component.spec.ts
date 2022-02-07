import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseViewComponent } from './modcase-view.component';

describe('ModCaseViewComponent', () => {
  let component: ModCaseViewComponent;
  let fixture: ComponentFixture<ModCaseViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseViewComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
