import {
    Component, EventEmitter, Input, OnChanges,
    OnInit, Output, SimpleChanges
} from '@angular/core';
import { Store } from '@ngrx/store';
import * as moment from 'moment';
import * as fromRoot from '../../../../store/reducers';

@Component({
    selector: 'days',
    templateUrl: './day.component.html'
})

export class DayComponent implements OnInit, OnChanges {
    // Input
    @Input('minDate') public minDate: moment.Moment = null;
    @Input('maxDate') public maxDate: moment.Moment = null;
    @Input('daySelected') public daySelected: any;
    @Input('monthSelected') public monthSelected: any;
    @Input('yearSelected') public yearSelected: any;
    @Input('pageCurrent') public pageCurrent: any;

    // Output
    @Output('selectDayEmit') public selectDayEmit: EventEmitter<any> = new EventEmitter();

    // Variable
    private rows: DayArray[] = [];
    private readonly numberOfDayOn6Week: number = 42;
    private readonly hour1Day: number = 23;

    constructor(private store: Store<fromRoot.State>) { }

    public ngOnInit(): void {
        this.rows = [];
        this.generateDay();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['pageCurrent'] || changes['daySelected'] || changes['monthSelected']
            || changes['yearSelected'] || changes['minDate'] || changes['maxDate']) {
            this.generateDay();
        }
    }

    private selectDay(day: ObjectDay): void {
        if (!day.disabled) {
            this.daySelected = day.day;
            this.monthSelected = day.month;
            this.yearSelected = day.year;
            this.selectDayEmit.emit(day);
            this.generateDay();
        }
    }

    private generateDay(): void {
        const startDate: Date = new Date(this.yearSelected, this.monthSelected, this.daySelected);
        startDate.setMonth(startDate.getMonth() + this.pageCurrent);
        const startingDay: number = 0;
        const year: any = startDate.getFullYear();
        const month: any = startDate.getMonth();
        const firstDayOfMonth: any = new Date(year, month, 1);
        const difference: any = startingDay - firstDayOfMonth.getDay();
        const numDisplayedFromPreviousMonth: any = (difference > 0) ? 7 - difference : -difference;
        const firstDate: any = new Date(firstDayOfMonth.getTime());

        if (numDisplayedFromPreviousMonth > 0) {
            firstDate.setDate(-numDisplayedFromPreviousMonth + 1);
        }
        this.getDates(firstDate, month);
    }

    private getDates(startDate: Date, month: any): void {
        const current: any = new Date(startDate.getTime());
        this.rows = [];
        let row: number = 1;
        let days: ObjectDay[] = [];
        // 42 is the number of days on a six-week calendar
        for (let i: number = 1; i <= this.numberOfDayOn6Week; i++) {
            const date: Date = new Date(current.getTime());
            this.fixTimeZone(date);
            const dateObject: ObjectDay = new ObjectDay(
                date.getDate(), date.getMonth(), date.getFullYear(),
                this.checkRange(date), date.getMonth() !== month);
            days.push(dateObject);
            if (i % 7 === 0) {
                this.rows.push(new DayArray(row, days));
                row++;
                days = [];
            }
            current.setDate(current.getDate() + 1);
        }
    }

    private fixTimeZone(date: Date): any {
        const hours: any = date.getHours();
        date.setHours(hours === this.hour1Day ? hours + 2 : 0);
    }

    private checkRange(date: Date): boolean {
        if (this.minDate !== null && this.minDate.isAfter(moment(date), 'days')) {
            return true;
        }

        if (this.maxDate !== null && this.maxDate.isBefore(moment(date), 'days')) {
            return true;
        }

        return false;
    }
}

// tslint:disable-next-line:max-classes-per-file
class DayArray {
    public row: number;
    public listDays: ObjectDay[];
    constructor(row: number, listDays: ObjectDay[]) {
        this.row = row;
        this.listDays = listDays;
    }
}

// tslint:disable-next-line:max-classes-per-file
export class ObjectDay {
    public day: any;
    public month: any;
    public year: any;
    public disabled: boolean;
    public rangeOut: boolean;

    constructor(
        day: any, month: any, year: any, disabled: boolean,
        rangeOut: boolean) {
        this.day = day;
        this.month = month;
        this.year = year;
        this.disabled = disabled;
        this.rangeOut = rangeOut;
    }
}
