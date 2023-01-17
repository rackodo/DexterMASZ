import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { GuildAuditLogConfig } from 'src/app/models/GuildAuditLogConfig';
import { GuildAuditLogRuleDefinition } from 'src/app/models/GuildAuditLogRuleDefinition';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-auditlog-config',
  templateUrl: './auditlog-config.component.html',
  styleUrls: ['./auditlog-config.component.css']
})
export class AuditlogConfigComponent implements OnInit {

  types: GuildAuditLogRuleDefinition[] = [
    {
      type: 0,
      key: 'MessageSent',
      channelFilter: true,
      roleFilter: true
    },
    {
      type: 1,
      key: 'MessageUpdated',
      channelFilter: true,
      roleFilter: true
    },
    {
      type: 2,
      key: 'MessageDeleted',
      channelFilter: true,
      roleFilter: true
    },
    {
      type: 3,
      key: 'UsernameUpdated'
    },
    {
      type: 4,
      key: 'AvatarUpdated',
      roleFilter: true
    },
    {
      type: 5,
      key: 'NicknameUpdated',
      roleFilter: true
    },
    {
      type: 6,
      key: 'UserRolesUpdated',
      roleFilter: true
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
      key: 'InviteCreated',
      channelFilter: true
    },
    {
      type: 12,
      key: 'InviteDeleted',
      channelFilter: true
    },
    {
      type: 13,
      key: 'ThreadCreated',
      channelFilter: true
    },
    {
      type: 14,
      key: 'VoiceJoined',
      channelFilter: true
    },
    {
      type: 15,
      key: 'VoiceLeft',
      channelFilter: true
    },
    {
      type: 16,
      key: 'VoiceMoved',
      channelFilter: true
    },
    {
      type: 17,
      key: 'ReactionAdded',
      channelFilter: true
    },
    {
      type: 18,
      key: 'ReactionRemoved',
      channelFilter: true
    }
  ];

  public guildId!: string;
  public guildInfo!: DiscordGuild;
  public guildChannels!: DiscordChannel[];
  public initialConfigs!: Promise<GuildAuditLogConfig[]>;

  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private translator: TranslateService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.guildId = params.get('guildid') as string;
      this.reload();
    });
  }

  reload() {
    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.guildInfo = data;
    }, () => {
      this.toastr.error(this.translator.instant('GuildAuditLogConfig.FailedToLoadGuild'));
    });

    this.api.getSimpleData(`/discord/guilds/${this.guildId}/textableChannels`).subscribe((data: DiscordChannel[]) => {
      this.guildChannels = data.sort((a, b) => (a.position > b.position) ? 1 : -1);
    }, () => {
      this.toastr.error(this.translator.instant('GuildAuditLogConfig.FailedToLoadChannels'));
    });

    this.initialConfigs = this.api.getSimpleData(`/guilds/${this.guildId}/auditlog`).toPromise();
  }
}
