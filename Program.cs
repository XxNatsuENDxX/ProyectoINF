// ============================================================================
// ASIGNATURA: Programación II (INF-512)
// PROYECTO:   Sistema Multicanal de Notificaciones — Reto de Diseño Polimórfico
// INTEGRANTES:
//   - Alcequiez Peña, Jeremy Junior
//   - Aybar Montes De Oca, Cesar Adrian
//   - Faneyte Carbuccia, Dennis Keith
//   - Garcia Disla, Jose Armando
// ============================================================================

using SistemaNotificaciones;

Console.Title = "INF-512 | Sistema Multicanal de Notificaciones";
Console.OutputEncoding = System.Text.Encoding.UTF8;

MostrarEncabezado();

bool salir = false;
while (!salir)
{
    Console.WriteLine("\n============================= MENÚ PRINCIPAL =============================");
    Console.WriteLine("1. Ejecutar Demostración Completa del Sistema");
    Console.WriteLine("2. Demostración de Polimorfismo Dinámico (Tipo Declarado vs Tipo Real)");
    Console.WriteLine("3. Demostración de Sobrecarga (Overload) y Sobrescritura (Override)");
    Console.WriteLine("4. Demostración de Interfaces de Capacidad");
    Console.WriteLine("5. Demostración de Validaciones de Datos y Restricciones");
    Console.WriteLine("6. Demostración de Extensibilidad (Canal WhatsApp)");
    Console.WriteLine("7. Salir");
    Console.Write("\nSeleccione una opción (1-7): ");

    string? opcion = Console.ReadLine();
    Console.WriteLine();

    switch (opcion?.Trim())
    {
        case "1":
            EjecutarDemostracionCompleta();
            break;
        case "2":
            DemostrarPolimorfismoTipoDeclaradoVsReal();
            break;
        case "3":
            DemostrarSobrecargaVsSobrescritura();
            break;
        case "4":
            DemostrarInterfacesCapacidad();
            break;
        case "5":
            DemostrarValidacionesEncapsulamiento();
            break;
        case "6":
            DemostrarExtensibilidadWhatsApp();
            break;
        case "7":
            salir = true;
            Console.WriteLine("Saliendo de la aplicación... ¡Hasta luego!");
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Opción no válida. Por favor, seleccione un número entre 1 y 7.");
            Console.ResetColor();
            break;
    }
}

void EjecutarDemostracionCompleta()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║                 DEMOSTRACIÓN COMPLETA DEL SISTEMA MULTICANAL                 ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
    Console.ResetColor();

    var gestor = new GestorNotificaciones();

    Console.WriteLine("\n[1] REGISTRO DE NOTIFICACIONES EN EL GESTOR:");
    var correo1 = new NotificacionCorreo(
        "estudiante@uasd.edu.do",
        "Calificaciones Publicadas",
        "Su calificación final ya está disponible en el portal.",
        Prioridad.Alta
    );
    gestor.Agregar(correo1);

    var sms1 = new NotificacionSMS(
        "+1-809-555-0199",
        "Código de seguridad 2FA: 948210. Expira en 5 minutos.",
        Prioridad.Critica
    );
    gestor.Agregar(sms1);

    var push1 = new NotificacionPush(
        "token-dispositivo-xyz-88",
        "Su paquete ha salido del centro de distribución.",
        Prioridad.Normal
    );
    gestor.Agregar(push1);

    var loteAdicional = new Notificacion[]
    {
        new NotificacionCorreo("soporte@uasd.edu.do", "Boletín Informativo", "Novedades del campus.", Prioridad.Baja),
        new NotificacionPush("token-dispositivo-abc-12", "Recordatorio de reunión a las 6:00 PM.", Prioridad.Alta),
        new NotificacionWhatsApp("+1-829-555-7744", "Confirmación de cita médica para mañana.", Prioridad.Critica)
    };
    gestor.Agregar(loteAdicional);

    Console.WriteLine($"    Total de notificaciones agregadas: {gestor.Total}");

    Console.WriteLine("\n[2] PROGRAMACIÓN DE ENVÍOS (IProgramable):");
    gestor.ProgramarCompatibles(DateTime.Now.AddHours(2));

    Console.WriteLine("\n[3] PROCESAMIENTO Y DESPACHO POR PRIORIDAD:");
    gestor.ProcesarTodas();

    Console.WriteLine("\n[4] CONFIRMACIÓN DE ENTREGA (IConfirmable):");
    gestor.ConfirmarCompatibles();

    gestor.MostrarReporte();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n>>> Demostración completada satisfactoriamente. <<<");
    Console.ResetColor();
}

void DemostrarPolimorfismoTipoDeclaradoVsReal()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- POLIMORFISMO DINÁMICO: TIPO DECLARADO VS TIPO REAL ---");
    Console.ResetColor();

    Console.WriteLine("Instanciando objeto con tipo base declarado:");
    Console.WriteLine("   Notificacion n = new NotificacionSMS(\"+1-809-555-1122\", \"Alerta de cuenta\", Prioridad.Alta);");
    Console.WriteLine("   n.Enviar();\n");

    Notificacion n = new NotificacionSMS("+1-809-555-1122", "Alerta de cuenta", Prioridad.Alta);

    Console.WriteLine($"• Tipo Declarado en compilación: {nameof(Notificacion)}");
    Console.WriteLine($"• Tipo Real en ejecución: {n.GetType().Name}");
    Console.WriteLine("\nInvocando n.Enviar():");
    n.Enviar();
}

void DemostrarSobrecargaVsSobrescritura()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- SOBRECARGA (OVERLOAD) Y SOBRESCRITURA (OVERRIDE) ---");
    Console.ResetColor();

    var correo = new NotificacionCorreo(
        "maria@ejemplo.com",
        "Recordatorio de Pago",
        "Su factura vence el próximo viernes.",
        Prioridad.Normal
    );

    Console.WriteLine("1. Sobrescritura (Override) de Resumen():");
    Console.WriteLine($"   correo.Resumen() -> {correo.Resumen()}\n");

    Console.WriteLine("2. Sobrecarga (Overload) de Resumen(bool):");
    Console.WriteLine($"   correo.Resumen(true) -> {correo.Resumen(true)}\n");

    Console.WriteLine("3. Sobrecargas en GestorNotificaciones:");
    Console.WriteLine("   • Agregar(Notificacion)");
    Console.WriteLine("   • Agregar(IEnumerable<Notificacion>)");
}

void DemostrarInterfacesCapacidad()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- EVALUACIÓN DE INTERFACES DE CAPACIDAD ---");
    Console.ResetColor();

    var lista = new List<Notificacion>
    {
        new NotificacionCorreo("admin@uasd.edu.do", "Servidor", "Reinicio programado.", Prioridad.Normal),
        new NotificacionSMS("+1-809-555-4321", "Código bancario: 1234", Prioridad.Critica),
        new NotificacionPush("token-push-mobile-01", "Alerta push estándar", Prioridad.Baja)
    };

    foreach (var item in lista)
    {
        Console.WriteLine($"Objeto: {item.GetType().Name} ({item.Destinatario})");
        Console.WriteLine($"  IProgramable: {item is IProgramable}");
        Console.WriteLine($"  IConfirmable: {item is IConfirmable}");
        Console.WriteLine($"  IRegistrable: {item is IRegistrable}");
    }
}

void DemostrarValidacionesEncapsulamiento()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- VALIDACIONES Y REGLAS DE NEGOCIO ---");
    Console.ResetColor();

    Console.WriteLine("1. Validación de destinatario obligatorio:");
    try
    {
        _ = new NotificacionSMS("", "Mensaje de prueba", Prioridad.Normal);
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Excepción capturada: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\n2. Validación de límite de 160 caracteres en SMS:");
    string textoLargo = new string('X', 165);
    try
    {
        _ = new NotificacionSMS("+1-809-555-9999", textoLargo, Prioridad.Alta);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Excepción capturada: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\n3. Control de transiciones de estado:");
    Console.WriteLine("La propiedad 'Estado' posee 'protected set', permitiendo transiciones únicamente mediante las operaciones del dominio.");
}

void DemostrarExtensibilidadWhatsApp()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- EXTENSIBILIDAD DE CANALES ---");
    Console.ResetColor();

    var gestor = new GestorNotificaciones();

    Console.WriteLine("Registrando NotificacionWhatsApp en el gestor...");
    var whatsapp = new NotificacionWhatsApp(
        "+1-829-999-0011",
        "Su paquete ha sido entregado en la sucursal de Santo Domingo.",
        Prioridad.Alta
    );
    gestor.Agregar(whatsapp);

    Console.WriteLine("Procesando todas las notificaciones:");
    gestor.ProcesarTodas();
}

void MostrarEncabezado()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("================================================================================");
    Console.WriteLine("                   UNIVERSIDAD AUTÓNOMA DE SANTO DOMINGO (UASD)                 ");
    Console.WriteLine("                  FACULTAD DE CIENCIAS - ESCUELA DE INFORMÁTICA                 ");
    Console.WriteLine("                       INF-512 | PROGRAMACIÓN II                                ");
    Console.WriteLine("                  Reto de Diseño Polimórfico - Notificaciones                   ");
    Console.WriteLine("--------------------------------------------------------------------------------");
    Console.WriteLine(" Integrantes del Equipo:                                                        ");
    Console.WriteLine("   • Alcequiez Peña, Jeremy Junior                                              ");
    Console.WriteLine("   • Aybar Montes De Oca, Cesar Adrian                                          ");
    Console.WriteLine("   • Faneyte Carbuccia, Dennis Keith                                            ");
    Console.WriteLine("   • Garcia Disla, Jose Armando                                                 ");
    Console.WriteLine("================================================================================");
    Console.ResetColor();
}
