import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseTableComponent } from './modcase-table.component';

describe('ModCaseTableComponent', () => {
  let component: ModCaseTableComponent;
  let fixture: ComponentFixture<ModCaseTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
