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
/// Canal de notificación mediante Mensaje de Texto Móvil (SMS).
/// </summary>
public class NotificacionSMS : Notificacion, IConfirmable, IRegistrable
{
    public const int MaximoCaracteres = 160;

    public DateTime? FechaConfirmacion { get; private set; }

    public NotificacionSMS(string telefono, string mensaje, Prioridad prioridad)
        : base(telefono, ValidarMensajeSMS(mensaje), prioridad)
    {
    }

    private static string ValidarMensajeSMS(string mensaje)
    {
        if (mensaje != null && mensaje.Length > MaximoCaracteres)
        {
            throw new ArgumentOutOfRangeException(
                nameof(mensaje),
                $"El SMS no puede superar {MaximoCaracteres} caracteres (longitud recibida: {mensaje.Length}).");
        }
        return mensaje!;
    }

    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            Console.WriteLine($"  [SMS -> Red Móvil] Destino: {Destinatario} | Chars: {Mensaje.Length}/{MaximoCaracteres} | Texto: \"{Mensaje}\"");
            Estado = EstadoNotificacion.Enviada;
        }
        catch (Exception)
        {
            Estado = EstadoNotificacion.Fallida;
            throw;
        }
    }

    public void ConfirmarEntrega()
    {
        if (Estado != EstadoNotificacion.Enviada)
            throw new InvalidOperationException($"No se puede confirmar la entrega del SMS [{Id}] porque su estado es {Estado} (debe estar Enviada).");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [SMS CONFIRMADO] Entrega validada para el número {Destinatario} a las {FechaConfirmacion:T}.");
    }

    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-SMS] ID={Id} | Destino={Destinatario} | Caracteres={Mensaje.Length} | Prioridad={Prioridad} | Estado={Estado}";
    }
}
