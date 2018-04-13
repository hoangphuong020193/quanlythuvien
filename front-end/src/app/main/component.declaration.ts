/*Components*/
import * as fromCommon from '../components/common';
import * as fromHome from '../components/home';
import * as fromLogin from '../components/login';
import * as fromChooseBook from '../components/choose-book';
import * as fromCheckOut from '../components/check-out';
import * as fromMyBook from '../components/my-book';
import * as fromSearchResult from '../components/search-result';
import * as fromBookView from '../components/book-view';
import * as fromNotification from '../components/notification';
import * as fromAdmin from '../components/admin';

export const CommonComponents: any = [
    fromCommon.FavoriteStarComponent,
    fromCommon.ToastComponent,
    fromCommon.PopupConfirmComponent,
    fromCommon.CheckboxComponent,
    fromCommon.PaginationComponent,
    fromCommon.LoadingPageComponent,
    fromCommon.DropDownComponent,
    fromCommon.BookImageComponent,
    fromCommon.DateTimePickerComponent,
    fromCommon.DayComponent,
    fromCommon.MonthComponent,
    fromCommon.YearComponent,
];

export const Components: any = [
    fromHome.HomeComponent,
    fromLogin.LoginPopupComponent,
    fromChooseBook.ChooseBookComponent,
    fromChooseBook.BookSectionComponent,
    fromChooseBook.BookItemComponent,
    fromChooseBook.BookDetailComponent,
    fromCheckOut.CheckOutComponent,
    fromCheckOut.BookInCartComponent,
    fromCheckOut.BookInCartCheckComponent,
    fromCheckOut.PopupCheckOutSuccessComponent,
    fromMyBook.MyBookComponent,
    fromSearchResult.SearchResultComponent,
    fromBookView.BookViewComponent,
    fromNotification.NotificationComponent,
    fromAdmin.AdminComponent,
    fromAdmin.BorrowReturnBookComponent,
    fromAdmin.BookManagementComponent,
    fromAdmin.BookEditorPopupComponent,
    fromAdmin.SupplierManagementComponent,
    fromAdmin.SupplierEditorPopupComponent,
    fromAdmin.PublisherManagementComponent,
    fromAdmin.PublisherEditorPopupComponent,
    fromAdmin.CategoryManagementComponent,
    fromAdmin.CategoryEditorPopupComponent,
    fromAdmin.LibraryManagementComponent,
    fromAdmin.LibraryEditorPopupComponent,
    fromAdmin.ReportComponent,
    fromAdmin.UserDebtBookComponent,
    fromAdmin.TopBookComponent,
    fromAdmin.ReadStatisticsComponent,
    fromAdmin.BorrowStatusComponent,
    fromAdmin.CategoryReportComponent,
];

// Modal
export const LoginPopupComponent: any = fromLogin.LoginPopupComponent;
export const PopupConfirmComponent: any = fromCommon.PopupConfirmComponent;
export const PopupCheckOutSuccessComponent: any = fromCheckOut.PopupCheckOutSuccessComponent;
export const BookEditorPopupComponent: any = fromAdmin.BookEditorPopupComponent;
export const SupplierEditorPopupComponent: any = fromAdmin.SupplierEditorPopupComponent;
export const PublisherEditorPopupComponent: any = fromAdmin.PublisherEditorPopupComponent;
export const CategoryEditorPopupComponent: any = fromAdmin.CategoryEditorPopupComponent;
export const LibraryEditorPopupComponent: any = fromAdmin.LibraryEditorPopupComponent;
