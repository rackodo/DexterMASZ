import { Component, Input } from '@angular/core';
import { UserMapExpanded } from 'src/app/models/UserMapExpanded';

@Component({
  selector: 'app-quicksearch-usermaps-result',
  templateUrl: './quicksearch-usermaps-result.component.html',
  styleUrls: ['./quicksearch-usermaps-result.component.css']
})
export class QuickSearchUserMapsResultComponent {

  @Input() usermaps!: UserMapExpanded[];
  @Input() searchedFor!: string;
  constructor() { }

}
