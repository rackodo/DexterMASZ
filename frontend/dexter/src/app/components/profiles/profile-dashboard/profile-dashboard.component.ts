import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { DashboardTabs } from 'src/app/models/DashboardTabs';

@Component({
  selector: 'app-profile-dashboard',
  templateUrl: './profile-dashboard.component.html',
  styleUrls: ['./profile-dashboard.component.css']
})
export class ProfileDashboardComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.queryParams.subscribe((params: any) => {
      if (params["selectedTab"] !== undefined) {
        for (let i = 0; i < this.tabs.length; i++) {
          let t = this.tabs[i];
          if (t.component === params["selectedTab"]) {
            this.selectedTab.setValue(i);
          }
        }
      }
    });
  }

  public selectedTab = new FormControl(0);

  public tabs: DashboardTabs[] = [
    {
      "icon": "bolt",
      "component": "overview"
    },
    {
      "icon": "local_activity", //or "card_membership"
      "component": "rankcard"
    }
  ]

  ngOnInit(): void {
  }

}
