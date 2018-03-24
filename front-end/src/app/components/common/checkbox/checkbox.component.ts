import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'checkbox',
    templateUrl: 'checkbox.component.html'
})

export class CheckboxComponent implements OnInit {
    @Input('checked') private checked: boolean = false;
    @Input('disabled') private disabled: boolean = false;
    @Input('text') private text: string = '';
    @Output('checkedCallback') private checkedCallback: EventEmitter<boolean> = new EventEmitter();
    // tslint:disable-next-line:no-empty
    constructor() { }

    // tslint:disable-next-line:no-empty
    public ngOnInit(): void { }

    private onClick(): void {
        if (!this.disabled) {
            this.checked = !this.checked;
            this.checkedCallback.emit(this.checked);
        }
    }
}
