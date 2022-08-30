import {COMMA, ENTER, SPACE} from '@angular/cdk/keycodes';
import { HttpParams } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, ReplaySubject } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ModCaseTemplate } from 'src/app/models/ModCaseTemplate';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordUser } from 'src/app/models/DiscordUser';
import { ModCaseTemplateExpanded } from 'src/app/models/ModCaseTemplateExpanded';
import { ApiService } from 'src/app/services/api.service';
import { AuthService } from 'src/app/services/auth.service';
import { ModCaseTemplateSettings, ModCaseTemplateExpandedPermission } from 'src/app/models/ModCaseTemplateSettings';
import { TemplateCreateDialogComponent } from '../../dialogs/template-create-dialog/template-create-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { AppUser } from 'src/app/models/AppUser';
import { PunishmentType } from 'src/app/models/PunishmentType';
import { SeverityType } from 'src/app/models/SeverityType';
import { ApiEnum } from 'src/app/models/ApiEnum';
import { EnumManagerService } from 'src/app/services/enum-manager.service';
import { ApiEnumTypes } from 'src/app/models/ApiEnumTypes';
import * as moment from 'moment';
import { TranslateService } from '@ngx-translate/core';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { ModCaseLabel } from 'src/app/models/ModCaseLabel';
import { go, highlight } from 'fuzzysort';

@Component({
  selector: 'app-modcase-add',
  templateUrl: './modcase-add.component.html',
  styleUrls: ['./modcase-add.component.css']
})
export class ModCaseAddComponent implements OnInit {

  public punishedUntilChangeForPicker: ReplaySubject<Date> = new ReplaySubject<Date>(1);
  public punishedUntil?: moment.Moment;

  public templateFormGroup!: FormGroup;
  public userFormGroup!: FormGroup;
  public infoFormGroup!: FormGroup;
  public punishmentFormGroup!: FormGroup;
  public filesFormGroup!: FormGroup;
  public optionsFormGroup!: FormGroup;

  @ViewChild("fileInput", {static: false}) fileInput!: ElementRef;
  public filesToUpload: any[] = [];

  readonly separatorKeysCodes: number[] = [ENTER, COMMA, SPACE];
  public caseLabels: string[] = [];
  public labelInputForm: FormControl = new FormControl();
  @ViewChild('labelInput') labelInput?: ElementRef<HTMLInputElement>;
  public filteredLabels: ReplaySubject<ModCaseLabel[]> = new ReplaySubject(1);
  public remoteLabels: ModCaseLabel[] = [];

  public userForm = new FormControl();
  public filteredUsers!: Observable<DiscordUser[]>;

  public templateSearch: string = "";

  public savingCase: boolean = false;

  public guildId!: string;
  public users: ContentLoading<DiscordUser[]> = { loading: true, content: [] };
  public templates: ContentLoading<ModCaseTemplateExpanded[]> = { loading: true, content: [] };
  public allTemplates: ModCaseTemplateExpanded[] = [];
  public punishmentOptions: ContentLoading<ApiEnum[]> = { loading: true, content: [] };
  public severityOptions: ContentLoading<ApiEnum[]> = { loading: true, content: [] };
  
  public currentUser!: AppUser;
  constructor(private _formBuilder: FormBuilder, private api: ApiService, private toastr: ToastrService, private authService: AuthService, private router: Router, private route: ActivatedRoute, private dialog: MatDialog, private enumManager: EnumManagerService, private translator: TranslateService) {
    this.labelInputForm.valueChanges.subscribe(data => {
      const localeLowerCaseCopy = this.caseLabels.slice().map(x => x.toLowerCase());
      this.filteredLabels.next(data ? this._filterLabel(data) : this.remoteLabels.slice(0, 5).filter(x => !localeLowerCaseCopy.includes(x.label.toLowerCase())));
    });
   }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;

    this.userFormGroup = this._formBuilder.group({
      user: ['', Validators.required]
    });
    this.infoFormGroup = this._formBuilder.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.required]
    });
    this.punishmentFormGroup = this._formBuilder.group({
      punishmentType: ['', Validators.required],
	  severityType: ['None']	  
    });
    this.filesFormGroup = this._formBuilder.group({
      files: ['']
    });

    this.punishmentFormGroup.get('punishmentType')?.valueChanges.subscribe((val: PunishmentType) => {
      if (val !== PunishmentType.Ban && val !== PunishmentType.Mute) {
        this.punishedUntil = undefined;
      }
    });

    this.filteredUsers = this.userFormGroup.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value.user))
      );
    this.reload();
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

  private _filterLabel(value: string): ModCaseLabel[] {
    const localeLowerCaseCopy = this.caseLabels.slice().map(x => x.toLowerCase());
    if (! value)
    {
      return this.remoteLabels.slice(0, 5).filter(x => !localeLowerCaseCopy.includes(x.label.toLowerCase()));
    }

    const filterValue = value.trim().toLowerCase();
    return go(filterValue, this.remoteLabels.slice().filter(x => !localeLowerCaseCopy.includes(x.label.toLowerCase())), { key: 'label' }).map(r => ({
      label: highlight(r),
      cleanValue: r.obj.label,
      count: r.obj.count
    }as ModCaseLabel)).sort((a, b) => b.count - a.count).slice(0, 5);
  }

  uploadInit() {
    const fileInput = this.fileInput.nativeElement;
    fileInput .onchange = () => {
      for (let index = 0; index < fileInput .files.length; index++)
      {
        const file = fileInput.files[index];
        this.filesToUpload.push({ data: file, inProgress: false, progress: 0});
      }
    };
    fileInput.click();
  }

  searchTemplate() {
    if (!this.templateSearch?.trim()) {
      this.templates = { loading: false, content: this.allTemplates.slice(0, 3) };
    }
    const search = this.templateSearch.toLowerCase();
    this.templates = { loading: false, content: this.allTemplates.filter(x =>
        x.caseTemplate.templateName.includes(search) ||
        x.caseTemplate.caseTitle.includes(search) ||
        x.caseTemplate.caseDescription.includes(search) ||
        x.caseTemplate.caseLabels.includes(search) ||

        (x.creator?.username + "#" + x.creator?.discriminator).includes(search) ||

        x.guild.name.includes(search) ||
        x.guild.id.toString().includes(search)
      ).slice(0, 3)};
  }

  reload() {
    this.users = { loading: true, content: [] };
    this.templates = { loading: true, content: [] };
    this.punishmentOptions = { loading: true, content: [] };
    this.templateSearch = "";
    this.allTemplates = [];

    const params = new HttpParams()
          .set('partial', 'true');
    this.api.getSimpleData(`/discord/guilds/${this.guildId}/users`, true, params).subscribe(data => {
      this.users.content = data;
      this.users.loading = false;
    }, error => {
      console.error(error);
      this.users.loading = false;
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToLoad.UserList'));
    });

    this.enumManager.getEnum(ApiEnumTypes.PUNISHMENT).subscribe(data => {
      this.punishmentOptions.content = data;
      this.punishmentOptions.loading = false;
    }, error => {
      console.error(error);
      this.punishmentOptions.loading = false;
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToLoad.PunishmentEnum'));
    });

    this.enumManager.getEnum(ApiEnumTypes.SEVERITY).subscribe(data => {
      this.severityOptions.content = data;
      this.severityOptions.loading = false;
    }, error => {
      console.error(error);
      this.severityOptions.loading = false;
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToLoad.SeverityEnum'));
    });
	
    this.reloadTemplates();

    this.authService.getUserProfile().subscribe((data) => {
      this.currentUser = data;
    });

    this.api.getSimpleData(`/guilds/${this.guildId}/cases/labels`).subscribe(labels => {
      this.remoteLabels = labels;
      this.filteredLabels.next(this._filterLabel(this.labelInputForm.value));
    });
  }

  reloadTemplates() {
    const params = new HttpParams()
          .set('guildid', this.guildId.toString())
    this.api.getSimpleData(`/templatesview`, true, params).subscribe(data => {
      this.allTemplates = data;
      this.searchTemplate();
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToLoad.CaseTemplates'));
      this.searchTemplate();
    });
  }

  applyTemplate(template: ModCaseTemplate, stepper: any) {
    stepper?.next();
    this.infoFormGroup.setValue({
      title: template.caseTitle,
      description: template.caseDescription
    });
    this.caseLabels = template.caseLabels;
    this.punishmentFormGroup.setValue({
      punishmentType: template.casePunishmentType,
	  severityType: template.caseSeverityType
    });
    if (template.casePunishedUntil) {
      this.punishedUntilChangeForPicker.next(template.casePunishedUntil);
    }
    this.toastr.success(this.translator.instant('ModCaseDialog.AppliedTemplate'));
  }

  punishedUntilChanged(date: moment.Moment) {
    this.punishedUntil = date;
  }

  createCase() {
    this.savingCase = true;
	
    const data = {
      title: this.infoFormGroup.value.title,
      description: this.infoFormGroup.value.description,
      userid: this.userFormGroup.value.user?.trim(),
      labels: this.caseLabels,
      punishmentType: this.punishmentFormGroup.value.punishmentType,
	  severityType: this.getSeverity(),
      punishedUntil: this.punishedUntil?.toISOString(),
    }

    const params = new HttpParams()
      .set('handlePunishment', this.punishmentFormGroup.value.handlePunishment ? 'true' : 'false');
	  
	console.log(data);
	
    this.api.postSimpleData(`/guilds/${this.guildId}/cases`, data, params, true, true).subscribe(data => {
      const caseId = data.caseId;
      this.router.navigate(['guilds', this.guildId, 'cases', caseId], { queryParams: { 'reloadfiles': this.filesToUpload.length ?? '0' } });
      this.savingCase = false;
      this.toastr.success(this.translator.instant('ModCaseDialog.CaseCreated'));
      this.filesToUpload.forEach(element => this.uploadFile(element.data, caseId));
    }, error => {
      console.error(error);
      this.savingCase = false;
    });
  }

  deleteTemplate(templateId: number) {
    this.api.deleteData(`/templates/${templateId}`).subscribe(() => {
      this.reloadTemplates();
      this.toastr.success(this.translator.instant('ModCaseDialog.TemplateDeleted'));
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToDeleteTemplate'));
    })
  }

  saveTemplate() {
    this.savingCase = true;
    let templateSetting: ModCaseTemplateSettings = {
      name: '',
      viewPermission: ModCaseTemplateExpandedPermission.Self
    };
    const confirmDialogRef = this.dialog.open(TemplateCreateDialogComponent, {
      data: templateSetting
    });
    confirmDialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        const data = {
          templatename: templateSetting.name,
          viewPermission: templateSetting.viewPermission,
          title: this.infoFormGroup.value.title,
          description: this.infoFormGroup.value.description,
          labels: this.caseLabels,
          punishmentType: this.punishmentFormGroup.value.punishmentType,
          punishedUntil: this.punishedUntil?.toISOString(),
		  severityType: this.getSeverity()
        };

        const params = new HttpParams()
          .set('guildid', this.guildId.toString());

        this.api.postSimpleData(`/templates`, data, params, true, true).subscribe(() => {
          this.toastr.success(this.translator.instant('ModCaseDialog.TemplateSaved'));
          this.savingCase = false;
        }, error => {
          console.error(error);
          this.toastr.error(this.translator.instant('ModCaseDialog.FailedToSaveTemplate'));
          this.savingCase = false;
        });
      } else {
        this.savingCase = false;
      }
    });
  }

  uploadFile(file: any, caseId: string) {
    this.api.postFile(`/guilds/${this.guildId}/cases/${caseId}/files`, file).subscribe(() => {
      this.toastr.success(this.translator.instant('ModCaseDialog.FileUploaded'));
    }, (error) => {
      console.error(error);
      this.toastr.error(this.translator.instant('ModCaseDialog.FailedToUploadFile'));
    });
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      const localeLowerCaseCopy = this.caseLabels.slice().map(x => x.toLowerCase());
      if (! localeLowerCaseCopy.includes(value.trim().toLowerCase())) {
        this.caseLabels.push(value.trim());
      }
    }

    if (input) {
      input.value = '';
    }
  }

  getSeverity(): SeverityType {
	let punishmentType = this.punishmentFormGroup.value.punishmentType;
	let severity = SeverityType.None;
	
	if (punishmentType != PunishmentType.Kick && punishmentType != PunishmentType.Ban)
		severity = this.punishmentFormGroup.value.severityType;
	return severity;
  }

  remove(label: string): void {
    const index = this.caseLabels.indexOf(label);

    if (index >= 0) {
      this.caseLabels.splice(index, 1);
    }
  }

  autoCompleteSelected(event: MatAutocompleteSelectedEvent) {
    this.labelInput!.nativeElement.value = '';
    this.labelInputForm.setValue(null);
    const localeLowerCaseCopy = this.caseLabels.slice().map(x => x.toLowerCase());
    if (! localeLowerCaseCopy.includes(event.option.value.toLowerCase())) {
      this.caseLabels.push(event.option.value);
    }
  }
}

