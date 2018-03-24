using AutoMapper;
using DryIoc;
using Library.Data.Services;
using Library.Library.Admin.Queries.GetListUserNotReturnBook;
using Library.Library.BookRequest.Queries.GetRequestInfoByCode;
using Library.Library.Book.Commands.CancelBook;
using Library.Library.Book.Commands.ReturnBook;
using Library.Library.Book.Commands.SaveBook;
using Library.Library.Book.Commands.SaveBookImage;
using Library.Library.Book.Commands.TakenBook;
using Library.Library.Book.Queries.BookBorrowAmount;
using Library.Library.Book.Queries.CheckBookExistsCode;
using Library.Library.Book.Queries.GetBookBorrow;
using Library.Library.Book.Queries.GetBookByBookCode;
using Library.Library.Book.Queries.GetBookDetail;
using Library.Library.Book.Queries.GetBookPhoto;
using Library.Library.Book.Queries.GetBookSection;
using Library.Library.Book.Queries.GetBookViewByCategory;
using Library.Library.Book.Queries.GetListBook;
using Library.Library.Book.Queries.GetListBookByRequestCode;
using Library.Library.Book.Queries.GetListNewBook;
using Library.Library.Book.Queries.SearchBook;
using Library.Library.Cart.Commands.AddBookToCart;
using Library.Library.Cart.Commands.BorrowBook;
using Library.Library.Cart.Commands.DeleteToCart;
using Library.Library.Cart.Commands.UpdateStatusBookInCart;
using Library.Library.Cart.Queries.GetBookInCartDetail;
using Library.Library.Cart.Queries.GetBookInCartForBorrow;
using Library.Library.Cart.Queries.GetListBookInCart;
using Library.Library.Cart.Queries.GetSlotAvailable;
using Library.Library.Category.Commands.SaveCategory;
using Library.Library.Category.Queries.GetCategory;
using Library.Library.Favorite.Commands.UpdateBookFavorite;
using Library.Library.Permission.Queries.GetPermissionByUserId;
using Library.Library.Publisher.Commands.SavePublisher;
using Library.Library.Publisher.Queries.GetListPublisher;
using Library.Library.Supplier.Commands.SaveSupplier;
using Library.Library.Supplier.Queries.GetListSupplier;
using Library.Library.UserAccount.Queries.GetUserInfo;
using Library.Library.UserAccount.Queries.GetUserInfoLogin;
using Library.Library.User.Queries.GetUserNotification;
using System;
using System.Net.Http;
using Library.Library.Library.Queries.GetListLibrary;
using Library.Library.Library.Commands.SaveLibrary;
using Library.Library.Admin.Queries.GetReadStatistic;
using Library.Library.Admin.Queries.GetBorrowStatus;
using Library.Library.Admin.Queries.GetCategoryReport;

namespace Library.ApiFramework.IoCRegistrar
{
    public class CompositionRoot
    {
        public CompositionRoot(IRegistrator registrator)
        {
            // general
            registrator.RegisterDelegate(resolver => Mapper.Instance, Reuse.Singleton);
            registrator.Register<Lazy<HttpClient>>(Reuse.InWebRequest);
            registrator.Register<IDbContext, ApplicationDbContext>(Reuse.InWebRequest);
            registrator.Register(typeof(IRepository<>), typeof(EfRepository<>), Reuse.InWebRequest);

            // User
            registrator.Register<IGetUserInfoLoginQuery, GetUserInfoLoginQuery>(Reuse.InWebRequest);
            registrator.Register<IGetUserInfoQuery, GetUserInfoQuery>(Reuse.InWebRequest);

            // Permission
            registrator.Register<IGetPermissionByUserIdQuery, GetPermissionByUserIdQuery>(Reuse.InWebRequest);

            // Category
            registrator.Register<IGetCategoryQuery, GetCategoryQuery>(Reuse.InWebRequest);
            registrator.Register<ISaveCategoryCommand, SaveCategoryCommand>(Reuse.InWebRequest);

            // Book
            registrator.Register<IGetBookPhotoQuery, GetBookPhotoQuery>(Reuse.InWebRequest);
            registrator.Register<IGetListBookNewQuery, GetListBookNewQuery>(Reuse.InWebRequest);
            registrator.Register<IGetBookDetailQuery, GetBookDetailQuery>(Reuse.InWebRequest);
            registrator.Register<IGetBookSectionQuery, GetBookSectionQuery>(Reuse.InWebRequest);
            registrator.Register<IGetListBookQuery, GetListBookQuery>(Reuse.InWebRequest);
            registrator.Register<ICheckBookCodeExistsQuery, CheckBookCodeExistsQuery>(Reuse.InWebRequest);
            registrator.Register<ISaveBookCommand, SaveBookCommand>(Reuse.InWebRequest);
            registrator.Register<IGetBookByBookCodeQuery, GetBookByBookCodeQuery>(Reuse.InWebRequest);
            registrator.Register<ISaveBookImageCommand, SaveBookImageCommand>(Reuse.InWebRequest);
            registrator.Register<IBookBorrowAmountQuery, BookBorrowAmountQuery>(Reuse.InWebRequest);

            // Cart
            registrator.Register<IGetBookInCartForBorrowQuery, GetBookInCartForBorrowQuery>(Reuse.InWebRequest);
            registrator.Register<IGetListBookInCartQuery, GetListBookInCartQuery>(Reuse.InWebRequest);
            registrator.Register<IGetBookInCartDetailQuery, GetBookInCartDetailQuery>(Reuse.InWebRequest);
            registrator.Register<IGetSlotAvailableQuery, GetSlotAvailableQuery>(Reuse.InWebRequest);
            registrator.Register<IAddBookToCartCommand, AddBookToCartCommand>(Reuse.InWebRequest);
            registrator.Register<IDeleteToCartCommand, DeleteToCartCommand>(Reuse.InWebRequest);
            registrator.Register<IUpdateStatusBookInCartCommand, UpdateStatusBookInCartCommand>(Reuse.InWebRequest);
            registrator.Register<IBorrowBookCommand, BorrowBookCommand>(Reuse.InWebRequest);
            registrator.Register<IGetBookViewByCategoryQuery, GetBookViewByCategoryQuery>(Reuse.InWebRequest);

            // Favorite
            registrator.Register<IUpdateBookFavoriteCommand, UpdateBookFavoriteCommand>(Reuse.InWebRequest);

            // User book
            registrator.Register<IGetBookBorrowQuery, GetBookBorrowQuery>(Reuse.InWebRequest);
            registrator.Register<IGetListBookByRequestCodeQuery, GetListBookByRequestCodeQuery>(Reuse.InWebRequest);
            registrator.Register<ITakenBookCommand, TakenBookCommand>(Reuse.InWebRequest);
            registrator.Register<IReturnBookCommand, ReturnBookCommand>(Reuse.InWebRequest);
            registrator.Register<ICancelBookCommand, CancelBookCommand>(Reuse.InWebRequest);

            // Search
            registrator.Register<ISearchBookQuery, SearchBookQuery>(Reuse.InWebRequest);

            // User notification
            registrator.Register<IGetUserNotificationQuery, GetUserNotificationQuery>(Reuse.InWebRequest);

            // Request
            registrator.Register<IGetRequestInfoByCodeQuery, GetRequestInfoByCodeQuery>(Reuse.InWebRequest);

            // Publisher
            registrator.Register<IGetListPublisherQuery, GetListPublisherQuery>(Reuse.InWebRequest);
            registrator.Register<ISavePublisherCommand, SavePublisherCommand>(Reuse.InWebRequest);

            // Supplier
            registrator.Register<IGetListSupplier, GetListSupplier>(Reuse.InWebRequest);
            registrator.Register<ISaveSupplierCommand, SaveSupplierCommand>(Reuse.InWebRequest);

            // Admin
            registrator.Register<IGetListUserNotReturnBookQuery, GetListUserNotReturnBookQuery>(Reuse.InWebRequest);
            registrator.Register<IReadStatisticQuery, ReadStatisticQuery>(Reuse.InWebRequest);
            registrator.Register<IGetBorrowStatusQuery, GetBorrowStatusQuery>(Reuse.InWebRequest);
            registrator.Register<IGetCategoryReportQuery, GetCategoryReportQuery>(Reuse.InWebRequest);

            // Library
            registrator.Register<IGetListLibraryQuery, GetListLibraryQuery>(Reuse.InWebRequest);
            registrator.Register<ISaveLibraryCommand, SaveLibraryCommand>(Reuse.InWebRequest);
        }
    }
}
