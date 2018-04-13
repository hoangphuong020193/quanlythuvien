import { Component, OnInit } from '@angular/core';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';

@Component({
    selector: 'check-out',
    templateUrl: './check-out.component.html'
})
export class CheckOutComponent implements OnInit {
    constructor() { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
    }
}
