using EmployeesManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace EmployeesManagementSystem.DataAccess
{
    public class DbInitializer
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager,
                                           RoleManager<IdentityRole> roleManager)
        {
            // ✅ إنشاء Roles
            string[] roles = new[] { "Admin", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ✅ إنشاء Admin
            string adminEmail = "admin@test.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "System Admin",
                    JoinDate = DateTime.UtcNow
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ✅ إنشاء موظف تجريبي
            string employeeEmail = "employee@test.com";
            var employee = await userManager.FindByEmailAsync(employeeEmail);
            if (employee == null)
            {
                employee = new ApplicationUser
                {
                    UserName = "employee",
                    Email = employeeEmail,
                    FullName = "Test Employee",
                    JoinDate = DateTime.UtcNow
                };
                await userManager.CreateAsync(employee, "Employee@123");
                await userManager.AddToRoleAsync(employee, "Employee");
            }
        }
    }
}
