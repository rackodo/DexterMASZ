export class DayInYear {
  constructor(public day: number, public month: number, public relative: boolean) {}

  compareTo(date: Date): number {
    if (!this.relative) {
      if (date.getMonth() < this.month) return -1;
      if (date.getMonth() > this.month) return 1;

      if (date.getDate() < this.day) return -1;
      if (date.getDate() > this.day) return 1;

      return 0;
    }

    return this.getNonRelative(date.getFullYear()).compareTo(date);
  }

  getNonRelative(year: number): DayInYear {
    if (!this.relative) return this;

    let dayabs = Math.abs(this.day);

    // 0..6 (Sun..Sat)
    let wd = dayabs % 7;
    // 0 = first, 1 = second, 2 = third...
    // -1 = last, -2 = second to last...
    let wdcnt = dayabs / 7;

    if (this.day < 0) {
      wdcnt = -wdcnt - 1;
    }

    new Date(year, this.month, 1)


  }
}
