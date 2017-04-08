using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Services.Enums;
using Services.Interfaces.Repository;
using Services.Objects;
using Services.Service;

namespace Services.Tests.Service
{
    [TestClass]
    public class ServiceOfInspectionTests
    {
        private IInspectionRepository repo;
        private ServiceOfInspection service;

        [TestInitialize]
        public void Inicializacao()
        {
            var mock = new MockRepository();
            repo = mock.Stub<IInspectionRepository>();
            using (mock.Record())
            {
                var inscpectBad = InspectionGood(1);
                inscpectBad.UsuarioId = 15000;
                var goodList = new List<Inspection> { InspectionGood(2), InspectionGood(4) };

                //Save OK
                repo.Expect(r => r.Add(Arg<Inspection>.Matches(x => x.Id == 0))).Repeat.Any().Return(InspectionGood(2));

                //Save Invalid
                repo.Expect(r => r.Find(Arg<int>.Is.Equal(1))).Return(null).Repeat.Any();

                // Remove Invalid
                repo.Expect(r => r.Find(Arg<int>.Is.Equal(-2))).Repeat.Any().Return(InspectionGood(-2));
                repo.Expect(r => r.Remove(Arg<int>.Is.Equal(-2))).Return(false).Repeat.Any();

                // Remove Invalid2
                repo.Expect(r => r.Find(Arg<int>.Is.Equal(4))).Return(InspectionGood(4)).Repeat.Any();

                // Remove OK
                repo.Expect(r => r.Find(Arg<int>.Is.Equal(3))).Return(InspectionGood(3)).Repeat.Any();
                repo.Expect(r => r.Remove(Arg<int>.Is.Equal(3))).Return(true).Repeat.Any();

                // GetAllByUser & GetLast OK
                repo.Expect(r => r.GetAllByUser(Arg<User>.Matches(x=> x.Id == 1))).Return(goodList).Repeat.Any();

                // GetAllByUser & GetLast Invalid
                repo.Expect(r => r.GetAllByUser(Arg<User>.Matches(x => x.Id == 2))).Return(new List<Inspection>()).Repeat.Any();

                // GetAllByDates OK
                repo.Expect(r => r.GetAllByDates(Arg<User>.Matches(x => x.Id == 1), Arg<DateTime>.Is.Equal(DateTime.Today), Arg<DateTime>.Is.Equal(DateTime.Today))).Return(goodList).Repeat.Any();

                // GetAllByDates Invalid
                repo.Expect(r => r.GetAllByDates(Arg<User>.Matches(x => x.Id == 2), Arg<DateTime>.Is.Equal(DateTime.Today), Arg<DateTime>.Is.Equal(DateTime.Today))).Return(new List<Inspection>()).Repeat.Any();
            }

            //Set Mock on Service.
            service = new ServiceOfInspection(repo);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void Save_should_be_Ok()
        {
            var inspect = InspectionGood(0);
            var result = service.Save(inspect);
            result.Should().Be(inspect.Id);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void Save_should_be_Invalid()
        {
            var inspect = InspectionGood(0);
            try
            {
                service.Save(inspect);
            }
            catch (InvalidOperationException e)
            {
                e.Message.Should().Be("UsuarioId:O usuário deve ser responsável pela fiscalização.");
            }
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void Remove_should_be_Ok()
        {
            var inspect = InspectionGood(3);
            service.Remove(inspect);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void Remove_should_be_Invalid()
        {
            var inspect = InspectionGood(-2);
            try
            {
                service.Remove(inspect);
            }
            catch (InvalidOperationException e)
            {
                e.Message.Should().Be("Error during the removal.");
            }
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void Remove_should_be_Invalid2()
        {
            var inspect = InspectionGood(4);
            inspect.UsuarioId = -1;
            try
            {
                service.Remove(inspect);
            }
            catch (InvalidOperationException e)
            {
                e.Message.Should().Be("UsuarioId:O usuário deve ser responsável pela fiscalização.\r\nUsuarioId:O usuário deve ser informado.");
            }
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetLast_should_be_Ok()
        {
            var results = service.GetLast(1).ToList();
            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(2);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetLast_should_be_Invalid()
        {
            var results = service.GetLast(2).ToList();
            results.Should().BeEmpty();
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetAfterDate_should_be_Ok()
        {
            var results = service.GetAfterDate(1, DateTime.Today).ToList();
            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(2);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetAfterDate_should_be_Invalid()
        {
            var results = service.GetAfterDate(2, DateTime.Today).ToList();
            results.Should().BeEmpty();
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetAll_should_be_Ok()
        {
            var results = service.GetAll(1).ToList();
            results.Should().NotBeNullOrEmpty();
            results.Count.Should().Be(2);
        }

        [TestMethod, TestCategory("ServiceOfInspection")]
        public void GetAll_should_be_Invalid()
        {
            var results = service.GetAll(2).ToList();
            results.Should().BeEmpty();
        }

        private Inspection InspectionGood(int id)
        {
            return new Inspection
            {
                Create = DateTime.Today,
                Id = id,
                Latitude = 10.02f,
                Longitude = -10.02f,
                UsuarioId = 1,
                Precisao = 1,
                Local = "Right here, right now",
                Observacao = "Fat lazy boy...",
                TipoLancamento = EnumTipoLancamento.Manual
            };
        }
    }
}