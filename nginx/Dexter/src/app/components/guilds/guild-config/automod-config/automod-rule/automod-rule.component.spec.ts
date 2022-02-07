import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoModRuleComponent } from './automod-rule.component';

describe('AutoModRuleComponent', () => {
  let component: AutoModRuleComponent;
  let fixture: ComponentFixture<AutoModRuleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoModRuleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoModRuleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
