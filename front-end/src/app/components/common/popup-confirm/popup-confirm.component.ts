import { Component, OnInit } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';

@Component({
    selector: 'popup-confirm',
    templateUrl: './popup-confirm.component.html'
})

export class PopupConfirmComponent
    extends DialogComponent<any, boolean> implements OnInit {

    public title: string;
    public message: string;
    public typeButton: number = TypeButton.ConfirmCancel;

    private labelButtonConfirm: string;
    private labelButtonCancel: string;

    constructor(
        dialogService: DialogService) {
        super(dialogService);
    }

    public ngOnInit(): void {
        switch (this.typeButton) {
            case TypeButton.ConfirmCancel:
            default:
                this.labelButtonConfirm = 'Xác nhận';
                this.labelButtonCancel = 'Huỷ';
                break;
            case TypeButton.YesNo:
                this.labelButtonConfirm = 'Có';
                this.labelButtonCancel = 'Không';
                break;
            case TypeButton.SureCancel:
                this.labelButtonConfirm = 'Chắc chắn';
                this.labelButtonCancel = 'Huỷ';
                break;
        }
    }

    public onConfirm(): void {
        this.result = true;
        this.close();
    }

    public onCancel(): void {
        this.result = false;
        this.close();
    }
}

export enum TypeButton {
    ConfirmCancel = 0,
    YesNo = 1,
    SureCancel = 2
}
