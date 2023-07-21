using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RegistroNotas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}",
                defaults: new {controller="Auth", action="Login"}
            );

            routes.MapRoute(
                name: "Register",
                url: "{controller}/{action}",
                defaults: new { controller = "Auth", action = "Register" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "{controller}/{action}",
                defaults: new { controller = "Auth", action = "Logout" }
            );


            routes.MapRoute(
                name: "Estudent",
                url: "{controller}/{action}",
                defaults: new { controller = "Estudent", action = "Index" }
            );

            routes.MapRoute(
                name: "Teacher",
                url: "{controller}/{action}",
                defaults: new { controller = "Teacher", action = "Index" }
            );

            routes.MapRoute(
                name: "User",
                url: "{controller}/{action}",
                defaults: new { controller = "User", action = "Index" }
            );

            routes.MapRoute(
               name: "Subject",
               url: "{controller}/{action}",
               defaults: new { controller = "Subject", action = "Index" }
           );

            routes.MapRoute(
               name: "Course",
               url: "{controller}/{action}",
               defaults: new { controller = "Course", action = "Index" }
           );

            routes.MapRoute(
               name: "Notes",
               url: "{controller}/{action}",
               defaults: new { controller = "Note", action = "NotesTeacher" }
           );


        }
    }
}
