import { Component } from '@angular/core';
import { DialogComponent, DialogService } from 'angularx-bootstrap-modal';
import { PublisherService } from '../../../../services/publisher.service';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../../../store/reducers';
import { Publisher } from '../../../../models/publisher.model';

@Component({
    selector: 'publisher-editor-popup',
    templateUrl: './publisher-editor-popup.component.html'
})

export class PublisherEditorPopupComponent extends DialogComponent<any, any> {

    public publisher: Publisher;
    private errorMessage: string = '';

    constructor(
        public dialogService: DialogService,
        private store: Store<fromRoot.State>,
        private publisherService: PublisherService) {
        super(dialogService);
    }

    private onSave(): void {
        this.publisherService.savePublisher(this.publisher).subscribe((res) => {
            if (res) {
                this.publisher.id = res;
                this.result = this.publisher;
                this.close();
            } else {
                this.errorMessage = 'Lưu không thành công';
            }
        });
    }
}
