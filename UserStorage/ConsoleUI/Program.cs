using System;
using System.Collections.Generic;
using FibbonacciGenerator;
using Storage.Loader;
using Storage.Service;
using Storage.Strategy;
using Storage.UserInfo;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var storage = new UserService(new Master(new List<Func<User, bool>>()), new Loader(), new Fibbonacci());
            storage.Load();
            storage.Add(new User {FirstName = "qwerty"});
            storage.Save();
            Console.ReadLine();
        }
    }
}
