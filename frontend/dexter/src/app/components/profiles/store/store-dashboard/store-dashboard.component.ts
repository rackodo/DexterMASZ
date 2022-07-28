import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent implements OnInit {

  coming_soon_date = new Date("2023-01-01");
  constructor() { }

  ngOnInit(): void {
  }

}
