using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _db;
        public MongoContext(IOptions<MongoSettings> opts)
        {
            var client = new MongoClient(opts.Value.ConnectionString);
            _db = client.GetDatabase(opts.Value.DatabaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name) =>
          _db.GetCollection<T>(name);
    }
    
}
