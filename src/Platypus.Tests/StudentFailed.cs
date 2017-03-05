using Platypus.Event;

namespace Platypus.Tests
{
    public class StudentFailed : IEvent
    {
        public int Version { get; }
        public decimal GPA { get; }

        public StudentFailed(int version, decimal gpa)
        {
            Version = version;
            GPA = gpa;
        }
    }
}