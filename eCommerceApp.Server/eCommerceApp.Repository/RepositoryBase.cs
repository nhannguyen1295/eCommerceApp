using System;
using System.Linq;
using System.Linq.Expressions;
using eCommerceApp.Contract;
using eCommerceApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerceApp.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryDataContext repository;
        public RepositoryBase(RepositoryDataContext repository) => this.repository = repository;

        public IQueryable<T> FindAll(bool trackChanges)
        => !trackChanges ? repository.Set<T>().AsNoTracking() : repository.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        => !trackChanges ? repository.Set<T>().Where(expression).AsNoTracking() : repository.Set<T>().Where(expression);
        public void Create(T entity) => repository.Set<T>().Add(entity);
        public void Update(T entity) => repository.Set<T>().Update(entity);
        public void Delete(T entity) => repository.Set<T>().Remove(entity);
    }
}