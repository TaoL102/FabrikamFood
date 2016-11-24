using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.DataModels
{
    public class SocialLoginResult
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string SocialId
        {
            get { return string.IsNullOrEmpty(Sub) ? Id : Sub; }
        }

        public string Email { get; set; }
        public string Sub { get; set; }
        public string Id { get; set; }
    }
}
