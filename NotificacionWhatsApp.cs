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
/// Canal de notificación mediante WhatsApp Business API.
/// </summary>
public class NotificacionWhatsApp : Notificacion, IConfirmable, IRegistrable
{
    public DateTime? FechaConfirmacion { get; private set; }

    public NotificacionWhatsApp(string numeroTelefono, string mensaje, Prioridad prioridad)
        : base(numeroTelefono, mensaje, prioridad)
    {
    }

    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            Console.WriteLine($"  [WHATSAPP -> Cloud API] Número: {Destinatario} | Mensaje: \"{Mensaje}\"");
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
            throw new InvalidOperationException($"No se puede confirmar la entrega de WhatsApp [{Id}] porque su estado es {Estado}.");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [WHATSAPP CONFIRMADO] Entrega confirmada para {Destinatario} a las {FechaConfirmacion:T}.");
    }

    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-WHATSAPP] ID={Id} | Número={Destinatario} | Prioridad={Prioridad} | Estado={Estado}";
    }
}
