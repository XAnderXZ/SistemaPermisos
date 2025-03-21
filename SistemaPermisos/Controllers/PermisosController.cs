﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPermisos.Data;
using SistemaPermisos.Models;
using SistemaPermisos.ViewModels;

namespace SistemaPermisos.Controllers
{
    public class PermisosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PermisosController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Permisos
        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario está autenticado
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var rol = HttpContext.Session.GetString("UsuarioRol");

            // Si es administrador, mostrar todos los permisos
            if (rol == "Admin")
            {
                var permisos = await _context.Permisos
                    .Include(p => p.Usuario)
                    .OrderByDescending(p => p.FechaSolicitud)
                    .ToListAsync();
                return View(permisos);
            }

            // Si es docente, mostrar solo sus permisos
            var misPermisos = await _context.Permisos
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.FechaSolicitud)
                .ToListAsync();

            return View(misPermisos);
        }

        // GET: Permisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permiso == null)
            {
                return NotFound();
            }

            // Verificar que el usuario tenga acceso a este permiso
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var rol = HttpContext.Session.GetString("UsuarioRol");

            if (usuarioId == null || (permiso.UsuarioId != usuarioId && rol != "Admin"))
            {
                return RedirectToAction("Login", "Account");
            }

            return View(permiso);
        }

        // GET: Permisos/Create
        public IActionResult Create()
        {
            // Verificar si el usuario está autenticado
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Obtener información del usuario para prellenar el formulario
            var usuario = _context.Usuarios.Find(usuarioId);
            if (usuario != null)
            {
                var viewModel = new PermisoViewModel
                {
                    Fecha = DateTime.Now,
                    Cedula = usuario.Cedula ?? "",
                    Puesto = usuario.Puesto ?? ""
                };
                return View(viewModel);
            }

            return View(new PermisoViewModel { Fecha = DateTime.Now });
        }

        // POST: Permisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PermisoViewModel model)
        {
            // Verificar si el usuario está autenticado
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Procesar la imagen del comprobante
                string rutaComprobante = null;
                if (model.Comprobante != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "comprobantes");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Comprobante.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Asegurar que el directorio existe
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Comprobante.CopyToAsync(fileStream);
                    }

                    rutaComprobante = "/uploads/comprobantes/" + uniqueFileName;
                }

                var permiso = new Permiso
                {
                    UsuarioId = usuarioId.Value,
                    Fecha = model.Fecha,
                    HoraDesde = model.HoraDesde,
                    HoraHasta = model.HoraHasta,
                    JornadaCompleta = model.JornadaCompleta,
                    MediaJornada = model.MediaJornada,
                    CantidadLecciones = model.CantidadLecciones,
                    Cedula = model.Cedula,
                    Puesto = model.Puesto,
                    Condicion = model.Condicion,
                    TipoMotivo = model.TipoMotivo,
                    TipoConvocatoria = model.TipoConvocatoria,
                    Motivo = model.TipoMotivo == "Asuntos personales" ? model.Motivo : model.TipoMotivo,
                    Observaciones = model.Observaciones,
                    HoraSalida = model.HoraSalida,
                    RutaComprobante = rutaComprobante,
                    Estado = "Pendiente",
                    FechaSolicitud = DateTime.Now,
                    RutaJustificacion = null
                };

                // Actualizar información del usuario si es necesario
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario != null)
                {
                    if (string.IsNullOrEmpty(usuario.Cedula))
                    {
                        usuario.Cedula = model.Cedula;
                    }
                    if (string.IsNullOrEmpty(usuario.Puesto))
                    {
                        usuario.Puesto = model.Puesto;
                    }
                    _context.Update(usuario);
                }

                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Permisos/Justify/5
        public async Task<IActionResult> Justify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            // Verificar que el usuario tenga acceso a este permiso
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null || permiso.UsuarioId != usuarioId)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new JustificarPermisoViewModel
            {
                PermisoId = permiso.Id
            };

            return View(viewModel);
        }

        // POST: Permisos/Justify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Justify(JustificarPermisoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var permiso = await _context.Permisos.FindAsync(model.PermisoId);
                if (permiso == null)
                {
                    return NotFound();
                }

                // Verificar que el usuario tenga acceso a este permiso
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
                if (usuarioId == null || permiso.UsuarioId != usuarioId)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Procesar la imagen de justificación
                if (model.Justificacion != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "justificaciones");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Justificacion.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Asegurar que el directorio existe
                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Justificacion.CopyToAsync(fileStream);
                    }

                    permiso.RutaJustificacion = "/uploads/justificaciones/" + uniqueFileName;
                    permiso.Justificado = true;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        // GET: Permisos/Resolve/5 (Solo para administradores)
        public async Task<IActionResult> Resolve(int? id)
        {
            // Verificar que sea administrador
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (permiso == null)
            {
                return NotFound();
            }

            var viewModel = new ResolucionPermisoViewModel
            {
                PermisoId = permiso.Id
            };

            ViewBag.Permiso = permiso;

            return View(viewModel);
        }

        // POST: Permisos/Resolve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(ResolucionPermisoViewModel model)
        {
            // Verificar que sea administrador
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var permiso = await _context.Permisos.FindAsync(model.PermisoId);
                if (permiso == null)
                {
                    return NotFound();
                }

                permiso.Resolucion = model.Resolucion;
                permiso.ObservacionesResolucion = model.ObservacionesResolucion;
                permiso.TipoRebajo = model.TipoRebajo;

                // Actualizar el estado según la resolución
                if (model.Resolucion == "Aceptar lo solicitado" || model.Resolucion == "Acoger convocatoria")
                {
                    permiso.Estado = "Aprobado";
                }
                else if (model.Resolucion == "Denegar lo solicitado")
                {
                    permiso.Estado = "Rechazado";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, volver a cargar el permiso para la vista
            var permisoDb = await _context.Permisos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == model.PermisoId);

            ViewBag.Permiso = permisoDb;

            return View(model);
        }

        // GET: Permisos/Approve/5 (Solo para administradores) - Método obsoleto, usar Resolve
        public async Task<IActionResult> Approve(int? id)
        {
            // Verificar que sea administrador
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            permiso.Estado = "Aprobado";
            permiso.Resolucion = "Aceptar lo solicitado";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Permisos/Reject/5 (Solo para administradores) - Método obsoleto, usar Resolve
        public async Task<IActionResult> Reject(int? id)
        {
            // Verificar que sea administrador
            var rol = HttpContext.Session.GetString("UsuarioRol");
            if (rol != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            permiso.Estado = "Rechazado";
            permiso.Resolucion = "Denegar lo solicitado";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

