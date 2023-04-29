using DesertCamel.BaseMicroservices.SuperBootstrap.Base;
using DesertCamel.BaseMicroservices.SuperIdentity.Extensions;
using DesertCamel.BaseMicroservices.SuperIdentity.Models;
using DesertCamel.BaseMicroservices.SuperIdentity.Models.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientAuthorityService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ClientService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.OauthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.PermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.ResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RolePermissionService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleResourceService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.RoleService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserAuthService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserPoolService;
using DesertCamel.BaseMicroservices.SuperIdentity.Services.UserService;
using Serilog;

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
            services.AddScoped<IRoleResourceService, RoleResourceService>();
            services.AddScoped<IClientAuthorityService, ClientAuthorityService>();
            services.AddScoped<IOauthService, OauthService>();
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.Configure<ClientConfig>(Configuration.GetSection(ClientConfig.ClientConfigSection));

            services.AddBootstrapBase(Configuration);
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
            app.UseCors();
            app.UseAuthorization();

            app.UseBootstrapBase();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
