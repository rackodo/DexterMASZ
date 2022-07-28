import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { CalcGuildUserLevel } from 'src/app/models/CalcGuildUserLevel';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-guild-leaderboard',
  templateUrl: './guild-leaderboard.component.html',
  styleUrls: ['./guild-leaderboard.component.css']
})
export class GuildLeaderboardComponent implements OnInit {

  guildId = "0";
  order = "total";
  page = 1;
  loadedUsers: CalcGuildUserLevel[] = [];
  canLoadMoreUsersAfter = true;
  DEFAULT_PAGE_SIZE = 100;

  constructor(private api: ApiService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.guildId = this.activatedRoute.snapshot.paramMap.get("guildid") ?? "0";
    this.order = this.activatedRoute.snapshot.queryParamMap.get("order") ?? "total";
    this.page = Number(this.activatedRoute.snapshot.queryParamMap.get("page")) ?? 1;
    if (this.guildId == "0") return;

    this.loadMoreUsersAfter();
  }

  requestPage(page: number) : Observable<CalcGuildUserLevel[]> {
    return this.api.getSimpleData(`/levels/guilds/${this.guildId}/users?order=${this.order}&page=${page}&pageSize=${this.DEFAULT_PAGE_SIZE}`);
  }

  loadMoreUsersBefore() {
    if (this.page <= 1) return;

    this.requestPage(this.page - 1).subscribe(data => {
      this.page--;
      for (let u of this.loadedUsers) {
        data.push(u);
      }
      this.loadedUsers = data;
    })
  }

  loadMoreUsersAfter() {
    if (!this.canLoadMoreUsersAfter) return;

    let newPage = this.page - 1 + (this.loadedUsers.length / this.DEFAULT_PAGE_SIZE);
    this.requestPage(newPage).subscribe(data => {
      if (data.length < this.DEFAULT_PAGE_SIZE) this.canLoadMoreUsersAfter = false;

      for (let u of data) {
        this.loadedUsers.push(u);
      }
    })
  }

}
