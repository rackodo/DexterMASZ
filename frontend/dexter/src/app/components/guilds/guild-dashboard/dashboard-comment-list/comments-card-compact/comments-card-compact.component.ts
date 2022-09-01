import { Component, Input } from '@angular/core';
import { ModCaseCommentExpandedTable } from 'src/app/models/ModCaseCommentExpandedTable';

@Component({
  selector: 'app-comments-card-compact',
  templateUrl: './comments-card-compact.component.html',
  styleUrls: ['./comments-card-compact.component.css']
})
export class CommentsCardCompactComponent {

  @Input() entry!: ModCaseCommentExpandedTable;
  constructor() { }

}
