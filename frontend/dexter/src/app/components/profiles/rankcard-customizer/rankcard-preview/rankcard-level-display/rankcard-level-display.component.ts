import { Component, Input } from '@angular/core';
import { ExperienceModel } from 'src/app/models/ExperienceModel';
import { RankcardFlags, UserRankcardConfig } from 'src/app/models/UserRankcardConfig';
import { rawToSuffixForm, uintColorToCss } from '../rankcard-preview.component';

@Component({
  selector: 'app-rankcard-level-display',
  templateUrl: './rankcard-level-display.component.html',
  styleUrls: ['./rankcard-level-display.component.css']
})
export class RankcardLevelDisplayComponent {

  @Input() xpModel! : ExperienceModel;
  @Input() model! : UserRankcardConfig;
  @Input() isMain = true;
  @Input() typeName = "Total";
  @Input() rank = 123;

  one = 1n;
  number = Number;
  levelLabelWidth = 175;

  constructor() { }

  insetMainXpFlag() { return (this.model.rankcardFlags & RankcardFlags.InsetMainXP) > 0; }

  xpClipPath() {return 'inset(0 '+ (1 - (Number(this.xpModel.xpResidual) / Number(this.xpModel.xpLevel))) * 100 +'% 0 0)'}
  xpContrastColor() {
    let componentSum = 0n;
    let c = BigInt(this.model.xpColor);

    componentSum += c & 0x0000ffn;
    componentSum += c & 0x00ff00n >> 8n;
    componentSum += c & 0xff0000n >> 16n;
    componentSum /= 3n;

    //console.log("Comp Sum = " + componentSum + " for color " + c.toString(16));

    return (componentSum > 0xb8n) ? "black" : "white";
  }

  rawToSuffixForm(amount: bigint | number) {
    return rawToSuffixForm(amount);
  };
  uintColorToCss(color: bigint) { return uintColorToCss(color); }
}
