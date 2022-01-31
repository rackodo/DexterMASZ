import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { GuildAuditConfig } from 'src/app/models/GuildAuditConfig';
import { GuildAuditRuleDefinition } from 'src/app/models/GuildAuditRuleDefinition';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-auditlog-config',
  templateUrl: './auditlog-config.component.html',
  styleUrls: ['./auditlog-config.component.css']
})
export class AuditlogConfigComponent implements OnInit {

  types: GuildAuditRuleDefinition[] = [
    {
      type: 0,
      key: 'MessageSent'
    },
    {
      type: 1,
      key: 'MessageUpdated'
    },
    {
      type: 2,
      key: 'MessageDeleted'
    },
    {
      type: 3,
      key: 'UsernameUpdated'
    },
    {
      type: 4,
      key: 'AvatarUpdated'
    },
    {
      type: 5,
      key: 'NicknameUpdated'
    },
    {
      type: 6,
      key: 'UserRolesUpdated'
    },
    {
      type: 7,
      key: 'UserJoined'
    },
    {
      type: 8,
      key: 'UserRemoved'
    },
    {
      type: 9,
      key: 'BanAdded'
    },
    {
      type: 10,
      key: 'BanRemoved'
    },
    {
      type: 11,
      key: 'InviteCreated'
    },
    {
      type: 12,
      key: 'InviteDeleted'
    },
    {
      type: 13,
      key: 'ThreadCreated'
    }
  ];

  public guildId!: string;
  public guildInfo!: DiscordGuild;
  public guildChannels!: DiscordChannel[];
  public initialConfigs!: Promise<GuildAuditConfig[]>;

  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private translator: TranslateService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.guildId = BigInt(params.get('guildid'));
      this.reload();
    });
  }

  reload() {
    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.guildInfo = data;
    }, () => {
      this.toastr.error(this.translator.instant('GuildAuditConfig.FailedToLoadGuild'));
    });

    this.api.getSimpleData(`/discord/guilds/${this.guildId}/channels`).subscribe((data: DiscordChannel[]) => {
      this.guildChannels = data.filter(x => x.type === 0).sort((a, b) => (a.position > b.position) ? 1 : -1);
    }, () => {
      this.toastr.error(this.translator.instant('GuildAuditConfig.FailedToLoadChannels'));
    });

    this.initialConfigs = this.api.getSimpleData(`/guilds/${this.guildId}/auditlog`).toPromise();
  }
}
