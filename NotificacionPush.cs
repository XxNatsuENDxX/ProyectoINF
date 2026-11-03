namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante Alerta Push a dispositivos móviles (FCM / APNs).
/// Implementa IProgramable e IRegistrable.
/// 
/// JUSTIFICACIÓN DE DISEÑO:
/// NotificacionPush NO implementa IConfirmable porque las alertas push estándar no garantizan
/// un acuse de recibo de lectura bidireccional. Si IConfirmable fuera obligatorio en Notificacion,
/// esta clase se vería obligada a implementar un método ficticio o lanzar NotSupportedException,
/// violando el Principio de Segregación de Interfaces (ISP).
/// </summary>
public class NotificacionPush : Notificacion, IProgramable, IRegistrable
{
    public string TokenDispositivo { get; }
    public DateTime? FechaProgramada { get; private set; }

    public NotificacionPush(string tokenDispositivo, string mensaje, Prioridad prioridad)
        : base(tokenDispositivo, mensaje, prioridad)
    {
        TokenDispositivo = Destinatario;
    }

    /// <summary>
    /// Sobrescritura (override) del método polimórfico Enviar() para servicios Push.
    /// </summary>
    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            Console.WriteLine($"  [PUSH -> FCM/APNs] Token: {Destinatario} | Alerta: \"{Mensaje}\"");
            Estado = EstadoNotificacion.Enviada;
        }
        catch (Exception)
        {
            Estado = EstadoNotificacion.Fallida;
            throw;
        }
    }

    /// <summary>
    /// Implementación del contrato IProgramable.
    /// </summary>
    public void Programar(DateTime fechaHora)
    {
        if (fechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha de programación de la alerta push debe ser futura.", nameof(fechaHora));

        Estado = EstadoNotificacion.Programada;
        FechaProgramada = fechaHora;
        Console.WriteLine($"  [PUSH PROGRAMADO] Alerta encolada para el dispositivo {Destinatario} a las {FechaProgramada:g}.");
    }

    /// <summary>
    /// Implementación del contrato IRegistrable.
    /// </summary>
    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-PUSH] ID={Id} | Token={Destinatario} | Prioridad={Prioridad} | Estado={Estado}";
    }
}

