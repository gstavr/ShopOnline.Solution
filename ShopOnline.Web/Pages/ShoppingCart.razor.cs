using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        public string ErrorMessage { get; set; }

        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.DeleteItem(id);
                
                RemoveCartItem(id);
                CartChanged();

            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message; ;
            }
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            this.TotalPrice = this.ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            this.TotalQuantity = this.ShoppingCartItems.Sum(x => x.Qty);
        }

        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }
        private async void RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(cartItemDto);

            await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);

        }

        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            this.ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);

        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if(qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returndUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

                    await UpdateItemTotalPriceAsync(returndUpdateItemDto);

                    CartChanged();
                    await JSRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);
                }

                var item = this.ShoppingCartItems.FirstOrDefault(x => x.Id == id);
                if(item is null)
                {
                    item.Qty = 1;
                    item.TotalPrice = item.Price;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected async void UpdateQty_Input(int id) {
            await JSRuntime.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, true);
        }

        private async Task UpdateItemTotalPriceAsync(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if(item is not null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }

            await this.ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
        }
    }

}
