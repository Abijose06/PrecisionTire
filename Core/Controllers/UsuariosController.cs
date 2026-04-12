using Core.Helpers;
using Core.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Core.Controllers
{
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        private GomasContext db = new GomasContext();
        // 1. Declaración del motor de logs para este controlador
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("registro")]
        public IHttpActionResult RegistrarUsuario(RegistroRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Documento))
                return BadRequest("Datos incompletos.");

            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    bool existe = db.Usuarios.Any(u => u.Documento == request.Documento);
                    if (existe)
                    {
                        log.Warn($"Intento de registro duplicado para el documento: {request.Documento}");
                        return BadRequest("Ya existe un usuario registrado con este documento.");
                    }

                    // 1. Crear el Usuario base
                    Usuario nuevoUsuario = new Usuario
                    {
                        TipoDocumento = request.TipoDocumento,
                        Documento = request.Documento,
                        Nombres = request.Nombres,
                        Apellidos = request.Apellidos,
                        Telefono = request.Telefono,
                        Correo = request.Correo,
                        ClaveHash = SeguridadHelper.HashPassword(request.ClaveHash),
                        Rol = request.Rol,
                        Estado = true
                    };

                    db.Usuarios.Add(nuevoUsuario);
                    db.SaveChanges(); // Guarda y genera el IdUsuario

                    string rol = request.Rol.ToLower();

                    // 2. Inteligencia de Creación de Perfil (Cliente vs Empleado)
                    if (rol == "cliente" || rol == "clienteweb")
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO tblCliente (IdUsuario, Estado) VALUES (@p0, 1)", nuevoUsuario.IdUsuario);
                        log.Info($"Perfil de Cliente creado para el Usuario ID: {nuevoUsuario.IdUsuario}");
                    }
                    else if (rol == "cajero" || rol == "administrador")
                    {
                        if (request.Sueldo == null || request.IdSucursal == null)
                        {
                            throw new Exception("Para registrar un empleado, el Sueldo y el IdSucursal son obligatorios.");
                        }

                        string sqlEmpleado = "INSERT INTO tblEmpleado (IdUsuario, Sueldo, FechaIngreso, IdSucursal, Estado) VALUES (@p0, @p1, GETDATE(), @p2, 1)";
                        db.Database.ExecuteSqlCommand(sqlEmpleado, nuevoUsuario.IdUsuario, request.Sueldo, request.IdSucursal);
                        log.Info($"Perfil de Empleado creado para el Usuario ID: {nuevoUsuario.IdUsuario} en Sucursal {request.IdSucursal}");
                    }

                    transaccion.Commit();
                    return Ok(new { Mensaje = "Usuario registrado exitosamente.", IdUsuario = nuevoUsuario.IdUsuario });
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    log.Error("Error crítico al intentar registrar un nuevo usuario.", ex);
                    return InternalServerError(new Exception("Error al registrar el usuario: " + ex.Message));
                }
            }
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginRequest request)
        {
            try
            {
                // 1. Buscamos el usuario por Tipo y Número de documento
                var usuario = db.Usuarios.FirstOrDefault(u =>
                    u.TipoDocumento == request.TipoDocumento &&
                    u.Documento == request.Documento &&
                    u.Estado == true);

                // 2. Si no existe o la clave (hasheada) no coincide, fuera.
                if (usuario == null || !SeguridadHelper.VerificarHash(request.Password, usuario.ClaveHash))
                {
                    return Content(HttpStatusCode.Unauthorized, "Credenciales incorrectas.");
                }

                // 3. Buscamos si es Cliente o Empleado para devolver el perfil completo
                var cliente = db.Clientes.FirstOrDefault(c => c.IdUsuario == usuario.IdUsuario);
                var empleado = db.Database.SqlQuery<EmpleadoLoginDTO>(
                    "SELECT IdEmpleado, IdSucursal FROM tblEmpleado WHERE IdUsuario = @p0",
                    usuario.IdUsuario).FirstOrDefault();

                return Ok(new
                {
                    IdUsuario = usuario.IdUsuario,
                    IdCliente = cliente?.IdCliente,
                    IdEmpleado = empleado?.IdEmpleado,
                    IdSucursal = empleado?.IdSucursal,
                    NombreCompleto = usuario.Nombres + " " + usuario.Apellidos,
                    Rol = usuario.Rol,
                    Token = "TOKEN-SIMULADO-" + Guid.NewGuid().ToString() // Aquí iría un JWT real
                });
            }
            catch (Exception ex)
            {
                log.Error("Error en Login", ex);
                return InternalServerError(new Exception("Error al procesar el inicio de sesión."));
            }
        }

        // Clase auxiliar para el query de empleado
        private class EmpleadoLoginDTO { public int IdEmpleado { get; set; } public int IdSucursal { get; set; } }
        // Ruta: GET api/usuarios/buscar/{documento}
        [HttpGet]
        [Route("buscar/{documento}")]
        public IHttpActionResult BuscarClientePorDocumento(string documento)
        {
            try
            {
                // Hacemos un JOIN para traer los datos del usuario y su IdCliente al mismo tiempo
                string sql = @"
                    SELECT c.IdCliente, u.Nombres, u.Apellidos, u.Documento, u.Telefono 
                    FROM tblCliente c
                    INNER JOIN tblUsuario u ON c.IdUsuario = u.IdUsuario
                    WHERE u.Documento = @p0 AND u.Estado = 1 AND c.Estado = 1";

                var cliente = db.Database.SqlQuery<ClienteBuscadoDTO>(sql, documento).FirstOrDefault();

                if (cliente == null)
                {
                    return NotFound(); // El cajero sabrá que tiene que registrarlo primero
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                log.Error($"Error al buscar cliente con documento {documento}", ex);
                return InternalServerError(new Exception("Error al buscar el cliente."));
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    public class LoginRequest
    {
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Password { get; set; }
    }

    public class ClienteBuscadoDTO
    {
        public int IdCliente { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
    }
    public class DatosEmpleadoDTO
    {
        public int IdEmpleado { get; set; }
        public int? IdSucursal { get; set; }
    }

    public class RegistroRequest
    {
        public int TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
        public string Rol { get; set; }

        // --- Campos exclusivos para cuando se registra un Empleado ---
        public decimal? Sueldo { get; set; }
        public int? IdSucursal { get; set; }
    }
}
