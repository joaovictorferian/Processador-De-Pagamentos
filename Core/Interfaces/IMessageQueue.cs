using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMessageQueue
    {
        Task PublishAsync(string queue, string message);
        Task SubscribeAsync(string queue, Func<string, Task> onMessage);
    }
}

