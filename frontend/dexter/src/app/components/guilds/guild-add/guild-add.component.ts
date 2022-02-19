import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { ApiEnum } from 'src/app/models/ApiEnum';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { DiscordApplication } from 'src/app/models/DiscordApplication';
import { ApiService } from 'src/app/services/api.service';
import { ApplicationInfoService } from 'src/app/services/application-info.service';
import { AuthService } from 'src/app/services/auth.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-guild-add',
  templateUrl: './guild-add.component.html',
  styleUrls: ['./guild-add.component.css']
})
export class GuildAddComponent implements OnInit {

  public rolesGroup!: FormGroup;
  public channelsGroup!: FormGroup;
  public configGroup!: FormGroup;

  public queryGroup!: FormGroup;

  public guilds!: ContentLoading<DiscordGuild[]>;
  public searchGuilds!: string;
  public showGuilds: DiscordGuild[] = [];
  public clientId!: string;

  public selectedGuild: DiscordGuild|undefined;
  public selectedGuildDetails!: ContentLoading<DiscordGuild>;

  public allLanguages: ApiEnum[] = [];

  constructor(private _formBuilder: FormBuilder, private api: ApiService, private toastr: ToastrService, private authService: AuthService, private router: Router, private applicationInfoService: ApplicationInfoService, private translator: TranslateService, private enumManager: EnumManagerService) { }

  ngOnInit(): void {
    this.rolesGroup = this._formBuilder.group({
      adminRoles: ['', Validators.required],
      modRoles: ['', Validators.required]
    });
    this.channelsGroup = this._formBuilder.group({
      staffChannels: [''],
      botChannels: ['']
    });
    this.configGroup = this._formBuilder.group({
      staff: ['', Validators.pattern("^https://discord(app)?\.com/api/webhooks/.+$")],
      admin: ['', Validators.pattern("^https://discord(app)?\.com/api/webhooks/.+$")],
      strictPermissionCheck: [''],
      executeWhoIsOnJoin: [''],
      publishModeratorInfo: [''],
      preferredLanguage: [''],
    });
    this.queryGroup = this._formBuilder.group({
      importExistingBans: [''],
    });
    this.applicationInfoService.currentApplicationInfo.subscribe((data: DiscordApplication) => {
      this.clientId = data.id;
    });
    this.loadLanguages();
    this.reloadGuilds();
  }

  reloadGuilds() {
    this.searchGuilds = '';
    this.guilds = { loading: true, content: [] };
    this.showGuilds = [];

    this.api.getSimpleData('/discord/guilds').subscribe(data => {
      this.guilds = { loading: false, content: data };
      this.showGuilds = data;
    }, error => {
      console.error(error);
      this.guilds.loading = false;
      this.toastr.error(this.translator.instant('GuildDialog.FailedToLoadGuilds'));
    });
  }

  loadLanguages() {
    this.enumManager.getEnum(ApiEnumTypes.LANGUAGE).subscribe((data: ApiEnum[]) => {
      this.allLanguages = data;
    });
  }

  onSearch() {
    if (this.searchGuilds.trim() === '') {
      this.showGuilds = this.guilds.content as DiscordGuild[];
    }
    this.showGuilds = this.guilds.content?.filter(x => x.name.toLowerCase().includes(this.searchGuilds.toLowerCase()) || x.id.toString().includes(this.searchGuilds)) as DiscordGuild[];
  }

  resetSearch() {
    this.searchGuilds = '';
    this.onSearch();
  }

  generateRoleColor(role: DiscordRole): string {
    return '#' + role.color.toString(16);
  }

  selectGuild(id: string) {
    this.searchGuilds = '';
    this.selectedGuild = this.guilds.content?.find(x => x.id === id) as DiscordGuild;
    this.selectedGuildDetails = { loading: true, content: undefined };
    this.api.getSimpleData(`/discord/guilds/${id}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.selectedGuildDetails = { loading: false, content: data };
    }, error => {
      console.error(error);
      this.selectedGuildDetails.loading = false;
    });
  }

  invite() {
    this.selectedGuildDetails = { loading: true, content: undefined };
    var win = window.open(
      `https://discord.com/oauth2/authorize?client_id=${this.clientId}&permissions=8&scope=bot%20applications.commands&guild_id=${this.selectedGuild?.id}`,
      "Secure Payment", "status=yes;width=150,height=400");
    if (win === null) {
      this.toastr.error(this.translator.instant('GuildDialog.FailedInviteWindow'));
      return;
    }
    var timer = setInterval(function(callback: any, id: string, context: any) {
      if (win?.closed) {
          clearInterval(timer);
          callback.bind(context, id)();
      }
    }, 500, this.selectGuild, this.selectedGuild?.id, this);
  }

  registerGuild() {
    const data = {
      guildid:                        this.selectedGuild?.id,
      modRoles:                       this.rolesGroup.value.modRoles,
      adminRoles:                     this.rolesGroup.value.adminRoles,
      staffChannels:                  this.channelsGroup.value.staffChannels,
      botChannels:                    this.channelsGroup.value.botChannels,
      staffWebhook:         		  this.configGroup.value?.staff?.trim()         != '' ? this.configGroup?.value?.staff           		       : null,
	  adminWebhook:         		  this.configGroup.value?.admin?.trim()         != '' ? this.configGroup?.value?.admin           		       : null,
      strictModPermissionCheck:       this.configGroup.value?.strictPermissionCheck != '' ? this.configGroup.value?.strictPermissionCheck ?? false : false,
      executeWhoIsOnJoin:             this.configGroup.value?.executeWhoIsOnJoin    != '' ? this.configGroup.value?.executeWhoIsOnJoin    ?? false : false,
      publishModeratorInfo:           this.configGroup.value?.publishModeratorInfo  != '' ? this.configGroup.value?.publishModeratorInfo  ?? false : false,
      preferredLanguage:              this.configGroup.value?.preferredLanguage     != '' ? this.configGroup.value?.preferredLanguage     ?? null  : null
    };
    let params = new HttpParams()
                      .set("importExistingBans", this.queryGroup.value?.importExistingBans ? 'true' : 'false');
    this.api.postSimpleData('/guilds/', data, params).subscribe(() => {
      this.toastr.success(this.translator.instant('GuildDialog.GuildCreated'));
      this.authService.resetCache();
      this.router.navigate(['guilds']);
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('GuildDialog.FailedToRegisterGuild'));
    })
  }

  unselectGuild() {
    this.selectedGuild = undefined;
    this.selectedGuildDetails = { loading: true, content: undefined };
    this.reloadGuilds();
    this.rolesGroup.reset();
    this.channelsGroup.reset();
    this.configGroup.reset();
  }
}
