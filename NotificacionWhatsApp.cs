namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante WhatsApp Business API.
/// Implementa IConfirmable e IRegistrable, pero NO IProgramable.
/// 
/// ESTE ARCHIVO RESPONDE DIRECTAMENTE A:
/// - Pregunta 7.1 del Banco Docente: "Agregue conceptualmente NotificacionWhatsApp, que puede registrar
///   y confirmar entrega, pero no programarse. ¿Qué debe crear o modificar?"
/// - Modificación en vivo 10.1: Demuestra que agregar un nuevo canal solo requiere crear este subtipo
///   y agregarlo a la lista; el consumidor (GestorNotificaciones y su foreach) PERMANECE 100% INTACTO.
/// </summary>
public class NotificacionWhatsApp : Notificacion, IConfirmable, IRegistrable
{
    public DateTime? FechaConfirmacion { get; private set; }

    public NotificacionWhatsApp(string numeroTelefono, string mensaje, Prioridad prioridad)
        : base(numeroTelefono, mensaje, prioridad)
    {
    }

    /// <summary>
    /// Sobrescritura (override) del método polimórfico Enviar() para WhatsApp API.
    /// </summary>
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

    /// <summary>
    /// Doble check azul (acuse de recibo / confirmación de lectura).
    /// </summary>
    public void ConfirmarEntrega()
    {
        if (Estado != EstadoNotificacion.Enviada)
            throw new InvalidOperationException($"No se puede confirmar la entrega de WhatsApp [{Id}] porque su estado es {Estado}.");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [WHATSAPP CONFIRMADO] Doble check azul verificado para {Destinatario} a las {FechaConfirmacion:T}.");
    }

    /// <summary>
    /// Registro de auditoría para mensajería instantánea.
    /// </summary>
    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-WHATSAPP] ID={Id} | Número={Destinatario} | Prioridad={Prioridad} | Estado={Estado}";
    }
}
