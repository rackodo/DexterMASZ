import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuickSearchUserMapsResultComponent } from './quicksearch-usermaps-result.component';

describe('QuickSearchUserMapsResultComponent', () => {
  let component: QuickSearchUserMapsResultComponent;
  let fixture: ComponentFixture<QuickSearchUserMapsResultComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QuickSearchUserMapsResultComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QuickSearchUserMapsResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
