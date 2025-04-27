using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams productSpec) : base(x =>
        (string.IsNullOrEmpty(productSpec.Search) || x.Name.ToLower().Contains(productSpec.Search)) &&
        (productSpec.Brands.Count == 0 || productSpec.Brands.Contains(x.Brand)) &&
        (productSpec.Types.Count == 0 || productSpec.Types.Contains(x.Type)))
    {
        ApplyPaging(productSpec.PageSize * (productSpec.PageIndex - 1), productSpec.PageSize);
        switch (productSpec.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
