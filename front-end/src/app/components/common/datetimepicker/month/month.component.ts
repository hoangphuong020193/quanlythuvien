import {
    Component, EventEmitter, Input, OnChanges,
    OnInit, Output, SimpleChanges
} from '@angular/core';
import * as moment from 'moment';

export class ObjectMonth {
    public monthId: any;
    public month: any;
    public year: any;
    public disabled: boolean;
    constructor(monthId: any, month: any, year: any, disabled: boolean) {
        this.monthId = monthId;
        this.month = month;
        this.year = year;
        this.disabled = disabled;
    }
}

class MonthArray {
    public row: number;
    public listMonths: any[];
    constructor(row: number, listMonths: any[]) {
        this.row = row;
        this.listMonths = listMonths;
    }
}

@Component({
    selector: 'months',
    templateUrl: './month.component.html'
})

export class MonthComponent implements OnInit, OnChanges {
    @Input('minDate') public minDate: moment.Moment;
    @Input('maxDate') public maxDate: moment.Moment;
    @Input('monthSelected') public monthSelected: any;
    @Input('yearSelected') public yearSelected: any;
    @Input('pageCurrent') public pageCurrent: any;

    @Output('selectMonthEmit') public selectMonthEmit: EventEmitter<any> = new EventEmitter();

    private rows: MonthArray[] = [];
    private monthDefine: string[] = ['January', 'February', 'March', 'April', 'May',
        'June', 'July', 'August', 'September', 'October', 'November', 'December'];

    // tslint:disable-next-line:no-empty
    constructor() { }

    public ngOnInit(): void {
        this.rows = [];
        this.generateMonth();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['pageCurrent'] || changes['minDate'] || changes['maxDate'] || changes['yearSelected']) {
            this.generateMonth();
        }
    }

    private selectMonth(month: ObjectMonth): void {
        if (!month.disabled) {
            this.selectMonthEmit.emit(month);
        }
    }

    private generateMonth(): void {
        this.rows = [];
        let row: number = 1;
        let months: ObjectMonth[] = [];
        for (let i: number = 1; i <= this.monthDefine.length; i++) {
            const month: any = this.monthDefine[i - 1];
            const year: any = this.yearSelected + this.pageCurrent;
            months.push(new ObjectMonth(i - 1, month, year, this.checkRange(new Date(year, month, 1))));
            if (i % 3 === 0) {
                this.rows.push(new MonthArray(row, months));
                row++;
                months = [];
            }
        }
    }

    private checkRange(date: Date): boolean {
        if (this.minDate !== null && date.getFullYear() < this.minDate.year()
        && date.getMonth() < this.minDate.month()) {
            return true;
        }

        if (this.maxDate !== null &&
            ((date.getFullYear() > this.maxDate.year())
                || (date.getFullYear() > this.maxDate.year() && date.getMonth() > this.maxDate.month()))) {
            return true;
        }
        return false;
    }
}
