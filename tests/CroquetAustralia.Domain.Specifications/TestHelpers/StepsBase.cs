namespace CroquetAustralia.Domain.Specifications.TestHelpers
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