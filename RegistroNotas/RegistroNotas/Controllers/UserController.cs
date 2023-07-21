using RegistroNotas.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistroNotas.Models;

namespace RegistroNotas.Controllers
{
    public class UserController : Controller
    {
        // Acción que muestra la lista de usuarios (que incluye profesores y estudiantes) en la página principal de administración de usuarios.
        public ActionResult Index()
        {
            // Utiliza "using" para asegurar que se liberan los recursos correctamente después de la operación.
            using (registro_notasEntities1 db = new registro_notasEntities1())
            {
                // Verifica si el usuario ha iniciado sesión (estado "Logged").
                if ((Session["State"] != null && Session["State"].ToString() == "Logged"))
                {
                    // Obtiene la lista de usuarios desde la base de datos y la convierte en una lista de UserListView.
                    List<UserListView> lstUsers = (from d in db.users
                                                   select new UserListView
                                                   {
                                                       id = d.id,
                                                       name = d.name,
                                                       password = d.password,
                                                       id_role = d.id_role,
                                                       id_student = d.id_student,
                                                       id_teacher = d.id_teacher
                                                   }).ToList();

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

                    // Obtiene la lista de estudiantes desde la base de datos y la convierte en una lista de EstudentListView.
                    List<EstudentListView> lstEstudent = (from d in db.estudents
                                                          select new EstudentListView
                                                          {
                                                              id = d.id,
                                                              name = d.name,
                                                              last_name = d.last_name,
                                                              dni = d.dni,
                                                              email = d.email
                                                          }).ToList();

                    // Crea un objeto UserGeneraView que contiene las listas de usuarios, profesores y estudiantes.
                    UserGeneraView userGeneraView = new UserGeneraView
                    {
                        TeacherListViews = lstTeachers,
                        EstudentListViews = lstEstudent,
                        UserListViews = lstUsers

                    };

                    // Devuelve la vista "Index" con el objeto UserGeneraView como modelo.
                    return View(userGeneraView);
                }

                // Si el usuario no ha iniciado sesión, redirige a la acción "Login" del controlador "Auth".
                return RedirectToAction("Login", "Auth");
            }

        }

        // Acción para crear un nuevo usuario (que puede ser un profesor o un estudiante).
        [HttpPost]
        public ActionResult CreateUser(UserListView userListView)
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
                    // Crea un nuevo objeto "user" con los datos proporcionados en el objeto "userListView".
                    var oUser = new user();
                    oUser.name = userListView.name;
                    oUser.password = userListView.password;
                    oUser.id_role = userListView.id_role;
                    oUser.id_student = userListView.id_student;
                    oUser.id_teacher = userListView.id_teacher;

                    // Agrega el nuevo objeto "user" a la base de datos y guarda los cambios con EF.
                    db.users.Add(oUser);
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

        // Acción para obtener los detalles de un usuario por su ID.
        [HttpPost]
        public ActionResult GetUser(int id)
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
                    // Busca el usuario en la base de datos por su ID y crea un objeto UserListView con los datos del usuario.
                    UserListView userListView = (from d in db.users
                                                 where d.id == id
                                                 select new UserListView
                                                 {
                                                     id = d.id,
                                                     name = d.name,
                                                     password = d.password,
                                                     id_role = (int)d.id_role,
                                                     id_student = (int)d.id_student,
                                                     id_teacher = (int)d.id_teacher
                                                 }).FirstOrDefault();

                    // Devuelve un objeto JSON con un indicador de éxito y los datos del usuario.
                    return Json(new { success = true, data = userListView }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, devuelve un objeto JSON con un indicador de fallo y el mensaje de la excepción.
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Acción para actualizar los datos de un usuario existente.
        [HttpPost]
        public ActionResult UpdateUser(UserListView userListView)
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
                    // Busca el usuario en la base de datos por su ID.
                    var oUser = db.users.Find(userListView.id);

                    // Actualiza los datos del usuario con los datos proporcionados en el objeto "userListView".
                    oUser.name = userListView.name;
                    oUser.password = userListView.password;
                    oUser.id_role = userListView.id_role;
                    oUser.id_student = userListView.id_student;
                    oUser.id_teacher = userListView.id_teacher;

                    // Establece el estado del objeto "oUser" como modificado y guarda los cambios con EF.
                    db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
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

        // Acción para eliminar un usuario por su ID.
        [HttpPost]
        public ActionResult DeleteUser(int id)
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
                    // Busca el usuario en la base de datos por su ID y lo elimina.
                    var oUser = db.users.Find(id);
                    db.users.Remove(oUser);
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
