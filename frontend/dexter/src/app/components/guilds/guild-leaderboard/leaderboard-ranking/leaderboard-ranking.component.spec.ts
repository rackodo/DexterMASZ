import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeaderboardRankingComponent } from './leaderboard-ranking.component';

describe('LeaderboardRankingComponent', () => {
  let component: LeaderboardRankingComponent;
  let fixture: ComponentFixture<LeaderboardRankingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LeaderboardRankingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LeaderboardRankingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
