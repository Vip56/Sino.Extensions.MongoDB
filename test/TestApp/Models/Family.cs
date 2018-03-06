using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Sino.Domain.Entities.Auditing;
using System;

namespace TestApp.Models
{
    public class Family : FullAuditedEntity<ObjectId>
    {
        [JsonProperty(PropertyName = "id")]
        public override ObjectId Id { get; set; }

        public string LastName { get; set; }

        public string District { get; set; }

        public Parent[] Parents { get; set; }

        public Child[] Children { get; set; }

        public Address Address { get; set; }

        public bool IsRegistered { get; set; }

        public long Time { get; set; }

        public JobType Job { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
