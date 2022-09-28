import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { TranslateService } from '@ngx-translate/core';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { ContentLoading } from 'src/app/models/ContentLoading';

@Component({
  selector: 'app-guild-leaderboard',
  templateUrl: './guild-leaderboard.component.html',
  styleUrls: ['./guild-leaderboard.component.css']
})
export class GuildLeaderboardComponent implements OnInit {

  selectedTab = new FormControl(1);
  orders = ["text", "total", "voice"];
  icons = ["text_fields", "star", "keyboard_voice"]
  themes = ["5da25f", "80ced6", "ffca54"]

  private guildId!: string;
  public guild: ContentLoading<DiscordGuild> = { loading: true, content: undefined };
  
  constructor(private api: ApiService, private route: ActivatedRoute, private toastr : ToastrService, private translator: TranslateService) {}

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get("guildid") ?? "0";

    let index = this.orders.indexOf(this.route.snapshot.queryParamMap.get("order") ?? "total");
    this.selectedTab.setValue(index < 0 ? 1 : index);
	
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
  }
}
