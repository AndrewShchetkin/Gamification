using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationContext db;
        public MessageRepository(ApplicationContext context)
        {
            db = context;
        }

        public async Task<CommonMessage> Add(CommonMessage newMessage)
        {
            db.CommonMessages.Add(newMessage);
            await db.SaveChangesAsync();
            return newMessage;
        }

        public async Task<List<CommonMessage>> GetAll()
        {
            return await db.CommonMessages.ToListAsync();
        }
    }
}
