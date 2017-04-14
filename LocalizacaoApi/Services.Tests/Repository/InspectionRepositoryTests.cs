using System;
using System.Configuration;
using System.Data;
using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Services.Enums;
using Services.Objects;
using Services.Repository;

namespace Services.Tests.Repository
{
    [TestClass]
    public class InspectionRepositoryTests
    {
        private const string CmdDelete = "DELETE FROM FIS_LOCALIZACOES WHERE desc_local = '{0}' and desc_observacao = '{1}'";
        private IDbConnection db;
        private User fakeUser;
        private Inspection fakeInspec;

        [TestInitialize]
        public void Initialize()
        {
            db = new MySqlConnection(ConfigurationManager.ConnectionStrings["mysqlString"].ConnectionString);
            CleanDb();
            RecreateFakeUserOnDb();
            fakeInspec = CreateFakeInspection();
        }

        [TestCleanup]
        public void FinalizeTest()
        {
            CleanDb();
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Get_all_inspections_by_user_id()
        {
            var repo = new InspectionRepository();
            repo.Add(fakeInspec);

            var results = repo.GetAllByUser(fakeUser);

            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(1);

            var first = results[0];
            CompareObjs(fakeInspec, first);
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Get_all_inspections_by_invalid_user_id()
        {
            var repo = new InspectionRepository();
            var userInvalida = new User {Id = -1};
            var results = repo.GetAllByUser(userInvalida);

            results.Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Get_all_inspections_by_date_interval()
        {
            var repo = new InspectionRepository();
            repo.Add(fakeInspec);
            var user = new User {Id = fakeInspec.UsuarioId};
            var results = repo.GetAllByDates(user, DateTime.Today, DateTime.Today.AddDays(1));

            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(1);

            var first = results[0];
            CompareObjs(fakeInspec, first);
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Add_inspection()
        {
            const string kennedySpaceCenter = "Kennedy Space Center";
            const string localCapeCanaveral = "Cape Canaveral";
            try
            {
                var repo = new InspectionRepository();
                var newFakeInspect = CreateFakeInspection();
                newFakeInspect.Local      = localCapeCanaveral;
                newFakeInspect.Latitude   = 28.4941382;
                newFakeInspect.Longitude  = -80.6958792;
                newFakeInspect.Precisao   = 4.5678;
                newFakeInspect.Observacao = kennedySpaceCenter;
                repo.Add(newFakeInspect);
                newFakeInspect.Id.Should().NotBe(0);

                var result = repo.Find(newFakeInspect.Id);
                CompareObjs(newFakeInspect, result);
            }
            finally
            {
                var sqlRemoveFakeInspection = string.Format(CmdDelete, localCapeCanaveral, kennedySpaceCenter);
                db.Execute(sqlRemoveFakeInspection);
            }
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Update_inspection()
        {
            RecreateFakeUserOnDb();
            var repo = new InspectionRepository();
            const string localAnoVouves = "Ano Vouves";
            const string obsOldestOliveTree = "oldest olive tree";
            try
            {
                //Save
                repo.Add(fakeInspec);

                var newFakeInspect = repo.Find(fakeInspec.Id);
                newFakeInspect.Should().NotBeNull();
                //Alter
                newFakeInspect.Local      = localAnoVouves;
                newFakeInspect.Latitude   = 35.4870762;
                newFakeInspect.Longitude  = 23.7847959;
                newFakeInspect.Observacao = obsOldestOliveTree;
                repo.Update(newFakeInspect);

                //Compare.
                var result = repo.Find(newFakeInspect.Id);
                result.Should().NotBeNull();
                CompareObjs(newFakeInspect, result);
            }
            finally
            {
                var sqlRemoveFakeInspection = string.Format(CmdDelete, localAnoVouves, obsOldestOliveTree);
                db.Execute(sqlRemoveFakeInspection);
                fakeInspec = CreateFakeInspection();
                repo.Add(fakeInspec);
            }
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Remove_inspection_true()
        {
            //Prep
            RecreateFakeUserOnDb();
            var repo = new InspectionRepository();
            var newFakeInspect        = CreateFakeInspection();
            newFakeInspect.Create     = DateTime.Today.AddDays(-15);
            newFakeInspect.Local      = "Todelete";
            newFakeInspect.Observacao = "ToDelete";
            repo.Add(newFakeInspect);
            newFakeInspect.Id.Should().NotBe(0);

            //Its on db...
            var result = repo.Find(newFakeInspect.Id);
            result.Should().NotBeNull();

            //Killing.
            var removed = repo.Remove(newFakeInspect.Id);
            removed.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("InspectionRepo")]
        public void Remove_inspection_false()
        {
            //Prep
            var repo = new InspectionRepository();
            //Killing.
            var removed = repo.Remove(-1);
            removed.Should().BeFalse();
        }

        private Inspection CreateFakeInspection()
        {
            return new Inspection
            {
                Create         = DateTime.Now,
                Latitude       = -16.6956874,
                Longitude      = -49.5844635,
                Local          = "Goiânia",
                Observacao     = "Great place to live.",
                Precisao       = 5,
                TipoLancamento = EnumTipoLancamento.Manual,
                UsuarioId      = fakeUser.Id
            };
        }

        private void CleanDb()
        {
            const string sqlRemoveFakeInspection = @"DELETE FROM FIS_LOCALIZACOES WHERE USUARIO_ID IN (SELECT USUARIO_ID FROM USUARIOS WHERE user_name = 'FakeUserInspectionTest' and Senha = '1234') ";
            const string sqlRemoveFakeUser = @"DELETE FROM USUARIOS WHERE user_name = 'FakeUserInspectionTest' and Senha = '1234'";
            db.Execute(sqlRemoveFakeInspection);
            db.Execute(sqlRemoveFakeUser);
        }

        private void RecreateFakeUserOnDb()
        {
            fakeUser = new User { Password = "1234", UserName = "FakeUserInspectionTest" };
            var repoUser = new UserRepository();
            repoUser.Add(fakeUser);
        }

        private void CompareObjs(Inspection expected, Inspection obtain)
        {
            obtain.Id.Should().Be(expected.Id);
            obtain.Latitude.Should().Be(expected.Latitude);
            obtain.Longitude.Should().Be(expected.Longitude);
            obtain.Local.Should().Be(expected.Local);
            obtain.Observacao.Should().Be(expected.Observacao);
            obtain.UsuarioId.Should().Be(expected.UsuarioId);
            obtain.Create.ToString("F").Should().Be(expected.Create.ToString("F"));
            obtain.Precisao.Should().Be(expected.Precisao);
            obtain.TipoLancamento.Should().Be(expected.TipoLancamento);
        }
    }
}