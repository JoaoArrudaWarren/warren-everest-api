﻿using Bogus;
using DomainModels.Models;
using System.Collections.Generic;

namespace ApiFirst.Tests.Fixtures.DomainServices
{
    public class PortfolioProductFixture
    {
        public static List<PortfolioProduct> GeneratePortfolioProductFixture(int quantity)
        {
            return new Faker<PortfolioProduct>("en_US")
                .CustomInstantiator(p => new PortfolioProduct(
                    portfolioId: 1,
                    productId: 1))
                .Generate(quantity);
        }
        public static PortfolioProduct GeneratePortfolioProductFixture()
        {
            return new Faker<PortfolioProduct>("en_US")
                .CustomInstantiator(p => new PortfolioProduct(
                    portfolioId: 1,
                    productId: 1))
                .Generate();
        }
    }
}
