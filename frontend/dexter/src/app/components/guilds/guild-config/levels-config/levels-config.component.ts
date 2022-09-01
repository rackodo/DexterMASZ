import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { NumberInputDialogComponent } from 'src/app/components/dialogs/number-input-dialog/number-input-dialog.component';
import { DiscordChannel } from 'src/app/models/DiscordChannel';
import { DiscordGuild } from 'src/app/models/DiscordGuild';
import { DiscordRole } from 'src/app/models/DiscordRole';
import { GuildLevelConfig } from 'src/app/models/GuildLevelConfig';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-levels-config',
  templateUrl: './levels-config.component.html',
  styleUrls: ['./levels-config.component.css']
})
export class LevelsConfigComponent implements OnInit {

  public guildId!: string;
  public guildInfo!: DiscordGuild;
  public guildRoles!: DiscordRole[];
  public guildChannels!: DiscordChannel[];
  public initialConfig!: Promise<GuildLevelConfig>;

  formGroup!: FormGroup;
  advFormGroup!: FormGroup;

  levels : Record<number, string[]> = {};
  levelUpMessageOverrides : Record<number, string> = {};
  levelsKeys : number[] = [];
  overrideKeys : number[] = [];
  tryingToSaveConfig = false;

  TEMPLATE_DEFAULT = "{USER} reached level {LEVEL}!";
  TEMPLATE_CHARACTER_LIMIT = 250;

  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private _formBuilder: FormBuilder, public dialog: MatDialog) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.guildId = params.get('guildid') as string;
      this.reload();
    });

    const minTextXp = new FormControl(15, [Validators.min(0), Validators.max(1000), Validators.required]);
    const minVoiceXp = new FormControl(10, [Validators.min(0), Validators.max(1000), Validators.required]);

    this.formGroup = this._formBuilder.group({
      xpInterval: new FormControl(60, [Validators.min(10), Validators.max(3600), Validators.required]),
      minTextXp: minTextXp,
      minVoiceXp: minVoiceXp,
      voiceXpRequiredMembers: new FormControl(3, [Validators.required]),
      voiceXpCountMutedMembers: new FormControl(false),
      handleRoles: new FormControl(false),
      disabledXpChannels: new FormControl([]),
      nicknameDisabledRole: new FormControl('0'),
      nicknameDisabledReplacement: new FormControl('0'),
      levelUpTemplate: new FormControl("{USER} reached level {LVL}!", [Validators.required, Validators.maxLength(this.TEMPLATE_CHARACTER_LIMIT)]),
      sendTextLevelUps: new FormControl(true),
      sendVoiceLevelUps: new FormControl(false),
      textLevelUpChannel: new FormControl('0'),
      voiceLevelUpChannel: new FormControl('0')
    });
    this.formGroup.addControl("maxTextXp", new FormControl(25, [Validators.min(0), Validators.max(1000), Validators.required, this.validRange(minTextXp)]));
    this.formGroup.addControl("maxVoiceXp", new FormControl(15, [Validators.min(0), Validators.max(1000), Validators.required, this.validRange(minVoiceXp)]));

    minTextXp.valueChanges.subscribe(() => {
      this.formGroup.get("maxTextXp")?.updateValueAndValidity();
    })
    minVoiceXp.valueChanges.subscribe(() => {
      this.formGroup.get("maxVoiceXp")?.updateValueAndValidity();
    })

    this.advFormGroup = this._formBuilder.group({
      coefficients: new FormControl('75.83333, 22.5, 1.66667', [Validators.required, this.validCoefficients])
    })
  }

  reload() {
    this.api.getSimpleData(`/discord/guilds/${this.guildId}`).subscribe((data: DiscordGuild) => {
      data.roles = data.roles.sort((a, b) => (a.position < b.position) ? 1 : -1);
      this.guildInfo = data;
      this.guildRoles = data.roles;
    }, () => {
      this.toastr.error("Failed to load guild data");
    });

    this.api.getSimpleData(`/discord/guilds/${this.guildId}/channels`).subscribe((data: DiscordChannel[]) => {
      this.guildChannels = data.filter(x => x.type === 0).sort((a, b) => (a.position > b.position) ? 1 : -1);
    }, () => {
      this.toastr.error("Failed to load channels");
    });

    this.initialConfig = this.api.getSimpleData(`/guilds/${this.guildId}/levels`).toPromise();
    this.initialConfig.then((data : GuildLevelConfig) => {
      this.applyConfig(data);
    }).catch((err) => {
      this.toastr.error("Failed to load existing leveling configuration: " + err.message);
    })
  }

  applyConfig(config: GuildLevelConfig) {
    this.formGroup.setValue({
      xpInterval: config.xpInterval,
      minTextXp: config.minimumTextXpGiven,
      maxTextXp: config.maximumTextXpGiven,
      minVoiceXp: config.minimumVoiceXpGiven,
      maxVoiceXp: config.maximumVoiceXpGiven,
      disabledXpChannels: config.disabledXpChannels,
      voiceXpRequiredMembers: config.voiceXpRequiredMembers,
      voiceXpCountMutedMembers: config.voiceXpCountMutedMembers,
      handleRoles: config.handleRoles,
      nicknameDisabledRole: config.nicknameDisabledRole,
      nicknameDisabledReplacement: config.nicknameDisabledReplacement,
      levelUpTemplate: config.levelUpTemplate,
      sendTextLevelUps: config.sendTextLevelUps,
      sendVoiceLevelUps: config.sendVoiceLevelUps,
      textLevelUpChannel: config.textLevelUpChannel,
      voiceLevelUpChannel: config.voiceLevelUpChannel
    })

    this.levels = config.levels;
    this.levelsKeys = Object.keys(this.levels).map(n => Number(n));
    this.levelUpMessageOverrides = config.levelUpMessageOverrides;
    this.overrideKeys = Object.keys(this.levelUpMessageOverrides).map(n => Number(n));
  }

  saveConfig() {
    this.tryingToSaveConfig = true;
    const data: GuildLevelConfig = {
      "id": this.guildId,
      "coefficients": [0].concat((this.advFormGroup.value.coefficients as string)?.split(",").map(n => Number(n)) ?? [75.83333, 22.5, 1.66667]),
      "xpInterval": this.formGroup.value.xpInterval ?? 60,
      "minimumTextXpGiven": this.formGroup.value.minTextXp ?? 15,
      "maximumTextXpGiven": this.formGroup.value.maxTextXp ?? 25,
      "minimumVoiceXpGiven": this.formGroup.value.minVoiceXp ?? 10,
      "maximumVoiceXpGiven": this.formGroup.value.maxVoiceXp ?? 15,
      "levelUpTemplate": this.formGroup.value.levelUpTemplate ?? this.TEMPLATE_DEFAULT,
      "voiceLevelUpChannel": this.formGroup.value.voiceLevelUpChannel,
      "textLevelUpChannel": this.formGroup.value.textLevelUpChannel,

      "disabledXpChannels": this.formGroup.value.disabledXpChannels,
      "levels": this.levels,
      "levelUpMessageOverrides": this.levelUpMessageOverrides,

      "handleRoles": this.formGroup.value.handleRoles ?? false,
      "sendTextLevelUps": this.formGroup.value.sendTextLevelUps ?? false,
      "sendVoiceLevelUps": this.formGroup.value.sendVoiceLevelUps ?? false,
      "voiceXpCountMutedMembers": this.formGroup.value.voiceXpCountMutedMembers ?? false,
      "voiceXpRequiredMembers": this.formGroup.value.voiceXpRequiredMembers ?? 3,

      "nicknameDisabledRole": this.formGroup.value.nicknameDisabledRole,
      "nicknameDisabledReplacement": this.formGroup.value.nicknameDisabledReplacement
    }

    this.api.putSimpleData(`/guilds/${this.guildId}/levels`, data).subscribe((data) => {
      this.tryingToSaveConfig = false;
      this.toastr.success("Level Configuration Saved Successfully");
    }, error => {
      console.error(error);
      this.tryingToSaveConfig = false;
      this.toastr.error("Failed to save Level Configuration: " + error.message);
    });
  }

  validRange(minControl: AbstractControl | null): ValidatorFn {
    if (minControl == null) return (_) => null
    return ((control) => {
      if (minControl.value > control.value) {
        return {"range": {min: minControl.value, actual: control.value}};
      }
      else {
        return null;
      }
    })
  }

  validCoefficients(control: AbstractControl): ValidationErrors | null {
    let nums = (control.value as string).split(',');
    if (nums.length < 1) return {"count": "You must include at least one coefficient"};
    if (nums.length > 9) return {"count": "You must include at most nine coefficients"};
    for (let n of nums) {
      const parsed = Number(n);
      if (parsed === NaN) return {"parse": `Invalid value: "${n}". Values must all be numeric`};
      if (parsed <= 0) return {"nonpositive": "All given values must be strictly greater than zero."}
    }

    return null;
  }

  allErrors(control: AbstractControl | null) : string[] {
    if (control == null) return ["Control Not Found"];
    const errors = control.errors;
    if (errors == null) return [];

    if (errors["required"])
      return ["This input is required"]
    else if (errors["range"])
      return ["Must be at least " + errors["range"].min]

    let res : string[] = [];
    for (let k of Object.keys(errors)) {
      if (k == "min")
        res.push("Value must be at least " + errors[k].min);
      else if (k == "max")
        res.push("Value must be at most " + errors[k].max);
      else if (k == "minLength")
        res.push("The given value is too short.")
      else if (k == "maxLength")
        res.push("The given value is too long.")
      else
        res.push(`${k}: ${JSON.stringify(errors[k])}`);
    }
    return res;
  }

  asItems<K extends string | number, T>(dict: Record<K, T>): {key: K, value: T}[] {
    let res = [];
    for (let kstr of Object.keys(dict)) {
      let k = kstr as K;
      res.push({key: k, value: dict[k]})
    }
    return res.sort((a, b) => a.key < b.key ? -1 : 1);
  }

  generateRoleColor(role: DiscordRole): string {
    return '#' + role.color.toString(16);
  }

  requestLevelDialog(title: string, forbidden: number[]): Observable<number> {
    console.log("Opening dialog with title: " + title);
    let ref = this.dialog.open(NumberInputDialogComponent, {
      data: {value: 0, title: title, min: 0, label: "Level", forbidden: forbidden}
    });

    return ref.afterClosed();
  }

  addItemOverrides() {
    let keys = Object.keys(this.levelUpMessageOverrides).map(k => Number(k));
    this.requestLevelDialog("Add Override", keys).subscribe((n) => {
      console.log("Detected dialog closing with n = " + n)
      if (n === undefined || n < 0) return;
      if (keys.includes(n)) return;
      this.levelUpMessageOverrides[n] = this.TEMPLATE_DEFAULT;
      this.overrideKeys.push(n);
      this.overrideKeys.sort((a, b) => Number(a) < Number(b) ? -1 : 1);
    })
  }

  removeItemOverride(key: number) {
    delete this.levelUpMessageOverrides[key];
    this.overrideKeys.splice(this.overrideKeys.indexOf(key), 1);
  }

  addItemRoles() {
    let keys = Object.keys(this.levels).map(k => Number(k));
    this.requestLevelDialog("Add Ranked Role", keys).subscribe((n) => {
      if (n === undefined) return;
      if (keys.includes(n)) return;
      this.levels[n] = [];
      this.levelsKeys.push(n);
      this.levelsKeys.sort((a, b) => Number(a) < Number(b) ? -1 : 1);
    })
  }

  removeItemRoles(key: number) {
    delete this.levels[key];
    this.levelsKeys.splice(this.levelsKeys.indexOf(key), 1);
  }
}
