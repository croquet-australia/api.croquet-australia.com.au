using ApprovalTests.Core;
using ApprovalTests.Namers;

namespace CroquetAustralia.QueueProcessor.Tests.TestHelpers
{
    public class ApprovalNamer : IApprovalNamer
    {
        public ApprovalNamer(string fileName) : this(new UnitTestFrameworkNamer().SourcePath, fileName)
        {
        }

        public ApprovalNamer(string sourcePath, string fileName)
        {
            SourcePath = sourcePath;
            Name = fileName;
        }

        public string SourcePath { get; }
        public string Name { get; }
    }
}