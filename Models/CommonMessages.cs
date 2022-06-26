using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class CommonMessage
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Автор сообщения
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Время отправки сообщения
        /// </summary>
        public DateTime DispatchTime { get; set; }
    }
}
