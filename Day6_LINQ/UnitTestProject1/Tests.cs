using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class Tests
    {
        private readonly List<User> userListFirst = new List<User>
        {
            new User
            {
                Age = 21,
                Gender = Gender.Man,
                Name = "User1",
                Salary = 21000
            },

            new User
            {
                Age = 30,
                Gender = Gender.Female,
                Name = "Liza",
                Salary = 30000
            },

            new User
            {
                Age = 18,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 19000
            },
            new User
            {
                Age = 32,
                Gender = Gender.Female,
                Name = "Ann",
                Salary = 36200
            },
            new User
            {
                Age = 45,
                Gender = Gender.Man,
                Name = "Alex",
                Salary = 54000
            }
        };

        private readonly List<User> userListSecond = new List<User>
        {
            new User
            {
                Age = 23,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 24000
            },

            new User
            {
                Age = 30,
                Gender = Gender.Female,
                Name = "Liza",
                Salary = 30000
            },

            new User
            {
                Age = 23,
                Gender = Gender.Man,
                Name = "Max",
                Salary = 24000
            },
            new User
            {
                Age = 32,
                Gender = Gender.Female,
                Name = "Kate",
                Salary = 36200
            },
            new User
            {
                Age = 45,
                Gender = Gender.Man,
                Name = "Alex",
                Salary = 54000
            },
            new User
            {
                Age = 28,
                Gender = Gender.Female,
                Name = "Kate",
                Salary = 21000
            }
        };

        [TestMethod]
        public void SortByName()
        {
            var expectedData = userListFirst[4];

            //ToDo Add code first list
            var actualDataFirstList = userListFirst.OrderBy(u => u.Name).ToList();
            Assert.IsTrue(actualDataFirstList[0].Equals(expectedData));
        }

        [TestMethod]
        public void SortByNameDescending()
        {
            var expectedData = userListFirst[4];

            //ToDo Add code first list
            var actualDataSecondList = userListFirst.OrderBy(u => u.Name).ToList();

            Assert.IsTrue(actualDataSecondList[0].Equals(expectedData));       
        }

        [TestMethod]
        public void SortByNameAndAge()
        {
            var expectedData = userListSecond[4];

            //ToDo Add code second list
            var actualDataSecondList = userListFirst.OrderBy(u => u.Name).ThenBy(u=>u.Age).ToList();
            Assert.IsTrue(actualDataSecondList[0].Equals(expectedData));
        }

        [TestMethod]
        public void RemovesDuplicate()
        {
            var expectedData = new List<User> {userListSecond[0], userListSecond[1], userListSecond[3], userListSecond[4],userListSecond[5]};

            //ToDo Add code second list
            var actualDataSecondList = userListSecond.Distinct().ToList();

            CollectionAssert.AreEqual(expectedData, actualDataSecondList);
        }

        [TestMethod]
        public void ReturnsDifferenceFromFirstList()
        {
            var expectedData = new List<User> { userListFirst[0], userListFirst[2], userListFirst[3] };

            //ToDo Add code first list
            var actualData = userListFirst.Except(userListSecond).ToList();


            CollectionAssert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void SelectsValuesByNameMax()
        {
            var expectedData = new List<User> { userListSecond[0], userListSecond[2] };

            //ToDo Add code for second list
            var actualData = userListSecond.Where(u => u.Name == "Max").ToList();

            CollectionAssert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void ContainOrNotContainName()
        {
            //name max 
            //ToDo Add code for second list
            var isContain = userListSecond.Any(u => u.Name == "Max");

            Assert.IsTrue(isContain);

            // name obama
            //ToDo add code for second list
            isContain = userListSecond.Any(u => u.Name == "obama");

            Assert.IsFalse(isContain);
        }

        [TestMethod]
        public void AllListWithName()
        {
            //name max 
            //ToDo Add code for second list
            var isAll = userListSecond.All(u => u.Name == "Max");
            
            Assert.IsFalse(isAll);
        }

        [TestMethod]
        public void ReturnsOnlyElementByNameMax()
        {
            try
            {
                //ToDo Add code for second list
                userListSecond.Single(u => u.Name == "Max");
                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Последовательность содержит более одного соответствующего элемента", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
           
        }

        [TestMethod]
        public void ReturnsOnlyElementByNameNotOnList()
        {
            try
            {
                //ToDo Add code for second list
                var y = userListSecond.Single(u => u.Name == "FDGCHJBKL");
                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Последовательность не содержит соответствующий элемент", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
           
        }

        [TestMethod]
        public void ReturnsOnlyElementOrDefaultByNameNotOnList()
        {
            //ToDo Add code for second list
            var actualData = userListSecond.SingleOrDefault(u => u.Name == "Ldfsdfsfd");

            Assert.IsTrue(actualData == null);
        }


        [TestMethod]
        public void ReturnsTheFirstElementByNameNotOnList()
        {
            try
            {
                //ToDo Add code for second list
                userListSecond.First(u => u.Name == "kgjilhkgkgfdguk");
                Assert.Fail();
            }
            catch (InvalidOperationException ie)
            {
                Assert.AreEqual("Последовательность не содержит соответствующий элемент", ie.Message);
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message);
            }
        }

        [TestMethod]
        public void ReturnsTheFirstElementOrDefaultByNameNotOnList()
        {
            //ToDo Add code for second list
            //name Ldfsdfsfd
            var actualData = userListSecond.FirstOrDefault(u => u.Name == "Ldfsdfsfd");

            Assert.IsTrue(actualData == null);
        }

        [TestMethod]
        public void GetMaxSalaryFromFirst()
        {
            var expectedData = 54000;
            var actualData = new User {Salary = userListFirst.Max(u => u.Salary)};

            //ToDo Add code for first list

            Assert.IsTrue(expectedData == actualData.Salary);
        }

        [TestMethod]
        public void GetCountUserWithNameMaxFromSecond()
        {
            var expectedData = 2;
          
            //ToDo Add code for second list
            var actualData = userListSecond.Count(u => u.Name == "Max");

            Assert.IsTrue(expectedData == actualData);
        }

        [TestMethod]
        public void Join()
        {
            var nameInfo = new[]
            {
                new {name = "Max", Info = "info about Max"},
                new {name = "Alan", Info = "About Alan"},
                new {name = "Alex", Info = "About Alex"}
            }.ToList();

            var expectedData = new[] 
            { 
               new {Name = "Max", Age = 23, Salary = (decimal)24000, Gender = Gender.Man, Info = "info about Max"},
               new {Name = "Max", Age = 23, Salary = (decimal)24000, Gender = Gender.Man, Info = "info about Max"},
               new {Name = "Alex", Age = 45, Salary = (decimal)54000, Gender = Gender.Man, Info = "About Alex"}
            }.ToList();

            //ToDo Add code for second list
            var actualData = userListSecond.Join(nameInfo, p => p.Name, t => t.name, 
             (p, t) => new { Name = p.Name, Age = p.Age, Salary = p.Salary, Gender = p.Gender, Info = t.Info}).ToList(); 

            CollectionAssert.AreEqual(expectedData, actualData);
        }
    }
}
