import { Component, Input, OnInit, ViewChild } from '@angular/core';

@Component({
    selector: 'loading-page',
    templateUrl: './loading-page.component.html'
})
export class LoadingPageComponent implements OnInit {
    @Input('backdrop') private backdrop: boolean = true;
    @Input('className') private className: string = '';
    @Input('local') private local: boolean = false;
    @ViewChild('element') private element: any;

    // tslint:disable-next-line:no-empty
    constructor() { }

    public ngOnInit(): any {
        if (this.className !== '') {
            this.element.nativeElement.classList.add(this.className);
        }
    }
}
