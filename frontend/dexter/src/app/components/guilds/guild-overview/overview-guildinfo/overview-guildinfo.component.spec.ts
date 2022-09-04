import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuildInfoComponent } from './overview-guildinfo.component';

describe('GuildInfoComponent', () => {
  let component: GuildInfoComponent;
  let fixture: ComponentFixture<GuildInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuildInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuildInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
