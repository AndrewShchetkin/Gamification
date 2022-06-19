using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Gamification.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(string username, string message)
        {
            await Clients.All.SendAsync("messageReceived", username, message);
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            //_connections[Context.ConnectionId] = userConnection;

            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            //await SendUsersConnected(userConnection.Room);
        }

        public async Task SendMessage(string message, string roomName)
        {
            //if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            //{
                await Clients.Group(roomName).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, new DateTime());
            //}
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            //if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            //{
            //    _connections.Remove(Context.ConnectionId);
            //    Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
            //    SendUsersConnected(userConnection.Room);
            //}

            return base.OnDisconnectedAsync(exception);
        }


    }
}
