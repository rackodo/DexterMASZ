import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ImageVersion } from 'src/app/models/ImageVersion';

@Component({
  selector: 'app-update-warning',
  templateUrl: './update-warning.component.html',
  styleUrls: ['./update-warning.component.css']
})
export class UpdateWarningComponent implements OnInit {

  @Input() newestVersionObservable?: Observable<ImageVersion> = undefined;
  public newestVersion?: ImageVersion = undefined;

  constructor() { }

  ngOnInit(): void {
    if (this.newestVersionObservable) {
      this.newestVersionObservable.subscribe(version => {
        this.newestVersion = version;
      });
    }
  }
}
