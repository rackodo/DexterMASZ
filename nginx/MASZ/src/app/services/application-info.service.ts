import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { DiscordApplication } from '../models/DiscordApplication';

@Injectable({
  providedIn: 'root'
})
export class ApplicationInfoService {

  currentApplicationInfo: ReplaySubject<DiscordApplication> = new ReplaySubject<DiscordApplication>(1);

  constructor() { }

  infoChanged(data: DiscordApplication) {
    this.currentApplicationInfo.next(data);
    if (data?.iconUrl) {
      document.getElementById('favicon')?.setAttribute('href', data.iconUrl);
    }
  }
}
