import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-quicksearch-moderation-result',
  templateUrl: './quicksearch-moderation-result.component.html',
  styleUrls: ['./quicksearch-moderation-result.component.css']
})
export class QuickSearchModerationResultComponent {

  @Input() entry!: any;
  constructor() { }

}
