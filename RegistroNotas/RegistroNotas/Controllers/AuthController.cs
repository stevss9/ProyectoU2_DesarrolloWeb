using System;
using System.Linq;
using System.Web.Mvc;
using RegistroNotas.Models;

namespace RegistroNotas.Controllers
{
    public class AuthController : Controller
    {
        // Aqui creamo el método que maneja la solicitud inicial de la página de inicio de sesión
        public ActionResult Login()
        {
            // Si el usuario ya ha iniciado sesión (determinado por Session["State"]),
            // redirige al usuario a la página de inicio (Home)
            if (Session["State"] != null && Session["State"].ToString() == "Logged")
            {
                return RedirectToAction("Index", "Home");
            }

            // De lo contrario, devuelve la vista de inicio de sesión para mostrar el formulario de inicio de sesión
            return View("Login");
        }

        // Método que maneja el envío del formulario de inicio de sesión mediante una solicitud POST
        [HttpPost]
        public ActionResult LoginMethod(string username, string password)
        {
            try
            {
                // Utilizando un contexto de base de datos para consultar las credenciales del usuario en la base de datos
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Verifica si el usuario con el nombre de usuario y contraseña proporcionados existe en la base de datos
                    var oUser = (from d in db.users where d.name == username && d.password == password select d).FirstOrDefault();

                    // Si el usuario no existe, devuelve un mensaje de error
                    if (oUser == null)
                    {
                        ViewBag.Error = "Usuario o contraseña incorrectos";

                        // Devuelve una respuesta JSON que indica el fallo del inicio de sesión
                        return Json(new { success = false, message = "Usuario o contraseña incorrectos" }, JsonRequestBehavior.AllowGet);
                    }

                    // Si el usuario existe, almacena sus detalles en la sesión
                    Session["State"] = "Logged"; // Indica que el usuario ha iniciado sesión
                    Session["User"] = oUser.name; // Almacena el nombre del usuario en la sesión
                    Session["Role"] = oUser.id_role; // Almacena el ID del rol del usuario en la sesión
                    Session["Id"] = oUser.id; // Almacena el ID del usuario en la sesión
                    Session["Id_Student"] = oUser.id_student; // Almacena el ID del estudiante asociado al usuario en la sesión

                    // Devuelve una respuesta JSON que indica el éxito del inicio de sesión
                    return Json(new { success = true, message = "Login Correcto" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante el proceso de inicio de sesión, devuelve un mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que maneja el proceso de cierre de sesión del usuario
        public ActionResult Logout()
        {
            // Borra todas las variables de sesión relacionadas con el estado de inicio de sesión del usuario
            Session["State"] = null;
            Session["User"] = null;
            Session["Role"] = null;
            Session["Id"] = null;

            // Redirige al usuario de nuevo a la página de inicio de sesión después del cierre de sesión
            return RedirectToAction("Login");
        }
    }
}