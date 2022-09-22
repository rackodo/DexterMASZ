import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-punishment-config',
  templateUrl: './punishment-config.component.html',
  styleUrls: ['./punishment-config.component.css']
})
export class PunishmentConfigComponent implements OnInit {
  public guildId!: string;
  public guildInfo!: DiscordGuild;
  public guildChannels!: DiscordChannel[];

  public finalWarnMuteTime: string = "";

  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;
    this.reload();
  }

  updateFinalMuteTime() {
    
  }

  reload() {
    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.guildInfo = data;
    }, () => {
      this.toastr.error(this.translator.instant('AutoModConfig.FailedToLoadGuild'));
    });
  }
}
