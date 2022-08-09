import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'unitFormat'
})
export class UnitFormatPipe implements PipeTransform {

  units = [
    {value: 1e0, unit: ""},
    {value: 1e3, unit: "K"},
    {value: 1e6, unit: "M"},
    {value: 1e9, unit: "B"},
    {value: 1e12, unit: "T"},
    {value: 1e15, unit: "Q"}
  ].reverse();

  transform(value: number | bigint, precision: number = 3): unknown {
    if (typeof(value) === 'bigint') value = Number(value);

    for (let u of this.units) {
      if (value > u.value) {
        return `${+(value / u.value).toPrecision(precision)}${u.unit}`
      }
    }

    return +value.toPrecision(precision);
  }

}
