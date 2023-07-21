using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class CourseController : Controller
    {
        // Método que muestra la vista principal de cursos y profesores
        public ActionResult Index()
        {
            // Comprueba si el usuario ha iniciado sesión
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de profesores desde la base de datos
                    List<TeacherListView> lstTeachers = (from d in db.teachers
                                                         select new TeacherListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             last_name = d.last_name,
                                                             specialty = d.specialty,
                                                             email = d.email
                                                         }).ToList();
                    // Obtiene la lista de cursos desde la base de datos
                    List<CourseListView> lstCourses = (from d in db.cours
                                                       select new CourseListView
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           id_teacher = d.id_teacher
                                                       }).ToList();

                    // Crea un objeto CourseGeneral que contiene las listas de profesores y cursos
                    CourseGeneral courseGeneral = new CourseGeneral
                    {
                        TeacherListViews = lstTeachers,
                        CourseListViews = lstCourses
                    };

                    // Devuelve la vista con el objeto CourseGeneral como modelo
                    return View(courseGeneral);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }

        // Método que crea un nuevo curso mediante una solicitud POST
        [HttpPost]
        public ActionResult CreateCourse(CourseListView courseListView)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la creación de cursos
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Crea un nuevo objeto 'cours' y asigna los valores del modelo recibidos desde el formulario
                    cours course = new cours();
                    course.id = courseListView.id;
                    course.name = courseListView.name;
                    course.id_teacher = courseListView.id_teacher;

                    // Agrega el curso a la base de datos y guarda los cambios
                    db.cours.Add(course);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Curso creado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la creación del curso, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que obtiene los detalles de un curso mediante una solicitud POST
        [HttpPost]
        public ActionResult GetCourse(int id)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir obtener detalles de cursos
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el curso con el ID especificado en la base de datos
                    var course = db.cours.Find(id);
                    CourseListView courseListView = new CourseListView();
                    courseListView.id = course.id;
                    courseListView.name = course.name;
                    courseListView.id_teacher = course.id_teacher;

                    // Devuelve una respuesta JSON con el éxito de la operación y los detalles del curso
                    return Json(new { success = true, data = courseListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la obtención de los detalles del curso, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que actualiza un curso existente mediante una solicitud POST
        [HttpPost]
        public ActionResult UpdateCourse(CourseListView courseListView)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la actualización de cursos
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el curso con el ID especificado en la base de datos
                    var course = db.cours.Find(courseListView.id);
                    course.id = courseListView.id;
                    course.name = courseListView.name;
                    course.id_teacher = courseListView.id_teacher;

                    // Marca el curso como modificado y guarda los cambios en la base de datos
                    db.Entry(course).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Curso actualizado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la actualización del curso, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que elimina un curso mediante una solicitud POST
        [HttpPost]
        public ActionResult DeleteCourse(int id)
        {
            // Verifica si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                // Si no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
                return RedirectToAction("Login", "Auth");
            }

            // Verifica si el usuario tiene el rol de administrador (rol "1") para permitir la eliminación de cursos
            if (Session["Role"] == null || Session["Role"].ToString() != "1")
            {
                // Si el usuario no tiene el rol de administrador, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca el curso con el ID especificado en la base de datos
                    var course = db.cours.Find(id);

                    // Remueve el curso de la base de datos y guarda los cambios
                    db.cours.Remove(course);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Curso eliminado correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la eliminación del curso, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
