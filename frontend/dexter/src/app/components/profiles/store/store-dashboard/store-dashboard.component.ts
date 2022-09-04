import { Component } from '@angular/core';

@Component({
  selector: 'app-store-dashboard',
  templateUrl: './store-dashboard.component.html',
  styleUrls: ['./store-dashboard.component.css']
})
export class StoreDashboardComponent  {

  coming_soon_date = new Date("2023-01-01");
  constructor() { }

}
