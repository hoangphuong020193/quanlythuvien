using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Supplier.ViewModels;

namespace Library.Library.Supplier.Commands.SaveSupplier
{
    public class SaveSupplierCommand : ISaveSupplierCommand
    {
        private readonly IRepository<Suppliers> _supplierRepository;

        public SaveSupplierCommand(IRepository<Suppliers> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<CommandResult> ExecuteAsync(SupplierViewModel model)
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
                    var supplier = await _supplierRepository.GetByIdAsync(model.Id);
                    if (supplier == null)
                    {
                        return CommandResult.Failed(new CommandResultError()
                        {
                            Code = (int)HttpStatusCode.NotFound,
                            Description = "Data not found"
                        });
                    }

                    supplier.Name = model.Name;
                    supplier.Address = model.Address;
                    supplier.Phone = model.Phone;
                    supplier.Email = model.Email;
                    supplier.Enabled = model.Enabled;

                    await _supplierRepository.UpdateAsync(supplier);
                }
                else
                {
                    Suppliers entity = new Suppliers();
                    entity.Name = model.Name;
                    entity.Address = model.Address;
                    entity.Phone = model.Phone;
                    entity.Email = model.Email;
                    entity.Enabled = model.Enabled;

                    await _supplierRepository.InsertAsync(entity);
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

        private bool ValidationData(SupplierViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Name))
            {
                return false;
            }
            return true;
        }
    }
}
