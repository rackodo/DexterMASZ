import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-switch',
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.css']
})
export class SwitchComponent implements OnInit {

  @Input() value = false;
  @Output() valueChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
  }

  /*
  toggle(event: Event) {
    if (event.target === null) return;
    let v = (event.target as any)["checked"] as boolean;
    if (v === undefined) return;

    this.value = v;
  }
  */
}
