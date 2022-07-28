export interface UserRankcardConfig {
    id: bigint;
    xpColor: bigint;
    offColor: bigint;
    levelBgColor: bigint;
    titleBgColor: bigint;
    background: string;
    rankcardFlags: RankcardFlags;
}

export enum RankcardFlags {
    None = 0,
    PfpBackground = 1 << 0,
    ClipPfp = 1 << 1,
    DisplayPfp = 1 << 2,
    ShowHybrid = 1 << 3,
    InsetMainXP = 1 << 4
}
