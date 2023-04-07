using DesertCamel.BaseMicroservices.SuperIdentity.Extensions;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.PermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService;

namespace DesertCamel.BaseMicroservices.SuperIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSuperIdentityDbContext(Configuration);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserPoolService, UserPoolService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IRolePermissionService, RolePermissionService>();

            services.Configure<ClientConfig>(Configuration.GetSection(ClientConfig.ClientConfigSection));
            
            services.AddOptions();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.RunSuperIdentityDbMigration(Configuration);

            app.UseRouting();
            app.UseSuperCognitoCorsPolicy();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
