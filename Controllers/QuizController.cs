using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    public class UserMod // for test
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }
    

    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public ActionResult Import ([FromForm(Name = "file")] IFormFile excel, [FromForm(Name = "name")] string quizName,
            [FromForm(Name = "db")] string dateBegin, [FromForm(Name = "de")] string dateEnd)
        {
            List<UserMod> users = new List<UserMod>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                excel.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) 
                    {
                        users.Add(new UserMod { Name = reader.GetValue(0).ToString(), Age = reader.GetValue(1).ToString() });
                    }
                }
            }

            return Ok(new { name = users[1].Name, age = users[1].Age, quizName= quizName, dateBegin= dateBegin, dateEnd= dateEnd });
        }
    }
}
