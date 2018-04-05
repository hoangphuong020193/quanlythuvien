import { Component, OnInit, Input } from '@angular/core';
import { Book } from '../../../models/book.model';
import { NgxCarousel } from 'ngx-carousel';
import { BookService } from '../../../services/book.service';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';
import { BookSection } from '../../../shareds/constant/book-section.constant';
import { RouterService } from '../../../services/router.service';

@Component({
    selector: 'book-section',
    templateUrl: './book-section.component.html'
})

export class BookSectionComponent implements OnInit {
    @Input('typeSection') public typeSection: string = '';

    public carouselOption: NgxCarousel;

    private books: Book[] = [];

    constructor(
        private bookService: BookService,
        private routerService: RouterService) { }

    public ngOnInit(): void {
        this.carouselOption = {
            grid: { xs: 2, sm: 3, md: 3, lg: 4, all: 0 },
            slide: 4,
            speed: 400,
            animation: 'lazy',
            point: {
                visible: false
            },
            load: 2,
            touch: true,
            easing: 'ease'
        };

        switch (this.typeSection) {
            case BookSection.New:
                this.getLListNewBook();
                break;
            default:
                this.getTopBookSection();
                break;
        }
    }

    private getLListNewBook(): void {
        this.bookService.getListNewBook().subscribe((res) => {
            this.books = res;
            JQueryHelper.hideLoading();
        });
    }

    private getTopBookSection(): void {
        this.bookService.getTopBookInSection(this.typeSection).subscribe((res) => {
            this.books = res;
            JQueryHelper.hideLoading();
        });
    }
}
