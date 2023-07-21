using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class TeacherController : Controller
    {
        // Acción que muestra la lista de profesores en la página principal de administración de profesores.
        public ActionResult Index()
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de profesores desde la base de datos y la convierte en una lista de TeacherListView.
                    List<TeacherListView> lstTeachers = (from d in db.teachers
                                                         select new TeacherListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             last_name = d.last_name,
                                                             specialty = d.specialty,
                                                             email = d.email
                                                         }).ToList();

                    // Devuelve la vista "Index" con la lista de profesores como modelo.
                    return View(lstTeachers);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
            return RedirectToAction("Login", "Auth");
        }

        // Acción para crear un nuevo profesor.
        [HttpPost]
        public ActionResult CreateTeacher(TeacherListView teacherListView)
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1").
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la acción "Index" del controlador "Home".
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Crea un nuevo objeto "teacher" con los datos proporcionados en el objeto "teacherListView".
                    var oTeacher = new teacher();
                    oTeacher.name = teacherListView.name;
                    oTeacher.last_name = teacherListView.last_name;
                    oTeacher.specialty = teacherListView.specialty;
                    oTeacher.email = teacherListView.email;

                    // Agrega el nuevo objeto "teacher" a la base de datos y guarda los cambios con EF.
                    db.teachers.Add(oTeacher);
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Guardado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para obtener los detalles de un profesor por su ID.
        [HttpPost]
        public ActionResult GetTeacher(int id)
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1").
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la acción "Index" del controlador "Home".
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el profesor en la base de datos por su ID y crea un objeto TeacherListView con los datos del profesor.
                    var oTeacher = db.teachers.Find(id);
                    TeacherListView teacherListView = new TeacherListView();
                    teacherListView.id = oTeacher.id;
                    teacherListView.name = oTeacher.name;
                    teacherListView.last_name = oTeacher.last_name;
                    teacherListView.specialty = oTeacher.specialty;
                    teacherListView.email = oTeacher.email;

                    // Devuelve un objeto JSON con un indicador de éxito y los datos del profesor.
                    return Json(new { success = true, data = teacherListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para actualizar los datos de un profesor.
        [HttpPost]
        public ActionResult UpdateTeacher(TeacherListView teacherListView)
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1").
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la acción "Index" del controlador "Home".
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el profesor en la base de datos por su ID y actualiza sus datos con los nuevos proporcionados.
                    var oTeacher = db.teachers.Find(teacherListView.id);
                    oTeacher.name = teacherListView.name;
                    oTeacher.last_name = teacherListView.last_name;
                    oTeacher.specialty = teacherListView.specialty;
                    oTeacher.email = teacherListView.email;

                    // Establece el estado del objeto "oTeacher" como modificado y guarda los cambios con EF.
                    db.Entry(oTeacher).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Actualizado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para eliminar un profesor por su ID.
        [HttpPost]
        public ActionResult DeleteTeacher(int id)
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1").
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la acción "Index" del controlador "Home".
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el profesor en la base de datos por su ID y lo elimina.
                    var oTeacher = db.teachers.Find(id);
                    db.teachers.Remove(oTeacher);
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Eliminado Correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
