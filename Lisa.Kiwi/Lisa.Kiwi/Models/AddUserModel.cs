﻿using System.ComponentModel.DataAnnotations;

namespace Lisa.Kiwi.WebApi.Models
{
	public class AddUserModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[StringLength(
			100,
			// TODO: Dutch message.
			ErrorMessage = "PASSWORD_STRINGLENGTH"/*"The {0} must be at least {2} characters long."*/,
			MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }
	}
}