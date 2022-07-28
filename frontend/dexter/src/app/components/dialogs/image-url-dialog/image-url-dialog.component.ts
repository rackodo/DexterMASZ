import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-image-url-dialog',
  templateUrl: './image-url-dialog.component.html',
  styleUrls: ['./image-url-dialog.component.css']
})
export class ImageUrlDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ImageUrlDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UrlData,
  ) {}

  onNoClick(): void {
    this.dialogRef.close();
  }
}

export interface UrlData {
  title?: string;
  url: string
}
