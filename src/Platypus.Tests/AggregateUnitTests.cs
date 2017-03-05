using System.Linq;
using NUnit.Framework;
using Platypus.Configuration;

namespace Platypus.Tests
{
    [TestFixture]
    public class AggregateUnitTests
    {
        private TestAggregate _testAggregate;
        [SetUp]
        public void Setup()
        {
            DomainBootstrapper.With<StudentFailed, TestAggregate>((e, agg) =>
            {
                agg.GPA = e.GPA;
                agg.IsPassing = false;
            });
            _testAggregate = new TestAggregate();
        }

        [TearDown]
        public void TearDown()
        {
            DomainBootstrapper.Clear();
            _testAggregate = null;
        }

        [Test]
        public void DoesActionActOnAggregate()
        {
            var studentFailed = new StudentFailed(1, 1.1m);
            _testAggregate.ApplyChange(studentFailed, true);
            var changes = _testAggregate.GetChanges();
            Assert.AreEqual(_testAggregate.GPA, 1.1m);
            Assert.AreEqual(changes.Count(), 1);
        }
    }
}