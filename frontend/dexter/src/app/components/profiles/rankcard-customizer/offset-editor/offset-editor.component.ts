import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Offset2D } from 'src/app/models/Offset2D';
import { OffsetEditorSettingsDialog } from './settings-dialog/settings-dialog.component';

@Component({
  selector: 'app-offset-editor',
  templateUrl: './offset-editor.component.html',
  styleUrls: ['./offset-editor.component.css']
})
export class OffsetEditorComponent implements OnInit {

  @Input() settings!: OffsetEditorSettings;

  @Input() areaSize = {x: 100, y: 100};
  @Input() boxSize = {x: 10, y: 10};

  @Input() offset = {x: 0, y: 0};
  @Output() offsetChange = new EventEmitter<Offset2D>();

  @ViewChild("area") area!: ElementRef;

  initOffset = {x: 0, y: 0};
  absMouseOffset = {x: 0, y: 0};
  tracking = false;

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
    console.log(this.boxSize);
  }

  setOffset(x: number, y: number) {
    let min = {x: this.settings.margin.value, y: this.settings.margin.value};
    let max = {x: this.areaSize.x - this.settings.margin.value - this.boxSize.x,
               y: this.areaSize.y - this.settings.margin.value - this.boxSize.y};

    let snap = this.settings.snapping.value;
    x += snap / 2;
    y += snap / 2;
    x -= x % snap;
    y -= y % snap;

    if (x < min.x) x = min.x;
    else if (x > max.x) x = max.x;

    if (y < min.y) y = min.y;
    else if (y > max.y) y = max.y;

    x = Math.round(x);
    y = Math.round(y);

    this.offset.x = x;
    this.offset.y = y;
  }

  setOffsetX(e: Event) {
    let val = Number((e.target as HTMLInputElement).value);
    this.setOffset(val, this.offset.y);
  }

  setOffsetY(e: Event) {
    let val = Number((e.target as HTMLInputElement).value);
    this.setOffset(this.offset.x, val);
  }

  openSettings() {
    const ref = this.dialog.open(OffsetEditorSettingsDialog, {
      width: '450px',
      data: this.settings
    });

    ref.afterClosed().subscribe((result: OffsetEditorSettings) => {
      if (!result) return;

      this.settings.margin.value = result.margin.value;
      this.settings.snapping.value = result.snapping.value;
      this.settings.scale.value = result.scale.value;
    })
  }

  mouseDown(event: MouseEvent) {
    this.initOffset = {x: this.offset.x, y: this.offset.y};
    this.absMouseOffset = {x: event.pageX, y: event.pageY};
    this.tracking = true;
  }

  change = {x: 0, y: 0}
  updateTracking(event: MouseEvent) {
    if (!this.tracking) return;

    let changeX = event.pageX - this.absMouseOffset.x;
    let changeY = event.pageY - this.absMouseOffset.y;
    this.change = {x: changeX, y: changeY};

    this.setOffset(
      this.initOffset.x + (changeX / this.settings.scale.value),
      this.initOffset.y + (changeY / this.settings.scale.value)
    )
  }

  endTracking(event: MouseEvent) {
    if (!this.tracking) return;

    this.updateTracking(event);
    this.tracking = false;
  }

  @HostListener('mousemove', ['$event']) mouseMove(event: MouseEvent) { this.updateTracking(event); }

  @HostListener('mouseup', ['$event']) mouseUp(event: MouseEvent) { this.endTracking(event); }

  @HostListener('mouseexit', ['$event']) mouseExit(event: MouseEvent) { this.endTracking(event); }
}

export interface OffsetEditorSettings {
  snapping: ValueRange,
  margin: ValueRange,
  scale: ValueRange
}

export interface ValueRange {
  value: number,
  range?: {min: number, max: number}
}
