namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante Correo Electrónico.
/// </summary>
public class NotificacionCorreo : Notificacion, IProgramable, IConfirmable, IRegistrable
{
    public string Asunto { get; }
    public DateTime? FechaProgramada { get; private set; }
    public DateTime? FechaConfirmacion { get; private set; }

    public NotificacionCorreo(string correoDestinatario, string asunto, string cuerpoMensaje, Prioridad prioridad)
        : base(correoDestinatario, cuerpoMensaje, prioridad)
    {
        if (string.IsNullOrWhiteSpace(asunto))
            throw new ArgumentException("El asunto del correo no puede estar vacío.", nameof(asunto));

        Asunto = asunto.Trim();
    }

    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            Console.WriteLine($"  [CORREO -> SMTP] Para: {Destinatario} | Asunto: \"{Asunto}\" | Mensaje: \"{Mensaje}\"");
            Estado = EstadoNotificacion.Enviada;
        }
        catch (Exception)
        {
            Estado = EstadoNotificacion.Fallida;
            throw;
        }
    }

    /// <summary>
    /// Envía el correo especificando un remitente personalizado.
    /// </summary>
    public void Enviar(string remitentePersonalizado)
    {
        Console.WriteLine($"  [CORREO CON REMITENTE] Desde: {remitentePersonalizado}");
        Enviar();
    }

    public override string Resumen()
    {
        return $"{base.Resumen()} | Asunto: \"{Asunto}\"";
    }

    /// <summary>
    /// Genera el resumen incluyendo opcionalmente el cuerpo completo del mensaje.
    /// </summary>
    public string Resumen(bool incluirMensajeCompleto)
    {
        return incluirMensajeCompleto
            ? $"{Resumen()} | Cuerpo: \"{Mensaje}\""
            : Resumen();
    }

    public void Programar(DateTime fechaHora)
    {
        if (fechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha y hora de programación debe ser en el futuro.", nameof(fechaHora));

        Estado = EstadoNotificacion.Programada;
        FechaProgramada = fechaHora;
        Console.WriteLine($"  [CORREO PROGRAMADO] Agendado para {Destinatario} el {FechaProgramada:g}.");
    }

    public void ConfirmarEntrega()
    {
        if (Estado != EstadoNotificacion.Enviada)
            throw new InvalidOperationException($"No se puede confirmar la entrega del correo [{Id}] porque su estado es {Estado}.");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [CORREO CONFIRMADO] Acuse de recibo/lectura verificado para {Destinatario} a las {FechaConfirmacion:T}.");
    }

    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-CORREO] ID={Id} | Para={Destinatario} | Asunto={Asunto} | Estado={Estado}";
    }
}
