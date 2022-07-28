import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuildLeaderboardComponent } from './guild-leaderboard.component';

describe('GuildLeaderboardComponent', () => {
  let component: GuildLeaderboardComponent;
  let fixture: ComponentFixture<GuildLeaderboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuildLeaderboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuildLeaderboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
