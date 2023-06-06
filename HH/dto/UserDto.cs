using System;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HH.dto
{
	public class UserDto
	{

        [BindNever]
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [BindNever]
        public double? Asset { get; set; } //总资产
        [BindNever]
        public string? Token { get; set; }
        public UserDto()
		{
		}
	}
}

