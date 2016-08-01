using System;
using System.Collections.Generic;
using System.Threading;
using WcfClient.MasterServiceReference; 

namespace WcfClient
{
    public class Program
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
                Visas = new List<Visa>
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
                Gender = Gender.Female
            },
            new User
            {
                PersonalId = 3,
                FirstName = "Olga",
                LastName = "Lozovik",
                DateOfBirdth = new DateTime(1996, 6, 1),
                Gender = Gender.Female,
                Visas = new List<Visa>
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

        public static void Main(string[] args)
        {
            IServiceContract master = new ServiceContractClient();
            for (int j = 0; j < 5; j++)
            {
                master.Add(Users[0]);
                Thread.Sleep(500);
                master.Add(Users[1]);
                Thread.Sleep(500);
                master.Add(Users[2]);
                Thread.Sleep(500);
            }

            int id = master.Add(Users[0]);
            master.Delete(id);
            master.Search(null);

            Console.ReadLine();
        }
    }
}
