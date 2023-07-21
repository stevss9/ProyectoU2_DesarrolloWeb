using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class SubjectController : Controller
    {
        // Acción que muestra la página principal de administración de cursos/asignaturas.
        public ActionResult Index()
        {
            // Verifica si el usuario ha iniciado sesión (estado "Logged").
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de profesores (Teachers) desde la base de datos.
                    List<TeacherListView> lstTeachers = (from d in db.teachers
                                                         select new TeacherListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             last_name = d.last_name,
                                                             specialty = d.specialty,
                                                             email = d.email
                                                         }).ToList();

                    // Obtiene la lista de asignaturas (Subjects) desde la base de datos.
                    List<SubjectListView> lstCourses = (from d in db.subjects
                                                        select new SubjectListView
                                                        {
                                                            id = d.id,
                                                            name = d.name,
                                                            description = d.description,
                                                            id_teacher = d.id_teacher
                                                        }).ToList();

                    // Crea un objeto "SubjectGeneral" que contiene ambas listas para pasar a la vista.
                    SubjectGeneral subjectGeneral = new SubjectGeneral
                    {
                        teacherListViews = lstTeachers,
                        subjectListViews = lstCourses
                    };

                    // Devuelve la vista "Index" con el objeto "subjectGeneral" como modelo.
                    return View(subjectGeneral);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
            return RedirectToAction("Login", "Auth");
        }

        // Acción para crear un nuevo curso/asignatura.
        [HttpPost]
        public ActionResult CreateSubject(SubjectListView subjectListView)
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
                    // Crea un nuevo objeto "subject" con los datos proporcionados en el objeto "subjectListView".
                    subject subject = new subject();
                    subject.id = subjectListView.id;
                    subject.name = subjectListView.name;
                    subject.description = subjectListView.description;
                    subject.id_teacher = subjectListView.id_teacher;

                    // Agrega el nuevo objeto "subject" a la base de datos y guarda los cambios con EF.
                    db.subjects.Add(subject);
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Se ha creado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para obtener los detalles de un curso/asignatura por su ID.
        [HttpPost]
        public ActionResult GetSubject(int id)
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
                    // Obtiene los detalles del curso/asignatura que coincide con el ID proporcionado desde la base de datos.
                    SubjectListView subjectListView = (from d in db.subjects
                                                       where d.id == id
                                                       select new SubjectListView
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           description = d.description,
                                                           id_teacher = d.id_teacher
                                                       }).FirstOrDefault();

                    // Devuelve un objeto JSON con un indicador de éxito y los datos del curso/asignatura.
                    return Json(new { success = true, data = subjectListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para actualizar los datos de un curso/asignatura.
        [HttpPost]
        public ActionResult UpdateSubject(SubjectListView subjectListView)
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
                    // Busca el curso/asignatura en la base de datos por su ID y lo actualiza con los nuevos datos proporcionados.
                    subject subject = db.subjects.Find(subjectListView.id);
                    subject.name = subjectListView.name;
                    subject.description = subjectListView.description;
                    subject.id_teacher = subjectListView.id_teacher;

                    // Establece el estado del objeto "subject" como modificado y guarda los cambios con EF.
                    db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Se ha actualizado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para eliminar un curso/asignatura por su ID.
        [HttpPost]
        public ActionResult DeleteSubject(int id)
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
                    // Busca el curso/asignatura en la base de datos por su ID y lo elimina.
                    subject subject = db.subjects.Find(id);
                    db.subjects.Remove(subject);
                    db.SaveChanges();
                }

                // Devuelve un objeto JSON con un indicador de éxito y un mensaje de éxito.
                return Json(new { success = true, message = "Se ha eliminado el curso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
