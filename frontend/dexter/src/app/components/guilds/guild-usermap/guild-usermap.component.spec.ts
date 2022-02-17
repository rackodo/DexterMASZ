import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuildUserMappingComponent } from './guild-usermap.component';

describe('GuildUserMappingComponent', () => {
  let component: GuildUserMappingComponent;
  let fixture: ComponentFixture<GuildUserMappingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GuildUserMappingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GuildUserMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
