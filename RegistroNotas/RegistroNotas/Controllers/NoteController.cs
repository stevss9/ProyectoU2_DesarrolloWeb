using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;
using RegistroNotas.Models.ViewModels;

namespace RegistroNotas.Controllers
{
    public class NoteController : Controller
    {
        // Método que muestra la vista principal para la gestión de notas (ViewNotes)
        public ActionResult Index()
        {
            return View();
        }

        // Método que muestra las notas de un estudiante en específico
        public ActionResult NotesEstudent()
        {
            // Comprueba si el usuario ha iniciado sesión
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                // Obtiene el ID del estudiante desde la sesión
                int id = Convert.ToInt32(Session["Id_Student"]);

                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de notas del estudiante desde base de datos 
                    List<NoteListView> stNotes = (from d in db.notes
                                                  where d.id_student == id
                                                  select new NoteListView
                                                  {
                                                      id = d.id,
                                                      note_1 = d.note_1,
                                                      note_2 = d.note_2,
                                                      note_3 = d.note_3,
                                                      note_4 = d.note_4,
                                                      id_student = d.id_student,
                                                      id_course = d.id_course,
                                                  }).ToList();

                    // Obtiene la lista de cursos desde la base de datos
                    List<CourseListView> courseListViews = (from d in db.cours
                                                            select new CourseListView
                                                            {
                                                                id = d.id,
                                                                name = d.name,
                                                                id_teacher = d.id_teacher,
                                                            }).ToList();

                    // Crea un objeto NoteGeneral que contiene las listas de notas y cursos
                    NoteGeneral noteGeneral = new NoteGeneral
                    {
                        noteListViews = stNotes,
                        courseListViews = courseListViews
                    };

                    // Devuelve la vista "ViewNotes" con el objeto NoteGeneral como modelo
                    return View("ViewNotes", noteGeneral);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }

        // Método que obtiene detalles de una nota por su ID mediante una solicitud POST
        [HttpPost]
        public ActionResult GetNoteById(int id)
        {
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                try
                {
                    // Obtiene la nota por su ID desde la base de datos
                    using (registro_notasEntities1 db = new registro_notasEntities1())
                    {
                        NoteListView noteListView = (from d in db.notes
                                                     where d.id == id
                                                     select new NoteListView
                                                     {
                                                         id = d.id,
                                                         note_1 = d.note_1,
                                                         note_2 = d.note_2,
                                                         note_3 = d.note_3,
                                                         note_4 = d.note_4,
                                                         id_student = d.id_student,
                                                         id_course = d.id_course,
                                                     }).FirstOrDefault();

                        // Obtiene el curso asociado a la nota por su ID
                        CourseListView courseListView = (from d in db.cours
                                                         where d.id == noteListView.id_course
                                                         select new CourseListView
                                                         {
                                                             id = d.id,
                                                             name = d.name,
                                                             id_teacher = d.id_teacher,
                                                         }).FirstOrDefault();

                        // Obtiene el profesor asociado al curso por su ID
                        TeacherListView teacherListView = (from d in db.teachers
                                                           where d.id == courseListView.id_teacher
                                                           select new TeacherListView
                                                           {
                                                               id = d.id,
                                                               name = d.name,
                                                               last_name = d.last_name,
                                                           }).FirstOrDefault();

                        // Obtiene la materia asociada al profesor por su ID
                        SubjectListView subjectListView = (from d in db.subjects
                                                           where d.id_teacher == teacherListView.id
                                                           select new SubjectListView
                                                           {
                                                               id = d.id,
                                                               name = d.name,
                                                               id_teacher = d.id_teacher,
                                                           }).FirstOrDefault();

                        // Devuelve una respuesta JSON con los detalles de la nota, el curso, el profesor y la materia asociada
                        return Json(new { success = true, data = noteListView, course = courseListView, teacher = teacherListView, subject = subjectListView }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    // Si ocurre una excepción durante la obtención de los detalles de la nota, devuelve una respuesta JSON con el mensaje de error
                    return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }

        // Método que muestra las notas de todos los estudiantes para la gestión de notas por parte de profesores
        public ActionResult NotesTeacher()
        {
            // Comprueba si el usuario ha iniciado sesión
            if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la lista de todas las notas desde la base de datos
                    List<NoteListView> stNotes = (from d in db.notes
                                                  select new NoteListView
                                                  {
                                                      id = d.id,
                                                      note_1 = d.note_1,
                                                      note_2 = d.note_2,
                                                      note_3 = d.note_3,
                                                      note_4 = d.note_4,
                                                      id_student = d.id_student,
                                                      id_course = d.id_course,
                                                  }).ToList();

                    // Obtiene la lista de todos los estudiantes desde la base de datos
                    List<EstudentListView> lstUsers = (from d in db.estudents
                                                       select new EstudentListView
                                                       {
                                                           id = d.id,
                                                           name = d.name,
                                                           last_name = d.last_name,
                                                       }).ToList();

                    // Obtiene la lista de todos los cursos desde la base de datos
                    List<CourseListView> courseListViews = (from d in db.cours
                                                            select new CourseListView
                                                            {
                                                                id = d.id,
                                                                name = d.name,
                                                                id_teacher = d.id_teacher,
                                                            }).ToList();

                    // Crea un objeto NoteGeneral que contiene las listas de notas, estudiantes y cursos
                    NoteGeneral noteGeneral = new NoteGeneral
                    {
                        estudentListViews = lstUsers,
                        noteListViews = stNotes,
                        courseListViews = courseListViews
                    };

                    // Devuelve la vista "ManagingNotes" con el objeto NoteGeneral como modelo
                    return View("ManagingNotes", noteGeneral);
                }
            }

            // Si el usuario no ha iniciado sesión, redirige al formulario de inicio de sesión (Auth/Login)
            return RedirectToAction("Login", "Auth");
        }

        // Método que obtiene detalles de una nota por su ID mediante una solicitud GET
        public ActionResult GetNote(int id)
        {
            // Comprueba si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            // Comprueba si el usuario tiene el rol de profesor (rol "3") para permitir la obtención de detalles de notas
            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                // Si el usuario no tiene el rol de profesor, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Obtiene la nota por su ID desde la base de datos
                    NoteListView noteListView = (from d in db.notes
                                                 where d.id == id
                                                 select new NoteListView
                                                 {
                                                     id = d.id,
                                                     note_1 = d.note_1,
                                                     note_2 = d.note_2,
                                                     note_3 = d.note_3,
                                                     note_4 = d.note_4,
                                                     id_student = d.id_student,
                                                     id_course = d.id_course,
                                                 }).FirstOrDefault();

                    // Devuelve una respuesta JSON con los detalles de la nota
                    return Json(new { success = true, data = noteListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la obtención de los detalles de la nota, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que crea una nueva nota mediante una solicitud POST
        [HttpPost]
        public ActionResult CreateNote(NoteListView noteListView)
        {
            // Comprueba si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            // Comprueba si el usuario tiene el rol de profesor (rol "3") para permitir la creación de notas
            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                // Si el usuario no tiene el rol de profesor, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Crea un objeto "note" con los datos proporcionados en el modelo NoteListView
                    note note = new note();
                    note.note_1 = noteListView.note_1;
                    note.note_2 = noteListView.note_2;
                    note.note_3 = noteListView.note_3;
                    note.note_4 = noteListView.note_4;
                    note.id_student = noteListView.id_student;
                    note.id_course = noteListView.id_course;

                    // Agrega la nueva nota a la base de datos y guarda los cambios
                    db.notes.Add(note);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Nota creada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la creación de la nota, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que actualiza una nota existente mediante una solicitud POST
        [HttpPost]
        public ActionResult UpdateNote(NoteListView noteListView)
        {
            // Comprueba si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            // Comprueba si el usuario tiene el rol de profesor (rol "3") para permitir la actualización de notas
            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                // Si el usuario no tiene el rol de profesor, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca la nota por su ID en la base de datos
                    note note = db.notes.Find(noteListView.id);

                    // Actualiza los datos de la nota con los proporcionados en el modelo NoteListView
                    note.note_1 = noteListView.note_1;
                    note.note_2 = noteListView.note_2;
                    note.note_3 = noteListView.note_3;
                    note.note_4 = noteListView.note_4;
                    note.id_student = noteListView.id_student;
                    note.id_course = noteListView.id_course;

                    // Establece el estado de la nota como modificado y guarda los cambios en la base de datos
                    db.Entry(note).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Nota editada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la actualización de la nota, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método que elimina una nota por su ID mediante una solicitud POST
        [HttpPost]
        public ActionResult DeleteNote(int id)
        {
            // Comprueba si el usuario ha iniciado sesión
            if (Session["State"] == null || Session["State"].ToString() != "Logged")
            {
                return RedirectToAction("Login", "Auth");
            }

            // Comprueba si el usuario tiene el rol de profesor (rol "3") para permitir la eliminación de notas
            if (Session["Role"] == null || Session["Role"].ToString() != "3")
            {
                // Si el usuario no tiene el rol de profesor, redirige a la página de inicio (Home)
                return RedirectToAction("Index", "Home");
            }

            try
            {
                using (registro_notasEntities1 db = new registro_notasEntities1())
                {
                    // Busca la nota por su ID en la base de datos
                    note note = db.notes.Find(id);

                    // Elimina la nota de la base de datos y guarda los cambios
                    db.notes.Remove(note);
                    db.SaveChanges();
                }

                // Devuelve una respuesta JSON con el éxito de la operación
                return Json(new { success = true, message = "Nota eliminada correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción durante la eliminación de la nota, devuelve una respuesta JSON con el mensaje de error
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
