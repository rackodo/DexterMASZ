import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseCardCompactComponent } from './modcase-card-compact.component';

describe('ModCaseCardCompactComponent', () => {
  let component: ModCaseCardCompactComponent;
  let fixture: ComponentFixture<ModCaseCardCompactComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseCardCompactComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseCardCompactComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
