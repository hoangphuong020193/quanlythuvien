import { Component, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../store/reducers';
import { Publisher } from '../../../models/index';
import { DialogService } from 'angularx-bootstrap-modal';
import {
    PublisherEditorPopupComponent
} from './publisher-editor-popup/publisher-editor-popup.component';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { JsHelper } from '../../../shareds/helpers/js.helper';
import { PublisherService } from '../../../services/publisher.service';

@Component({
    selector: 'publisher-management',
    templateUrl: './publisher-management.component.html'
})

export class PublisherManagementComponent implements OnInit {

    private listPublishers: Publisher[] = [];
    private listOrigins: Publisher[] = [];

    @ViewChild('inputBox') private inputBoxElement: any;

    constructor(
        private store: Store<fromRoot.State>,
        private publisherService: PublisherService,
        private dialogService: DialogService) { }

    public ngOnInit(): void {
        JQueryHelper.showLoading();
        this.store.select(fromRoot.getPublisher).subscribe((res) => {
            if (res) {
                this.listOrigins = res;
                this.listPublishers = this.listOrigins;
                JQueryHelper.hideLoading();
            }
        });
    }

    private onKeyUp(): void {
        const value: string = this.inputBoxElement.nativeElement.value;
        if (value !== '') {
            this.listPublishers =
                this.listOrigins.filter((x) =>
                    x.name.toLowerCase().search(value.toLowerCase()) !== -1);
        } else {
            this.listPublishers = this.listOrigins;
        }
    }

    private addNew(): void {
        this.publisherEditor(new Publisher());
    }

    private publisherEditor(publisher: Publisher, index: number = -1): void {
        this.dialogService.addDialog(PublisherEditorPopupComponent, {
            publisher: JsHelper.cloneObject(publisher)
        }).subscribe((res) => {
            if (res) {
                // TODO
            }
        });
    }

    private updateStatus(index: number, enabled: boolean): void {
        this.listPublishers[index].enabled = enabled;
        this.publisherService.savePublisher(this.listPublishers[index]).subscribe();
    }
}
