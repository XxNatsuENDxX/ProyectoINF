// ============================================================================
// ASIGNATURA: Programación II (INF-512)
// PROYECTO:   Sistema Multicanal de Notificaciones — Reto de Diseño Polimórfico
// INTEGRANTES:
//   - Alcequiez Peña, Jeremy Junior
//   - Aybar Montes De Oca, Cesar Adrian
//   - Faneyte Carbuccia, Dennis Keith
//   - Garcia Disla, Jose Armando
// ============================================================================

namespace SistemaNotificaciones;

/// <summary>
/// Representa los posibles estados en el ciclo de vida de una notificación.
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
/// Niveles de prioridad para el ordenamiento y procesamiento de notificaciones.
/// </summary>
public enum Prioridad
{
    Baja = 1,
    Normal = 2,
    Alta = 3,
    Critica = 4
}
