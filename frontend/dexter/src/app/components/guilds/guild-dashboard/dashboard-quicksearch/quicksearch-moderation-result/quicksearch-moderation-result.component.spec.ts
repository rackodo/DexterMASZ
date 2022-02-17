import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuickSearchModerationResultComponent } from './quicksearch-moderation-result.component';

describe('QuickSearchModerationResultComponent', () => {
  let component: QuickSearchModerationResultComponent;
  let fixture: ComponentFixture<QuickSearchModerationResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuickSearchModerationResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuickSearchModerationResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
