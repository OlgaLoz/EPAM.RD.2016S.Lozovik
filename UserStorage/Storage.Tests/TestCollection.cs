using System;
using System.Collections.Generic;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Tests
{
    public static class TestCollection
    {
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