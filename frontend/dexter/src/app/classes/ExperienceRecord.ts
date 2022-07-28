import { ExperienceModel } from "../models/ExperienceModel";

export class ExperienceRecord implements ExperienceModel {
  xp = 0n
  level = 0n
  xp_level = 100n
  xp_residual = 0n

  constructor(xp: bigint, level: bigint, xp_level: bigint, xp_residual: bigint) {
    this.xp = xp
    this.level = level
    this.xp_level = xp_level
    this.xp_residual = xp_residual
  }
}
