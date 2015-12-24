namespace CroquetAustralia.QueueProcessor.Email
{
    public class EmailAddress
    {
        public EmailAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }
        public string Name { get; }
    }
}