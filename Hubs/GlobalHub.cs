using AutoMapper;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Gamification.Models.DTO;
using Gamification.Models.DTO.Team;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Hubs
{
    public class GlobalHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public GlobalHub(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public override Task OnConnectedAsync()
        {
                return base.OnConnectedAsync();
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

        public async Task ConnectToGeneralGroup ()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "generalGroup");

            //_connections[Context.ConnectionId] = userConnection;

            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            //await SendUsersConnected(userConnection.Room);
        }

        public async Task JoinTeamGroup(string teamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);

            //_connections[Context.ConnectionId] = userConnection;

            //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            //await SendUsersConnected(userConnection.Room);
        }

        #region Методы для обработки сообщений


        public async Task SendMessage(string message, string group)
        {

            var newMessage = await _messageRepository.Add(new CommonMessage
            {
                Author = Context.User.Identity.Name,
                Text = message,
                DispatchTime = DateTime.Now.ToUniversalTime(),
                Group = group
            });
            var messageDto = _mapper.Map<CommonMessage, CommonMessageDto>(newMessage);

            await Clients.Group(group).SendAsync("ReceiveMessage", messageDto);
            //await Clients.All.SendAsync("ReceiveMessage", messageDto);
            //CommonMessage newMessage = new CommonMessage
            //{
            //    Author = Context.User.Identity.Name,
            //    Text = message,
            //    DispatchTime = DateTime.Now.ToUniversalTime(),
            //};

            //await _messageRepository.Add(newMessage);

            //if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            //{
            //    await Clients.Group(roomName).SendAsync("ReceiveMessage", Context.User.Identity.Name, message, new DateTime(), roomName);
            //}
        }

        #endregion
    }
}
