import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'beautifyFileName'
})
export class BeautifyFileNamePipe implements PipeTransform {

  transform(value: string): unknown {
    value = value.replace("_", " ");
    let dotIndex = value.lastIndexOf(".");
    if (dotIndex > -1) {
      value = value.substring(0, dotIndex);
    }
    let slashIndex = value.lastIndexOf("/");
    if (slashIndex > -1) {
      value = value.substring(slashIndex + 1);
    }

    return value;
  }

}
