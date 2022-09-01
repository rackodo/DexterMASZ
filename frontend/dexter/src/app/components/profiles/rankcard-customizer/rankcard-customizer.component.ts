import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { UserRankcardConfigUtility } from 'src/app/classes/UserRankcardConfig';
import { AppUser } from 'src/app/models/AppUser';
import { DiscordUser } from 'src/app/models/DiscordUser';
import { RankcardFlags, UserRankcardConfig } from 'src/app/models/UserRankcardConfig';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { API_URL } from '../../../config/config';
import { ImageUrlDialogComponent } from '../../dialogs/image-url-dialog/image-url-dialog.component';
import { OffsetEditorSettings } from './offset-editor/offset-editor.component';

@Component({
  selector: 'app-rankcard-customizer',
  templateUrl: './rankcard-customizer.component.html',
  styleUrls: ['./rankcard-customizer.component.css']
})
export class RankcardCustomizerComponent implements AfterViewInit {

  constructor(private api: ApiService, private auth: AuthService, private toastr: ToastrService, private cd: ChangeDetectorRef, public dialog: MatDialog) { }

  @ViewChild("xpColorSelector") xpColorSelector!: ElementRef;
  @ViewChild("xpColorSlider") xpColorSlider!: ElementRef;
  @ViewChild("offColorSelector") offColorSelector!: ElementRef;
  @ViewChild("offColorSlider") offColorSlider!: ElementRef;
  @ViewChild("levelBgColorSelector") levelBgColorSelector!: ElementRef;
  @ViewChild("levelBgColorSlider") levelBgColorSlider!: ElementRef;
  @ViewChild("titleBgColorSelector") titleBgColorSelector!: ElementRef;
  @ViewChild("titleBgColorSlider") titleBgColorSlider!: ElementRef;

  @ViewChild("backgroundColorSelector") backgroundColorSelector!: ElementRef;
  @ViewChild("backgroundCustomSelector") backgroundCustomSelector!: ElementRef;
  @ViewChild("backgroundDefaultSelector") backgroundDefaultSelector!: ElementRef;

  @ViewChild("fileInput") fileInput!: ElementRef;

  model : UserRankcardConfig = UserRankcardConfigUtility.default;

  user : DiscordUser | undefined;
  username: string = "Username#0123";
  pfp: string = "/assets/img/default_profile.png";
  userId : bigint = 0n;

  rankcardSize = {x: 1350, y: 450};
  titleSize = {x: this.rankcardSize.x, y: 100};
  levelsSize = {x: 1000, y: this.rankcardSize.y - this.titleSize.y};
  pfpSize = {x: this.levelsSize.y, y: this.levelsSize.y};

  offsetEditorSettings: OffsetEditorSettings = {
    scale: {value: 0.3, range: {min: 0.1, max: 1}},
    margin: {value: -25, range: {min: -1000, max: 0}},
    snapping: {value: 1, range: {min: 1, max: 100}}
  }

  displayPfp = true;
  pfpBackground = true;
  clipPfp = true;
  showHybrid = true;
  insetMainXp = false;

  ngAfterViewInit(): void {
    this.setUpValues(this.model)

    this.auth.getUserProfile().subscribe(async (data: AppUser) => {
      this.user = data.discordUser;
      this.userId = BigInt(data.discordUser.id);
      this.username = `${data.discordUser.username}#${data.discordUser.discriminator}`;
      this.pfp = data.discordUser.imageUrl;
      console.log(`Setting up existing values for user: ${this.user.username}#${this.user.discriminator}`)
      this.api.getSimpleData(`/levels/rankcard/${this.userId}`, true).subscribe((data: UserRankcardConfig) => {
        this.model = data;
        this.setUpValues(this.model);
      })
    })

    RankcardCustomizerComponent.getDefaultBgOptions(this.api).subscribe((data: string[]) => {
      this.defaultBgOptions = [];
      for (let opt of data) {
        this.defaultBgOptions.push(this.defaultBgToUrl(opt));
      }
    })
  }

  setUpValues(model: UserRankcardConfig): void {
    let xpC = this.setColor(this.xpColorSelector, model.xpColor);
    let offC = this.setColor(this.offColorSelector, model.offColor);
    let lvlBgC = this.setColor(this.levelBgColorSelector, model.levelBgColor);
    let ttlBgC = this.setColor(this.titleBgColorSelector, model.titleBgColor);
    this.xpColorOpacity = xpC.opacity;
    this.offColorOpacity = offC.opacity;
    this.levelBgOpacity = lvlBgC.opacity;
    this.titleBgOpacity = ttlBgC.opacity;

    this.displayPfp = (model.rankcardFlags & RankcardFlags.DisplayPfp) != RankcardFlags.None;
    this.pfpBackground = (model.rankcardFlags & RankcardFlags.PfpBackground) != RankcardFlags.None;
    this.clipPfp = (model.rankcardFlags & RankcardFlags.ClipPfp) != RankcardFlags.None;
    this.showHybrid = (model.rankcardFlags & RankcardFlags.ShowHybrid) != RankcardFlags.None;
    this.insetMainXp = (model.rankcardFlags & RankcardFlags.InsetMainXP) != RankcardFlags.None;

    if (model.background.startsWith(`${API_URL}/levels/default`)) {
      this.backgroundMode = this.DEFAULT;
      this.defaultImageChoice = model.background;
    }
    else if (new RegExp("#[0-9a-fA-F]{3,8}").test(model.background)) {
      this.backgroundMode = this.SOLID;
      this.solidColorChoice = model.background;
    }
    else {
      this.backgroundMode = this.CUSTOM;
      this.customLink = model.background;
    }

    this.model.titleOffset = model.titleOffset;
    this.model.levelOffset = model.levelOffset;
    this.model.pfpOffset = model.pfpOffset;

    this.cd.detectChanges();
  }

  defaultBgToUrl = RankcardCustomizerComponent.defaultBgToUrl;
  static defaultBgToUrl(name : string) : string {
    return `${API_URL}/levels/default/images/${name}`;
  }

  static getDefaultBgOptions(api : ApiService) : Observable<string[]> {
    return api.getSimpleData("/levels/default/images", true)
  }
  defaultBgOptions : string[] = [this.defaultBgToUrl("default.png")]

  formatOpacity(val: number) : string {
    return (val * 100).toFixed(1) + "%"
  }

  readonly SOLID = 0;
  readonly CUSTOM = 1;
  readonly DEFAULT = 2;
  backgroundMode = this.DEFAULT;
  bgOptTabs = [this.SOLID, this.CUSTOM, this.DEFAULT]
  bgOptTabNames = ["Solid Color", "Custom Image", "Default Image"]
  bgOptTabIcons = ["format_color_fill", "add_to_photos", "insert_photo"]
  changesPresent = false;
  applyingChanges = false;

  updateColorFromEvent(item: string, event: Event) {
    if (event.target === null) return;
    let value = (event.target as any)["value"] as string;
    if (value === undefined) return;

    this.updateColor(item, value);
  }

  updateColor(item: string, value: string) {
    this.changesPresent = true;

    let prev = (this.model as any)[item];
    if (prev === undefined) {
      console.error(`Invalid color key passed for updateColor: '${item}'`)
      return;
    }

    let alpha = BigInt(prev) & 0xff000000n
    let numval = BigInt(parseInt(value.substring(1), 16));
    let color = alpha | numval;

    if (item == "xpColor") this.model.xpColor = color;
    else if (item == "offColor") this.model.offColor = color;
    else if (item == "levelBgColor") this.model.levelBgColor = color;
    else if (item == "titleBgColor") this.model.titleBgColor = color;

    this.cd.detectChanges();
  }

  xpColorOpacity = 1;
  offColorOpacity = 1;
  levelBgOpacity = 1;
  titleBgOpacity = 1;
  updateAlpha(item: string, value: number) {
    this.changesPresent = true;

    let prev = (this.model as any)[item];
    if (prev === undefined) {
      console.error(`Invalid color key passed for updateAlpha: '${item}'`)
      return;
    }

    let basecol = BigInt(prev) & 0x00ffffffn
    let alpha = BigInt(Math.round(value * 0xff))
    let color = (alpha << 24n) | basecol;

    if (item == "xpColor") this.model.xpColor = color;
    else if (item == "offColor") this.model.offColor = color;
    else if (item == "levelBgColor") this.model.levelBgColor = color;
    else if (item == "titleBgColor") this.model.titleBgColor = color;

    this.cd.detectChanges();
  }

  updateModelFlags() {
    this.setFlag(RankcardFlags.DisplayPfp, this.displayPfp);
    this.setFlag(RankcardFlags.PfpBackground, this.pfpBackground);
    this.setFlag(RankcardFlags.ClipPfp, this.clipPfp);
    this.setFlag(RankcardFlags.ShowHybrid, this.showHybrid);
    this.setFlag(RankcardFlags.InsetMainXP, this.insetMainXp);
  }

  setFlag(flag: RankcardFlags, value: boolean) {
    this.changesPresent = true;

    this.model.rankcardFlags &= ~flag;
    if (value) this.model.rankcardFlags |= flag;
  }

  setColor(picker: ElementRef, value: bigint): {color: string, opacity: number} {
    value = BigInt(value);
    let color = "#" + (value & 0x00ffffffn).toString(16).padStart(6, '0');
    let alpha = Number(value >> 24n) / 0xff;
    (picker.nativeElement).setAttribute("value", color);
    //console.log(`Set color ${color} and alpha ${alpha} for picker & slider set`);
    return {color: color, opacity: alpha}
  }

  setToggle(checkbox: ElementRef, value: boolean) {
    let cbox : HTMLInputElement = checkbox.nativeElement as HTMLInputElement;
    if (cbox === undefined) {
      console.error("Unable to convert toggle reference to HTMLInputElement!")
      return;
    }

    //console.log(`Set value of checkbox to ${value}`)
    cbox.checked = value;
  }

  solidColorChoice = "#000000";
  setSolidBackgroundColor() {
    if (this.backgroundMode != this.SOLID) return;
    this.changesPresent = true;

    let picker = this.backgroundColorSelector.nativeElement as HTMLInputElement;
    this.solidColorChoice = picker.value;
    this.model.background = this.solidColorChoice;
  }

  customLink = "";
  setCustomBackgroundImage() {
    if (this.backgroundMode != this.CUSTOM) return;
    this.changesPresent = true;

    this.model.background = this.customLink;
  }

  changeFileInput() {
    const files = (this.fileInput.nativeElement as HTMLInputElement).files;
    if (files === null || files.length < 1) return;

    this.handleFileInput(files[0]);
  }

  triggerFileChooser() {
    console.log("Activating file chooser");
    (this.fileInput.nativeElement as HTMLInputElement).click();
  }

  triggerURLDialog() {
    const ref = this.dialog.open(ImageUrlDialogComponent, {
      width: '450px',
      data: {url: this.customLink, title: "Rankcard Background from URL"}
    });

    ref.afterClosed().subscribe(result => {
      this.customLink = result;
      this.setCustomBackgroundImage();
    })
  }

  handleFileDrop(event: any) {
    event.preventDefault();
    const files = event.dataTransfer.files as FileList;
    if (files === null || files.length < 1) return;

    console.log(files[0]);
    this.handleFileInput(files[0]);
  }

  handleFileDragOver(event: Event) {
    event.stopPropagation();
    event.preventDefault();
  }

  handleFileInput(data: File) {
    this.changesPresent = true;

    this.api.postFile(`/levels/${this.userId}/images`, data).subscribe((data : {"fileName" : string}) => {
      this.customLink = `${API_URL}/levels/${this.userId}/images/${data.fileName}`;
      this.setCustomBackgroundImage();
    }, (err) => {
      console.log("Unable to post background image, " + err.message);
      this.toastr.error(err.message, "Unable to Upload Image");
    });
  }

  defaultImageChoice = this.defaultBgOptions[0];
  setDefaultBackgroundImage() {
    if (this.backgroundMode != this.DEFAULT) return;
    this.changesPresent = true;

    //let selector = this.backgroundDefaultSelector.nativeElement as HTMLInputElement;
    this.model.background = this.defaultImageChoice;
  }

  setBackgroundMode(mode : number) {
    this.backgroundMode = mode;

    if (mode == this.SOLID) this.setSolidBackgroundColor();
    else if (mode == this.CUSTOM) this.setCustomBackgroundImage();
    else if (mode == this.DEFAULT) this.setDefaultBackgroundImage();
  }

  prepareModel(obj : any) {
    return JSON.parse(JSON.stringify(obj, (key, value) =>
      typeof value === 'bigint'
        ? value.toString()
        : value // return everything else unchanged
    ));
  }

  uploadChanges() {
    if (this.model.id == 0n) {
      console.error("Unable to post changes, unauthorized user; ID is 0");
      return;
    }

    this.applyingChanges = true;

    this.api.postSimpleData("/levels/rankcard", this.prepareModel(this.model)).subscribe(() => {
      this.changesPresent = false;
      this.applyingChanges = false;
      this.toastr.success("Changes saved successfully.")
    }, (err) => {
      console.log(`Unable to complete apply request, received: ${err}`);
      this.applyingChanges = false;
      this.changesPresent = true;
      this.toastr.error(err.message);
    })
  }

}
