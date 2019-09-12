﻿using ShopFilip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopFilip.Helpers
{
    public static class ViewModelFactory
    {
        public static ProductViewModel MapProductToViewModel(Product model)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price,
                Photo = model.Photo,
            };
            return viewModel;
        }


        public static ProductViewModel MapProductsToViewModel(IEnumerable<Product> models)
        {
            ProductViewModel viewModel = new ProductViewModel()
            {
                Id = models.First().Id,
                Name = models.First().Name,
                Price = models.First().Price,
                Photo = models.First().Photo,
            };
            return viewModel;
        }

        public static Product MapToModel(ProductViewModel viewModel)
        {
            Product model = new Product()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                Photo = viewModel.Photo,
            };
            return model;
        }
    }
}