import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuickSearchCaseResultComponent } from './quicksearch-case-result.component';

describe('QuickSearchCaseResultComponent', () => {
  let component: QuickSearchCaseResultComponent;
  let fixture: ComponentFixture<QuickSearchCaseResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuickSearchCaseResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuickSearchCaseResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
