import {
    Component, EventEmitter, Input, OnInit,
    Output, SimpleChanges, ViewChild
} from '@angular/core';
import Popper from 'popper.js';
import { OnChanges } from '@angular/core';
import { JQueryHelper } from '../../../shareds/helpers/jquery.helper';

@Component({
    selector: 'dropdown',
    templateUrl: './dropdown.component.html'
})

export class DropDownComponent implements OnInit, OnChanges {
    // Input
    @Input('listData') public listData: DropDownData[] = [];
    @Input('iconClass') public iconClass: string = '';
    @Input('addClass') public addClass: string = '';
    @Input('disabled') public disabled: boolean = false;
    @Input('selectedItemId') public selectedItemId: number = -1;
    @Input('allowNoSelect') public allowNoSelect: boolean = false;
    @Input('showSeletedItem') public showSeletedItem: boolean = true;
    @Input('placeholder') public placeholder: string = '';

    // Output
    @Output('selectedItemEmit') public selectedItemEmit: EventEmitter<DropDownData> = new EventEmitter();

    // ViewChild
    @ViewChild('dropdown') private dropdownElement: any;
    @ViewChild('dropdownMenu') private dropdownMenuElement: any;
    @ViewChild('dropdownToggle') private dropdownToggleElement: any;
    @ViewChild('iconToggle') private iconToggleElement: any;

    // letiable
    private selectedItem: DropDownData;
    private currentSelectedIndex: number = -1;
    private popper: Popper;

    // Selector
    private dropdownMenuClass: string = '.dropdown-menu';
    private dropdownToggleClass: string = '.dropdown-toggle';

    private readonly EmptyDataId: number = -2;

    constructor() {
        this.selectedItemEmit = new EventEmitter();
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (changes['selectedItemId'] && changes['selectedItemId'].currentValue) {
            this.defaultSelectItem();
        }

        if (changes['listData'] && changes['listData'].currentValue) {
            this.defaultSelectItem();
        }
    }

    public ngOnInit(): void {
        if (this.listData) {

            if (this.allowNoSelect) {
                this.listData.unshift(new DropDownData(null, ' '));
            }

            this.defaultSelectItem();

            if (this.addClass !== '') {
                $(this.dropdownToggleElement.nativeElement).addClass(this.addClass);
            }
            if (this.iconClass !== '') {
                $(this.iconToggleElement.nativeElement).addClass(this.iconClass);
            }
        }

        JQueryHelper.onClickOutside('.dropdown', () => {
            if (this.dropdownMenuElement) {
                $(this.dropdownMenuElement.nativeElement).removeClass('show');
            }
        });
    }

    public selectItem(key: any, value: any, enabled: boolean, event: any): void {
        if (enabled) {
            const item: DropDownData = new DropDownData(key, value);
            this.selectedItem = item;
            this.selectedItemEmit.emit(item);
            $(this.dropdownMenuElement.nativeElement).removeClass('show');
            event.preventDefault();
        }
    }

    private toggleDropdown(): void {
        if ($(this.dropdownMenuElement.nativeElement).hasClass('show')) {
            $(this.dropdownMenuElement.nativeElement).removeClass('show');
            this.popper.destroy();
        } else {
            $(this.dropdownMenuClass).removeClass('show');
            if (!this.disabled) {
                this.currentSelectedIndex = -1;
                setTimeout(() => {
                    $(this.dropdownMenuElement.nativeElement).addClass('show');
                    this.popper =
                        new Popper(this.dropdownElement.nativeElement,
                            this.dropdownMenuElement.nativeElement, {
                                placement: 'bottom',
                                modifiers: {
                                    computeStyle: {
                                        gpuAcceleration: false
                                    }
                                }
                            });
                }, 0);
            }
        }
    }

    private defaultSelectItem(): void {
        if (!this.listData) { return; }
        this.selectedItem = this.selectedItemId !== -1 && this.selectedItemId !== 0 ?
            this.listData.find((x) => x.key === this.selectedItemId) :
            new DropDownData(-1, this.placeholder !== '' ? this.placeholder : 'Chọn...');
        if (this.selectedItemId === null) {
            this.selectedItem = new DropDownData(null, ' ');
        }
        if (this.selectedItemId === this.EmptyDataId) {
            this.selectedItem = new DropDownData(null, '');
        }
    }
}

// tslint:disable-next-line:max-classes-per-file
export class DropDownData {
    public key: any;
    public value: any;
    public seperateLine?: boolean;
    public enabled?: boolean = true;

    constructor(key: any = 0, value: any, seperateLine: boolean = false, enabled: boolean = true) {
        this.key = key;
        this.value = value;
        this.seperateLine = seperateLine;
        this.enabled = enabled;
    }
}
