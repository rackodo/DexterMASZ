import { Component, OnInit } from '@angular/core';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { Guideline } from 'src/app/models/Guideline';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.css']
})
export class TermsComponent implements OnInit {

  public terms: ContentLoading<Guideline[]> = { loading: true, content: [] };

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.reload();
  }

  private reload() {
    this.terms = { loading: true, content: [] };
    this.api.getSimpleData('/static/terms.json', false).subscribe((data) => {
      this.terms.content = data;
      this.terms.loading = false;
    }, () => {
      this.terms.loading = false;
    });
  }

}
