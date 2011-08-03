using EventServer.Core.Domain;

namespace EventServer.Core
{
    public class Settings
    {
        private const int DefaultId = 1;
        private static readonly object mutex = new object();
        private static volatile AppSettings instance;

        public static AppSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (mutex)
                    {
                        if (instance == null)
                        {
                            instance = LoadSettings();
                        }
                    }
                }

                return instance;
            }
        }

        public static void SaveSettings(AppSettings settings)
        {
            lock (mutex)
            {
                instance = settings;
                Ioc.Resolve<IRepository>().Save(settings);
            }

        }

        private static AppSettings LoadSettings()
        {
            var repository = Ioc.Resolve<IRepository>();

            return repository.Get<AppSettings>(DefaultId) ?? repository.Save(new AppSettings());

        }
    }
}