using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeamA.CustomerAccounts.Models.ViewModels
{
    public class UpdateUserVm
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "First Name is required"), MinLength(2, ErrorMessage = "Invalid first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), MinLength(2, ErrorMessage = "Invalid last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required"), MinLength(5, ErrorMessage = "Invalid Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Postcode is required"), DataType(DataType.PostalCode)]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Email is required"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required"), DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
