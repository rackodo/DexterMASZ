import { Component, OnInit } from '@angular/core';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { Guideline } from 'src/app/models/Guideline';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-legal',
  templateUrl: './legal.component.html',
  styleUrls: ['./legal.component.css']
})

export class LegalComponent implements OnInit {

  public legal: ContentLoading<Guideline[]> = { loading: true, content: [] };

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.reload();
  }

  private reload() {
    this.legal = { loading: true, content: [] };
    this.api.getSimpleData('/static/legal.json', false).subscribe((data) => {
      this.legal.content = data;
      this.legal.loading = false;
    }, () => {
      this.legal.loading = false;
    });
  }

}
