using System;
using System.Configuration;
using NUnit.Framework;

namespace Acid.PuntoPagos.Sdk.Test
{
    [TestFixture]
    public class PuntoPagoFixture
    {
        [SetUp]
        public void Setup()
        {
            ClearConfigInAppConfig();
        }
        [TearDown]
        public void TearDown()
        {
            ClearConfigInAppConfig();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void if_key_is_null_or_empty_throw_argument_exception()
        {
            new PuntoPago().SetKey("").SetSecretCode("SecretCode").SetEnvironment(EnvironmentForPuntoPago.Sandbox).CreateTransaction();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void if_secret_code_is_null_or_empty_throw_argument_exception()
        {
            new PuntoPago().SetKey("Key").SetSecretCode("").SetEnvironment(EnvironmentForPuntoPago.Sandbox).CreateTransaction();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void if_not_set_configuration_by_code_and_not_exist_puntopago_secret_in_app_setting_then_argument_exception()
        {
            new PuntoPago().CreateTransaction();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void if_not_set_configuration_by_code_and_not_exist_puntopago_key_in_app_setting_then_argument_exception()
        {
            AddConfigInAppConfig("PuntoPago-Secret", "Secret");
            new PuntoPago().CreateTransaction();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void if_not_set_configuration_by_code_and_not_exist_puntopago_environment_in_app_setting_then_argument_exception()
        {
            AddConfigInAppConfig("PuntoPago-Secret", "Secret");
            AddConfigInAppConfig("PuntoPago-Key", "Key");
            new PuntoPago().CreateTransaction();
        }

        [Test]
        public void if_set_all_configuration_by_appseting_when_call_create_transaction_return_new_transaction_object()
        {
            AddConfigInAppConfig("PuntoPago-Secret", "Secret");
            AddConfigInAppConfig("PuntoPago-Key", "Key");
            AddConfigInAppConfig("PuntoPago-Environment", "Sandbox");
            var puntoPagos = new PuntoPago().CreateTransaction();

            Assert.NotNull(puntoPagos);
        }

        [Test]
        public void if_set_all_configuration_by_code_when_call_create_transaction_return_new_transaction_object()
        {
            var puntoPagos = new PuntoPago().SetKey("TheKey").SetSecretCode("SecretCode").SetEnvironment(EnvironmentForPuntoPago.Sandbox).CreateTransaction();
            
            Assert.NotNull(puntoPagos);
        }

        private static void AddConfigInAppConfig(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private static void ClearConfigInAppConfig()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}