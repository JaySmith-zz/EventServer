using System;
using EventServer.Core.Domain;

namespace EventServer.Core
{
    public static class Settings
    {
        private const int DefaultId = 1;

        static Settings()
        {
            Instance = LoadSettings();
        }

        public static AppSettings Instance { get; private set; }

        private static AppSettings LoadSettings()
        {
            var repository = Ioc.Resolve<IRepository>();

            return repository.Get<AppSettings>(DefaultId) ?? repository.Save(new AppSettings());
        }

        public static void SaveSettings(AppSettings settings)
        {
            Instance = settings;

            Ioc.Resolve<IRepository>().Save<AppSettings>(settings);
        }
    }
}