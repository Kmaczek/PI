using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.Services.Interfaces
{
    public interface IPasswordHashing
    {
        HashedPassword HashPassword(string password);
        bool ComparePasswords(string plainPassword, byte[] userSalt, byte[] hashedPassword);
    }
}
