using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace QuizMaker.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            
        }
    }
}
