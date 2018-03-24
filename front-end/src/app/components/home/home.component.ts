import { Component, OnInit } from '@angular/core';
import { DialogService } from 'angularx-bootstrap-modal';
import { ComponentId } from '../../shareds/enums/component.enum';
import { User } from '../../models/user.model';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../store/reducers';
import * as userAction from '../../store/actions/user.action';
import * as bookAction from '../../store/actions/book.action';
import { JQueryHelper } from '../../shareds/helpers/jquery.helper';
import { RouterService } from '../../services/router.service';
import { LoginPopupComponent } from '../login/login.component';
import { CartService } from '../../services/cart.service';
import { BookInCart, Permission } from '../../models/index';
import { Observable } from 'rxjs';
import { BookStatus } from '../../shareds/enums/book-status.enum';
import { KeyCode } from '../../shareds/enums/keycode.enum';
import { NotificationService } from '../../services/notification.service';
import { Notifications } from '../../models/notification.model';
import { PermissionId } from '../../shareds/enums/permission.enum';

@Component({
  selector: 'home',
  templateUrl: 'home.component.html'
})

export class HomeComponent implements OnInit {
  private ComponentId: any = ComponentId;
  private user: User = null;

  private toggleMenuNotify: boolean = false;
  private toggleMenuLogin: boolean = false;

  private numberBookInCart: number = 0;
  private numberNewNotification: number = 0;
  private listNotification: Notifications[] = [];
  private isAdmin: boolean = false;

  constructor(
    private store: Store<fromRoot.State>,
    private dialogService: DialogService,
    private routerService: RouterService,
    private cartService: CartService,
    private notificationService: NotificationService) { }

  public ngOnInit(): void {
    this.store.select(fromRoot.getCurrentUser).subscribe((user: User) => {
      this.user = user;
    });

    JQueryHelper.onClickOutside('.user-login', () => {
      this.toggleMenuLogin = false;
    });
    JQueryHelper.onClickOutside('.user-notify', () => {
      this.toggleMenuNotify = false;
    });

    this.cartService.getBookInCart().subscribe();
    this.store.select(fromRoot.getBookInCart).subscribe((res) => {
      if (res) {
        this.numberBookInCart = res.filter((x) => x.status === BookStatus.InOrder).length;
      }
    });

    this.store.select(fromRoot.getPermission).subscribe((res) => {
      this.isAdmin = res.some((x) => x.groupPermissionId === PermissionId.ADMIN);
    });

    this.notificationService.getNotification().subscribe();
    this.store.select(fromRoot.getNotification).subscribe((res) => {
      if (res) {
        this.numberNewNotification = res.filter((x) => x.isNew).length;
        this.listNotification = res.slice(0, 5);
      }
    });
  }

  private viewMenuLogin(): void {
    if (!this.user) {
      this.dialogService.addDialog(LoginPopupComponent, {}).subscribe();
    } else {
      this.toggleMenuLogin = !this.toggleMenuLogin;
    }
  }

  private navigateToHome(): void {
    this.routerService.home();
  }

  private logout(event: any): void {
    this.store.dispatch(new userAction.Logout(null));
    this.store.dispatch(new bookAction.ClearBookInCart(null));
    this.user = null;
    event.stopPropagation();
    this.toggleMenuLogin = false;
    this.routerService.home();
  }

  private viewMenuNotify(): void {
    if (!this.user) {
      this.dialogService.addDialog(LoginPopupComponent, {}).subscribe();
    } else {
      this.toggleMenuNotify = !this.toggleMenuNotify;
      this.numberNewNotification = 0;
    }
  }

  private viewBookInCart(): void {
    if (!this.user) {
      this.dialogService.addDialog(LoginPopupComponent, {
        reloadPage: false
      }).subscribe((res) => {
        if (res) {
          this.routerService.checkOutCart();
        }
      });
    } else {
      this.routerService.checkOutCart();
    }
  }

  private viewMyBook(): void {
    if (!this.user) {
      this.dialogService.addDialog(LoginPopupComponent, {
        reloadPage: false
      }).subscribe((res) => {
        if (res) {
          this.routerService.myBook();
        }
      });
    } else {
      this.routerService.myBook();
    }
  }

  private onKeyPress(event: any): void {
    if (event.keyCode === KeyCode.Enter || event.keyChar === KeyCode.Enter) {
      this.searchBook();
    }
  }

  private searchBook(): void {
    const searchString: string = $('#search-box').val().toString();
    this.routerService.search(searchString);
  }
}
