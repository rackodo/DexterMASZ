import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-quicksearch-case-result',
  templateUrl: './quicksearch-case-result.component.html',
  styleUrls: ['./quicksearch-case-result.component.css']
})
export class QuickSearchCaseResultComponent {

  @Input() caseEntry!: any;
  constructor() { }

}
