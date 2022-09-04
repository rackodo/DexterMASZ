import { Component, Input } from '@angular/core';
import { ServiceStatus } from 'src/app/models/ServiceStatus';

@Component({
  selector: 'app-statcard',
  templateUrl: './statcard.component.html',
  styleUrls: ['./statcard.component.css']
})
export class StatcardComponent {

  @Input() titleKey: string = "";
  @Input() text?: string | number | undefined;
  @Input() emote?: string;
  @Input() renderPing?: ServiceStatus = undefined;
  @Input() warningPingLimit: number = 200;
  @Input() errorPingLimit: number = 400;

  constructor() { }

}
