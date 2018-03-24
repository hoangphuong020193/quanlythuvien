import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'favorite-star',
    templateUrl: './favorite-star.component.html'
})
export class FavoriteStarComponent {
    @Input('favorited') public favorited: boolean = false;
    @Output('callback') public callback: EventEmitter<boolean> = new EventEmitter();

    private onClick(): void {
        this.favorited = !this.favorited;
        this.callback.emit(this.favorited);
    }
}
