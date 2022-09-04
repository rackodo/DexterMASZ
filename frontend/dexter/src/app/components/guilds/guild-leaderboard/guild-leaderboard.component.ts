import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-guild-leaderboard',
  templateUrl: './guild-leaderboard.component.html',
  styleUrls: ['./guild-leaderboard.component.css']
})
export class GuildLeaderboardComponent {

  selectedTab = new FormControl(1);
  orders = ["text", "total", "voice"];
  icons = ["text_fields", "star", "keyboard_voice"]
  themes = ["5da25f", "80ced6", "ffca54"]

  constructor(private route: ActivatedRoute) {
    let index = this.orders.indexOf(this.route.snapshot.queryParamMap.get("order") ?? "total");
    this.selectedTab.setValue(index < 0 ? 1 : index);
  }

}
