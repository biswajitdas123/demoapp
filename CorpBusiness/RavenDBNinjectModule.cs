﻿using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;
using System.Text;


namespace CorpBusiness
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
                    NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(ac887293-c89f-4475-a597-e5c73deba807);
                var documentstore = new EmbeddableDocumentStore { DataDirectory = "App_Data", UseEmbeddedHttpServer = true };
                return documentstore.Initialize();
            }).InSingletonScope();
            Bind<IDocumentSession>().ToMethod(Context => Context.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }

    }
}
