using System;
using System.Net;
using System.Threading.Tasks;
using HRM.CrossCutting.Command;
using Library.Data.Entities.Library;
using Library.Data.Services;
using Library.Library.Category.ViewModels;

namespace Library.Library.Category.Commands.SaveCategory
{
    public class SaveCategoryCommand : ISaveCategoryCommand
    {
        private readonly IRepository<Categories> _categoryRepository;

        public SaveCategoryCommand(IRepository<Categories> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CommandResult> ExecuteAsync(CategoryViewModel model)
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
                    var category = await _categoryRepository.GetByIdAsync(model.Id);
                    if (category == null)
                    {
                        return CommandResult.Failed(new CommandResultError()
                        {
                            Code = (int)HttpStatusCode.NotFound,
                            Description = "Data not found"
                        });
                    }

                    category.CategoryName = model.CategoryName;
                    category.Type = model.Type;
                    category.Enabled = model.Enabled;

                    await _categoryRepository.UpdateAsync(category);
                }
                else
                {
                    Categories entity = new Categories();
                    entity.CategoryName = model.CategoryName;
                    entity.Type = model.Type;
                    entity.Enabled = model.Enabled;

                    await _categoryRepository.InsertAsync(entity);
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

        private bool ValidationData(CategoryViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.CategoryName))
            {
                return false;
            }
            return true;
        }
    }
}
