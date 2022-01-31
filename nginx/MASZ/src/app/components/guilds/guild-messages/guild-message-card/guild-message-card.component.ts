import { ThisReceiver } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from 'src/app/components/dialogs/confirmation-dialog/confirmation-dialog.component';
import { ScheduledMessageEditDialogComponent } from 'src/app/components/dialogs/scheduled-message-edit-dialog/scheduled-message-edit-dialog.component';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { ScheduledMessageExtended } from 'src/app/models/ScheduledMessageExtended';
import { ScheduledMessage } from 'src/app/models/ScheduledMessage';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-guild-message-card',
  templateUrl: './guild-message-card.component.html',
  styleUrls: ['./guild-message-card.component.css']
})
export class GuildMessageCardComponent implements OnInit {

  public guildId!: bigint;
  public isAdminOrHigher: boolean = false;

  public status?: string;
  public failureReason?: string;

  @Input() message!: ScheduledMessageExtended;
  @Output() deleteEvent = new EventEmitter<number>();

  constructor(private auth: AuthService, private enumManager: EnumManagerService, private toastr: ToastrService, private route: ActivatedRoute, private api: ApiService, private dialog: MatDialog, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = BigInt(this.route.snapshot.paramMap.get('guildid'));

    this.auth.isAdminInGuild(this.guildId).subscribe(data => {
      this.isAdminOrHigher = data;
    });

    this.enumManager.getEnum(ApiEnumTypes.SCHEDULEMESSAGESTATUS).subscribe(data => {
      this.status = data?.find(x => x.key === this.message.scheduledMessage.status)?.value;
    });
    this.enumManager.getEnum(ApiEnumTypes.SCHEDULEMESSAGEFAILUREREASON).subscribe(data => {
      this.failureReason = data?.find(x => x.key === this.message.scheduledMessage.failureReason)?.value;
    });
  }

  editMessage() {
    let messageDto: ScheduledMessage = {
      id: this.message.scheduledMessage.id,
      content: this.message.scheduledMessage.content,
      scheduledFor: this.message.scheduledMessage.scheduledFor,
      name: this.message.scheduledMessage.name,
      channelId: this.message.scheduledMessage.channelId
    } as ScheduledMessage;
    const editDialogRef = this.dialog.open(ScheduledMessageExtendedEditDialogComponent, {
      data: messageDto,
      minWidth: '800px'
    });
    editDialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        let body = {
          name: messageDto.name,
          content: messageDto.content,
          channelId: messageDto.channelId,
          scheduledFor: messageDto.scheduledFor?.toISOString()
        }

        this.api.putSimpleData(`/guilds/${this.guildId}/scheduledmessages/${this.message.scheduledMessage.id}`, body, undefined, true, true).subscribe((data) => {
          this.message = data;
          this.toastr.success(this.translator.instant('ScheduledMessageEditDialog.EditSuccess'));
        }, error => {
          console.error(error);
        });
      }
    });
  }

  deleteMessage() {
    const editDialogRef = this.dialog.open(ConfirmationDialogComponent);
    editDialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.api.deleteData(`/guilds/${this.guildId}/scheduledmessages/${this.message.scheduledMessage.id}`, undefined, true, true).subscribe(() => {
          this.toastr.success(this.translator.instant('ScheduledMessageEditDialog.DeleteSuccess'));
          this.deleteEvent.emit(this.message.scheduledMessage.id);
        });
      }
    });
  }
}
