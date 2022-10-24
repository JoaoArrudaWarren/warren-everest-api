﻿using AppModels.EnumModels;
using System;

namespace AppModels.AppModels.Products
{
    public class CreateProduct
    {
        public string Symbol { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime IssuanceAt { get; set; }
        public DateTime ExpirationAt { get; set; }
        public ProductType Type { get; set; }
    }
}