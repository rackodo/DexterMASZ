import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { CalcGuildUserLevel } from 'src/app/models/CalcGuildUserLevel';
import { ApiService } from 'src/app/services/api.service';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { ContentLoading } from 'src/app/models/ContentLoading';

@Component({
  selector: 'app-leaderboard-ranking',
  templateUrl: './leaderboard-ranking.component.html',
  styleUrls: ['./leaderboard-ranking.component.css']
})
export class LeaderboardRankingComponent implements OnInit {

  @Input() order : string = "total";

  TESTING = false;

  private guildId!: string;
  public guild: ContentLoading<DiscordGuild> = { loading: true, content: undefined };

  page = 1;
  loading = false;
  loadingAfter = false;
  loadedUsers: CalcGuildUserLevel[] = [];
  canLoadMoreUsersAfter = true;
  DEFAULT_PAGE_SIZE = 100;

  constructor(private api: ApiService, private route: ActivatedRoute, private toastr : ToastrService) {}

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get("guildid") ?? "0";

    this.reload();
  }

  private reload() {
    this.guild = { loading: true, content: undefined };

    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe(data => {
      this.guild = { loading: false, content: data };
    }, error => {
      console.error(error);
      this.guild.loading = false;
      this.toastr.error(this.translator.instant("GuildInfoCard.FailedToLoad"));
    });
	
    this.page = Number(this.route.snapshot.queryParamMap.get("page"));
    if (this.page < 1) this.page = 1;
    if (this.guildId == "0") return;

    this.loadMoreUsersAfter();
  }

  private requestPage(page: number) : Observable<CalcGuildUserLevel[]> {
    this.loading = true;
    const result = this.api.getSimpleData(`/levels/guilds/${this.guildId}/users?order=${this.order}&page=${page}&pageSize=${this.DEFAULT_PAGE_SIZE}`);
    result.subscribe(() => {}, (err) => {
      this.toastr.error("Request timed out, please try again after a few seconds.");
      this.loading = false;
    });
    return result;
  }

  public loadMoreUsersBefore() {
    if (this.page <= 1) {this.loading = false; return};
    this.loadingAfter = false;

    this.requestPage(this.page - 1).subscribe(data => {
      this.page--;
      for (let u of this.loadedUsers) {
        data.push(u);
      }
      this.loadedUsers = data;
      this.loading = false;
    })
  }

  public loadMoreUsersAfter() {
    if (!this.canLoadMoreUsersAfter) {this.loading = false; return};
    this.loadingAfter = true;

    let newPage = this.page + (this.loadedUsers.length / this.DEFAULT_PAGE_SIZE);
    this.requestPage(newPage).subscribe(data => {
      if (data.length < this.DEFAULT_PAGE_SIZE)
		this.canLoadMoreUsersAfter = false;

      for (let u of data) {
        this.loadedUsers.push(u);
      }
      this.loading = false;
    })
  }
}
