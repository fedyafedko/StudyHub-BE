using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace StudyHub.DAL.EF;
public class DBInitializer
{
    public static async void Initialize(IApplicationBuilder applicationBuilder)
    {
        var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var roles = new List<string>
        {
            "Admin",
            "Teacher",
            "Student"
        };

        foreach (var role in roles) 
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
