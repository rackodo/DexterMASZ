import { Component, Inject } from '@angular/core';
import { AbstractControl, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-number-input-dialog',
  templateUrl: './number-input-dialog.component.html',
  styleUrls: ['./number-input-dialog.component.css']
})
export class NumberInputDialogComponent {

  controller!: FormControl;

  constructor(
    public dialogRef: MatDialogRef<NumberInputDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NumberData,
  ) {
    let validators = [];
    if (data.min) validators.push(Validators.min(data.min))
    if (data.max) validators.push(Validators.max(data.max))
    if (data.forbiddenValues) {
      validators.push((control: AbstractControl) => {
        if (data.forbiddenValues === undefined) return null;
        if (data.forbiddenValues.includes(control.value)) {
          return {"forbid": "This value isn't available"};
        }
        return null;
      })
    }

    this.controller = new FormControl(data.value, validators)
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}

export interface NumberData {
  title?: string,
  label?: string,
  forbiddenValues?: number[],
  min?: number,
  max?: number,
  value: number
}
