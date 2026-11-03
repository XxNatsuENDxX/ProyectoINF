namespace SistemaNotificaciones;

/// <summary>
/// Representa el ciclo de vida de una notificación.
/// Las transiciones son controladas por la jerarquía mediante encapsulamiento (set protegido).
/// </summary>
public enum EstadoNotificacion
{
    Pendiente,
    Programada,
    Enviada,
    Fallida,
    Confirmada,
    Cancelada
}

/// <summary>
/// Nivel de criticidad para el procesamiento de notificaciones.
/// Los valores numéricos permiten que el GestorNotificaciones ordene por prioridad descendente:
/// Critica (4) -> Alta (3) -> Normal (2) -> Baja (1).
/// </summary>
public enum Prioridad
{
    Baja = 1,
    Normal = 2,
    Alta = 3,
    Critica = 4
}

