﻿using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FullName { get; set; }
    }
}