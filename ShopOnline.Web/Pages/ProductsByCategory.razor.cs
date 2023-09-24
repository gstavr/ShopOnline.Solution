using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsByCategoryBase:ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }
        
        public string CategoryName { get; set; }    
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                // Products = await ProductService.GetItemsByCateogry(CategoryId);
                Products = await GetProductCollectionByCategoryId(CategoryId);
                

                if (Products is not null && Products.Any())
                {
                    var productDto = Products.FirstOrDefault(x=>x.CategoryId.Equals(CategoryId));
                    if(productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                }

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ManageProductsLocalStorageService.GetCollection();

            if(productCollection is not null)
            {
                return productCollection.Where(x => x.CategoryId.Equals(categoryId));
            }

            return await ProductService.GetItemsByCateogry(categoryId);
        }
    }
}
