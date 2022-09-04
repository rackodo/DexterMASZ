import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { GuildAuditLogConfig } from 'src/app/models/GuildAuditLogConfig';
import { GuildAuditLogRuleDefinition } from 'src/app/models/GuildAuditLogRuleDefinition';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-auditlog-config-rule',
  templateUrl: './auditlog-config-rule.component.html',
  styleUrls: ['./auditlog-config-rule.component.css']
})
export class AuditlogConfigRuleComponent implements OnInit {

  configForm!: FormGroup;
  @Input() definition!: GuildAuditLogRuleDefinition;
  @Input() guildChannels!: DiscordChannel[];
  @Input() guild!: DiscordGuild;
  @Input() initialConfigs!: Promise<GuildAuditLogConfig[]>;
  public guildId!: string;
  public enableConfig: boolean = false;
  public tryingToSaveConfig: boolean = false;
  public initStuff: boolean = true;

  public initRowsCustomWords = 1;

  constructor(private route: ActivatedRoute, private api: ApiService, private toastr: ToastrService, private _formBuilder: FormBuilder, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;

    this.configForm = this._formBuilder.group({
      channel: [''],
      pingRoles: [''],
      ignoreRoles: [''],
      ignoreChannels: ['']
    });

    this.configForm.valueChanges.subscribe(() => {
      if (!this.tryingToSaveConfig && !this.initStuff) {
        this.saveConfig();
      }
    });

    this.initialConfigs.then((data: GuildAuditLogConfig[]) => {
      // if type in initial loaded configs
      if (data.filter(x => x.guildAuditLogEvent == this.definition.type).length) {
        this.enableConfig = true;
        this.applyConfig(data.filter(x => x.guildAuditLogEvent == this.definition.type)[0]);
        this.initStuff = false;
      } else {
        this.enableConfig = false;
        this.initStuff = false;
      }
    });
  }

  generateRoleColor(role: DiscordRole): string {
    return '#' + role.color.toString(16);
  }

  reload() {
    this.enableConfig = false;
    this.api.getSimpleData(`/guilds/${this.guildId}/auditlog/${this.definition.type}`).subscribe((data) => {
      this.enableConfig = true;
      this.applyConfig(data);
    });
  }

  applyConfig(config: GuildAuditLogConfig) {
    this.configForm.setValue({
      channel: config.channelId,
      pingRoles: config.pingRoles,
      ignoreRoles: config.ignoreRoles,
      ignoreChannels: config.ignoreChannels
    });
  }

  onRuleToggle(event: any) {
    if (!event) {
      this.deleteConfig();
    }
  }

  deleteConfig() {
    this.tryingToSaveConfig = true;
    this.api.deleteData(`/guilds/${this.guildId}/auditlog/${this.definition.type}`).subscribe(() => {
      this.toastr.success(this.translator.instant('GuildAuditLogConfig.ConfigDeleted'));
      this.configForm.setValue({
        channel: null,
        pingRoles: null,
      });
      this.tryingToSaveConfig = false;
    }, error => {
      console.error(error);
      if (error?.error?.status !== 404 && error?.status !== 404) {
        this.toastr.error(this.translator.instant('GuildAuditLogConfig.FailedToDeleteConfig'));
      }
      this.tryingToSaveConfig = false;
    });
  }

  saveConfig() {
    this.tryingToSaveConfig = true;
    const data = {
      "GuildAuditLogEvent": this.definition.type,
      "ChannelId": this.configForm.value.channel ? this.configForm.value.channel : 0,
      "PingRoles": this.configForm.value.pingRoles ? this.configForm.value.pingRoles : [],
      "IgnoreRoles": this.configForm.value.ignoreRoles ? this.configForm.value.ignoreRoles : [],
      "IgnoreChannels": this.configForm.value.ignoreChannels ? this.configForm.value.ignoreChannels : []
    }

    this.api.putSimpleData(`/guilds/${this.guildId}/auditlog`, data).subscribe((data) => {
      this.toastr.success(this.translator.instant('GuildAuditLogConfig.SavedConfig'));
      this.applyConfig(data);
      this.tryingToSaveConfig = false;
    }, error => {
      console.error(error);
      this.tryingToSaveConfig = false;
      this.toastr.error(this.translator.instant('GuildAuditLogConfig.FailedToSaveConfig'))
    });
  }
}
