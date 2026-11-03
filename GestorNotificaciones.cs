// ============================================================================
// ASIGNATURA: Programación II (INF-512)
// PROYECTO:   Sistema Multicanal de Notificaciones — Reto de Diseño Polimórfico
// INTEGRANTES:
//   - Alcequiez Peña, Jeremy Junior
//   - Aybar Montes De Oca, Cesar Adrian
//   - Faneyte Carbuccia, Dennis Keith
//   - Garcia Disla, Jose Armando
// ============================================================================

namespace SistemaNotificaciones;

/// <summary>
/// Gestor encargado de almacenar y orquestar el despacho de notificaciones.
/// </summary>
public class GestorNotificaciones
{
    private readonly List<Notificacion> _notificaciones = new();

    public IReadOnlyList<Notificacion> Notificaciones => _notificaciones.AsReadOnly();

    public int Total => _notificaciones.Count;

    /// <summary>
    /// Agrega una notificación individual a la colección.
    /// </summary>
    public void Agregar(Notificacion notificacion)
    {
        if (notificacion == null)
            throw new ArgumentNullException(nameof(notificacion), "No se puede agregar una notificación nula.");

        _notificaciones.Add(notificacion);
    }

    /// <summary>
    /// Agrega un conjunto de notificaciones a la colección.
    /// </summary>
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
    /// Procesa todas las notificaciones registradas ordenadas por nivel de prioridad descendente.
    /// </summary>
    public void ProcesarTodas()
    {
        Console.WriteLine("\n----------------------------------------------------------------------------------");
        Console.WriteLine(">>> INICIANDO PROCESAMIENTO DE NOTIFICACIONES POR PRIORIDAD <<<");
        Console.WriteLine("----------------------------------------------------------------------------------");

        var listaOrdenada = _notificaciones
            .OrderByDescending(n => n.Prioridad)
            .ToList();

        int contador = 1;
        foreach (Notificacion n in listaOrdenada)
        {
            Console.WriteLine($"\n[Paso {contador++}] Procesando: {n.Resumen()}");

            try
            {
                n.Enviar();
                Console.WriteLine($"  ✓ Estado actualizado a: {n.Estado}");

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
    /// Programa todas las notificaciones compatibles para una fecha determinada.
    /// </summary>
    public void ProgramarCompatibles(DateTime fechaHora)
    {
        Console.WriteLine($"\n--- Programando notificaciones programables para {fechaHora:g} ---");
        foreach (var notificacion in _notificaciones)
        {
            if (notificacion is IProgramable programable)
            {
                programable.Programar(fechaHora);
            }
        }
    }

    /// <summary>
    /// Confirma la entrega de todas las notificaciones enviadas compatibles.
    /// </summary>
    public void ConfirmarCompatibles()
    {
        Console.WriteLine("\n--- Confirmando entregas para canales con acuse de recibo ---");
        foreach (var notificacion in _notificaciones)
        {
            if (notificacion is IConfirmable confirmable && notificacion.Estado == EstadoNotificacion.Enviada)
            {
                confirmable.ConfirmarEntrega();
            }
        }
    }

    /// <summary>
    /// Muestra un resumen del estado actual de todas las notificaciones en el gestor.
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
