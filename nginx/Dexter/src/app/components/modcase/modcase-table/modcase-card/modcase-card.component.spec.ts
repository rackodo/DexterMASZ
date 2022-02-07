import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModCaseCardComponent } from './modcase-card.component';

describe('ModCaseCardComponent', () => {
  let component: ModCaseCardComponent;
  let fixture: ComponentFixture<ModCaseCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModCaseCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModCaseCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
