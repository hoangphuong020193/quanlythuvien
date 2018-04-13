import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

@Component({
    selector: 'popup-check-out-success',
    templateUrl: './popup-check-out-success.component.html'
})

export class PopupCheckOutSuccessComponent
    extends DialogComponent<any, boolean> {

    public requestCode: string;
    private elementType: 'url' | 'canvas' | 'img' = 'url';

    constructor(
        dialogService: DialogService) {
        super(dialogService);
    }
}
