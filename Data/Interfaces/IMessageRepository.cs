using Gamification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Data.Interfaces
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Добавить сообщение
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        Task<CommonMessage> Add(CommonMessage newMessage);
        
        /// <summary>
        /// Получить все сообщения из БД
        /// </summary>
        /// <returns></returns>
        Task<List<CommonMessage>> GetAll();
    }
}
