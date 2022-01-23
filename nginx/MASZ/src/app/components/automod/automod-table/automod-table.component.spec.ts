import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoModTableComponent } from './automod-table.component';

describe('AutoModTableComponent', () => {
  let component: AutoModTableComponent;
  let fixture: ComponentFixture<AutoModTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoModTableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AutoModTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
