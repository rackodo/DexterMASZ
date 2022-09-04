import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { GuildDeleteDialogData } from 'src/app/models/GuildDeleteDialogData';

@Component({
  selector: 'app-guild-delete-dialog',
  templateUrl: './guild-delete-dialog.component.html',
  styleUrls: ['./guild-delete-dialog.component.css']
})
export class GuildDeleteDialogComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public guildToDelete: GuildDeleteDialogData) { }

}
