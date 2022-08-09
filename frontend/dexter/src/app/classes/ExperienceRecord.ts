import { ExperienceModel } from "../models/ExperienceModel";

export class ExperienceRecord implements ExperienceModel {
  xp = 0n
  level = 0n
  xpLevel = 100n
  xpResidual = 0n

  constructor(xp: bigint, level: bigint, xp_level: bigint, xp_residual: bigint) {
    this.xp = xp
    this.level = level
    this.xpLevel = xp_level
    this.xpResidual = xp_residual
  }
}
