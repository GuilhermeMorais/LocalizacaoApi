using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Services.Enums;
using Services.Interfaces.Repository;
using Services.Objects;
using Services.Validations;

namespace Services.Tests.Validations
{
    [TestClass]
    public class InspectionValidationTests
    {
        private IInspectionRepository repo;

        [TestInitialize]
        public void Inicializacao()
        {
            var mock = new MockRepository();
            repo = mock.Stub<IInspectionRepository>();
            using (mock.Record())
            {
                repo.Expect(r => r.Find(Arg<int>.Is.Equal(1))).Return(InspectionGood()).Repeat.Any();
            }
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Add_should_be_Ok()
        {
            var obj = InspectionGood();
            var validador = new InspectionValidation(repo);
            validador.AddChecks();

            var result = validador.Validate(obj);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_be_Ok()
        {
            var obj = InspectionGood();
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_be_Ok()
        {
            var obj = InspectionGood();
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();

            var result = validador.Validate(obj);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_validate_Missing_User()
        {
            var obj = InspectionGood();
            obj.UsuarioId = -1;
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);

            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "O usuário deve ser informado."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_validate_Create_Missing_date()
        {
            var obj = InspectionGood();
            obj.Create = DateTime.MinValue;
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A data da criação deve ser definida."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_validate_5Days_window()
        {
            var obj = InspectionGood();
            obj.Create = DateTime.Today.AddMonths(-1);
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A alteração não pode ocorrer depois de 5 dias de publicado."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_validate_Invalid_Owner()
        {
            var obj = InspectionGood();
            obj.UsuarioId = 999;
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "O usuário deve ser responsável pela fiscalização."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_invalid_user()
        {
            var obj = InspectionGood();
            obj.UsuarioId = -1;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "O usuário deve ser informado."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_create()
        {
            var obj = InspectionGood();
            obj.Create = DateTime.MinValue;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A data da criação deve ser definida."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_5Days_window()
        {
            var obj = InspectionGood();
            obj.Create = DateTime.Today.AddMonths(-1);
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A alteração não pode ocorrer depois de 5 dias de publicado."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_missing_latitude()
        {
            var obj = InspectionGood();
            obj.Latitude = 0;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A latitude deve ser informada."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_missing_longitude()
        {
            var obj = InspectionGood();
            obj.Longitude = 0;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A longitude deve ser informada."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_precision()
        {
            var obj = InspectionGood();
            obj.Precisao = 10;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A precisão deve ser inferior a 5 metros."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_precision2()
        {
            var obj = InspectionGood();
            obj.Precisao = -0.1f;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A precisão deve ser inferior a 5 metros."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_missing_observation()
        {
            var obj = InspectionGood();
            obj.Observacao = string.Empty;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A descrição deve ser informada."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validade_missing_localDescrition()
        {
            var obj = InspectionGood();
            obj.Local = string.Empty;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "O local deve ser informado."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Add_should_validade_today_date()
        {
            var obj = InspectionGood();
            obj.Create = DateTime.Today.AddDays(-1);
            var validador = new InspectionValidation(repo);
            validador.AddChecks();

            var result = validador.Validate(obj);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.IsTrue(result.Errors.ToList().Exists(e => e.ErrorMessage == "A criação deve ser a data de hoje."));
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Update_should_validate_null()
        {
            var obj = (Inspection)null;
            var validador = new InspectionValidation(repo);
            validador.UpdateChecks();
            try
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                validador.Validate(obj);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("Cannot a pass null model to Validate.");
            }
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Remove_should_validate_null()
        {
            var obj = (Inspection)null;
            var validador = new InspectionValidation(repo);
            validador.RemoveChecks();
            try
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                validador.Validate(obj);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("Cannot a pass null model to Validate.");
            }
        }

        [TestMethod, TestCategory("InspectionValidation")]
        public void Add_should_validate_null()
        {
            var obj = (Inspection)null;
            var validador = new InspectionValidation(repo);
            validador.AddChecks();
            try
            {
                // ReSharper disable once ExpressionIsAlwaysNull
                validador.Validate(obj);
            }
            catch (ArgumentNullException ex)
            {
                ex.ParamName.Should().Be("Cannot a pass null model to Validate.");
            }
        }

        public Inspection InspectionGood()
        {
            return new Inspection
            {
                Create = DateTime.Today,
                Id = 1,
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