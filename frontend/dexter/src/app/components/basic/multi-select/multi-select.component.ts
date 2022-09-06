import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatSelect } from '@angular/material/select';
import { ReplaySubject, Subject } from 'rxjs';
import { filter, take, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-multi-select',
  templateUrl: './multi-select.component.html',
  styleUrls: ['./multi-select.component.css']
})
export class MultiSelectComponent implements OnInit, AfterViewInit, OnDestroy {

  @Input() public elements!: ReplaySubject<any[]>;
  public _elements: any[] = [];
  public selectedElements: any[] = [];
  public filteredElements: ReplaySubject<any[]> = new ReplaySubject<any[]>(1);
  protected filteredElementsCache: any[] = [];

  @Input() public displayPredicate: (x: any) => string = (x: any) => { return x };
  @Input() public idPredicate: (x: any) => string = (x: any) => { return x };
  @Input() public searchPredicate: (x: any, search: string) => boolean = () => { return true; };
  @Input() public compareWithPredicate: (x: any, y: any) => boolean = (x: any, y: any) => { return x && y };
  @Input() placeholderKey = 'Select.Select';
  @Input() placeholderSearchKey = 'Select.Search';

  @Output() public selected = new EventEmitter<any[]>();

  public multiCtrl: FormControl = new FormControl();
  public multiFilterCtrl: FormControl = new FormControl();

  public filterAmount: number = 20;
  public hasMoreToLoad: boolean = true;

  protected _onDestroy = new Subject<void>();

  @ViewChild('multiSelect', { static: true }) multiSelect?: MatSelect;

  constructor() { }

  ngOnInit() {
    this.elements.subscribe(val => {
      this._elements = val;

      // set initial selection
      // this.multiCtrl.setValue([this._elements[10], this._elements[11], this._elements[12]]);

      // load the initial bank list
      this.filteredElements.next(this._elements.slice(0, this.filterAmount));
    });

    // listen for search field value changes
    this.multiFilterCtrl.valueChanges
      .pipe(takeUntil(this._onDestroy))
      .subscribe(() => {
        this.hasMoreToLoad = true;
        this.filterBanksMulti();
      });

    this.multiCtrl.valueChanges.subscribe(val => {
      this.selected.emit(val);
    });
  }

  ngAfterViewInit() {
    this.setInitialValue();
  }

  ngOnDestroy() {
    this._onDestroy.next();
    this._onDestroy.complete();
  }

  increaseFilter() {
    this.filterAmount += 10;
    this.filterBanksMulti();
  }

  /**
   * Sets the initial value after the filteredBanks are loaded initially
   */
  protected setInitialValue() {
    this.filteredElements
      .pipe(take(1), takeUntil(this._onDestroy))
      .subscribe(() => {
        // setting the compareWith property to a comparison function
        // triggers initializing the selection according to the initial value of
        // the form control (i.e. _initializeSelection())
        // this needs to be done after the filteredBanks are loaded initially
        // and after the mat-option elements are available
        if (this.multiSelect) {
          this.multiSelect.compareWith = this.compareWithPredicate;
        }
      });
  }

  protected filterBanksMulti() {
    if (!this._elements) {
      return;
    }
    // get the search keyword
    let search = this.multiFilterCtrl.value;
    if (!search) {
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the banks
    this.filteredElementsCache = this._elements.slice().filter(x => this.searchPredicate(x, search));
    this.filteredElements.next(this.filteredElementsCache.slice(0, this.filterAmount));

    if (this.filteredElementsCache.length < this.filterAmount)
      this.hasMoreToLoad = false;
  }
}
