using MongoDB.Bson;
using Sino.Dependency;
using Sino.Domain.Repositories;
using System;
using TestApp.Models;

namespace TestApp.Repositories
{
    public interface IFamilyRepository : IRepository<Family, ObjectId> , ISingletonDependency
    {

    }
}
