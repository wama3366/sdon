using Microsoft.AspNetCore.Identity;

namespace IdentityAndAccess.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string SubjectId => Id;
}