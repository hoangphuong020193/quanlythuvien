using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Publisher.ViewModels;

namespace Library.Library.Publisher.Commands.SavePublisher
{
    public class SavePublisherCommand : ISavePublisherCommand
    {
        private readonly IRepository<Publishers> _publisherRepository;

        public SavePublisherCommand(IRepository<Publishers> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public async Task<CommandResult> ExecuteAsync(PublisherViewModel model)
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
                    var publisher = await _publisherRepository.GetByIdAsync(model.Id);
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

                    await _publisherRepository.UpdateAsync(publisher);
                }
                else
                {
                    Publishers entity = new Publishers();
                    entity.Name = model.Name;
                    entity.Address = model.Address;
                    entity.Phone = model.Phone;
                    entity.Email = model.Email;
                    entity.Enabled = model.Enabled;

                    await _publisherRepository.InsertAsync(entity);
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

        private bool ValidationData(PublisherViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            return true;
        }
    }
}
