using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabrikamFood.Helpers
{
    public interface IShareable
    {
        void Share(string title, string content);
        void Share(string title, string content,string picUrl);
    }
}
