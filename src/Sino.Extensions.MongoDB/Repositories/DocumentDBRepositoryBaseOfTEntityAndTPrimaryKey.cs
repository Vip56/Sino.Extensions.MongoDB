using MongoDB.Driver;
using Sino.Domain.Entities;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace Sino.Extensions.MongoDB.Repositories
{
    public abstract class DocumentDBRepositoryBase<TEntity, TPrimaryKey> : AbpRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey> , new()
    {
        protected IDocumentDBConfiguration Configuration { get; set; }

        protected MongoClient Client { get; set; }
        protected IMongoDatabase Database { get; set; }
        protected IMongoCollection<TEntity> Collection { get; set; }

        protected abstract string CollectionName { get; }

        protected string DatabaseName { get; set; }

        public DocumentDBRepositoryBase(IDocumentDBConfiguration configuration)
        {
            Configuration = configuration;
            DatabaseName = configuration.DataBase;
            Client = new MongoClient($"mongodb://{Configuration.UserName}:{Configuration.Password}@{Configuration.Host}?ssl=true");
            Database = Client.GetDatabase(DatabaseName);
            Collection = Database.GetCollection<TEntity>(CollectionName);
        }

        public override Task<int> CountAsync(IQueryObject<TEntity> query)
        {
            throw new NotImplementedException();
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            TEntity result = null;
            IQueryable<TEntity> query = Collection.AsQueryable();
            query = query.Where(x => x.Id.Equals(id));
            result = query.FirstOrDefault();
            return Task.FromResult(result);
        }

        public override Task<IEnumerable<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(Collection.AsQueryable().ToList().AsEnumerable());
        }

        public override Task<TEntity> GetAsync(TPrimaryKey id)
        {
            TEntity result = null;
            IQueryable<TEntity> query = Collection.AsQueryable();
            query = query.Where(x => x.Id.Equals(id));
            result = query.First();
            return Task.FromResult(result);
        }

        public override Task<Tuple<int, IList<TEntity>>> GetListAsync(IQueryObject<TEntity> query)
        {
            IQueryable<TEntity> selfQuery = Collection.AsQueryable();

            var b = query.QueryExpression.WhereAnd();
            selfQuery = selfQuery.Where(b);

            if (query.OrderField != null)
            {
                if (query.OrderSort == SortOrder.ASC)
                {
                    selfQuery = selfQuery.OrderBy(query.OrderField);
                }
                else
                {
                    selfQuery = selfQuery.OrderByDescending(query.OrderField);
                }
            }

            List<TEntity> items = new List<TEntity>();
            if(query.Count == -1)
            {
                items = selfQuery.ToList();
                return Task.FromResult(new Tuple<int, IList<TEntity>>(items.Count, items));
            }
            else
            {
                items = selfQuery.Skip(query.Skip).Take(query.Count + 1).ToList();
                int total = query.Count;
                if (items.Count > query.Count)
                    total = query.Count * 2 + query.Skip;
                return Task.FromResult(new Tuple<int, IList<TEntity>>(total, items.Take(query.Count).ToList()));
            }
        }

        public override Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            await Collection.ReplaceOneAsync(filter, entity);
            return entity;
        }
    }
}
