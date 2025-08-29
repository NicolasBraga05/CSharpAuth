using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpAuth.Models;

namespace CSharpAuth.Dtos
{
    public class UserResponse
    {
        public string Message { get; set; }

        public UserDto User { get; set; }

        public List<UserDto> Users { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}