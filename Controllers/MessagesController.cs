using AutoMapper;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Gamification.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    [Route(template: "api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        [HttpPost(template: "addmessage")]
        public async Task<ActionResult> AddMesssage(CommonMessageDto commonMessageDto)
        {
            var newMessage = new CommonMessage
            {
                Author = commonMessageDto.Author,
                Text = commonMessageDto.Text,
                DispatchTime = DateTime.Now
            };

            await _messageRepository.Add(newMessage);

            return Ok();
        }

        [HttpGet(template: "getAllCommonMessages")]
        public async Task<ActionResult<List<CommonMessageDto>>> GetAllCommonMessages()
        {
            var messages = await _messageRepository.GetAll();
            var messagesDto = _mapper.Map<List<CommonMessage>, List<CommonMessageDto>>(messages);
            // упорядочить по времени на стороне контроллера
            return Ok(messagesDto);
        }
    }
}
