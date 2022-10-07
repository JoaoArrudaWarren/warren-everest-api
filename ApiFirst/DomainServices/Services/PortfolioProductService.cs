﻿using DomainModels.Models;
using DomainServices.Interfaces;
using EntityFrameworkCore.UnitOfWork.Interfaces;
using Infrastructure.Data.Context;
using System;
using System.Threading.Tasks;

namespace DomainServices.Services
{
    public class PortfolioProductService : IPortfolioProductService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWork _unitOfWork;

        public PortfolioProductService(IUnitOfWork<WarrenContext> unitOfWork, IRepositoryFactory<WarrenContext> repository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositoryFactory = repository ?? (IRepositoryFactory)_unitOfWork;
        }

        public async Task CreateRelationAsync(Portfolio portfolio, Product product)
        {
            var repository = _unitOfWork.Repository<PortfolioProduct>();

            var relation = new PortfolioProduct(portfolio.Id, product.Id);
            await repository.AddAsync(relation).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteRelationAsync(Portfolio portfolio, Product product)
        {
            var repository = _unitOfWork.Repository<PortfolioProduct>();

            var relationToRemove = await GetByRelationAsync(portfolio.Id, product.Id);

            repository.Remove(relationToRemove);
            _unitOfWork.SaveChanges();
        }

        public async Task<PortfolioProduct> GetByRelationAsync(long portfolioId, long productId)
        {
            var repository = _repositoryFactory.Repository<PortfolioProduct>();

            var query = repository.SingleResultQuery()
                        .AndFilter(portfolioproduct => portfolioproduct.ProductId == productId && portfolioproduct.PortfolioId == portfolioId);

            var relation = await repository.FirstOrDefaultAsync(query);

            if (relation is null)
            {
                throw new ArgumentNullException($"Nenhuma relação encontrada para ProductId: {productId} e PortfolioId: {portfolioId}");
            }
            return relation;
        }

        public bool RelationExists(long portfolioId, long productId)
        {
            var repository = _repositoryFactory.Repository<PortfolioProduct>();

            return repository.Any(portfolioproduct => portfolioproduct.PortfolioId == portfolioId && portfolioproduct.ProductId == productId);
        }
    }
}