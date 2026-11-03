namespace SistemaNotificaciones;

/// <summary>
/// Modela la capacidad opcional de agendar una notificación para una fecha y hora futura.
/// Se define como interfaz (en lugar de método abstracto en Notificacion) porque no todos los canales
/// admiten programación diferida (ej: alertas de emergencia por SMS o canales de envío inmediato).
/// </summary>
public interface IProgramable
{
    DateTime? FechaProgramada { get; }
    void Programar(DateTime fechaHora);
}

/// <summary>
/// Modela la capacidad opcional de registrar y recibir un acuse de recibo o confirmación de lectura.
/// No todas las vías de comunicación lo soportan (por ejemplo, notificaciones Push estándar de un solo sentido).
/// </summary>
public interface IConfirmable
{
    DateTime? FechaConfirmacion { get; }
    void ConfirmarEntrega();
}

/// <summary>
/// Modela la capacidad de generar un registro estructurado para fines de auditoría y bitácora.
/// </summary>
public interface IRegistrable
{
    string ObtenerRegistro();
}

/// <summary>
/// Modela la capacidad transversal y opcional de cancelar un envío pendiente.
/// Permite extender el sistema hacia nuevos requisitos sin modificar la jerarquía base de clases.
/// </summary>
public interface ICancelable
{
    bool EsCancelada { get; }
    void Cancelar(string motivo);
}
