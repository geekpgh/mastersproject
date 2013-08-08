[assembly: WebActivator.PreApplicationStartMethod(typeof(BrewersBuddy.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(BrewersBuddy.App_Start.NinjectWebCommon), "Stop")]

namespace BrewersBuddy.App_Start
{
    using BrewersBuddy.Models;
    using BrewersBuddy.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<BrewersBuddyContext>().ToSelf().InRequestScope();
            kernel.Bind<IBatchService>().To<BatchService>().InRequestScope();
            kernel.Bind<IBatchRatingService>().To<BatchRatingService>().InRequestScope();
            kernel.Bind<IBatchNoteService>().To<BatchNoteService>().InRequestScope();
            kernel.Bind<IBatchActionService>().To<BatchActionService>().InRequestScope();
            kernel.Bind<IMeasurementService>().To<MeasurementService>().InRequestScope();
            kernel.Bind<IRecipeService>().To<RecipeService>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>().InRequestScope();
            kernel.Bind<IContainerService>().To<ContainerService>().InRequestScope();
            kernel.Bind<IBatchCommentService>().To<BatchCommentService>().InRequestScope();
        }
    }
}
