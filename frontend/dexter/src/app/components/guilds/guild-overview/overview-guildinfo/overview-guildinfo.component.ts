import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { ApiService } from 'src/app/services/api.service';
import { GuildOverviewComponent } from '../guild-overview.component';

@Component({
  selector: 'app-dashboard-guildinfo',
  templateUrl: './overview-guildinfo.component.html',
  styleUrls: ['./overview-guildinfo.component.css']
})
export class GuildInfoComponent implements OnInit {

  private guildId!: string;
  public guild: ContentLoading<DiscordGuild> = { loading: true, content: undefined };
  
  constructor(private route: ActivatedRoute, private api: ApiService, public overview: GuildOverviewComponent, private toastr: ToastrService, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;
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
