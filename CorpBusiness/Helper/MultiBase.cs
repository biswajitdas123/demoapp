using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Web.Helpers;

namespace CorpBusiness.Helper
{
    public abstract class MultiBase :Controller
    {
       
        protected override void ExecuteCore()
        {
            string CultureName = null;
            string Language = null;
            HttpCookie CultureCookie = Request.Cookies["Arsenal"];
            if (CultureCookie != null)
            {
                Language = CultureCookie.Value;
                switch (Language)
                {
                    case "English":
                        CultureName = "en-us";
                        break;
                    case "French":
                        CultureName = "fr";
                        break;

                }

            }
            else
            {
                CultureName = "en-us";
            }
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            base.ExecuteCore();
        }
    }
}