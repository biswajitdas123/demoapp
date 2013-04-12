using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;
using System.Text;


namespace Bsnsapp
{
    public class RavenDBNinjectModule: NinjectModule
    {
        public override void Load()
        {          
            Bind<IDocumentStore>().ToMethod(context =>
            {

                using (var d1 = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    UseEmbeddedHttpServer = false
                }.Initialize())
                using (var d2 = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    UseEmbeddedHttpServer = false
                    
                }.Initialize())
                    NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(9e81dc47-f831-4d8a-b70b-d3b5c6a8b4ce);
                var documentstore = new EmbeddableDocumentStore { 
                   DataDirectory = "App_Data", 
                   UseEmbeddedHttpServer = true };
                return documentstore.Initialize();
            }).InSingletonScope();
            Bind<IDocumentSession>().ToMethod(Context => Context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }

    }
}
