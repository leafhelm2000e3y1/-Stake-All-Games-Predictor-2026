using CoreBusiness;

namespace UseCases
{
    public interface IGetCategoryByIdUseCase
    {
        Category Execute(int categoryId);
    }
}