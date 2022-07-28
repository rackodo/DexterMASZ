import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'coming-soon',
  templateUrl: './coming-soon.component.html',
  styleUrls: ['./coming-soon.component.css']
})
export class ComingSoonComponent implements OnInit {

  @Input() date : Date | undefined;
  @Input() title : string | undefined;
  @Input() subtitle : string | undefined;
  @Input() description : string | undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
