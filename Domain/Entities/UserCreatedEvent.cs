using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserCreatedEvent
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
