using System;
using System.Collections.Generic;
using System.Net;
using Configurator.Factory;
using Configurator.Logging;
using FibbonacciGenerator;
using Storage.Interfaces.Entities.UserInfo;
using Storage.Interfaces.Generator;
using Storage.Interfaces.Logger;
using Storage.Interfaces.Network;
using Storage.Interfaces.Repository;
using Storage.Interfaces.Validator;
using Storage.Network;
using Storage.Repository;
using Storage.Validator;

namespace Storage.Tests
{
    public static class TestInfo
    {
        private static Dictionary<Type, InstanceInfo> typesSingle = new Dictionary<Type, InstanceInfo>
        {
            { typeof(IGenerator), new InstanceInfo { Type = typeof(FibonacciGenerator).AssemblyQualifiedName } },
            { typeof(IRepository), new InstanceInfo { Type = typeof(FakeRepository).AssemblyQualifiedName } },
            { typeof(IValidator), new InstanceInfo { Type = typeof(UserValidator).AssemblyQualifiedName } },
            { typeof(ILogger), new InstanceInfo { Type = typeof(DefaultLogger).AssemblyQualifiedName } },
            { typeof(ISender), new InstanceInfo { Type = typeof(Sender).AssemblyQualifiedName, Params = new object[] { new List<IPEndPoint> { } } } },
            { typeof(IReceiver), new InstanceInfo { Type = typeof(Receiver).AssemblyQualifiedName, Params = new object[] { new IPEndPoint(IPAddress.None, 10001) } } }
        };

        public static DependencyFactory Factory => new DependencyFactory(typesSingle);
        
        public static List<User> Users { get; } = new List<User>
        {
            new User
            {
                PersonalId = 1,
                FirstName = "Maxim",
                LastName = "Harbachou",
                DateOfBirdth = new DateTime(1995, 7, 3),
                Gender = Gender.Male,
                Visas = new[]
                 {
                    new Visa
                    {
                        Country = "Bulgaria",
                        Start = new DateTime(2012, 7, 15),
                        End = new DateTime(2012, 8, 1)
                    }
                }
            },
            new User
            {
                PersonalId = 2,
                FirstName = "Natalia",
                LastName = "Vladimirova",
                DateOfBirdth = new DateTime(1995, 7, 3),
                Gender = Gender.Male
            },
            new User
            {
                PersonalId = 3,
                FirstName = "Olga",
                LastName = "Lozovik",
                DateOfBirdth = new DateTime(1996, 6, 1),
                Gender = Gender.Female,
                Visas = new[]
                {
                    new Visa
                    {
                        Country = "Bulgaria",
                        Start = new DateTime(2014, 7, 15),
                        End = new DateTime(2014, 8, 1)
                    },
                    new Visa
                    {
                        Country = "Poland",
                        Start = new DateTime(2013, 1, 1),
                        End = new DateTime(2015, 12, 31)
                    }
                }
            }
        };
    }
}