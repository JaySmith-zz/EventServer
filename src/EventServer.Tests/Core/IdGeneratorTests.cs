using System;
using EventServer.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventServer.Tests.Core
{
    namespace IdGeneratorTests
    {
        [TestClass]
        public class When_getting_new_ids
        {
            [TestMethod]
            public void Should_get_next_hi_on_first_pull()
            {
                int _nextHi = 0;
                IdGenerator.Initialize(() => _nextHi++);

                Assert.AreEqual(1, IdGenerator.NextId());
                Assert.AreEqual(1, _nextHi);
            }

            [TestMethod]
            public void Should_not_get_next_hi_until_out_of_low()
            {
                int _nextHi = 0;
                IdGenerator.Initialize(() => _nextHi++);

                for (int nextId = 1; nextId <= IdGenerator.MaxLo; nextId++)
                {
                    Assert.AreEqual(nextId, IdGenerator.NextId());
                    Assert.AreEqual(1, _nextHi);
                }

                Assert.AreEqual(IdGenerator.MaxLo + 1, IdGenerator.NextId());
                Assert.AreEqual(2, _nextHi);

                Assert.AreEqual(IdGenerator.MaxLo + 2, IdGenerator.NextId());
                Assert.AreEqual(2, _nextHi);
            }
        }
    }
}