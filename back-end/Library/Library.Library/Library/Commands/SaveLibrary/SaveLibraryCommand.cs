using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Library.ViewModels;

namespace Library.Library.Library.Commands.SaveLibrary
{
    public class SaveLibraryCommand : ISaveLibraryCommand
    {
        private readonly IRepository<Libraries> _libraryRepository;

        public SaveLibraryCommand(IRepository<Libraries> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<CommandResult> ExecuteAsync(LibraryViewModel model)
        {
            try
            {
                if (!ValidationData(model))
                {
                    return CommandResult.Failed(new CommandResultError()
                    {
                        Code = (int)HttpStatusCode.NotAcceptable,
                        Description = "Data not correct"
                    });
                }

                if (model.Id != 0)
                {
                    var publisher = await _libraryRepository.GetByIdAsync(model.Id);
                    if (publisher == null)
                    {
                        return CommandResult.Failed(new CommandResultError()
                        {
                            Code = (int)HttpStatusCode.NotFound,
                            Description = "Data not found"
                        });
                    }

                    publisher.Name = model.Name;
                    publisher.Address = model.Address;
                    publisher.Phone = model.Phone;
                    publisher.Email = model.Email;
                    publisher.Enabled = model.Enabled;

                    await _libraryRepository.UpdateAsync(publisher);
                }
                else
                {
                    Libraries entity = new Libraries();
                    entity.Name = model.Name;
                    entity.Address = model.Address;
                    entity.Phone = model.Phone;
                    entity.Email = model.Email;
                    entity.Enabled = model.Enabled;

                    await _libraryRepository.InsertAsync(entity);
                    model.Id = entity.Id;
                }

                return CommandResult.SuccessWithData(model.Id);
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

        private bool ValidationData(LibraryViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            return true;
        }
    }
}
