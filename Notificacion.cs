namespace SistemaNotificaciones;

/// <summary>
/// Clase base abstracta que define los datos y el comportamiento común
/// para los distintos canales de notificación del sistema.
/// </summary>
public abstract class Notificacion
{
    public string Id { get; }
    public string Destinatario { get; }
    public string Mensaje { get; }
    public Prioridad Prioridad { get; }
    public EstadoNotificacion Estado { get; protected set; }
    public DateTime FechaCreacion { get; }

    /// <summary>
    /// Propiedad de conveniencia sinónimo de Mensaje.
    /// </summary>
    public string Contenido => Mensaje;

    /// <summary>
    /// Constructor protegido de la clase base.
    /// Valida que los datos obligatorios no sean nulos ni vacíos.
    /// </summary>
    protected Notificacion(string destinatario, string mensaje, Prioridad prioridad)
    {
        if (string.IsNullOrWhiteSpace(destinatario))
            throw new ArgumentException("El destinatario no puede ser nulo ni estar vacío.", nameof(destinatario));

        if (string.IsNullOrWhiteSpace(mensaje))
            throw new ArgumentException("El mensaje no puede ser nulo ni estar vacío.", nameof(mensaje));

        Id = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        Destinatario = destinatario.Trim();
        Mensaje = mensaje.Trim();
        Prioridad = prioridad;
        Estado = EstadoNotificacion.Pendiente;
        FechaCreacion = DateTime.Now;
    }

    /// <summary>
    /// Ejecuta el proceso de envío de la notificación a través del canal correspondiente.
    /// </summary>
    public abstract void Enviar();

    /// <summary>
    /// Genera una descripción legible de la notificación y su estado actual.
    /// </summary>
    public virtual string Resumen()
    {
        return $"[{Id}] {GetType().Name,-18} | Para: {Destinatario,-22} | Prioridad: {Prioridad,-7} | Estado: {Estado}";
    }

    /// <summary>
    /// Valida que la notificación se encuentre en un estado apto para su envío.
    /// </summary>
    protected void ValidarEnvioPendiente()
    {
        if (Estado is EstadoNotificacion.Enviada or EstadoNotificacion.Confirmada)
            throw new InvalidOperationException($"La notificación [{Id}] ya fue procesada anteriormente.");

        if (Estado is EstadoNotificacion.Cancelada)
            throw new InvalidOperationException($"La notificación [{Id}] fue cancelada y no puede enviarse.");
    }
}
