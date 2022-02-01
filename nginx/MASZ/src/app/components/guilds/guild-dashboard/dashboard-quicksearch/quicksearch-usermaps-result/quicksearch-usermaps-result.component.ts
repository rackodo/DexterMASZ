import { Component, Input, OnInit } from '@angular/core';
import { UserMapExpanded } from 'src/app/models/UserMapExpanded';

@Component({
  selector: 'app-quicksearch-usermaps-result',
  templateUrl: './quicksearch-usermaps-result.component.html',
  styleUrls: ['./quicksearch-usermaps-result.component.css']
})
export class QuicksearchUsermapsResultComponent implements OnInit {

  @Input() usermaps!: UserMapExpanded[];
  @Input() searchedFor!: string;
  constructor() { }

  ngOnInit(): void {
  }

}
