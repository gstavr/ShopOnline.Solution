﻿using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService:IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductCategoryDto>> GetProductCategories()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product/GetProductCategories");

                if (response.IsSuccessStatusCode) {

                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductCategoryDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductCategoryDto>>();

                }

                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code -  {response.StatusCode} Message - {message}");
            }
            catch (Exception ex )
            {

                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }

                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code -  {response.StatusCode} Message - {message}");
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Product");
                if(response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                }


                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code -  {response.StatusCode} Message - {message}");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetItemsByCateogry(int categoryId)
        {

            try
            {
                var response = await this.httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");

                if (response.IsSuccessStatusCode)
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();

                }

                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http Status Code -  {response.StatusCode} Message - {message}");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
