//importar las librerias
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class EstudentController : Controller
    {
        // Método que muestra la lista de estudiantes
        public ActionResult Index()
        {
            // Comprobar si el usuario ha iniciado sesión
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de estudiantes desde la base de datos
                    List<EstudentListView> lstEstudent = (from d in db.estudents
                                                          select new EstudentListView
                                                          {
                                                              id = d.id,
                                                              name = d.name,
                                                              last_name = d.last_name,
                                                              dni = d.dni,
                                                              email = d.email
                                                          }).ToList();

                    // Devuelve la vista con la lista de estudiantes como modelo
                    return View(lstEstudent);
                }
            }

            // Si el usuario no ha iniciado sesión, se redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }

        // Método que crea un nuevo estudiante mediante una solicitud POST
        [HttpPost]
        public ActionResult CreateEstudent(EstudentListView estudentListView)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la creación de estudiantes
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Crea un nuevo objeto 'estudent' y asigna los valores del modelo recibidos desde el formulario
                    var oEstudent = new estudent();
                    oEstudent.name = estudentListView.name;
                    oEstudent.last_name = estudentListView.last_name;
                    oEstudent.dni = estudentListView.dni;
                    oEstudent.email = estudentListView.email;

                    // Agrega el estudiante a la base de datos y guarda los cambios
                    db.estudents.Add(oEstudent);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Guardado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la creación del estudiante, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que obtiene los detalles de un estudiante mediante una solicitud POST
        [HttpPost]
        public ActionResult GetEstudent(int id)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir obtener detalles de estudiantes
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el estudiante con el ID especificado en la base de datos
                    var oEstudent = db.estudents.Find(id);
                    EstudentListView estudentListView = new EstudentListView();
                    estudentListView.id = oEstudent.id;
                    estudentListView.name = oEstudent.name;
                    estudentListView.last_name = oEstudent.last_name;
                    estudentListView.dni = oEstudent.dni;
                    estudentListView.email = oEstudent.email;

                    // Devuelve una respuesta JSON con el éxito de la operación y los detalles del estudiante
                    return Json(new { success = true, data = estudentListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la obtención de los detalles del estudiante, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que actualiza un estudiante existente mediante una solicitud POST
        [HttpPost]
        public ActionResult UpdateEstudent(EstudentListView estudentListView)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la actualización de estudiantes
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el estudiante con el ID especificado en la base de datos
                    var oEstudent = db.estudents.Find(estudentListView.id);
                    oEstudent.name = estudentListView.name;
                    oEstudent.last_name = estudentListView.last_name;
                    oEstudent.dni = estudentListView.dni;
                    oEstudent.email = estudentListView.email;

                    // Marca el estudiante como modificado y guarda los cambios en la base de datos
                    db.Entry(oEstudent).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Actualizado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la actualización del estudiante, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que elimina un estudiante mediante una solicitud POST
        [HttpPost]
        public ActionResult DeleteEstudent(int id)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la eliminación de estudiantes
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el estudiante con el ID especificado en la base de datos
                    var oEstudent = db.estudents.Find(id);

                    // Remueve el estudiante de la base de datos y guarda los cambios
                    db.estudents.Remove(oEstudent);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Eliminado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la eliminación del estudiante, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
