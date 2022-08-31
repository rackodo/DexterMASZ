import { RankcardFlags, UserRankcardConfig } from "../models/UserRankcardConfig";

export class UserRankcardConfigUtility {
  static default : UserRankcardConfig = {
    id: 0n,
    xpColor: 0xff70cefen,
    offColor: 0xffffffffn,
    levelBgColor: 0xff202225n,
    titleBgColor: 0xff202225n,
    titleOffset: {x: 0, y: 0},
    levelOffset: {x: 0, y: 100},
    pfpOffset: {x: 1000, y: 100},
    pfpRadiusFactor: 1,
    background: "#555555",
    rankcardFlags: RankcardFlags.DisplayPfp | RankcardFlags.PfpBackground | RankcardFlags.ClipPfp | RankcardFlags.ShowHybrid
  }
}
