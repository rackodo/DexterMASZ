import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { ApiEnum } from 'src/app/models/ApiEnum';
import { AutoModConfig } from 'src/app/models/AutoModConfig';
import { AutoModRuleDefinition } from 'src/app/models/AutoModRuleDefinitions';
import { ChannelNotificationBehavior } from 'src/app/models/ChannelNotificationBehavior';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { ApiService } from 'src/app/services/api.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-automod-rule',
  templateUrl: './automod-rule.component.html',
  styleUrls: ['./automod-rule.component.css']
})
export class AutoModRuleComponent implements OnInit {

  eventForm!: FormGroup;
  filterForm!: FormGroup;
  actionForm!: FormGroup;
  @Input() definition!: AutoModRuleDefinition;
  @Input() guildChannels!: DiscordChannel[];
  @Input() guild!: DiscordGuild;
  @Input() initialConfigs!: Promise<AutoModConfig[]>;
  public guildId!: string;
  public enableConfig: boolean = false;
  public tryingToSaveConfig: boolean = false;
  public automodActionOptions: ContentLoading<ApiEnum[]> = { loading: true, content: [] };
  public punishmentTypes: ContentLoading<ApiEnum[]> = { loading: true, content: [] };
  public automodChannelBehaviors: ContentLoading<ApiEnum[]> = { loading: true, content: [] };

  public initRowsCustomWords = 1;

  constructor(private route: ActivatedRoute, private api: ApiService, private toastr: ToastrService, private _formBuilder: FormBuilder, private enumManager: EnumManagerService, private translator: TranslateService) { }

  ngOnInit(): void {
    this.eventForm = this._formBuilder.group({
      limit: ['', this.definition.showLimitField ? Validators.min(-1) : null],
      timeLimit: ['', this.definition.showTimeLimitField ? Validators.min(-1) : null],
      customWord: ['', this.definition.requireCustomField ? Validators.required : null]
    });
    this.filterForm = this._formBuilder.group({
      excludeRoles: [''],
      excludeChannels: ['']
    });
    this.actionForm = this._formBuilder.group({
      dmNotification: [''],
      automodAction: ['', Validators.required],
      punishmentType: [''],
      punishmentDuration: [''],
      channelNotificationBehavior: ['']
    });

    this.actionForm.get('automodAction')?.valueChanges.subscribe(val => {
      if (val >= 2) {
          this.actionForm.controls['punishmentType'].setValidators([Validators.required]);
          this.actionForm.controls['punishmentType'].setValue(0);
          this.actionForm.controls['punishmentType'].updateValueAndValidity();
      } else {
          this.actionForm.controls['punishmentType'].clearValidators();
          this.actionForm.controls['punishmentType'].setValue(null);
          this.actionForm.controls['punishmentType'].updateValueAndValidity();
          this.actionForm.controls['punishmentDuration'].clearValidators();
          this.actionForm.controls['punishmentDuration'].setValue(null);
          this.actionForm.controls['punishmentDuration'].updateValueAndValidity();
      }
      if (val === 1 || val === 3) {
        if (this.actionForm.value.channelNotificationBehavior == 0) {
          this.actionForm.controls['channelNotificationBehavior'].setValue(ChannelNotificationBehavior.SendNotification);
        }
      }
    });

    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;
    this.initialConfigs.then((data: AutoModConfig[]) => {
      // if type in initial loaded configs
      if (data.filter(x => x.autoModType == this.definition.type).length) {
        this.enableConfig = true;
        this.applyConfig(data.filter(x => x.autoModType == this.definition.type)[0]);
      } else {
        this.enableConfig = false;
      }
    });

    this.enumManager.getEnum(ApiEnumTypes.AUTOMODACTION).subscribe((data) => {
      this.automodActionOptions.loading = false;
      this.automodActionOptions.content = data;
    }, (error) => {
      console.error(error);
      this.automodActionOptions.loading = false;
    });

    this.enumManager.getEnum(ApiEnumTypes.PUNISHMENT).subscribe((data) => {
      this.punishmentTypes.loading = false;
      this.punishmentTypes.content = data;
    }, (error) => {
      console.error(error);
      this.punishmentTypes.loading = false;
    });

    this.enumManager.getEnum(ApiEnumTypes.AUTOMODCHANNELNOTIFICATIONBEHAVIOR).subscribe((data) => {
      this.automodChannelBehaviors.loading = false;
      this.automodChannelBehaviors.content = data;
    }, (error) => {
      console.error(error);
      this.automodChannelBehaviors.loading = false;
    });
  }

  generateRoleColor(role: DiscordRole): string {
    return '#' + role.color.toString(16);
  }

  reload() {
    this.enableConfig = false;
    this.api.getSimpleData(`/guilds/${this.guildId}/automodconfig/${this.definition.type}`).subscribe((data) => {
      this.enableConfig = true;
      this.applyConfig(data);
    });
  }

  applyConfig(config: AutoModConfig) {
    this.eventForm.setValue({
      limit: this.definition.showLimitField ? config.limit : '',
      timeLimit: this.definition.showTimeLimitField ? config.timeLimitMinutes : '',
      customWord: this.definition.showCustomField ? config.customWordFilter : ''
    });
    if (this.definition.showCustomField) {
      this.initRowsCustomWords = Math.max(Math.min(config.customWordFilter?.split(/\r\n|\r|\n/)?.length ?? 0, 15), 2);
    }

    this.filterForm.setValue({ excludeRoles: config.ignoreRoles, excludeChannels: config.ignoreChannels });

    this.actionForm.setValue({
      dmNotification: config.sendDmNotification,
      automodAction: config.autoModAction,
      punishmentType: config.punishmentType,
      punishmentDuration: config.punishmentDurationMinutes,
      channelNotificationBehavior: config.channelNotificationBehavior
    });
  }

  onRuleToggle(event: any) {
    if (!event) {
      this.deleteConfig();
    }
  }

  deleteConfig() {
    this.api.deleteData(`/guilds/${this.guildId}/automodconfig/${this.definition.type}`).subscribe(() => {
      this.toastr.success(this.translator.instant('AutoModConfig.ConfigDeleted'));
      this.reload();
    }, error => {
      console.error(error);
      if (error?.error?.status !== 404 && error?.status !== 404) {
        this.toastr.error(this.translator.instant('AutoModConfig.FailedToDeleteConfig'));
      }
      this.reload();
    });
  }

  saveConfig(stepper: MatStepper) {
    this.tryingToSaveConfig = true;
    const data = {
      "AutoModType": this.definition.type,
      "AutoModAction": this.actionForm.value.automodAction,
      "PunishmentType": this.actionForm.value.punishmentType,
      "PunishmentDurationMinutes": this.actionForm.value.punishmentDuration !== "" ? this.actionForm.value.punishmentDuration : null,
      "IgnoreChannels": this.filterForm.value.excludeChannels !== "" ? this.filterForm.value.excludeChannels : [],
      "IgnoreRoles": this.filterForm.value.excludeRoles !== "" ? this.filterForm.value.excludeRoles : [],
      "TimeLimitMinutes": this.eventForm.value.timeLimit !== "" ? this.eventForm.value.timeLimit : null,
      "CustomWordFilter": this.eventForm.value.customWord !== "" ? this.eventForm.value.customWord : null,
      "Limit": this.eventForm.value.limit !== "" ? this.eventForm.value.limit : null,
      "SendDmNotification": this.actionForm.value.dmNotification !== "" ? this.actionForm.value.dmNotification : false,
      "ChannelNotificationBehavior": this.actionForm.value.channelNotificationBehavior !== "" ? this.actionForm.value.channelNotificationBehavior : 0
    }

    this.api.putSimpleData(`/guilds/${this.guildId}/automodconfig`, data).subscribe((data) => {
      this.tryingToSaveConfig = false;
      this.toastr.success(this.translator.instant('AutoModConfig.SavedConfig'));
      this.toastr.success(this.translator.instant('AutoModConfig.SavedConfig'));
      this.applyConfig(data);
      stepper.selectedIndex = 0;
    }, error => {
      console.error(error);
      this.tryingToSaveConfig = false;
      this.toastr.error(this.translator.instant('AutoModConfig.FailedToSaveConfig'))
    });
  }
}
