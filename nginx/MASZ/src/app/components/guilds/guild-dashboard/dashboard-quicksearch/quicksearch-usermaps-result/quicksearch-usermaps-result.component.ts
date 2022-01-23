import { Component, Input, OnInit } from '@angular/core';
import { UserMapView } from 'src/app/models/UserMapView';

@Component({
  selector: 'app-quicksearch-usermaps-result',
  templateUrl: './quicksearch-usermaps-result.component.html',
  styleUrls: ['./quicksearch-usermaps-result.component.css']
})
export class QuicksearchUsermapsResultComponent implements OnInit {

  @Input() usermaps!: UserMapView[];
  @Input() searchedFor!: string;
  constructor() { }

  ngOnInit(): void {
  }

}
