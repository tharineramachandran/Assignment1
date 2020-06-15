using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Task6.Models
{
  	public class StripeChargeModel
	{
		[Required]
		public string Token { get; set; }



		[Required]
		public double Amount { get; set; }



		// These fields are optional and are not
		// required for the creation of the token
		public string CardHolderName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string AddressCity { get; set; }
		public string AddressPostcode { get; set; }
		public string AddressCountry { get; set; }

		public string Expyear { get; set; }
		public string ExpMonth { get; set; }
		public string CVV { get; set; }
		public string CardNum { get; set; }

		[Display(Name = "Email address")]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]

		[Required(ErrorMessage = "The email address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
 		public string Email { get; set; }





	}
}