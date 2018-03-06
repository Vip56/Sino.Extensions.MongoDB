using TestApp.Models;
using MongoDB.Bson;
using Sino.Extensions.MongoDB.Repositories;
using Sino.Extensions.MongoDB;

namespace TestApp.Repositories
{
    public class FamilyRepository : DocumentDBRepositoryBase<Family, ObjectId>, IFamilyRepository
    {
        public FamilyRepository(IDocumentDBConfiguration configuration) : base(configuration) { }

        protected override string CollectionName
        {
            get
            {
                return "Family";
            }
        }
    }
}
