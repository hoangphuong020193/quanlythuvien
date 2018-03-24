import {
    Component, EventEmitter, Input, OnChanges,
    OnInit, Output, SimpleChanges
} from '@angular/core';
import * as moment from 'moment';

export class ObjectYear {
    public year: number;
    public disabled: boolean;
    constructor(year: number, disabled: boolean) {
        this.year = year;
        this.disabled = disabled;
    }
}

class YearArray {
    public row: number;
    public listYears: ObjectYear[];
    constructor(row: number, listYears: ObjectYear[]) {
        this.row = row;
        this.listYears = listYears;
    }
}

@Component({
    selector: 'years',
    templateUrl: './year.component.html'
})

export class YearComponent implements OnInit, OnChanges {

    @Input('minDate') public minDate: moment.Moment;
    @Input('maxDate') public maxDate: moment.Moment;
    @Input('yearSelected') public yearSelected: any;
    @Input('pageCurrent') public pageCurrent: any;

    @Output('selectYearEmit') public selectYearEmit: EventEmitter<any> = new EventEmitter();

    private rows: YearArray[] = [];

    // tslint:disable-next-line:no-empty
    constructor() { }

    public ngOnInit(): void {
        this.rows = [];
        this.generateYear();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['pageCurrent'] || changes['minDate'] || changes['maxDate']) {
            this.generateYear();
        }
    }

    private selectYear(yearObj: ObjectYear): void {
        if (!yearObj.disabled) { this.selectYearEmit.emit(yearObj); }
    }

    private generateYear(): void {
        this.rows = [];
        let row: number = 1;
        let years: ObjectYear[] = [];
        const yearStartRange: any = this.yearSelected + (20 * this.pageCurrent);

        for (let i: number = 1; i <= 20; i++) {
            const year: number = yearStartRange + i - 1;
            years.push(new ObjectYear(year, this.checkRange(year)));
            if (i % 5 === 0) {
                this.rows.push(new YearArray(row, years));
                row++;
                years = [];
            }
        }
    }

    private checkRange(year: any): boolean {
        if (this.minDate !== null && year < this.minDate.year()) {
            return true;
        }

        if (this.maxDate !== null && year > this.maxDate.year()) {
            return true;
        }
        return false;
    }
}
