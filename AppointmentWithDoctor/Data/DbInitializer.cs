using AppointmentScheduler.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Data
{
    public class DbInitializer : IDbIntializer
    {
        private readonly ApplicationDbContext dbcontext;
        public UserManager<ApplicationUser> usermanager { get; }
        public RoleManager<IdentityRole> rolemanager { get; }

        public DbInitializer(ApplicationDbContext _dbcontext,UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager) 
        {
            dbcontext = _dbcontext;
            rolemanager = _roleManager;
            usermanager = _userManager;
        }


        public void Intialize()
        {
            try
            {
                if(dbcontext.Database.GetPendingMigrations().Count() > 0)
                {
                    dbcontext.Database.Migrate();
                }
            }catch (Exception ex)
            {

            }
            if (dbcontext.Roles.Any(x => x.Name == Utilities.Helper.Admin)) return;

                rolemanager.CreateAsync(new IdentityRole(Utilities.Helper.Admin)).GetAwaiter().GetResult();
                rolemanager.CreateAsync(new IdentityRole(Utilities.Helper.Patient)).GetAwaiter().GetResult();
                rolemanager.CreateAsync(new IdentityRole(Utilities.Helper.Doctor)).GetAwaiter().GetResult();

            usermanager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gamil.com",
                Email = "admin@gamil.com",
                EmailConfirmed = true,
                Name = "Asif Ali"
            }, "Asd123@").GetAwaiter().GetResult();
            ApplicationUser user = dbcontext.Users.FirstOrDefault(x => x.Email == "admin@gamil.com");
            usermanager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
