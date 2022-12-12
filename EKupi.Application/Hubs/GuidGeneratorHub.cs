using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKupi.Application.Hubs
{
    public class GuidGeneratorHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await NewGuid(Guid.NewGuid());
        }
        public async Task NewGuid(Guid guid)
        {
            foreach (var item in Enumerable.Range(1, 100))
            {
                await Clients.All.SendAsync("guid-hub", Guid.NewGuid());
                await Task.Delay(100);
            }
        }
    }
}

