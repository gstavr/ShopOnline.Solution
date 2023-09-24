using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase:ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService  ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<CartItemDto> ShoppingCartItems { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        public ProductDto  Product { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
                
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            
        }

        protected async Task AddToCart_Click(CartItemDto cartItemDto)
        {
            try
            {
                var cartitemDto = await this.ShoppingCartService.AddItem(cartItemDto);

                if(cartitemDto != null)
                {
                    ShoppingCartItems.Add(cartitemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }
                NavigationManager.NavigateTo("/ShoppingCart");


            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<ProductDto> GetProductById(int id)
        {
            var productDtos = await ManageProductsLocalStorageService.GetCollection();
            if(productDtos is not null)
            {
                return productDtos.SingleOrDefault(x=>x.Id.Equals(id));
            }

            return null;
        }
    }
}
