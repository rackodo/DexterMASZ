import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { MatSliderModule } from '@angular/material/slider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatStepperModule } from '@angular/material/stepper';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { IndexComponent } from './components/basic/index/index.component';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AuthService } from './services/auth.service';
import { ApiInterceptor } from './services/api-interceptor';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiService } from './services/api.service';
import { AuthGuard } from './guards/auth.guard';
import { GuildCardComponent } from './components/guilds/guild-card/guild-card.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { CookieModule } from 'ngx-cookie';
import { PatchnotesComponent } from './components/basic/patchnotes/patchnotes.component';
import { TermsComponent } from './components/basic/terms/terms.component';
import { LegalComponent } from './components/basic/legal/legal.component';
import { EpiclistComponent } from './components/basic/epiclist/epiclist.component';
import { CommonModule } from '@angular/common';
import { GuildOverviewComponent } from './components/guilds/guild-overview/guild-overview.component';
import { GuildAddComponent } from './components/guilds/guild-add/guild-add.component';
import { GuildListComponent } from './components/guilds/guild-list/guild-list.component';
import { GuildDeleteDialogComponent } from './components/guilds/guild-delete-dialog/guild-delete-dialog.component';
import { ConfirmationDialogComponent } from './components/dialogs/confirmation-dialog/confirmation-dialog.component';
import { GuildEditComponent } from './components/guilds/guild-edit/guild-edit.component';
import { ModCaseAddComponent } from './components/modcase/modcase-add/modcase-add.component';
import { ModCaseEditComponent } from './components/modcase/modcase-edit/modcase-edit.component';
import { ModCaseViewComponent } from './components/modcase/modcase-view/modcase-view.component';
import { GuildDashboardComponent } from './components/guilds/guild-dashboard/guild-dashboard.component';
import { ModCaseTableComponent } from './components/modcase/modcase-table/modcase-table.component';
import { AutoModTableComponent } from './components/automod/automod-table/automod-table.component';
import { DashboardMotdComponent } from './components/guilds/guild-dashboard/dashboard-motd/dashboard-motd.component';
import { DashboardQuickSearchComponent } from './components/guilds/guild-dashboard/dashboard-quicksearch/dashboard-quicksearch.component';
import { GuildInfoComponent } from './components/guilds/guild-overview/overview-guildinfo/overview-guildinfo.component';
import { DashboardGuildStatsComponent } from './components/guilds/guild-dashboard/dashboard-guild-stats/dashboard-guild-stats.component';
import { DashboardCaseListComponent } from './components/guilds/guild-dashboard/dashboard-case-list/dashboard-case-list.component';
import { DashboardCommentListComponent } from './components/guilds/guild-dashboard/dashboard-comment-list/dashboard-comment-list.component';
import { DashboardChartsComponent } from './components/guilds/guild-dashboard/dashboard-charts/dashboard-charts.component';
import { GuildConfigComponent } from './components/guilds/guild-config/guild-config.component';
import { QuickSearchCaseResultComponent } from './components/guilds/guild-dashboard/dashboard-quicksearch/quicksearch-case-result/quicksearch-case-result.component';
import { QuickSearchModerationResultComponent } from './components/guilds/guild-dashboard/dashboard-quicksearch/quicksearch-moderation-result/quicksearch-moderation-result.component';
import { CountChartComponent } from './components/guilds/guild-dashboard/dashboard-charts/count-chart/count-chart.component';
import { ChartsModule } from 'ng2-charts';
import { ModCaseCardComponent } from './components/modcase/modcase-table/modcase-card/modcase-card.component';
import { ModCaseCardCompactComponent } from './components/guilds/guild-dashboard/dashboard-case-list/modcase-card-compact/modcase-card-compact.component';
import { DashboardAutoModSplitComponent } from './components/guilds/guild-dashboard/dashboard-automod-split/dashboard-automod-split.component';
import { AutoModCardComponent } from './components/automod/automod-card/automod-card.component';
import { CommentsCardCompactComponent } from './components/guilds/guild-dashboard/dashboard-comment-list/comments-card-compact/comments-card-compact.component';
import { MotdConfigComponent } from './components/guilds/guild-config/motd-config/motd-config.component';
import { AutoModConfigComponent } from './components/guilds/guild-config/automod-config/automod-config.component';
import { AutoModRuleComponent } from './components/guilds/guild-config/automod-config/automod-rule/automod-rule.component';
import { MaterialFileInputModule } from 'ngx-material-file-input';
import { CaseDeleteDialogComponent } from './components/dialogs/case-delete-dialog/case-delete-dialog.component';
import { CommentEditDialogComponent } from './components/dialogs/comment-edit-dialog/comment-edit-dialog.component';
import { NgxMatDatetimePickerModule, NgxMatTimepickerModule, NgxMatNativeDateModule } from '@angular-material-components/datetime-picker';
import { TemplateCreateDialogComponent } from './components/dialogs/template-create-dialog/template-create-dialog.component';
import { UserScanComponent } from './components/usergraph/userscan/userscan.component';
import { GuildUserMappingComponent } from './components/guilds/guild-usermap/guild-usermap.component';
import { GuildUserNotesComponent } from './components/guilds/guild-usernotes/guild-usernotes.component';
import { UserNoteCardComponent } from './components/guilds/guild-usernotes/usernote-card/usernote-card.component';
import { UserNoteEditDialogComponent } from './components/dialogs/usernote-edit-dialog/usernote-edit-dialog.component';
import { UserMapCardComponent } from './components/guilds/guild-usermap/usermap-card/usermap-card.component';
import { UserMapEditDialogComponent } from './components/dialogs/usermap-edit-dialog/usermap-edit-dialog.component';
import { QuickSearchUserMapsResultComponent } from './components/guilds/guild-dashboard/dashboard-quicksearch/quicksearch-usermaps-result/quicksearch-usermaps-result.component';
import { AdminStatsComponent } from './components/api/adminstats/adminstats.component';
import { AdminlistComponent } from './components/api/adminstats/adminlist/adminlist.component';
import { StatcardComponent } from './components/api/adminstats/statcard/statcard.component';
import { GuildIconComponent } from './components/basic/guild-icon/guild-icon.component';
import { OauthFailedComponent } from './components/errors/oauth-failed/oauth-failed.component';
import { EnumManagerService } from './services/enum-manager.service';
import { ApplicationInfoService } from './services/application-info.service';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DEFAULT_LANGUAGE } from './config/config';
import { DateDisplayComponent } from './components/basic/date-display/date-display.component';
import { FloorPipePipe } from './pipes/floor-pipe.pipe';
import { DateFormatPipe } from './pipes/date-format.pipe';
import { TimezoneService } from './services/timezone.service';
import { CookieTrackerService } from './services/cookie-tracker.service';
import { DatePickerComponent } from './components/basic/date-picker/date-picker.component';
import { AuditlogConfigComponent } from './components/guilds/guild-config/auditlog-config/auditlog-config.component';
import { AuditlogConfigRuleComponent } from './components/guilds/guild-config/auditlog-config/auditlog-config-rule/auditlog-config-rule.component';
import { MultiSelectComponent } from './components/basic/multi-select/multi-select.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { AppSettingsComponent } from './components/api/appsettings/appsettings.component';
import { GuildMessagesComponent } from './components/guilds/guild-messages/guild-messages.component';
import { GuildMessageCardComponent } from './components/guilds/guild-messages/guild-message-card/guild-message-card.component';
import { ScheduledMessageEditDialogComponent } from './components/dialogs/scheduled-message-edit-dialog/scheduled-message-edit-dialog.component';
import { ProfileDashboardComponent } from './components/profiles/profile-dashboard/profile-dashboard.component';
import { StoreDashboardComponent } from './components/profiles/store/store-dashboard/store-dashboard.component';
import { ProfileOverviewComponent } from './components/profiles/profile-overview/profile-overview.component';
import { RankcardCustomizerComponent } from './components/profiles/rankcard-customizer/rankcard-customizer.component';
import { RankcardPreviewComponent } from './components/profiles/rankcard-customizer/rankcard-preview/rankcard-preview.component';
import { ImageUrlDialogComponent } from './components/dialogs/image-url-dialog/image-url-dialog.component';
import { BeautifyFileNamePipe } from './pipes/beautify-file-name.pipe';
import { GuildLeaderboardComponent } from './components/guilds/guild-leaderboard/guild-leaderboard.component';
import { ComingSoonComponent } from './components/basic/coming-soon/coming-soon.component';
import { LevelsConfigComponent } from './components/guilds/guild-config/levels-config/levels-config.component';
import { NumberInputDialogComponent } from './components/dialogs/number-input-dialog/number-input-dialog.component';
import { LeaderboardItemComponent } from './components/guilds/guild-leaderboard/leaderboard-item/leaderboard-item.component';
import { LeaderboardRankingComponent } from './components/guilds/guild-leaderboard/leaderboard-ranking/leaderboard-ranking.component';
import { UnitFormatPipe } from './pipes/unit-format.pipe';
import { OffsetEditorComponent } from './components/profiles/rankcard-customizer/offset-editor/offset-editor.component';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    LegalComponent,
    GuildCardComponent,
    NotFoundComponent,
    PatchnotesComponent,
    TermsComponent,
    EpiclistComponent,
    GuildOverviewComponent,
    GuildAddComponent,
    GuildListComponent,
    GuildDeleteDialogComponent,
    ConfirmationDialogComponent,
    GuildEditComponent,
    ModCaseAddComponent,
    ModCaseEditComponent,
    ModCaseViewComponent,
    GuildDashboardComponent,
    ModCaseTableComponent,
    AutoModTableComponent,
    DashboardMotdComponent,
    DashboardQuickSearchComponent,
    GuildInfoComponent,
    DashboardGuildStatsComponent,
    DashboardCaseListComponent,
    DashboardCommentListComponent,
    DashboardChartsComponent,
    GuildConfigComponent,
    QuickSearchCaseResultComponent,
    QuickSearchModerationResultComponent,
    CountChartComponent,
    ModCaseCardComponent,
    ModCaseCardCompactComponent,
    DashboardAutoModSplitComponent,
    AutoModCardComponent,
    CommentsCardCompactComponent,
    MotdConfigComponent,
    AutoModConfigComponent,
    AutoModRuleComponent,
    CaseDeleteDialogComponent,
    CommentEditDialogComponent,
    TemplateCreateDialogComponent,
    UserScanComponent,
    GuildUserMappingComponent,
    GuildUserNotesComponent,
    UserNoteCardComponent,
    UserNoteEditDialogComponent,
    UserMapCardComponent,
    UserMapEditDialogComponent,
    QuickSearchUserMapsResultComponent,
    AdminStatsComponent,
    AdminlistComponent,
    StatcardComponent,
    GuildIconComponent,
    OauthFailedComponent,
    DateDisplayComponent,
    FloorPipePipe,
    DateFormatPipe,
    DatePickerComponent,
    AuditlogConfigComponent,
    AuditlogConfigRuleComponent,
    MultiSelectComponent,
    AppSettingsComponent,
    GuildMessagesComponent,
    GuildMessageCardComponent,
    ScheduledMessageEditDialogComponent,
    ProfileDashboardComponent,
    StoreDashboardComponent,
    ProfileOverviewComponent,
    RankcardCustomizerComponent,
    RankcardPreviewComponent,
    ImageUrlDialogComponent,
    BeautifyFileNamePipe,
    GuildLeaderboardComponent,
    ComingSoonComponent,
    LevelsConfigComponent,
    NumberInputDialogComponent,
    LeaderboardItemComponent,
    LeaderboardRankingComponent,
    UnitFormatPipe,
    OffsetEditorComponent
  ],
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgxMatNativeDateModule,
    CookieModule.forRoot(),
    ToastrModule.forRoot({
      progressBar: true,
      timeOut: 5000
    }),
    MatTooltipModule,
    MatAutocompleteModule,
    MaterialFileInputModule,
    MatProgressBarModule,
    MatChipsModule,
    MatSlideToggleModule,
    MatTabsModule,
    MatCheckboxModule,
    MatDialogModule,
    MatSelectModule,
    MatInputModule,
    MatStepperModule,
    MatExpansionModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatListModule,
    MatSidenavModule,
    MatIconModule,
    MatToolbarModule,
    MatButtonModule,
    MatSliderModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,

    BrowserModule,
    AppRoutingModule,
    ChartsModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      },
      defaultLanguage: DEFAULT_LANGUAGE,
      useDefaultLang: true
    }),
    NgxMatSelectSearchModule
  ],
  providers: [
    ToastrService,
    AuthService, {
      provide: HTTP_INTERCEPTORS,
      useClass: ApiInterceptor,
      multi: true
    },
    AuthGuard,
    EnumManagerService,
    ApiService,
    ApplicationInfoService,
    TimezoneService,
    CookieTrackerService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
