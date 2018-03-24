import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { Config } from '../../../config';

@Component({
    selector: 'book-image',
    templateUrl: './book-image.component.html'
})
export class BookImageComponent implements OnChanges {
    @Input('src') private src: string = '';
    @Input('code') private code: string = '';

    constructor() {
        this.generateImg();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['src'] || changes['code']) {
            this.generateImg();
        }
    }

    private generateImg(): void {
        if (this.code === '' && this.src === '') {
            this.noImg();
        } else if (this.code !== '') {
            this.getBookImgURL(this.code);
        }
    }

    private noImg(): void {
        this.src = './styles/img/no_book_cover.jpg';
    }

    private getBookImgURL(bookCode: string): void {
        this.src = Config.getBookImgApiUrl(bookCode);
    }
}
