import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { CalcGuildUserLevel } from 'src/app/models/CalcGuildUserLevel';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-guild-leaderboard',
  templateUrl: './guild-leaderboard.component.html',
  styleUrls: ['./guild-leaderboard.component.css']
})
export class GuildLeaderboardComponent implements OnInit {

  selectedTab = new FormControl(1);
  orders = ["text", "total", "voice"];
  icons = ["text_fields", "star", "keyboard_voice"]
  themes = ["5da25f", "80ced6", "ffca54"]

  constructor(private route: ActivatedRoute) {
    let index = this.orders.indexOf(this.route.snapshot.queryParamMap.get("order") ?? "total");
    this.selectedTab.setValue(index < 0 ? 1 : index);
  }

  ngOnInit(): void {}

}
