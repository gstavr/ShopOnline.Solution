﻿using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from ProductCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = ProductCategory.Id,
                        Name = ProductCategory.Name,
                        IconCSS = ProductCategory.IconCSS,
                    }).ToList();
        }
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products) {
            return (from product in products select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }


        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name,
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.Id equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        CartId = cartItem.CartId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = cartItem.Qty,
                        TotalPrice = cartItem.Qty * product.Price,
                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                CartId = cartItem.CartId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = cartItem.Qty,
                TotalPrice = cartItem.Qty * product.Price,
            };

        }
    }
}
