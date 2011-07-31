namespace EventServer.Core
{
    using System;
    using System.IO;
    using System.Web.Hosting;

    using EventServer.Models;

    public static class IdGenerator
    {
        static IdGenerator()
        {
            Initialize(GetNextHiDefaultImpl);
        }

        private static readonly object _lock = new object();
        private static Func<int> _getNextHi;
        private static int _hi;
        private static int _lo;

        public static int StartingHi
        {
            get { return 2; } // skip a couple blocks of numbers to leave some reserved IDs
        }
        public static int MaxLo
        {
            get { return 0xf; }
        }
        public static int StartingId
        {
            get { return StartingHi * MaxLo + 1; }
        }

        public static void Initialize(Func<int> getNextHi)
        {
            _lo = MaxLo + 1; // always start out invalid so we grab the next available hi on the first request
            _getNextHi = getNextHi;
        }

        public static int NextId()
        {
            lock (_lock)
            {
                if (_lo > MaxLo)
                {
                    int newHi = _getNextHi();
                    _hi = newHi * MaxLo;
                    _lo = 1;
                }

                return _hi + _lo++;
            }
        }

        private static int GetNextHiDefaultImpl()
        {
            var path = HostingEnvironment.MapPath("~/App_Data/nextHi.txt");

            int nextHi = !File.Exists(path) ? StartingHi : File.ReadAllText(path).ToInt();

            File.WriteAllText(path, (nextHi + 1).ToString());

            return nextHi;
        }
    }
}