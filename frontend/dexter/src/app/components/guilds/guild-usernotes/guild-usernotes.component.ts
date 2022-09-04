import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, ReplaySubject } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ContentLoading } from 'src/app/models/ContentLoading';
import { DiscordUser } from 'src/app/models/DiscordUser';
import { UserNoteExpanded } from 'src/app/models/UserNoteExpanded';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-guild-usernotes',
  templateUrl: './guild-usernotes.component.html',
  styleUrls: ['./guild-usernotes.component.css']
})
export class GuildUserNotesComponent implements OnInit {

  public newNoteFormGroup!: FormGroup;
  public filteredUsers!: Observable<DiscordUser[]>;
  public users: ContentLoading<DiscordUser[]> = { loading: true, content: [] };

  private guildId!: string;
  private timeout: any;
  public loading: boolean = true;
  public searchString: string = '';
  private $showUserNotes: ReplaySubject<UserNoteExpanded[]> = new ReplaySubject<UserNoteExpanded[]>(1);
  public showUserNotes: Observable<UserNoteExpanded[]> = this.$showUserNotes.asObservable();
  public allUserNotes: UserNoteExpanded[] = [];
  constructor(private api: ApiService, private toastr: ToastrService, private route: ActivatedRoute, private _formBuilder: FormBuilder, private translator: TranslateService) { }

  ngOnInit(): void {
    this.guildId = this.route.snapshot.paramMap.get('guildid') as string;

    this.resetForm();

    this.filteredUsers = this.newNoteFormGroup.valueChanges
      .pipe(
        startWith(''),
        map(value => this._filter(value.userid))
      );

    this.reloadData();
  }

  resetForm() {
    if (this.newNoteFormGroup) {
      this.newNoteFormGroup.reset();
    }
    this.newNoteFormGroup = this._formBuilder.group({
      userid: ['', Validators.required],
      description: ['', Validators.required]
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
    this.$showUserNotes.next([]);
    this.allUserNotes = [];
    this.users = { loading: true, content: [] };

    this.api.getSimpleData(`/guilds/${this.guildId}/usernoteview`).subscribe((data: UserNoteExpanded[]) => {
      this.allUserNotes = data;
      this.loading = false;
      this.search();
    }, error => {
      console.error(error);
      this.loading = false;
      this.toastr.error(this.translator.instant('UserNoteTable.FailedToLoad'));
    });

    const params = new HttpParams()
          .set('partial', 'true');
    this.api.getSimpleData(`/discord/guilds/${this.guildId}/users`, true, params).subscribe(data => {
      this.users.content = data;
      this.users.loading = false;
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('UserNoteTable.FailedToLoadUser'));
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
      this.$showUserNotes.next(this.allUserNotes.filter(
        x => x.userNote.creatorId.toString().includes(tempSearch) ||
            x.userNote.userId.toString().includes(tempSearch) ||
            x.userNote.description.toLowerCase().includes(tempSearch) ||
            (x.moderator?.username + "#" + x.moderator?.discriminator).toLowerCase().includes(tempSearch) ||
            (x.user?.username + "#" + x.user?.discriminator).toLowerCase().includes(tempSearch)
      ));
    } else {
      this.$showUserNotes.next(this.allUserNotes);
    }
  }

  removeNote(event: any) {
    this.allUserNotes = this.allUserNotes.filter(x => x.userNote.id !== event);
    this.search();
  }

  updateEvent() {
    this.reloadData();
  }

  createNote() {
    let data = {
      'userid': this.newNoteFormGroup.value.userid,
      'description': this.newNoteFormGroup.value.description
    };

    this.api.putSimpleData(`/guilds/${this.guildId}/usernote`, data).subscribe(() => {
      this.reloadData();
      this.resetForm();
      this.toastr.success(this.translator.instant('UserNoteTable.Created'));
    }, error => {
      console.error(error);
      this.toastr.error(this.translator.instant('UserNoteTable.FailedToCreate'));
    });
  }
}
