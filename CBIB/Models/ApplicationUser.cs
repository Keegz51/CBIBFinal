﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CBIB.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public long AuthorID { get; set; }

    }
}
