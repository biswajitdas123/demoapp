﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Activation;
using Ninject.Web.Mvc;
using CorpBusiness.Attributes;
using CorpBusiness.Validators;
using Raven.Client;
using Raven.Abstractions.Data;
using Raven.Client.Document;




namespace CorpBusiness
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
       
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Welcome", action = "Welcome", id = UrlParameter.Optional } // Parameter defaults
            );
           
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(SameAsAttribute), typeof(SameAsValidator));
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            //}

            //-----subrata-------------
            //AreaRegistration.RegisterAllAreas();

            //RegisterGlobalFilters(GlobalFilters.Filters);
            //RegisterRoutes(RouteTable.Routes);

            //var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            //parser.Parse();

            //Store = new DocumentStore
            //{
            //    ApiKey = parser.ConnectionStringOptions.ApiKey,
            //    Url = parser.ConnectionStringOptions.Url,
            //}.Initialize();
        }
       
    }
}