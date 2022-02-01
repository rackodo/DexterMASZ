import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, ReplaySubject } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordUser } from 'src/app/models/DiscordUser';
import { UserMapExpanded } from 'src/app/models/UserMapExpanded';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-guild-usermap',
  templateUrl: './guild-usermap.component.html',
  styleUrls: ['./guild-usermap.component.css']
})
export class GuildUsermappingComponent implements OnInit {

  public newMapFormGroup!: FormGroup;
  public filteredUsersA!: Observable<DiscordUser[]>;
  public filteredUsersB!: Observable<DiscordUser[]>;
  public users: ContentLoading<DiscordUser[]> = { loading: true, content: [] };

  private guildId!: string;
  private timeout: any;
  public loading: boolean = true;
  public searchString: string = '';
  private $showUserMaps: ReplaySubject<UserMapExpanded[]> = new ReplaySubject<UserMapExpanded[]>(1);
  public showUserMaps: Observable<UserMapExpanded[]> = this.$showUserMaps.asObservable();
  public allUserMaps: UserMapExpanded[] = [];
  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private _formBuilder: FormBuilder, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;

    this.resetForm();

    this.filteredUsersA = this.newMapFormGroup.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value.userA))
      );
    this.filteredUsersB = this.newMapFormGroup.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value.userB))
      );

    this.reloadData();
  }

  resetForm() {
    if (this.newMapFormGroup) {
      this.newMapFormGroup.reset();
    }
    this.newMapFormGroup = this._formBuilder.group({
      userA: ['', Validators.required],
      userB: ['', Validators.required],
      reason: ['', Validators.required]
    });
  }

  private _filter(value: string): DiscordUser[] {
    if (!value?.trim()) {
      return this.users.content?.filter(option => !option.bot)?.slice(0, 10) as DiscordUser[];
    }
    const filterValue = value.trim().toLowerCase();

    return this.users.content?.filter(option =>
       ((option.username + "#" + option.discriminator).toLowerCase().includes(filterValue) ||
       option.id.toString().includes(filterValue)) && !option.bot).slice(0, 10) as DiscordUser[];
  }

  private reloadData() {
    this.loading = true;
    this.$showUserMaps.next([]);
    this.allUserMaps = [];
    this.users = { loading: true, content: [] };

    this.api.getSimpleData(`/guilds/${this.guildId}/usermapview`).subscribe((data: UserMapExpanded[]) => {
      this.allUserMaps = data;
      this.loading = false;
      this.search();
    }, error => {
      console.error(error);
      this.loading = false;
      this.toastr.error(this.translator.instant('UsermapTable.FailedToLoad'));
    });

    const params = new HttpParams()
          .set('partial', 'true');
    this.api.getSimpleData(`/discord/guilds/${this.guildId}/users`, true, params).subscribe(data => {
      this.users.content = data;
      this.users.loading = false;
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('UsermapTable.FailedToLoadUser'));
    });
  }

  searchChanged(event: any) {
    clearTimeout(this.timeout);
    var $this = this;
    this.timeout = setTimeout(function () {
      if (event.keyCode != 13) {
        $this.search();
      }
    }, 100);
  }

  search() {
    let tempSearch = this.searchString.toLowerCase();
    if (tempSearch.trim()) {
      this.$showUserMaps.next(this.allUserMaps.filter(
        x => x.userMap.creatorUserId.toString().includes(tempSearch) ||
            x.userMap.userA.toString().includes(tempSearch) ||
            x.userMap.userB.toString().includes(tempSearch) ||
            x.userMap.reason.toLowerCase().includes(tempSearch) ||
            (x.moderator?.username + "#" + x.moderator?.discriminator).toLowerCase().includes(tempSearch) ||
            (x.userA?.username + "#" + x.userA?.discriminator).toLowerCase().includes(tempSearch) ||
            (x.userB?.username + "#" + x.userB?.discriminator).toLowerCase().includes(tempSearch)
      ));
    } else {
      this.$showUserMaps.next(this.allUserMaps);
    }
  }

  removeMap(event: any) {
    this.allUserMaps = this.allUserMaps.filter(x => x.userMap.id !== event);
    this.search();
  }

  updateEvent() {
    this.reloadData();
  }

  createMap() {
    let data = {
      'userA': this.newMapFormGroup.value.userA,
      'userB': this.newMapFormGroup.value.userB,
      'reason': this.newMapFormGroup.value.reason
    };

    this.api.postSimpleData(`/guilds/${this.guildId}/usermap`, data).subscribe(() => {
      this.reloadData();
      this.toastr.success(this.translator.instant('UsermapTable.Created'));
      this.resetForm();
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('UsermapTable.FailedToCreate'));
    });
  }

}
