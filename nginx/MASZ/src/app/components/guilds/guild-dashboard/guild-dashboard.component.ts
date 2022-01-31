import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-guild-dashboard',
  templateUrl: './guild-dashboard.component.html',
  styleUrls: ['./guild-dashboard.component.css']
})
export class GuildDashboardComponent implements OnInit {

  public guildId!: bigint;
  public isAdminOrHigher: boolean = false;
  constructor(private toastr: ToastrService, private route: ActivatedRoute, private api: ApiService, private auth: AuthService) { }

  ngOnInit(): void {
    this.guildId = BigInt(this.route.snapshot.paramMap.get('guildid'));
    this.auth.isAdminInGuild(this.guildId).subscribe((data) => {
      this.isAdminOrHigher = data;
    });
  }

}
