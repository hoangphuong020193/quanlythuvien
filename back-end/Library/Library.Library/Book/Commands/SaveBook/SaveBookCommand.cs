using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Book.Queries.GetBookByBookCode;
using Library.Library.Book.ViewModels;

namespace Library.Library.Book.Commands.SaveBook
{
    public class SaveBookCommand : ISaveBookCommand
    {
        private readonly IRepository<Books> _bookRepository;
        private readonly IGetBookByBookCodeQuery _getBookByBookCodeQuery;

        public SaveBookCommand(
            IRepository<Books> bookRepository,
            IGetBookByBookCodeQuery getBookByBookCodeQuery)
        {
            _bookRepository = bookRepository;
            _getBookByBookCodeQuery = getBookByBookCodeQuery;
        }

        public async Task<CommandResult> ExecuteAsync(BookViewModel model)
        {
            try
            {
                if (model.BookId == 0)
                {
                    Books entity = new Books();
                    entity.Id = 0;
                    entity.CategoryId = model.CategoryId;
                    entity.BookCode = model.BookCode;
                    entity.BookName = model.BookName;
                    entity.Tag = model.Tag;
                    entity.Description = model.Description;
                    entity.DateImport = DateTime.Now;
                    entity.Amount = model.Amount;
                    entity.AmountAvailable = model.Amount;
                    entity.Author = model.Author;
                    entity.PublisherId = model.PublisherId;
                    entity.SupplierId = model.SupplierId;
                    entity.Size = model.Size;
                    entity.Format = model.Format;
                    entity.PublicationDate = model.PublicationDate;
                    entity.Pages = model.Pages;
                    entity.MaximumDateBorrow = model.MaximumDateBorrow;
                    entity.LibraryId = model.LibraryId;
                    entity.Enabled = model.Enabled;

                    await _bookRepository.InsertAsync(entity);
                }
                else
                {
                    Books entity = await _bookRepository.GetByIdAsync(model.BookId);
                    entity.CategoryId = model.CategoryId;
                    entity.BookCode = model.BookCode;
                    entity.BookName = model.BookName;
                    entity.Tag = model.Tag;
                    entity.Description = model.Description;
                    entity.Amount = model.Amount;
                    entity.Author = model.Author;
                    entity.PublisherId = model.PublisherId;
                    entity.SupplierId = model.SupplierId;
                    entity.Size = model.Size;
                    entity.Format = model.Format;
                    entity.PublicationDate = model.PublicationDate;
                    entity.Pages = model.Pages;
                    entity.MaximumDateBorrow = model.MaximumDateBorrow;
                    entity.LibraryId = model.LibraryId;
                    entity.Enabled = model.Enabled;

                    await _bookRepository.UpdateAsync(entity);
                }

                model = await _getBookByBookCodeQuery.ExecuteAsync(model.BookCode);
                return CommandResult.SuccessWithData(model);
            }
            catch (Exception ex)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = ex.Message + (ex.InnerException != null ? "/r/rInner: " + ex.InnerException.Message : "")
                });
            }
        }
    }
}
