using System;
using System.Collections.Generic;
using System.Text;

namespace UserServiceLib
{
    public interface IUserRepository
    {
        bool EmailExists(string email);
        void Add(User user);
    }
}