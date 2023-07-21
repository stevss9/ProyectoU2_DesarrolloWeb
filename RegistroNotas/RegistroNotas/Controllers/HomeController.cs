using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistroNotas.Controllers
{
    public class HomeController : Controller
    {
        // Método que muestra la vista principal del sitio web (página de inicio)
        public ActionResult Index()
        {
            // Comprueba si el usuario ha iniciado sesión
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                // Si el usuario ha iniciado sesión, muestra la vista principal del sitio web (View)
                return View();
            }

            // Si el usuario no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }
    }
}
