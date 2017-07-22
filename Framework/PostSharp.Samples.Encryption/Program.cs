using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostSharp.Samples.Encryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Login(new LoginData { Login = "login", Password = "password"});
            Login("login", "password");
        }

        static void Login([ApplyFilters] LoginData loginData)
        {
            Console.WriteLine($"Login={loginData.Login}, Password={loginData.Password}");
        }

        static void Login(string login, [Reverse] string password )
        {
            Console.WriteLine($"Login={login}, Password={password}");
        }
    }

    class LoginData
    {
        public string Login;

        [Reverse]
        public string Password;

    }
}
