using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Summary.Business;
using Summary.Data.Infrastructure;
using Summary.Data.Repositories;
using Summary.Model;
using Summary.Model.Models;
using Summary.WebApi.Infrastructure.Extention;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Summary.WebApi.App_Start.Startup))]

namespace Summary.WebApi.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            ConfigDI(app);
            ConfigureAuth(app);
        }

        public void ConfigDI(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // DI
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerDependency();

            builder.RegisterType<SummaryDbContext>().AsSelf().InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(PostCategoryRepository).Assembly)
                    .Where(x => x.Name.EndsWith("Repository"))
                    .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(PostCategoryBusiness).Assembly)
                .Where(x => x.Name.EndsWith("Business"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Identity
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            
            builder.RegisterType<EmailService>().As<IIdentityMessageService>().InstancePerRequest();
            //builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Mapper
            var config = MapperConfig.Config();
            IMapper mapper = config.CreateMapper();

            builder.RegisterInstance(mapper);




            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver
        }
    }
}
