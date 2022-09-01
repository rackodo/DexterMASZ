import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-switch',
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.css']
})
export class SwitchComponent {

  @Input() value = false;
  @Output() valueChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor() { }

}
