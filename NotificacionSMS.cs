namespace SistemaNotificaciones;

/// <summary>
/// Canal de notificación mediante Mensaje de Texto Móvil (SMS).
/// Implementa IConfirmable e IRegistrable.
/// 
/// REGLA DE NEGOCIO:
/// Los mensajes SMS tienen una longitud máxima de 160 caracteres. Esta restricción
/// está encapsulada estrictamente dentro de esta clase (no en la clase base ni en el Main).
/// </summary>
public class NotificacionSMS : Notificacion, IConfirmable, IRegistrable
{
    /// <summary>
    /// Límite de caracteres estándar para un mensaje SMS.
    /// Modificar este valor aquí actualiza la regla en todo el sistema sin tocar otras clases.
    /// </summary>
    public const int MaximoCaracteres = 160;

    public DateTime? FechaConfirmacion { get; private set; }

    public NotificacionSMS(string telefono, string mensaje, Prioridad prioridad)
        : base(telefono, ValidarMensajeSMS(mensaje), prioridad)
    {
    }

    /// <summary>
    /// Validación de regla de negocio exclusiva del canal SMS.
    /// Se ejecuta antes de llamar al constructor base para garantizar que el objeto nunca sea inválido.
    /// </summary>
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

    /// <summary>
    /// Sobrescritura (override) del método polimórfico Enviar().
    /// Define el comportamiento especializado de transmisión a través de red celular.
    /// Actualiza el estado protegido a Enviada (o Fallida si ocurre un error).
    /// </summary>
    public override void Enviar()
    {
        ValidarEnvioPendiente();

        try
        {
            // Simulación del despacho a través de la pasarela SMS
            Console.WriteLine($"  [SMS -> Red Móvil] Destino: {Destinatario} | Chars: {Mensaje.Length}/{MaximoCaracteres} | Texto: \"{Mensaje}\"");
            Estado = EstadoNotificacion.Enviada;
        }
        catch (Exception)
        {
            Estado = EstadoNotificacion.Fallida;
            throw;
        }
    }

    /// <summary>
    /// Implementación del contrato IConfirmable.
    /// Permite registrar el acuse de recibo de entrega por la operadora telefónica.
    /// </summary>
    public void ConfirmarEntrega()
    {
        if (Estado != EstadoNotificacion.Enviada)
            throw new InvalidOperationException($"No se puede confirmar la entrega del SMS [{Id}] porque su estado es {Estado} (debe estar Enviada).");

        Estado = EstadoNotificacion.Confirmada;
        FechaConfirmacion = DateTime.Now;
        Console.WriteLine($"  [SMS CONFIRMADO] Entrega validada para el número {Destinatario} a las {FechaConfirmacion:T}.");
    }

    /// <summary>
    /// Implementación del contrato IRegistrable para bitácoras y trazabilidad.
    /// </summary>
    public string ObtenerRegistro()
    {
        return $"[AUDITORÍA-SMS] ID={Id} | Destino={Destinatario} | Caracteres={Mensaje.Length} | Prioridad={Prioridad} | Estado={Estado}";
    }
}

