import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiEnumTypes } from 'src/app/models/ApiEmumTypes';
import { AutoModEvent } from 'src/app/models/AutoModEvent';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-automod-card',
  templateUrl: './automod-card.component.html',
  styleUrls: ['./automod-card.component.css']
})
export class AutoModCardComponent implements OnInit {

  @Input() moderation!: AutoModEvent;

  iconsMap: { [key: number]: string} = {
    0: 'forward_to_inbox',
    1: 'sentiment_satisfied_alt',
    2: 'person',
    3: 'attach_file',
    4: 'description',
    5: 'history',
    6: 'do_not_disturb',
    7: 'manage_search',
    8: 'text_fields',
    9: 'link'
  };
  action: string = "Unknown";
  event: string = "Unknown";

  constructor(public router: Router, private enumManager: EnumManagerService) { }

  ngOnInit(): void {
    this.moderation.messageContent = this.moderation.messageContent.replace(/\n/g, "\\n");
    this.enumManager.getEnum(ApiEnumTypes.AUTOMODACTION).subscribe(data => {
      this.action = data?.find(x => x.key == this.moderation.autoModAction)?.value ?? "Unknown";
    });
    this.enumManager.getEnum(ApiEnumTypes.AUTOMODTYPE).subscribe(data => {
      this.event = data?.find(x => x.key == this.moderation.autoModType)?.value ?? "Unknown";
    });
  }

}
