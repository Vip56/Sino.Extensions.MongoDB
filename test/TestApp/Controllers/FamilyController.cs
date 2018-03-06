using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestApp.Repositories;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    public class FamilyController : Controller
    {
        protected IFamilyRepository Repository { get; set; }

        public FamilyController(IFamilyRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            for (var i = 0; i < 100; i++)
            {
                Family andersenFamily = new Family
                {
                    LastName = "Andersen" + i,
                    Parents = new Parent[]
                    {
                    new Parent { FirstName = "Thomas" + i },
                    new Parent { FirstName = "Mary Kay" + i }
                    },
                    Children = new Child[]
                    {
                    new Child
                    {
                        FirstName = "Henriette Thaulow" + i,
                        Gender = "female" + i,
                        Grade = 5 + i,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "Fluffy" }
                        }
                    }
                    },
                    District = "WA5" + i,
                    Address = new Address { State = "WA", County = "King", City = "Seattle" },
                    IsRegistered = true,
                    Job = JobType.B,
                    Time = DateTime.Now.Ticks
                };
                andersenFamily.CreationTime = DateTime.Now.AddSeconds(-i);

                await Repository.InsertAsync(andersenFamily);
            }
            return new string[] { "value1", "value2" };
        }

        [HttpGet("list")]
        public async Task<IList<Family>> GetList()
        {
            var query = new FamilyQuery();
            query.Skip = 0;
            query.Count = 10;
            query.OrderByDesc(x => x.CreationTime);
            var item3 = await Repository.GetListAsync(query);
            var t = item3.Item2.First();
            DateTime dt = new DateTime(t.Time);
            return item3.Item2;
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var list = await Repository.GetAllListAsync();
            var family = list.First();

            var item = await Repository.GetAsync(family.Id);

            var item2 = await Repository.GetListAsync(new FamilyQuery());

            var query = new FamilyQuery();
            query.Skip = 0;
            query.Count = 10;
            query.OrderByDesc(x => x.CreationTime);
            var item3 = await Repository.GetListAsync(query);

            var item5 = await Repository.FirstOrDefaultAsync(family.Id);

            Family andersenFamily = new Family
            {
                Id = family.Id,
                LastName = "ggggeee",
                Parents = new Parent[]
                {
                    new Parent { FirstName = "gesg" },
                    new Parent { FirstName = "ges gesg" }
                },
                Children = new Child[]
                {
                    new Child
                    {
                        FirstName = "gseg Thaulow",
                        Gender = "segesg",
                        Grade = 10,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "segesg" }
                        }
                    }
                },
                District = "WA5",
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = true,
                Time = DateTime.Now.Ticks
            };

            var item6 = await Repository.UpdateAsync(andersenFamily);

            var item7 = await Repository.FirstOrDefaultAsync(family.Id);

            return "value";
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
