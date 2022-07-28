import { Component, Input, OnInit } from '@angular/core';
import { ExperienceModel } from 'src/app/models/ExperienceModel';
import { RankcardFlags, UserRankcardConfig } from 'src/app/models/UserRankcardConfig';
import { RankcardCustomizerComponent } from '../rankcard-customizer.component';

@Component({
  selector: 'app-rankcard-preview',
  templateUrl: './rankcard-preview.component.html',
  styleUrls: ['./rankcard-preview.component.css']
})
export class RankcardPreviewComponent implements OnInit {

  @Input() txtXp : ExperienceModel = {xp: 82n, level: 0n, xp_level: 100n, xp_residual: 82n};
  @Input() vcXp : ExperienceModel = {xp: 105n, level: 1n, xp_level: 150n, xp_residual: 5n};
  @Input() totalXp : ExperienceModel = {xp: 187n, level: 1n, xp_level: 150n, xp_residual: 87n};

  @Input() model : UserRankcardConfig = {
    "id": 0n,
    "xpColor": 0xff70cefen,
    "offColor": 0xffffffffn,
    "levelBgColor": 0xff202225n,
    "titleBgColor": 0xff202225n,
    "background": "default",
    "rankcardFlags": RankcardFlags.DisplayPfp | RankcardFlags.PfpBackground | RankcardFlags.ClipPfp | RankcardFlags.ShowHybrid
  }

  @Input() username: string = "Username#0123";
  @Input() pfpUrl: string = "/assets/img/defaultProfile.png";
  @Input() defaultBgOptions : string[] = ["default"];

  uintColorToCss(color: bigint) : string {
    color = BigInt(color);
    return `#${(color & 0x00ffffffn).toString(16).padStart(6, '0')}${((color & 0xff000000n) >> 24n).toString(16).padStart(2, '0')}`
  }

  isNameColor(name: string) {
    const colorRegex = new RegExp('#[0-9a-fA-F]{3,8}');
    return colorRegex.test(name);
  }

  imageNameToCss(name: string) {
    if (this.isNameColor(name)) {
      return `background-color: ${name}`;
    }

    if (this.defaultBgOptions.includes(name)) {
      name = RankcardCustomizerComponent.defaultBgToUrl(name);
    }

    return `background-image: url(${name})`
  }

  imageNameToUrl(name: string) {
    if (this.isNameColor(name)) {
      return "";
    }
    if (this.defaultBgOptions.includes(name)) {
      name = RankcardCustomizerComponent.defaultBgToUrl(name);
    }
    return name;
  }

  rawToSuffixForm(amount: bigint | number) : string {
    amount = Number(amount)

    const suffixes = [
      {val: 1e15, suf: "Q"},
      {val: 1e12, suf: "T"},
      {val: 1e9, suf: "B"},
      {val: 1e6, suf: "M"},
      {val: 1e3, suf: "K"}
    ];

    for (let s of suffixes) {
      if (amount >= s.val)
        return `${(amount / s.val).toPrecision(3)}${s.suf}`
    }

    return amount.toString();
  }

  constructor() { }

  ngOnInit(): void {
  }

}
