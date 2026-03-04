using CoreBusiness;
using System.Collections.Generic;

namespace UseCases
{
    public interface IViewProductsByCategoryId
    {
        IEnumerable<Product> Execute(int categoryId);
    }
}