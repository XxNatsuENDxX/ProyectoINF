namespace SistemaNotificaciones;

/// <summary>
/// Gestor encargado de almacenar y orquestar el envío de notificaciones.
/// 
/// EVIDENCIA DE BAJO ACOPLAMIENTO (Pregunta 8.3):
/// Esta clase depende exclusivamente de la abstracción 'Notificacion' (y de contratos de interfaces
/// cuando procesa capacidades específicas). NO tiene dependencias directas con NotificacionSMS,
/// NotificacionCorreo o NotificacionPush para el despacho de mensajes.
/// </summary>
public class GestorNotificaciones
{
    /// <summary>
    /// Contenedor polimórfico principal.
    /// Almacena referencias del tipo declarado 'Notificacion', pero los objetos reales
    /// son subtipos derivados (NotificacionSMS, NotificacionCorreo, etc.).
    /// </summary>
    private readonly List<Notificacion> _notificaciones = new();

    /// <summary>
    /// Exposición segura de la colección en modo solo lectura para evitar mutaciones externas arbitrarias.
    /// </summary>
    public IReadOnlyList<Notificacion> Notificaciones => _notificaciones.AsReadOnly();

    public int Total => _notificaciones.Count;

    // =========================================================================
    // SOBRECARGA 1 (OVERLOAD):
    // Agrega una única notificación a la colección.
    // =========================================================================
    public void Agregar(Notificacion notificacion)
    {
        if (notificacion == null)
            throw new ArgumentNullException(nameof(notificacion), "No se puede agregar una notificación nula.");

        _notificaciones.Add(notificacion);
    }

    // =========================================================================
    // SOBRECARGA 2 (OVERLOAD):
    // Agrega un conjunto enumerable de notificaciones.
    // Misma operación 'Agregar', distinta lista de parámetros.
    // =========================================================================
    public void Agregar(IEnumerable<Notificacion> notificaciones)
    {
        if (notificaciones == null)
            throw new ArgumentNullException(nameof(notificaciones), "La lista de notificaciones no puede ser nula.");

        foreach (var notificacion in notificaciones)
        {
            Agregar(notificacion);
        }
    }

    /// <summary>
    /// POLIMORFISMO DINÁMICO EN ACCIÓN (Preguntas 3.1, 5.4, 7.5 y 8.3):
    /// 
    /// 1. POLÍTICA DE ORDENAMIENTO (Pregunta 5.4 y 7.5):
    ///    El Gestor es el responsable de ordenar por prioridad (Critica -> Alta -> Normal -> Baja)
    ///    porque el servicio que administra la colección es quien conoce el contexto global;
    ///    una notificación individual no debe ni puede ordenar a las demás.
    /// 
    /// 2. DESPACHO VIRTUAL (Preguntas 3.1 y 6):
    ///    En el bucle 'foreach (Notificacion n in ...)', el tipo declarado de 'n' es Notificacion.
    ///    Al invocar 'n.Enviar()', el CLR de .NET consulta la tabla de métodos virtuales (vtable)
    ///    y despacha dinámicamente la versión 'override' del tipo real del objeto en memoria.
    /// 
    /// 3. PRINCIPIO ABIERTO/CERRADO (OCP) (Pregunta 6 y 7.1):
    ///    Si añadimos NotificacionWhatsApp o NotificacionTelegram, ¡ESTE BUCLE NO SE MODIFICA!
    /// </summary>
    public void ProcesarTodas()
    {
        Console.WriteLine("\n----------------------------------------------------------------------------------");
        Console.WriteLine(">>> INICIANDO PROCESAMIENTO POLIMÓRFICO ORDENADO POR PRIORIDAD <<<");
        Console.WriteLine("----------------------------------------------------------------------------------");

        // Orden descendente por Prioridad: Critica (4), Alta (3), Normal (2), Baja (1)
        var listaOrdenada = _notificaciones
            .OrderByDescending(n => n.Prioridad)
            .ToList();

        int contador = 1;
        foreach (Notificacion n in listaOrdenada)
        {
            Console.WriteLine($"\n[Paso {contador++}] Procesando: {n.Resumen()}");

            try
            {
                // ¡AQUÍ OCURRE EL POLIMORFISMO DINÁMICO!
                // Tipo declarado: Notificacion
                // Tipo real: NotificacionSMS, NotificacionCorreo, NotificacionPush, etc.
                // Invocación a Enviar() mediante despacho virtual dinámico en runtime.
                n.Enviar();

                Console.WriteLine($"  ✓ Estado actualizado a: {n.Estado}");

                // Verificación de capacidad opcional mediante coincidencia de patrones con interfaces
                if (n is IRegistrable registrable)
                {
                    Console.WriteLine($"  Auditoría -> {registrable.ObtenerRegistro()}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ✗ Error al procesar notificación [{n.Id}]: {ex.Message}");
                Console.ResetColor();
            }
        }

        Console.WriteLine("\n----------------------------------------------------------------------------------");
        Console.WriteLine(">>> PROCESAMIENTO FINALIZADO <<<");
        Console.WriteLine("----------------------------------------------------------------------------------");
    }

    /// <summary>
    /// Programa todas las notificaciones que implementen el contrato IProgramable.
    /// Demuestra el uso polimórfico de interfaces como capacidades opcionales.
    /// </summary>
    public void ProgramarCompatibles(DateTime fechaHora)
    {
        Console.WriteLine($"\n--- Programando notificaciones con capacidad IProgramable para {fechaHora:g} ---");
        foreach (var notificacion in _notificaciones)
        {
            if (notificacion is IProgramable programable)
            {
                programable.Programar(fechaHora);
            }
        }
    }

    /// <summary>
    /// Confirma la entrega para todas las notificaciones enviadas que implementen IConfirmable.
    /// </summary>
    public void ConfirmarCompatibles()
    {
        Console.WriteLine("\n--- Confirmando entregas con capacidad IConfirmable ---");
        foreach (var notificacion in _notificaciones)
        {
            if (notificacion is IConfirmable confirmable && notificacion.Estado == EstadoNotificacion.Enviada)
            {
                confirmable.ConfirmarEntrega();
            }
        }
    }

    /// <summary>
    /// Genera y muestra un reporte general de todas las notificaciones en el gestor.
    /// </summary>
    public void MostrarReporte()
    {
        Console.WriteLine("\n======================= REPORTE GENERAL DEL GESTOR =======================");
        Console.WriteLine($"Total de notificaciones gestionadas: {_notificaciones.Count}");
        foreach (var n in _notificaciones)
        {
            Console.WriteLine(n.Resumen());
        }
        Console.WriteLine("==========================================================================");
    }
}
