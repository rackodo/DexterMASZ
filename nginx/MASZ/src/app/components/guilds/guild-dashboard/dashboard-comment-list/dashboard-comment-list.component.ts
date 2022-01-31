import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { ModCaseCommentExpandedTable } from 'src/app/models/ModCaseCommentExpandedTable';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-dashboard-comment-list',
  templateUrl: './dashboard-comment-list.component.html',
  styleUrls: ['./dashboard-comment-list.component.css']
})
export class DashboardCommentListComponent implements OnInit {

  public guildId!: bigint;
  public comments: ContentLoading<ModCaseCommentExpandedTable[]> = { loading: true, content: [] };
  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = BigInt(this.route.snapshot.paramMap.get('guildid'));
    this.reload();
  }

  private reload() {
    this.comments = { loading: true, content: [] };
    this.api.getSimpleData(`/guilds/${this.guildId}/dashboard/latestcomments`).subscribe((data) => {
      this.comments.content = data.slice(0, 5);
      this.comments.loading = false;
    }, error => {
      console.error(error);
      this.comments.loading = false;
      this.toastr.error(this.translator.instant('DashboardComments.FailedToLoad'));
    });
  }

}
