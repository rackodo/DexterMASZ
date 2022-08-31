import { Component, Input, OnInit } from '@angular/core';
import { ExperienceRecord } from 'src/app/classes/ExperienceRecord';
import { CalcGuildUserLevel } from 'src/app/models/CalcGuildUserLevel';

@Component({
  selector: 'app-leaderboard-item',
  templateUrl: './leaderboard-item.component.html',
  styleUrls: ['./leaderboard-item.component.css']
})
export class LeaderboardItemComponent implements OnInit {

  @Input() index : number = 1;
  @Input() order : string = "total";
  @Input() user! : CalcGuildUserLevel;
  json = JSON;
  record!: ExperienceRecord

  constructor() { }

  ngOnInit(): void {
    this.record = this.getXpModel();
  }

  getXpModel(): ExperienceRecord {
    if (this.order == "text") return this.user.textXp;
    if (this.order == "voice") return this.user.voiceXp;
    return this.user.totalXp;
  }

  calcLeftRot(): number {
    let f = Number(this.record.xpResidual) / Number(this.record.xpLevel);
    let rot = f * 360;
    return rot > 180 ? rot : 180;
  }

  calcRightRot(): number {
    let f = Number(this.record.xpResidual) / Number(this.record.xpLevel);
    let rot = f * 360;
    return rot > 180 ? 180 : rot;
  }

  rankColor(): string {
    if (this.index == 0) return "C9B037";
    if (this.index == 1) return "D7D7D7";
    if (this.index == 2) return "AD8A56";
    return "white";
  }

}
