using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationHelper.Models
{
    public class Login: MainModel
    {
        public string Username { get; set; }
        public string PinNumber { get; set; }
    }
}
