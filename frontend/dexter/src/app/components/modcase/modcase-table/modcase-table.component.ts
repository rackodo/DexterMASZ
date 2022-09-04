import { HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Moment } from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Observable, ReplaySubject } from 'rxjs';
import { ApiEnum } from 'src/app/models/ApiEnum';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import { DiscordUser } from 'src/app/models/DiscordUser';
import { ModCaseFilter } from 'src/app/models/ModCaseFilter';
import { ModCaseTable } from 'src/app/models/ModCaseTable';
import { ModCaseTableEntry } from 'src/app/models/ModCaseTableEntry';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { EnumManagerService } from 'src/app/services/enum-manager.service';

@Component({
  selector: 'app-modcase-table',
  templateUrl: './modcase-table.component.html',
  styleUrls: ['./modcase-table.component.css']
})
export class ModCaseTableComponent implements OnInit {

  @Input() apiUrl: string = 'modcasetable'
  currentPage: number = 0;

  showTable: ModCaseTableEntry[] = [];
  casesTable!: ModCaseTable;
  isModOrHigher!: Observable<boolean>;
  guildId!: string;
  @Input() uniqueIdentifier: string = "casetable";
  loading: boolean = true;

  @Input() search!: string;

  // filters
  public users: ReplaySubject<DiscordUser[]> = new ReplaySubject<DiscordUser[]>(1);
  public caseCreationTypes: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public punishmentTypes: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public editedStatus: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public commentLockedStatus: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public markedToDeleteStatus: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public punishmentActiveStatus: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1);
  public severityTypes: ReplaySubject<ApiEnum[]> = new ReplaySubject<ApiEnum[]>(1)

  public editStatusCtrl: FormControl = new FormControl();
  public commentLockedCtrl: FormControl = new FormControl();
  public markDeleteCtrl: FormControl = new FormControl();
  public punishmentActiveCtrl: FormControl = new FormControl();
  public severityTypeCtrl: FormControl = new FormControl();

  public userFilterPredicate = (user: DiscordUser, search: string) =>
      `${user.username.toLowerCase()}#${user.discriminator}`.indexOf(search.toLowerCase()) > -1 || user.id.toString() == search;
  public userDisplayPredicate = (user: DiscordUser) => `${user.username.toLowerCase()}#${user.discriminator}`;
  public userIdPredicate = (user: DiscordUser) => user.id;
  public userComparePredicate = (user: DiscordUser, user2: DiscordUser) => user?.id == user2?.id;

  public enumFilterPredicate = (enumType: ApiEnum, search: string) =>
      `${enumType.value.toLowerCase()}`.indexOf(search.toLowerCase()) > -1 || enumType.key.toString() == search;
  public enumDisplayPredicate = (enumType: ApiEnum) => enumType.value;

  public excludePermaPunishments: boolean = false;
  public useAdvancedFilters: boolean = true;

  public apiFilter: ModCaseFilter = {};

  constructor(public router: Router, private api: ApiService, private auth: AuthService, private route: ActivatedRoute, private toastr: ToastrService, private translator: TranslateService, private enumManager: EnumManagerService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((data) => {
      this.guildId = data.get('guildid') as string;
      this.isModOrHigher = this.auth.isModInGuild(this.guildId);
      this.reload();
    });

    this.editStatusCtrl.valueChanges.subscribe(value => {
      this.selectedEditedStatusChanged(value);
    });
    this.commentLockedCtrl.valueChanges.subscribe(value => {
      this.selectedCommentLockedStatusChanged(value);
    });
    this.markDeleteCtrl.valueChanges.subscribe(value => {
      this.selectedDeleteStatusChanged(value);
    });
    this.punishmentActiveCtrl.valueChanges.subscribe(value => {
      this.selectedPunishmentActiveStatusChanged(value);
    });
    this.severityTypeCtrl.valueChanges.subscribe(value => {
      this.selectedSeverityTypeChanged(value);
    });

    this.enumManager.getEnum(ApiEnumTypes.CASECREATIONTYPE).subscribe(data => {
      this.caseCreationTypes.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.PUNISHMENT).subscribe(data => {
      this.punishmentTypes.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.EDITSTATUS).subscribe(data => {
      this.editedStatus.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.LOCKEDCOMMENTSTATUS).subscribe(data => {
      this.commentLockedStatus.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.MARKEDTODELETESTATUS).subscribe(data => {
      this.markedToDeleteStatus.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.PUNISHMENTACTIVESTATUS).subscribe(data => {
      this.punishmentActiveStatus.next(data);
    });
    this.enumManager.getEnum(ApiEnumTypes.SEVERITY).subscribe(data => {
      this.severityTypes.next(data);
    });
  }

  reload() {
    this.loadFirstCases();
    this.loadUsers();
  }

  private loadUsers() {
    const params = new HttpParams()
          .set('partial', 'true');
    this.api.getSimpleData(`/discord/guilds/${this.guildId}/users`, true, params).subscribe(data => {
      this.users.next(data);
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToLoad.UserList'));
    });
  }

  searchChanged(event: any) {
    this.apiFilter.customTextFilter = event;
  }

  selectedUserChanged(users: DiscordUser[]) {
    this.apiFilter.userIds = users?.map(x => x.id) ?? [];
  }

  selectedModChanged(users: DiscordUser[]) {
    this.apiFilter.moderatorIds = users?.map(x => x.id) ?? [];
  }

  selectedCreationTypeChanged(types: ApiEnum[]) {
    this.apiFilter.creationTypes = types?.map(x => x.key) ?? [];
  }

  selectedPunishmentTypeChanged(types: ApiEnum[]) {
    this.apiFilter.punishmentTypes = types?.map(x => x.key) ?? [];
  }

  selectedEditedStatusChanged(type: ApiEnum) {
    this.apiFilter.edited = type.key === 0 ? undefined : type.key !== 1;
  };

  selectedSeverityTypeChanged(types: ApiEnum[]) {
    this.apiFilter.severityTypes = types?.map(x => x.key) ?? [];
  }

  selectedCommentLockedStatusChanged(type: ApiEnum) {
    this.apiFilter.lockedComments = type.key === 0 ? undefined : type.key !== 1;
  };

  selectedDeleteStatusChanged(type: ApiEnum) {
    this.apiFilter.markedToDelete = type.key === 0 ? undefined : type.key !== 1;
  };

  selectedPunishmentActiveStatusChanged(type: ApiEnum) {
    this.apiFilter.punishmentActive = type.key === 0 ? undefined : type.key !== 1;
  };

  selectedSinceChanged(date: Moment) {
    this.apiFilter.since = date?.toISOString();
  }

  selectedUntilChanged(date: Moment) {
    this.apiFilter.before = date?.toISOString();
  }

  selectedPunishmentSinceChanged(date: Moment) {
    this.apiFilter.punishedUntilMin = date?.toISOString();
  }

  selectedPunishmentUntilChanged(date: Moment) {
    this.apiFilter.punishedUntilMax = date?.toISOString();
  }

  loadFirstCases() {
    this.loading = true;
    this.currentPage = 0;
    this.api.postSimpleData(`/guilds/${this.guildId}/${this.apiUrl}`, this.apiFilter).subscribe((data: ModCaseTable) => {
      this.loading = false;
      this.casesTable = data;
      this.applyCurrentFilters();
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseTable.FailedToLoad'));
    });
  }

  loadNextPage() {
    // this.loading = true;
    this.currentPage++;
    const params = new HttpParams()
          .set('startPage', this.currentPage.toString());
    this.api.postSimpleData(`/guilds/${this.guildId}/${this.apiUrl}`, {}, params).subscribe((data: ModCaseTable) => {
      // this.loading = false;
      this.casesTable.cases.push(...data.cases);
      this.casesTable.fullSize = data.fullSize;
      this.applyCurrentFilters();
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseTable.FailedToLoad'));
    });
  }

  applyCurrentFilters() {
    let temp = this.casesTable.cases.slice();
    if (this.excludePermaPunishments) {
      temp = temp.filter(x => x.modCase.punishedUntil != null || x.modCase.punishmentType === 0 || x.modCase.punishmentType === 2);
    }
    this.showTable = temp;
  }

}
