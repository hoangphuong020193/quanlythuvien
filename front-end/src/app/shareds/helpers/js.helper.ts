import * as moment from 'moment';
import { TimeSheetWeek } from '../../models/timesheet.model';
import { Constant } from '../constant/constant.constant';
import { DayOfWeek } from '../enums/dayofweek.enum';

export class JsHelper {

    public static cloneObject<T>(sourceObject: T): T {
        return JSON.parse(JSON.stringify(sourceObject));
    }

    public static getRandomInt(min: number = 0, max: number): number {
        let minNum: number = Math.ceil(min);
        let maxNum: number = Math.floor(max);
        return Math.floor((Math.random() * (max - min)) + min);
    }
}
