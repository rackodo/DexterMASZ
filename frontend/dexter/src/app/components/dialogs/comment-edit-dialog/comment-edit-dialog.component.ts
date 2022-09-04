import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentEditDialog } from 'src/app/models/CommentEditDialog';

@Component({
  selector: 'app-comment-edit-dialog',
  templateUrl: './comment-edit-dialog.component.html',
  styleUrls: ['./comment-edit-dialog.component.css']
})
export class CommentEditDialogComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public comment: CommentEditDialog) { }

}
