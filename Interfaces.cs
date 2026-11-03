namespace SistemaNotificaciones;

/// <summary>
/// Define la capacidad de agendar el envío de una notificación para una fecha u hora futura.
/// </summary>
public interface IProgramable
{
    DateTime? FechaProgramada { get; }
    void Programar(DateTime fechaHora);
}

/// <summary>
/// Define la capacidad de registrar y procesar la confirmación de entrega de una notificación.
/// </summary>
public interface IConfirmable
{
    DateTime? FechaConfirmacion { get; }
    void ConfirmarEntrega();
}

/// <summary>
/// Define la capacidad de generar un registro estructurado para fines de auditoría y bitácora.
/// </summary>
public interface IRegistrable
{
    string ObtenerRegistro();
}

/// <summary>
/// Define la capacidad de cancelar una notificación antes de su envío.
/// </summary>
public interface ICancelable
{
    bool EsCancelada { get; }
    void Cancelar(string motivo);
}
