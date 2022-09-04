import { Component, Input } from '@angular/core';
import { InfoPanel } from 'src/app/models/InfoPanel';

@Component({
  selector: 'app-epiclist',
  templateUrl: './epiclist.component.html',
  styleUrls: ['./epiclist.component.css']
})
export class EpiclistComponent {

  @Input() loading: boolean = true;
  @Input() items: InfoPanel[] = [];

  constructor() { }

}
