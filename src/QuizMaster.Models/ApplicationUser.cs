using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace QuizMaster.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            
        }
    }
}
