using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Mobile.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;


        public static string Login
        {
            get => AppSettings.GetValueOrDefault(nameof(Login), "");

            set => AppSettings.AddOrUpdateValue(nameof(Login), value);

        }
        public static string Token
        {
            get => AppSettings.GetValueOrDefault(nameof(Token), "");

            set => AppSettings.AddOrUpdateValue(nameof(Token), value);

        }

        public static string Nome
        {
            get => AppSettings.GetValueOrDefault(nameof(Nome), "");

            set => AppSettings.AddOrUpdateValue(nameof(Nome), value);
        }
        public static string EnderecoApi {
            get => AppSettings.GetValueOrDefault(nameof(EnderecoApi), "");

            set => AppSettings.AddOrUpdateValue(nameof(EnderecoApi), value);
        }

        public static string  EnderecoApiMP {

            get => AppSettings.GetValueOrDefault(nameof(EnderecoApiMP), "");

            set => AppSettings.AddOrUpdateValue(nameof(EnderecoApiMP), value);

        }
        public static string idUsuario
        {

            get => AppSettings.GetValueOrDefault(nameof(idUsuario), "");

            set => AppSettings.AddOrUpdateValue(nameof(idUsuario), value);

        }

        public static string emissorExternoId
        {

            get => AppSettings.GetValueOrDefault(nameof(emissorExternoId), "");

            set => AppSettings.AddOrUpdateValue(nameof(emissorExternoId), value);

        }
        public static string qtdMidia
        {
            get => AppSettings.GetValueOrDefault(nameof(qtdMidia), "");

            set => AppSettings.AddOrUpdateValue(nameof(qtdMidia), value);

        }
    }
}
