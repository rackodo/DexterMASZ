import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseEditComponent } from './modcase-edit.component';

describe('ModCaseEditComponent', () => {
  let component: ModCaseEditComponent;
  let fixture: ComponentFixture<ModCaseEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
