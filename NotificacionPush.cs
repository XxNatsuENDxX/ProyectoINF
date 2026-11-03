namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante Alerta Push a dispositivos móviles.
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

    public void Programar(DateTime fechaHora)
    {
        if (fechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha de programación de la alerta push debe ser futura.", nameof(fechaHora));

        Estado = EstadoNotificacion.Programada;
        FechaProgramada = fechaHora;
        Console.WriteLine($"  [PUSH PROGRAMADO] Alerta encolada para el dispositivo {Destinatario} a las {FechaProgramada:g}.");
    }

    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-PUSH] ID={Id} | Token={Destinatario} | Prioridad={Prioridad} | Estado={Estado}";
    }
}
