import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseAddComponent } from './modcase-add.component';

describe('ModCaseAddComponent', () => {
  let component: ModCaseAddComponent;
  let fixture: ComponentFixture<ModCaseAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
