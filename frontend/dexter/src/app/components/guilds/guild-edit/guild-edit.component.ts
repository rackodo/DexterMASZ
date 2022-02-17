import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { ApiEnum } from 'src/app/models/ApiEnum';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { GuildConfig } from 'src/app/models/GuildConfig';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { ApiService } from 'src/app/services/api.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-guild-edit',
  templateUrl: './guild-edit.component.html',
  styleUrls: ['./guild-edit.component.css']
})
export class GuildEditComponent implements OnInit {

  public adminRolesGroup!: FormGroup;
  public modRolesGroup!: FormGroup;
  public staffChannelsGroup!: FormGroup;
  public configGroup!: FormGroup;

  public allLanguages: ApiEnum[] = [];

  public currentGuild: ContentLoading<DiscordGuild> = { loading: true, content: {} as DiscordGuild }
  public currentGuildConfig: ContentLoading<GuildConfig> = { loading: true, content: {} as GuildConfig }
  constructor(private api: ApiService, private route: ActivatedRoute, private router: Router, private toastr: ToastrService, private _formBuilder: FormBuilder, private translator: TranslateService, private enumManager: EnumManagerService) { }

  ngOnInit(): void {
    this.adminRolesGroup = this._formBuilder.group({
      adminRoles: ['', Validators.required]
    });
    this.modRolesGroup = this._formBuilder.group({
      modRoles: ['', Validators.required]
    });
    this.staffChannelsGroup = this._formBuilder.group({
      staffChannels: ['']
    });
    this.configGroup = this._formBuilder.group({
      internal: ['', Validators.pattern("^https://discord(app)?\.com/api/webhooks/.+$")],
      public: ['', Validators.pattern("^https://discord(app)?\.com/api/webhooks/.+$")],
      strictPermissionCheck: [''],
      executeWhoIsOnJoin: [''],
      publishModeratorInfo: [''],
      preferredLanguage: ['']
    });

      this.route.paramMap.subscribe(params => {
        const guildId = params.get("guildid");
        this.loadLanguages();
        this.loadGuild(guildId);
        this.loadConfig(guildId);
      });
  }

  generateRoleColor(role: DiscordRole): string {
    return '#' + role.color.toString(16);
  }

  loadGuild(id: string|null) {
    this.currentGuild = { loading: true, content: {} as DiscordGuild };
    this.api.getSimpleData(`/discord/guilds/${id}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.currentGuild = { loading: false, content: data };
    }, error => {
      console.error(error);
      this.currentGuild.loading = false;
      this.toastr.error(this.translator.instant('GuildDialog.FailedToLoadCurrentGuild'));
    });
  }

  loadLanguages() {
    this.enumManager.getEnum(ApiEnumTypes.LANGUAGE).subscribe((data: ApiEnum[]) => {
      this.allLanguages = data;
    });
  }

  loadConfig(id: string|null) {
    this.currentGuildConfig = { loading: true, content: {} as GuildConfig };
    this.api.getSimpleData(`/guilds/${id}`).subscribe((data: GuildConfig) => {
      this.modRolesGroup.setValue({ modRoles: data.modRoles });
      this.adminRolesGroup.setValue({ adminRoles: data.adminRoles});
      this.staffChannelsGroup.setValue({ muteRoles: data.staffChannels});
      this.configGroup.setValue({
        webhook: data.modNotificationWebhook,
        strictPermissionCheck: data.strictModPermissionCheck,
        executeWhoIsOnJoin: data.executeWhoIsOnJoin,
        publishModeratorInfo: data.publishModeratorInfo,
        preferredLanguage: data.preferredLanguage
      });
      this.currentGuildConfig = { loading: false, content: data };
    }, error => {
      console.error(error);
      this.currentGuildConfig.loading = false;
      this.toastr.error(this.translator.instant('GuildDialog.FailedToLoadCurrentGuild'));
    });
  }

  updateGuild() {
    const data = {
      modRoles: this.modRolesGroup.value.modRoles,
      adminRoles: this.adminRolesGroup.value.adminRoles,
      staffChannels: this.staffChannelsGroup.value.staffChannels != '' ? this.staffChannelsGroup.value.staffChannels : [],
      modNotificationWebhook: this.configGroup.value?.internal?.trim() ? this.configGroup?.value?.internal : null,
      strictModPermissionCheck: (this.configGroup.value?.strictPermissionCheck != '' ? this.configGroup.value?.strictPermissionCheck : false) ?? false,
      executeWhoIsOnJoin: (this.configGroup.value?.executeWhoIsOnJoin != '' ? this.configGroup.value?.executeWhoIsOnJoin : false) ?? false,
      publishModeratorInfo: (this.configGroup.value?.publishModeratorInfo != '' ? this.configGroup.value?.publishModeratorInfo : false) ?? false,
      preferredLanguage: this.configGroup.value?.preferredLanguage != '' ? this.configGroup.value?.preferredLanguage : 0
    }

    this.api.putSimpleData(`/guilds/${this.currentGuild?.content?.id}`, data).subscribe(() => {
      this.toastr.success(this.translator.instant('GuildDialog.GuildUpdated'));
      this.router.navigate(['guilds']);
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('GuildDialog.FailedToUpdateGuild'));
    })
  }
}
