using System.ComponentModel.DataAnnotations;
using EmptyStringGuard;
using NullGuard;
using ValidationFlags = NullGuard.ValidationFlags;

namespace CroquetAustralia.Domain.Features.TournamentEntry.Models
{
    [NullGuard(ValidationFlags.None)]
    [EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class Player
    {
        [Required(ErrorMessage = "Your first name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Your email is required."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Your phone is required.")]
        public string Phone { get; set; }

        // See SubmitEntry for handicap validation
        public decimal? Handicap { get; set; }

        public bool Under21 { get; set; }

        public bool FullTimeStudentUnder25 { get; set; }
    }
}