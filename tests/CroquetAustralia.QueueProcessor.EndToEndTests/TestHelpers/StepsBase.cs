namespace CroquetAustralia.QueueProcessor.EndToEndTests.TestHelpers
{
    public abstract class StepsBase
    {
        protected readonly Actual Actual;
        protected readonly Given Given;

        protected StepsBase(Given given, Actual actual)
        {
            Given = given;
            Actual = actual;
        }
    }
}