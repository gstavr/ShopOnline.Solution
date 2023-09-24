using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.API.Entities;
using ShopOnline.API.Repositories.Contracts;

namespace ShopOnline.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await this.shopOnlineDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await this.shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(x=>x.Id.Equals(id));

            return category;
        }

        public async Task<Product> GetItem(int id)
        {
            var product = await shopOnlineDbContext.Products.Include(p => p.ProductCategory).SingleOrDefaultAsync(p => p.Id.Equals(id));
            return product;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await shopOnlineDbContext.Products.Include(p => p.ProductCategory).ToListAsync();
            return products;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            //var products = await (from product in shopOnlineDbContext.Products
            //                      where product.CategoryId == id
            //                      select product).ToListAsync();

            var products = await shopOnlineDbContext.Products.Include(p => p.ProductCategory).Where(p => p.CategoryId.Equals(id)).ToListAsync();
            return products;
        }
    }
}
