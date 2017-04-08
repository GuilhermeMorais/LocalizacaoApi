using System.Configuration;
using System.Data;
using System.Linq;
using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Services.Interfaces.Repository;
using Services.Objects;
using Services.Repository;

namespace Services.Tests.Repository
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IDbConnection db;

        [TestInitialize]
        public void Initialize()
        {
            db =  new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlString"].ConnectionString);
        }

        [TestMethod]
        [TestCategory("UserRepo")]
        public void GetAll_shlould_return_results()
        {
            var repo = CreateRepository();

            var users = repo.GetAll();
            
            users.Should().NotBeNullOrEmpty();
            var one = users.First();
            one.Should().NotBeNull();
            one.Id.Should().BeGreaterThan(0);
            one.UserName.Should().NotBeNullOrEmpty();
            one.Password.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        [TestCategory("UserRepo")]
        public void Add_should_add_new_entry()
        {
            var repo = CreateRepository();

            var user = CreateFakeUser();
            repo.Add(user);
            user.Id.Should().NotBe(0);

            RemoveObject();
        }

        [TestMethod]
        [TestCategory("UserRepo")]
        public void Update_should_update_existing_entry()
        {
            var repo = CreateRepository();
            var user = CreateFakeUser();
            repo.Add(user);
            user.Id.Should().NotBe(0);

            user.Password = "987654321";
            user.UserName = "Fake2";
            repo.Update(user);

            var found = repo.Find(user.Id);

            found.Id.Should().Be(user.Id);
            found.UserName.Should().Be("Fake2");
            found.Password.Should().Be("987654321");
            RemoveObject();
        }

        [TestMethod]
        [TestCategory("UserRepo")]
        public void Check_User_and_Password_true()
        {
            var repo = CreateRepository();
            var user = CreateFakeUser();
            repo.Add(user);

            var searching = new User {Password = "1234", UserName = "FakeUser"};
            var exists = repo.CheckUserPassword(searching);

            exists.Should().BeTrue();

            RemoveObject();
        }
        
        [TestMethod]
        [TestCategory("UserRepo")]
        public void Check_User_and_Password_false()
        {
            var repo = CreateRepository();
            var user = CreateFakeUser();
            repo.Add(user);

            var searching = new User {Password = "1234", UserName = "FakeMissing"};
            var exists = repo.CheckUserPassword(searching);

            exists.Should().BeFalse();

            RemoveObject();
        }

        [TestMethod]
        [TestCategory("UserRepo")]
        public void Find_by_UserName_and_Password_true()
        {
            var repo = CreateRepository();
            var user = CreateFakeUser();
            repo.Add(user);

            var searching = new User { Password = "1234", UserName = "FakeUser" };
            var exists = repo.FindByUserNameAndPassword(searching);

            exists.Should().NotBeNull();
            exists.Id.Should().BeGreaterThan(0);
            exists.UserName.Should().Be(user.UserName);
            exists.Password.Should().Be(user.Password);

            RemoveObject();
        }

        private IUserRepository CreateRepository()
        {
            return new UserRepository();
        }

        private User CreateFakeUser()
        {
            return new User
            {
                Password = "1234",
                UserName = "FakeUser"
            };
        }

        private void RemoveObject()
        {
            db.Execute("DELETE FROM USUARIOS WHERE USER_NAME = 'FakeUser' AND SENHA = '1234';");
        }
    }
}
