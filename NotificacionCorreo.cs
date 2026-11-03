namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante Correo Electrónico (Email).
/// Implementa IProgramable, IConfirmable e IRegistrable.
/// 
/// Posee un atributo especializado ('Asunto') que no existe en otros canales,
/// y demuestra tanto sobrescritura (override) como sobrecarga de métodos (overload).
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

    /// <summary>
    /// Sobrescritura (override) polimórfica del método de envío por canal SMTP.
    /// </summary>
    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            // Simulación del protocolo SMTP
            Console.WriteLine($"  [CORREO -> SMTP] Para: {Destinatario} | Asunto: \"{Asunto}\" | Mensaje: \"{Mensaje}\"");
            Estado = EstadoNotificacion.Enviada;
        }
        catch (Exception)
        {
            Estado = EstadoNotificacion.Fallida;
            throw;
        }
    }

    // =========================================================================
    // SOBRECARGA DE MÉTODO (OVERLOAD):
    // Misma operación 'Enviar', pero con una lista de parámetros diferente (string remitente).
    // Se resuelve en tiempo de compilación (polimorfismo estático / early binding).
    // NO reemplaza al método Enviar(), sino que convive con él.
    // =========================================================================
    public void Enviar(string remitentePersonalizado)
    {
        Console.WriteLine($"  [CORREO CON REMITENTE] Desde: {remitentePersonalizado}");
        Enviar();
    }

    /// <summary>
    /// Sobrescritura (override) del método virtual Resumen() de la clase base.
    /// Reutiliza el formato base e incorpora el asunto exclusivo del correo.
    /// </summary>
    public override string Resumen()
    {
        return $"{base.Resumen()} | Asunto: \"{Asunto}\"";
    }

    // =========================================================================
    // SOBRECARGA DE MÉTODO (OVERLOAD):
    // Sobrecarga de 'Resumen' aceptando un booleano para decidir si mostrar o no el cuerpo.
    // Pregunta de examen: ¿Por qué no basta con cambiar el tipo de retorno para una sobrecarga?
    // Porque la firma del método en C# se compone del nombre y los tipos de sus parámetros.
    // =========================================================================
    public string Resumen(bool incluirMensajeCompleto)
    {
        return incluirMensajeCompleto
            ? $"{Resumen()} | Cuerpo: \"{Mensaje}\""
            : Resumen();
    }

    /// <summary>
    /// Implementación de IProgramable: agendamiento para entrega en fecha/hora futura.
    /// </summary>
    public void Programar(DateTime fechaHora)
    {
        if (fechaHora <= DateTime.Now)
            throw new ArgumentException("La fecha y hora de programación debe ser en el futuro.", nameof(fechaHora));

        Estado = EstadoNotificacion.Programada;
        FechaProgramada = fechaHora;
        Console.WriteLine($"  [CORREO PROGRAMADO] Agendado para {Destinatario} el {FechaProgramada:g}.");
    }

    /// <summary>
    /// Implementación de IConfirmable: confirmación de lectura mediante acuse de recibo.
    /// </summary>
    public void ConfirmarEntrega()
    {
        if (Estado != EstadoNotificacion.Enviada)
            throw new InvalidOperationException($"No se puede confirmar la entrega del correo [{Id}] porque su estado es {Estado}.");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [CORREO CONFIRMADO] Acuse de recibo/lectura verificado para {Destinatario} a las {FechaConfirmacion:T}.");
    }

    /// <summary>
    /// Implementación de IRegistrable para bitácoras y auditoría.
    /// </summary>
    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-CORREO] ID={Id} | Para={Destinatario} | Asunto={Asunto} | Estado={Estado}";
    }
}

