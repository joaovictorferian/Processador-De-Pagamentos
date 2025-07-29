using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura
{
    public interface IMongoContext
    {
        IMongoCollection<T> GetCollection<T>(string name);

    }
}
