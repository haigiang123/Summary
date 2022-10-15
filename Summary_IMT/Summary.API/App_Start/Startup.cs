using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Summary.API.Infrastructure.Extention;
using Summary.Business;
using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary.Model;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Summary.API.App_Start.Startup))]

namespace Summary.API.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigBuilder(app);
            ConfigureAuth(app);
        }

        public void ConfigBuilder(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<SummaryDbContext>().AsSelf().InstancePerRequest();

            // register for Identity
            //builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            //builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(PostCategoryRepository).Assembly)
                    .Where(x=>x.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(PostCategoryBusiness).Assembly)
                .Where(x => x.Name.EndsWith("Business"))
                .AsImplementedInterfaces().InstancePerRequest();

            var config = MapperConfig.Config();
            IMapper mapper = config.CreateMapper();

            builder.RegisterInstance(mapper);


            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver

            
        }
    }
}
