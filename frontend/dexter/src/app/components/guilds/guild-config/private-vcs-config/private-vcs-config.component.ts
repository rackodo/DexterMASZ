import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { GuildPrivateVcConfig } from 'src/app/models/GuildPrivateVcConfig';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-private-vcs-config',
  templateUrl: './private-vcs-config.component.html',
  styleUrls: ['./private-vcs-config.component.css']
})
export class PrivateVcsConfigComponent implements OnInit {

  public guildId!: string;
  public guildInfo!: DiscordGuild;
  public guildRoles!: DiscordRole[];
  public guildChannels!: DiscordChannel[];
  public initialConfig!: Promise<GuildPrivateVcConfig>;

  formGroup!: FormGroup;
  advFormGroup!: FormGroup;

  tryingToSaveConfig = false;

  TEMPLATE_DEFAULT = "Waiting Room"
  TEMPLATE_CHARACTER_LIMIT = 100;

  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private _formBuilder: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.guildId = params.get('guildid') as string;
      this.reload();
    });

    this.formGroup = this._formBuilder.group({
      allowedRoles: new FormControl([]),
      creatorRoles: new FormControl([]),
      waitingVcName: new FormControl(this.TEMPLATE_DEFAULT, [Validators.required, Validators.maxLength(this.TEMPLATE_CHARACTER_LIMIT)]),
      channelFilterRegex: new FormControl(""),
      privateCategoryId: new FormControl('0')
    });
  }

  reload() {
    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.guildInfo = data;
      this.guildRoles = data.roles;
    }, () => {
      this.toastr.error("Failed to load guild data");
    });

    this.api.getSimpleData(`/discord/guilds/${this.guildId}/textChannels`).subscribe((data: DiscordChannel[]) => {
      this.guildChannels = data.sort((a, b) => (a.position > b.position) ? 1 : -1);
    }, () => {
      this.toastr.error("Failed to load channels");
    });

    this.initialConfig = this.api.getSimpleData(`/guilds/${this.guildId}/privatevcconfig`).toPromise();
    this.initialConfig.then((data : GuildPrivateVcConfig) => {
      this.applyConfig(data);
    }).catch((err) => {
      this.toastr.error("Failed to load existing private vc configuration: " + err.message);
    })
  }

  applyConfig(config: GuildPrivateVcConfig) {
    this.formGroup.setValue({
      waitingVcName: config.waitingVcName ?? this.TEMPLATE_DEFAULT,
      privateCategoryId: config.privateCategoryId ?? 0,
      allowedRoles: config.allowedRoles ?? [],
      creatorRoles: config.creatorRoles ?? [],
      channelFilterRegex: config.channelFilterRegex ?? "",
    })
  }

  saveConfig() {
    this.tryingToSaveConfig = true;
    const data: GuildPrivateVcConfig = {
      "id": this.guildId,
      "waitingVcName": this.formGroup.value.levelUpTemplate ?? this.TEMPLATE_DEFAULT,
      "allowedRoles": this.formGroup.value.allowedRoles,
      "privateCategoryId": this.formGroup.value.privateCategoryId,
      "creatorRoles": this.formGroup.value.creatorRoles,
      "channelFilterRegex": this.formGroup.value.channelFilterRegex
    }

    this.api.putSimpleData(`/guilds/${this.guildId}/privatevcconfig`, data).subscribe((data) => {
      this.tryingToSaveConfig = false;
      this.toastr.success("Private Vc Configuration Saved Successfully");
    }, error => {
      console.error(error);
      this.tryingToSaveConfig = false;
      this.toastr.error("Failed to save Private Vc Configuration: " + error.message);
    });
  }
}
