using Gamification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Data.Interfaces
{
    public interface IMessageRepository
    {
        // Добавить сообщение в БД 
        Task Add(CommonMessage newMessage);
        // Получить все сообщения из бд 
        Task<List<CommonMessage>> GetAll();
    }
}
