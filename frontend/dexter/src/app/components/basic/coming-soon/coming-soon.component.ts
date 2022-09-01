import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-coming-soon',
  templateUrl: './coming-soon.component.html',
  styleUrls: ['./coming-soon.component.css']
})
export class ComingSoonComponent {

  @Input() date : Date | undefined;
  @Input() title : string | undefined;
  @Input() subtitle : string | undefined;
  @Input() description : string | undefined;

  constructor() { }

}
