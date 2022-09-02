import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ChartData, ChartConfiguration, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { ToastrService } from 'ngx-toastr';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { AutoModSplit } from 'src/app/models/AutoModSplit';
import { ApiService } from 'src/app/services/api.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-dashboard-automod-split',
  templateUrl: './dashboard-automod-split.component.html',
  styleUrls: ['./dashboard-automod-split.component.css']
})
export class DashboardAutoModSplitComponent implements OnInit {

  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;
  public loading: boolean = true;
  public foundContent: boolean = false;

  public chartOptions: ChartConfiguration['options'] = {
    plugins: {
      title: {
        text: '',
        display: true,
        color: 'rgba(232, 230, 227, 1)'
      },
      legend: {
        display: true,
        labels: {
          color: 'white'
        }
      },
    },
    responsive: true,
    maintainAspectRatio: true,
    borderColor: 'rgba(18, 18, 18, 0.2)'
  };

  public chartType: ChartType = 'pie';

  public chartData: ChartData<'pie'> = {
    labels: [],
    datasets: [ {
      data: [],
      backgroundColor: []
    } ]
  };

  public splittedData: AutoModSplit[] = [];

  constructor(private route: ActivatedRoute, private api: ApiService, private toastr: ToastrService, private translator: TranslateService, private enumManager: EnumManagerService) { }

  ngOnInit(): void {
    const guildId = this.route.snapshot.paramMap.get('guildid');
    this.initialize(guildId as string);
  }

  initialize(guildId: string) {
    this.loading = true;
    this.foundContent = false;
    this.api.getSimpleData(`/guilds/${guildId}/dashboard/automodchart`).subscribe((data: AutoModSplit[]) => {
      this.splittedData = data;
      this.chartData.datasets = [{ data: data.map(x => x.count), label: 'Count', backgroundColor: ['#d84315', '#f9a825', '#2e7d32', '#00695c', '#0277bd', '#1e0ead', '#6a1b9a', '#cc1097'] }];
      if (data.length) {
        this.foundContent = true;
        this.enumManager.getEnum(ApiEnumTypes.AUTOMODTYPE).subscribe(data => {
          this.chartData.labels = this.splittedData.map(d => data.find(x => x.key === d.type)?.value) as string[];
          this.chart?.update();
        });
      }
      this.loading = false;
    }, error => {
      console.error(error);
      this.loading = false;
      this.toastr.error(this.translator.instant('AutoModSplit.FailedToLoad'));
    });
  }
}
