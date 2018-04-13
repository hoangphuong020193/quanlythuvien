using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;

namespace Library.Library.Book.Commands.SaveBookImage
{
    public class SaveBookImageCommand : ISaveBookImageCommand
    {
        private IRepository<Books> _bookRepository;

        public SaveBookImageCommand(IRepository<Books> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<CommandResult> ExecuteAsync(byte[] img, int bookId)
        {
            try
            {
                var entity = await _bookRepository.GetByIdAsync(bookId);
                entity.BookImage = img;
                if (await _bookRepository.UpdateAsync(entity))
                {
                    return CommandResult.Success;
                }
                return CommandResult.Failed();
            }
            catch (Exception ex)
            {
                return CommandResult.Failed(new CommandResultError()
                {
                    Code = (int)HttpStatusCode.InternalServerError,
                    Description = ex.Message
                });
            }
        }
    }
}
