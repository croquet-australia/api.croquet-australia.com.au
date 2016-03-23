using ApprovalTests.Core;

namespace CroquetAustralia.QueueProcessor.Specifications.TestHelpers
{
    public class ApprovalNamer : IApprovalNamer
    {
        public ApprovalNamer(string sourcePath, string fileName)
        {
            SourcePath = sourcePath;
            Name = fileName;
        }

        public string SourcePath { get; }
        public string Name { get; }
    }
}