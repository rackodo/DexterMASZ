import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuildUserNotesComponent } from './guild-usernotes.component';

describe('GuildUserNotesComponent', () => {
  let component: GuildUserNotesComponent;
  let fixture: ComponentFixture<GuildUserNotesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuildUserNotesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuildUserNotesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
