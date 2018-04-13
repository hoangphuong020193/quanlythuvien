import { Routes, CanActivate } from '@angular/router';

// GUARD
import { HomeGuard } from '../guards/home.guard';
import { BookDetailComponent } from '../components/choose-book/book-detail/book-detail.component';
import { BookDetailGuard } from '../guards/book-detail.guard';
import { CheckOutGuard } from '../guards/check-out.guard';
import { BookInCartCheckGuard } from '../guards/book-in-cart-check.guard';
import { AdminGuard } from '../guards/admin.guard';
import { MyBookGuard } from '../guards/my-book.guard';

// COMPONENT
import { HomeComponent } from '../components/home';
import { ChooseBookComponent } from '../components/choose-book/choose-book.component';
import { CheckOutComponent } from '../components/check-out/check-out.component';
import {
  BookInCartCheckComponent
} from '../components/check-out/book-in-cart-check/book-in-cart-check.component';
import { MyBookComponent } from '../components/my-book/my-book.component';
import { SearchResultComponent } from '../components/search-result/search-result.component';
import { BookViewComponent } from '../components/book-view/book-view.component';
import { NotificationComponent } from '../components/notification/notification.component';
import { AdminComponent } from '../components/admin/admin.component';

export const ROUTES: Routes = [
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  {
    path: '',
    component: HomeComponent,
    canActivate: [HomeGuard],
    children: [
      {
        path: '',
        redirectTo: 'index',
        pathMatch: 'full'
      },
      {
        path: 'index',
        component: ChooseBookComponent
      },
      {
        path: 'book-detail/:bookCode',
        component: BookDetailComponent,
        canActivate: [BookDetailGuard]
      },
      {
        path: 'checkout/cart',
        component: CheckOutComponent,
        canActivate: [CheckOutGuard]
      },
      {
        path: 'checkout/checkout',
        component: BookInCartCheckComponent,
        canActivate: [BookInCartCheckGuard]
      },
      {
        path: 'my-book',
        component: MyBookComponent,
        canActivate: [MyBookGuard]
      },
      {
        path: 'search',
        component: SearchResultComponent
      },
      {
        path: 'book-view',
        component: BookViewComponent
      },
      {
        path: 'notification',
        component: NotificationComponent
      },
      {
        path: 'admin',
        component: AdminComponent,
        canActivate: [AdminGuard]
      },
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];
