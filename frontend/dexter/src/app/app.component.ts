import { MediaMatcher } from '@angular/cdk/layout';
import { HttpParams } from '@angular/common/http';
import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from './components/dialogs/confirmation-dialog/confirmation-dialog.component';
import { GuildDeleteDialogComponent } from './components/guilds/guild-delete-dialog/guild-delete-dialog.component';
import { DEFAULT_LANGUAGE, DEFAULT_TIMEZONE, LANGUAGES, TIMEZONES } from './config/config';
import { AppUser } from './models/AppUser';
import { DiscordApplication } from './models/DiscordApplication';
import { DiscordGuild } from './models/DiscordGuild';
import { GuildDeleteDialogData } from './models/GuildDeleteDialogData';
import { LocalAppSettings } from './models/LocalAppSettings';
import { ApiService } from './services/api.service';
import { ApplicationInfoService } from './services/application-info.service';
import { AuthService } from './services/auth.service';
import { CookieTrackerService } from './services/cookie-tracker.service';
import { TimezoneService } from './services/timezone.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {
  title = 'Dexter';
  mobileQuery: MediaQueryList;
  activatedNav: string[] = ['', ''];
  loggedIn: boolean = false;
  currentUser!: AppUser;
  guildDeleteDialogData!: GuildDeleteDialogData;
  @ViewChild('snav') snav: any;
  applicationInfo?: DiscordApplication = undefined;
  languages = LANGUAGES;
  timezones = TIMEZONES;
  currentAppSettings: LocalAppSettings = {
    language: DEFAULT_LANGUAGE,
    timezone: DEFAULT_TIMEZONE
  };

  public changeLanguage(lang: string) {
    this.translator.use(lang);
    this.currentAppSettings.language = lang;
    this.cookieTracker.updateSettings(this.currentAppSettings);
  }

  public changeTimezone(zone: string) {
    this.timezoneService.timezoneChanged(zone);
    this.currentAppSettings.timezone = zone;
    this.cookieTracker.updateSettings(this.currentAppSettings);
  }

  private _mobileQueryListener: () => void;

  constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private router: Router, public route: ActivatedRoute,
              private auth: AuthService, private toastr: ToastrService, private dialog: MatDialog, private api: ApiService, private matIconRegistry: MatIconRegistry, private domSanitizer: DomSanitizer, private applicationInfoService: ApplicationInfoService, private translator: TranslateService, private timezoneService: TimezoneService, private cookieTracker: CookieTrackerService) {
    this.mobileQuery = media.matchMedia('(max-width: 1000px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);

    this.matIconRegistry.addSvgIcon(
      "githublogo",
      this.domSanitizer.bypassSecurityTrustResourceUrl("/assets/img/github.svg"));
    this.matIconRegistry.addSvgIcon(
      "shards",
      this.domSanitizer.bypassSecurityTrustResourceUrl("/assets/img/shard.svg"));
  }

  ngOnInit(): void {
    this.router.events.subscribe((data) => {
      if (data instanceof NavigationEnd) {
        this.activatedNav = data.url.split('?')[0].split('/');
      }
    });

    if (this.auth.isLoggedIn()) {
      this.login();
    }

    this.applicationInfoService.currentApplicationInfo.subscribe(data => {
      this.applicationInfo = data;
    });

    this.timezoneService.selectedTimezone.subscribe(data => {
      this.currentAppSettings.timezone = data;
    });

    this.api.getSimpleData('/meta/application').subscribe((data: DiscordApplication) => {
      this.applicationInfoService.infoChanged(data);
    });

    this.translator.onLangChange.subscribe(() => {
      this.currentAppSettings.language = this.translator.currentLang;
    });

    this.cookieTracker.settings.subscribe((data: LocalAppSettings) => {
      if (TIMEZONES.includes(data.timezone)) {  // user might enter random stuff
        this.currentAppSettings.timezone = data.timezone;
        this.timezoneService.timezoneChanged(data.timezone);
      }
      if (LANGUAGES.filter(x => x.language === data.language).length > 0) {  // user might enter random stuff
        this.currentAppSettings.language = data.language;
        this.translator.use(data.language);
      }
    });
  }

  login() {
    this.loggedIn = true;
      this.auth.getUserProfile().subscribe((data: AppUser) => {
        this.loggedIn = true;
        this.currentUser = data;
      }, () => {
        this.loggedIn = false;
        this.currentUser = { } as AppUser;
    });
  }

  ngAfterViewInit(): void {
    if (! this.mobileQuery.matches) {
      this.snav?.open();
    }
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  deleteGuild(guild: DiscordGuild) {
    this.guildDeleteDialogData = {
      guild: guild,
      deleteData: false
    }
    const dialogRef = this.dialog.open(GuildDeleteDialogComponent, {
      data: this.guildDeleteDialogData
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const confirmDialogRef = this.dialog.open(ConfirmationDialogComponent);
        confirmDialogRef.afterClosed().subscribe(confirmed => {
          if (confirmed) {
            let params = new HttpParams()
              .set('deletedata', this.guildDeleteDialogData.deleteData ? 'true' : 'false');
            this.api.deleteData(`/guilds/${this.guildDeleteDialogData.guild.id}`, params).subscribe((data) => {
              this.toastr.success(this.translator.instant('Dexter.GuildDeleted'));
              this.auth.resetCache();
              this.login();
              this.router.navigate(['guilds']);
            }, (error) => {
              this.toastr.error(this.translator.instant('Dexter.FailedToDeleteGuild'));
            });
          }
        });
      }
    });
  }

  open(...target: any[]) {
    const url = target.join('/');
    if (url === 'guilds' && !this.loggedIn) {
      this.toastr.warning(this.translator.instant('Dexter.PleaseLoginFirst'))
    } else {
      this.router.navigateByUrl(url);
    }

    if (this.mobileQuery.matches) {
      this.snav?.toggle();
    }
  }
}
