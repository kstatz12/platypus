using Platypus.Domain;

namespace Platypus.Tests
{
    public class TestAggregate : AggregateRoot
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal GPA { get; set; }
        public bool IsPassing { get; set; }
    }
}