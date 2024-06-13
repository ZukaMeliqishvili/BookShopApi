using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookShopMVC.Models.User
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotNull]
        [MinLength(8)]
        [MaxLength(250)]
        [RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$")]
        public string Email { get; set; }

        [NotNull]
        [RegularExpression("^5\\d{8}$")]
        public string PhoneNumber { get; set; }

        [NotNull]
        [MinLength(10)]
        [MaxLength(250)]
        public string Address { get; set; }
    }
}
