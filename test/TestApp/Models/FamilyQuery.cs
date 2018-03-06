using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TestApp.Models
{
    public class FamilyQuery : QueryObject<Family>
    {
        public string District { get; set; } = "2";
        public string LastName { get; set; } = "1";
        public bool IsRegistered { get; set; } = true;

        public override List<Expression<Func<Family, bool>>> QueryExpression
        {
            get
            {
                var querys = new List<Expression<Func<Family, bool>>>();
                //querys.Add(f => f.District.Contains(District));
                //querys.Add(f => f.LastName.Contains(LastName));
                //querys.Add(f => f.IsRegistered == IsRegistered);
                querys.Add(f => f.Job == JobType.B);
                return querys;
            }
        }
    }
}
