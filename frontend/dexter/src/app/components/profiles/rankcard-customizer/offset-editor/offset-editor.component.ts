import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from '@angular/core';
import { relativeTimeThreshold } from 'moment';
import { Offset2D } from 'src/app/models/Offset2D';

@Component({
  selector: 'app-offset-editor',
  templateUrl: './offset-editor.component.html',
  styleUrls: ['./offset-editor.component.css']
})
export class OffsetEditorComponent implements OnInit {

  @Input() marginTolerance = 0;
  @Input() scale = 1;
  @Input() areaSize = {x: 100, y: 100};
  @Input() boxSize = {x: 10, y: 10};

  @Input() offset = {x: 0, y: 0};
  @Output() offsetChange = new EventEmitter<Offset2D>();

  @ViewChild("area") area!: ElementRef;

  initOffset = {x: 0, y: 0};
  absMouseOffset = {x: 0, y: 0};
  tracking = false;

  constructor() { }

  ngOnInit(): void {
    console.log(this.boxSize);
  }

  setOffset(x: number, y: number) {
    let min = {x: -this.marginTolerance, y: -this.marginTolerance};
    let max = {x: this.areaSize.x + this.marginTolerance - this.boxSize.x,
               y: this.areaSize.y + this.marginTolerance - this.boxSize.y};

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
      this.initOffset.x + (changeX / this.scale),
      this.initOffset.y + (changeY / this.scale)
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
