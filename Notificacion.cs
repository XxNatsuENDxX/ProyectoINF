namespace SistemaNotificaciones;

/// <summary>
/// Clase base abstracta que define el contrato común y los atributos compartidos
/// por todos los canales de notificación del sistema.
/// 
/// DECISIÓN DE DISEÑO:
/// Es 'abstract' porque representa un concepto genérico: no tiene sentido instanciar una
/// "Notificacion" sin especificar a través de qué medio físico se transmitirá (SMS, Correo, Push, etc.).
/// </summary>
public abstract class Notificacion
{
    public string Id { get; }
    public string Destinatario { get; }
    public string Mensaje { get; }
    public Prioridad Prioridad { get; }

    /// <summary>
    /// El estado tiene 'protected set' para que únicamente la jerarquía de herencia
    /// (la clase base o sus clases derivadas) pueda gobernar las transiciones de estado.
    /// Esto evita que un consumidor externo asigne estados inconsistentes de forma arbitraria.
    /// </summary>
    public EstadoNotificacion Estado { get; protected set; }

    public DateTime FechaCreacion { get; }

    /// <summary>
    /// Propiedad de conveniencia sinónimo de Mensaje.
    /// </summary>
    public string Contenido => Mensaje;

    /// <summary>
    /// Constructor protegido de la clase base. Realiza validaciones tempranas
    /// para impedir la creación de objetos en un estado inválido o incompleto.
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
    /// Operación polimórfica principal.
    /// Es abstracta porque cada canal concreto (SMS, Correo, Push, WhatsApp) requiere una lógica
    /// de envío totalmente distinta. La clase base impone el contrato a todas las subclases.
    /// En tiempo de ejecución, el despacho dinámico invoca la implementación 'override' del objeto real.
    /// </summary>
    public abstract void Enviar();

    /// <summary>
    /// Método virtual con una implementación base general.
    /// Las clases derivadas pueden invocarlo con base.Resumen() o sobrescribirlo (override)
    /// para añadir información especializada.
    /// </summary>
    public virtual string Resumen()
    {
        return $"[{Id}] {GetType().Name,-18} | Para: {Destinatario,-22} | Prioridad: {Prioridad,-7} | Estado: {Estado}";
    }

    /// <summary>
    /// Método auxiliar compartido para verificar si la notificación ya fue enviada previamente.
    /// Demuestra reutilización de lógica común en la clase base.
    /// </summary>
    protected void ValidarEnvioPendiente()
    {
        if (Estado is EstadoNotificacion.Enviada or EstadoNotificacion.Confirmada)
            throw new InvalidOperationException($"La notificación [{Id}] ya fue procesada anteriormente.");

        if (Estado is EstadoNotificacion.Cancelada)
            throw new InvalidOperationException($"La notificación [{Id}] fue cancelada y no puede enviarse.");
    }
}

