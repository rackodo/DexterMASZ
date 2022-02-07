import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from 'src/app/components/dialogs/confirmation-dialog/confirmation-dialog.component';
import { UsermapEditDialogComponent } from 'src/app/components/dialogs/usermap-edit-dialog/usermap-edit-dialog.component';
import { UserMapExpanded } from 'src/app/models/UserMapExpanded';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-usermap-card',
  templateUrl: './usermap-card.component.html',
  styleUrls: ['./usermap-card.component.css']
})
export class UsermapCardComponent implements OnInit {

  @Output() updateEvent = new EventEmitter<number>();
  @Output() deleteEvent = new EventEmitter<number>();
  @Input() userMap!: UserMapExpanded;
  @Input() showDeleteButton: boolean = true;
  constructor(private dialog: MatDialog, private api: ApiService, private toastr: ToastrService, private translator: TranslateService) { }

  ngOnInit(): void {
  }

  deleteMap() {
    const confirmDialogRef = this.dialog.open(ConfirmationDialogComponent);
    confirmDialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.api.deleteData(`/guilds/${this.userMap.userMap.guildId}/usermap/${this.userMap.userMap.id}`).subscribe(() => {
          this.deleteEvent.emit(this.userMap.userMap.id);
          this.toastr.success(this.translator.instant('UsermapCard.Deleted'));
        }, error => {
          console.error(error);
          this.toastr.error(this.translator.instant('UsermapCard.FailedToDelete'));
        })
      }
    });
  }

  editMap() {
    let userMapDto: any = {
      reason: this.userMap.userMap.reason
    };
    const editDialogRef = this.dialog.open(UsermapEditDialogComponent, {
      data: userMapDto,
      minWidth: '400px'
    });
    editDialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.api.putSimpleData(`/guilds/${this.userMap.userMap.guildId}/usermap/${this.userMap.userMap.id}`, userMapDto).subscribe(() => {
          this.toastr.success(this.translator.instant('UsermapCard.Updated'));
          this.userMap.userMap.reason = userMapDto.reason.trim();
          this.updateEvent.emit(0);
        }, error => {
          console.error(error);
          this.toastr.error(this.translator.instant('UsermapCard.FailedToUpdate'));
        });
      }
    });
  }

}
