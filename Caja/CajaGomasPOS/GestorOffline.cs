using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace CajaGomasPOS
{

    public static class GestorOffline
    {
        // La ruta donde se guardará el archivo en la misma carpeta del programa
        private static string rutaArchivo = "FacturasPendientes.json";

        // 1. Método para GUARDAR una factura cuando no hay conexión
        public static void GuardarFacturaLocal(object factura)
        {
            List<object> pendientes = LeerFacturasLocales();
            pendientes.Add(factura);

            // Convertimos la lista a JSON y la guardamos en el disco duro
            string json = JsonConvert.SerializeObject(pendientes, Formatting.Indented);
            File.WriteAllText(rutaArchivo, json);
        }

        // 2. Método para LEER las facturas guardadas
        public static List<object> LeerFacturasLocales()
        {
            if (!File.Exists(rutaArchivo))
                return new List<object>(); // Si no hay archivo, devolvemos una lista vacía

            string json = File.ReadAllText(rutaArchivo);
            return JsonConvert.DeserializeObject<List<object>>(json) ?? new List<object>();
        }

        // 3. Método para BORRAR el archivo una vez que se sincronizó todo con éxito
        public static void LimpiarFacturasSincronizadas()
        {
            if (File.Exists(rutaArchivo))
            {
                File.Delete(rutaArchivo);
            }
        }

        // 4. Saber cuántas hay pendientes
        public static int ContarPendientes()
        {
            return LeerFacturasLocales().Count;
        }
        // --- CÓDIGO NUEVO PARA USUARIOS OFFLINE ---

        // Ruta del nuevo archivo de usuarios
        private static string rutaUsuarios = "UsuariosOffline.json";

        // Clase para definir cómo es un usuario local
        public class UsuarioLocal
        {
            public string Documento { get; set; }
            public string Password { get; set; }
            public string Nombres { get; set; }
            public string Rol { get; set; }
        }

        // 1. Método que crea el archivo con 2 usuarios si no existe
        public static void InicializarUsuariosDeEmergencia()
        {
            if (!File.Exists(rutaUsuarios))
            {
                var usuariosBase = new List<UsuarioLocal>
        {
            // Usuario Administrador
            new UsuarioLocal { Documento = "admin", Password = "kali123", Nombres = "Supervisor Local", Rol = "Administrador" },
            
            // Usuario Empleado/Cajero
            new UsuarioLocal { Documento = "cajero", Password = "123", Nombres = "Cajero Local", Rol = "Cajero" }
        };

                // Creamos el archivo JSON
                string json = JsonConvert.SerializeObject(usuariosBase, Formatting.Indented);
                File.WriteAllText(rutaUsuarios, json);
            }
        }

        // 2. Método para comprobar si la clave es correcta
        public static UsuarioLocal ValidarLoginJSON(string documento, string password)
        {
            // Si borraste el archivo por error, lo vuelve a crear
            InicializarUsuariosDeEmergencia();

            // Lee los usuarios del archivo
            string json = File.ReadAllText(rutaUsuarios);
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioLocal>>(json);

            // Busca si hay alguno que coincida
            foreach (var u in usuarios)
            {
                if (u.Documento == documento && u.Password == password)
                {
                    return u; // ¡Lo encontró!
                }
            }

            return null; // No lo encontró o clave incorrecta
        }
    }
}