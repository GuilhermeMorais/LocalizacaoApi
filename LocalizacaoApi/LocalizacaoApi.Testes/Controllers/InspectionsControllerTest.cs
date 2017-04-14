using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using FluentAssertions;
using LocalizacaoApi.Controllers;
using LocalizacaoApi.Filters;
using LocalizacaoApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Services.Enums;
using Services.Interfaces.Service;
using Services.Objects;

namespace LocalizacaoApi.Testes.Controllers
{
    [TestClass]
    public class InspectionControllerTest
    {
        private InspectionsController ctrl;
        private IPrincipal originalPrincipal;

        [TestInitialize]
        public void Initialize()
        {
            originalPrincipal = Thread.CurrentPrincipal;
            InicializaController();
            PrepareIdentity("1@Demo");
        }

        [TestCleanup]
        public void TearDown()
        {
            Thread.CurrentPrincipal = originalPrincipal;
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestWithOutConnectionParameter()
        {
            var controller = new InspectionsController();
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestGetLast()
        {
            var response = ctrl.GetLast().ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<DtoInspection> inspecs));
            inspecs.Count().Should().Be(2);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestGetByPageWith1Result()
        {
            var response = ctrl.GetByPage(2).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<DtoInspection> inspecs));
            inspecs.Count().Should().Be(1);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestGetByPageWithNoResults()
        {
            var response = ctrl.GetByPage(3).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out IEnumerable<DtoInspection> inspecs));
            inspecs.Should().BeEmpty();
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestSummary()
        {
            var response = ctrl.Summay().ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out List<Tuple<DateTime, int>> tuples));
            tuples.Count.Should().Be(2);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestPostOk()
        {
            var dto = CreateFakeDto();

            var response = ctrl.Post(dto).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.Created);
            Assert.IsTrue(result.TryGetContentValue(out DtoInspection dtoReturned));

            dtoReturned.Latitude.Should().Be(dto.Latitude);
            dtoReturned.Longitude.Should().Be(dto.Longitude);
            dtoReturned.Local.Should().Be(dto.Local);
            dtoReturned.Observacao.Should().Be(dto.Observacao);
            dtoReturned.Id.Should().Be(10);
            dtoReturned.Precisao.Should().Be(dto.Precisao);
            dtoReturned.TipoLancamento.Should().Be(dto.TipoLancamento);
            dtoReturned.Create.Should().Be(DateTime.Today);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestPostInvalid()
        {
            var dto = CreateFakeDto();
            dto.Local = string.Empty;
            var response = ctrl.Post(dto).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            TestForBadRequest(result);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestPutOk()
        {
            var dto = CreateFakeDto();
            dto.Id = 5;
            var response = ctrl.Put(dto).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.OK);
            Assert.IsTrue(result.TryGetContentValue(out DtoInspection dtoReturned));

            dtoReturned.Latitude.Should().Be(dto.Latitude);
            dtoReturned.Longitude.Should().Be(dto.Longitude);
            dtoReturned.Local.Should().Be(dto.Local);
            dtoReturned.Observacao.Should().Be(dto.Observacao);
            dtoReturned.Id.Should().Be(5);
            dtoReturned.Precisao.Should().Be(dto.Precisao);
            dtoReturned.TipoLancamento.Should().Be(dto.TipoLancamento);
            dtoReturned.Create.Date.Ticks.Should().Be(DateTime.Today.Ticks);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestPutInvalid()
        {
            var dto = CreateFakeDto();
            dto.Id = -5;
            var response = ctrl.Put(dto).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            TestForBadRequest(result);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestDeleteOk()
        {
            var response = ctrl.Remove(7).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [TestMethod]
        [TestCategory("InspectionController")]
        public void TestDeleteInvalid()
        {
            var response = ctrl.Remove(-7).ExecuteAsync(CancellationToken.None);
            var result = response.Result;

            TestForBadRequest(result);
        }

        private void TestForBadRequest(HttpResponseMessage result)
        {
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.IsTrue(result.TryGetContentValue(out HttpError msgErro));
            msgErro.Message.Should().Contain("Invalid Id");
        }

        private void InicializaController()
        {
            var serviceMock = CreatMock();
            ctrl = new InspectionsController(serviceMock) {Configuration = new HttpConfiguration()};
            ctrl.Configuration.Filters.Add(new BasicAuthorizationAttribute());
            ctrl.ControllerContext.Configuration.Filters.Add(new BasicAuthorizationAttribute());
            ctrl.Request = new HttpRequestMessage();
            ctrl.Request.Headers.Add("Authorization", "Basic Z2xvcGVzOjEyMzQ=");
            ctrl.Request.RequestUri = new Uri("http://fake.path.com");
        }

        private IServiceOfInspection CreatMock()
        {
            var mock = new MockRepository();
            var service = mock.Stub<IServiceOfInspection>();

            using (mock.Record())
            {
                var fakeList = CreateFakeList().ToList();
                var invalidId = new InvalidOperationException("Invalid Id");

                service.Expect(s => s.GetLast(Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(1)))
                    .Repeat.Any()
                    .Return(fakeList);

                service.Expect(s => s.GetAll(Arg<int>.Is.Equal(1)))
                    .Repeat.Any()
                    .Return(fakeList);

                service.Expect(s => s.GetLast(Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(2)))
                    .Repeat.Any()
                    .Return(new[] {fakeList[0]});

                service.Expect(s => s.GetLast(Arg<int>.Is.Equal(1), Arg<int>.Is.Equal(3)))
                    .Repeat.Any()
                    .Return(new List<Inspection>());

                //Add OK
                service.Expect(s => s.Save(Arg<Inspection>.Matches(x => x.Id == 0 && !string.IsNullOrEmpty(x.Local))))
                    .WhenCalled(call =>
                    {
                        var objArg       = (Inspection) call.Arguments[0];
                        objArg.Id        = 10;
                        objArg.Create    = DateTime.Today;
                        call.ReturnValue = 1;
                    })
                    .Repeat.Once()
                    .Return(1);

                //Add BadRequest
                service.Expect(s => s.Save(Arg<Inspection>.Matches(x => x.Id == 0 && string.IsNullOrEmpty(x.Local))))
                    .Repeat.Once()
                    .Throw(invalidId);

                //Update OK
                service.Expect(s => s.Save(Arg<Inspection>.Matches(x => x.Id == 5)))
                    .WhenCalled(call =>
                    {
                        var objArg = (Inspection)call.Arguments[0];
                        objArg.Id = 5;
                        objArg.Create = DateTime.Today;
                        call.ReturnValue = 1;
                    })
                    .Repeat.Once()
                    .Return(1);

                //Update BadRequest
                service.Expect(s => s.Save(Arg<Inspection>.Matches(x => x.Id == -5)))
                    .Repeat.Once()
                    .Throw(invalidId);

                service.Expect(s => s.Remove(Arg<Inspection>.Matches(x => x.Id == 7))).Repeat.Once();

                service.Expect(s => s.Remove(Arg<Inspection>.Matches(x => x.Id == -7))).Repeat.Once().Throw(invalidId);
            }
            return service;
        }

        private IEnumerable<Inspection> CreateFakeList()
        {
            yield return new Inspection
            {
                Create         = DateTime.Today.AddDays(-10),
                Latitude       = -16.6956874,
                Longitude      = -49.5844635,
                Local          = "Goiânia",
                Observacao     = "Great place to live.",
                Precisao       = 5,
                TipoLancamento = EnumTipoLancamento.Manual,
                UsuarioId      = 1
            };

            yield return new Inspection
            {
                Create         = DateTime.Today.AddDays(-7),
                Local          = "Ano Vouves",
                Latitude       = 35.4870762,
                Longitude      = 23.7847959,
                Observacao     = "oldest olive tree",
                Precisao       = 5,
                TipoLancamento = EnumTipoLancamento.Manual,
                UsuarioId      = 1
            };
        }

        private DtoInspection CreateFakeDto()
        {
            return new DtoInspection
            {
                Latitude       = -16.6956874,
                Longitude      = -49.5844635,
                Local          = "Future",
                Observacao     = "Great place to fight for.",
                Precisao       = 5,
                TipoLancamento = EnumTipoLancamento.Manual
            };
        }

        private static void PrepareIdentity(string userName)
        {
            var roles = new List<string> { "Moderador", "Usuario" };
            var principal = new GenericPrincipal(new GenericIdentity(userName), roles.ToArray());
            Thread.CurrentPrincipal = principal;
        }
    }
}