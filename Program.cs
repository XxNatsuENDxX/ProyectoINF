using SistemaNotificaciones;

Console.Title = "INF-512 | Sistema Multicanal de Notificaciones - Reto de Diseño Polimórfico";
Console.OutputEncoding = System.Text.Encoding.UTF8;

MostrarEncabezado();

bool salir = false;
while (!salir)
{
    Console.WriteLine("\n============================= MENÚ PRINCIPAL =============================");
    Console.WriteLine("1. Ejecutar Demostración Sincrónica Oficial (60-90 segundos)");
    Console.WriteLine("2. Demostración de Polimorfismo Dinámico (Tipo Declarado vs Tipo Real)");
    Console.WriteLine("3. Demostración de Sobrecarga (Overload) vs Sobrescritura (Override)");
    Console.WriteLine("4. Demostración de Interfaces de Capacidad (IProgramable / IConfirmable)");
    Console.WriteLine("5. Demostración de Validaciones y Encapsulamiento (Límite SMS 160 y Datos Vacíos)");
    Console.WriteLine("6. Demostración de Extensibilidad (Agregar NotificacionWhatsApp sin tocar el Gestor)");
    Console.WriteLine("7. Salir");
    Console.Write("\nSeleccione una opción (1-7): ");

    string? opcion = Console.ReadLine();
    Console.WriteLine();

    switch (opcion?.Trim())
    {
        case "1":
            EjecutarDemostracionSincronica();
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
            Console.WriteLine("Saliendo del sistema... ¡Mucho éxito en la defensa sincrónica!");
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Opción no válida. Por favor, seleccione un número entre 1 y 7.");
            Console.ResetColor();
            break;
    }
}

// =============================================================================
// 1. DEMOSTRACIÓN SINCRÓNICA OFICIAL (60-90 SEGUNDOS)
// =============================================================================
void EjecutarDemostracionSincronica()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║            DEMOSTRACIÓN RÁPIDA PARA DEFENSA SINCRÓNICA (60-90s)             ║");
    Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
    Console.ResetColor();

    var gestor = new GestorNotificaciones();

    Console.WriteLine("\n[1] CREACIÓN DE OBJETOS POLIMÓRFICOS Y AGREGACIÓN AL GESTOR:");
    Console.WriteLine("    Se crean instancias de canales concretos y se agregan a List<Notificacion>.");

    // Uso de Sobrecarga 1: Agregar(Notificacion)
    var correo1 = new NotificacionCorreo(
        "estudiante@uasd.edu.do",
        "Calificaciones Publicadas",
        "Su nota de INF-512 ya está disponible en el portal.",
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

    // Uso de Sobrecarga 2: Agregar(IEnumerable<Notificacion>)
    var loteAdicional = new Notificacion[]
    {
        new NotificacionCorreo("soporte@uasd.edu.do", "Boletín Mensual", "Novedades del campus.", Prioridad.Baja),
        new NotificacionPush("token-dispositivo-abc-12", "Recordatorio de evento hoy a las 6:00 PM.", Prioridad.Alta),
        new NotificacionWhatsApp("+1-829-555-7744", "Hola, confirmamos su cita médica para mañana.", Prioridad.Critica)
    };
    gestor.Agregar(loteAdicional);

    Console.WriteLine($"    Total de notificaciones agregadas: {gestor.Total}");

    // Demostración de Programación (IProgramable)
    Console.WriteLine("\n[2] PROGRAMACIÓN MEDIANTE INTERFAZ (IProgramable):");
    Console.WriteLine("    Solo las notificaciones que implementan IProgramable serán agendadas.");
    gestor.ProgramarCompatibles(DateTime.Now.AddHours(2));

    // Demostración de Envío Polimórfico (Prioridad Critica -> Alta -> Normal -> Baja)
    Console.WriteLine("\n[3] ENVÍO POLIMÓRFICO ORDENADO POR PRIORIDAD:");
    Console.WriteLine("    Note cómo las notificaciones CRÍTICAS se procesan PRIMERO,");
    Console.WriteLine("    y cada objeto ejecuta su propio método Enviar() especializado.");
    gestor.ProcesarTodas();

    // Demostración de Confirmación (IConfirmable)
    Console.WriteLine("\n[4] CONFIRMACIÓN DE ENTREGA (IConfirmable):");
    Console.WriteLine("    Solo los canales compatibles con acuse de recibo confirman la entrega.");
    gestor.ConfirmarCompatibles();

    // Reporte Final
    gestor.MostrarReporte();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n>>> Demostración completada con éxito en menos de 90 segundos. <<<");
    Console.ResetColor();
}

// =============================================================================
// 2. POLIMORFISMO DINÁMICO: TIPO DECLARADO VS TIPO REAL
// =============================================================================
void DemostrarPolimorfismoTipoDeclaradoVsReal()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- DEMOSTRACIÓN: TIPO DECLARADO VS TIPO REAL ---");
    Console.ResetColor();

    Console.WriteLine("Línea de código de ejemplo:");
    Console.WriteLine("   Notificacion n = new NotificacionSMS(\"+1-809-555-1122\", \"Alerta de cuenta\", Prioridad.Alta);");
    Console.WriteLine("   n.Enviar();\n");

    // Tipo declarado: Notificacion (la clase base)
    // Tipo real: NotificacionSMS (la instancia concreta en memoria heap)
    Notificacion n = new NotificacionSMS("+1-809-555-1122", "Alerta de cuenta", Prioridad.Alta);

    Console.WriteLine($"• Tipo Declarado en tiempo de compilación: {nameof(Notificacion)}");
    Console.WriteLine($"• Tipo Real en tiempo de ejecución (GetType): {n.GetType().Name}");
    Console.WriteLine("\nInvocando n.Enviar()...");
    n.Enviar();

    Console.WriteLine("\nEXPLICACIÓN PARA EL DOCENTE:");
    Console.WriteLine("El compilador revisa que el método Enviar() exista en el tipo declarado 'Notificacion'.");
    Console.WriteLine("En tiempo de ejecución (runtime), el despachador dinámico examina la tabla de métodos virtuales (vtable)");
    Console.WriteLine("del objeto real y ejecuta la versión 'override' de NotificacionSMS. ¡Eso es polimorfismo dinámico!");
}

// =============================================================================
// 3. SOBRECARGA (OVERLOAD) VS SOBRESCRITURA (OVERRIDE)
// =============================================================================
void DemostrarSobrecargaVsSobrescritura()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- DEMOSTRACIÓN: SOBRECARGA (OVERLOAD) VS SOBRESCRITURA (OVERRIDE) ---");
    Console.ResetColor();

    var correo = new NotificacionCorreo(
        "maria@ejemplo.com",
        "Recordatorio de Pago",
        "Su factura vence el próximo viernes.",
        Prioridad.Normal
    );

    Console.WriteLine("1. SOBRESCRITURA (OVERRIDE):");
    Console.WriteLine("   NotificacionCorreo sobrescribe virtualmente Resumen() de la clase base Notificacion:");
    Console.WriteLine($"   Resultado de correo.Resumen():\n   -> {correo.Resumen()}\n");

    Console.WriteLine("2. SOBRECARGA (OVERLOAD):");
    Console.WriteLine("   NotificacionCorreo tiene otra versión de Resumen que recibe un booleano: Resumen(bool incluirMensaje):");
    Console.WriteLine($"   Resultado de correo.Resumen(true):\n   -> {correo.Resumen(true)}\n");

    Console.WriteLine("3. SOBRECARGA EN EL GESTOR:");
    Console.WriteLine("   El GestorNotificaciones ofrece:");
    Console.WriteLine("   • Agregar(Notificacion)               -> para un solo elemento");
    Console.WriteLine("   • Agregar(IEnumerable<Notificacion>)  -> para colecciones o arrays\n");

    Console.WriteLine("PREGUNTA CLAVE DEL DOCENTE:");
    Console.WriteLine("¿Por qué no basta con cambiar el tipo de retorno para crear una sobrecarga?");
    Console.WriteLine("RESPUESTA: En C#, la firma del método para fines de sobrecarga se compone del nombre");
    Console.WriteLine("y la lista de tipos de parámetros. El tipo de retorno no es parte de la firma de sobrecarga,");
    Console.WriteLine("porque el compilador no podría determinar a cuál método llamar en llamadas que no asignen el retorno.");
}

// =============================================================================
// 4. INTERFACES DE CAPACIDAD
// =============================================================================
void DemostrarInterfacesCapacidad()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- DEMOSTRACIÓN: INTERFACES DE CAPACIDAD OPCIONAL ---");
    Console.ResetColor();

    var lista = new List<Notificacion>
    {
        new NotificacionCorreo("admin@uasd.edu.do", "Servidor", "Reinicio programado.", Prioridad.Normal),
        new NotificacionSMS("+1-809-555-4321", "Código bancario: 1234", Prioridad.Critica),
        new NotificacionPush("token-push-mobile-01", "Alerta push estándar", Prioridad.Baja)
    };

    Console.WriteLine("Analizando capacidades de cada tipo:\n");
    foreach (var item in lista)
    {
        Console.WriteLine($"Objeto: {item.GetType().Name} ({item.Destinatario})");
        Console.WriteLine($"  ¿Es IProgramable? {item is IProgramable}");
        Console.WriteLine($"  ¿Es IConfirmable? {item is IConfirmable}");
        Console.WriteLine($"  ¿Es IRegistrable? {item is IRegistrable}");
    }

    Console.WriteLine("\nJUSTIFICACIÓN PARA EL DOCENTE:");
    Console.WriteLine("Si hubiésemos definido Programar() o ConfirmarEntrega() como métodos abstractos en Notificacion,");
    Console.WriteLine("NotificacionPush se vería obligada a implementar ConfirmarEntrega() sin que tenga sentido,");
    Console.WriteLine("violando el Principio de Segregación de Interfaces (ISP).");
}

// =============================================================================
// 5. VALIDACIONES Y ENCAPSULAMIENTO
// =============================================================================
void DemostrarValidacionesEncapsulamiento()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- DEMOSTRACIÓN: VALIDACIONES Y ENCAPSULAMIENTO ---");
    Console.ResetColor();

    Console.WriteLine("1. PRUEBA: Validación temprana de destinatario vacío en la clase base:");
    try
    {
        _ = new NotificacionSMS("", "Mensaje de prueba", Prioridad.Normal);
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✓ Excepción esperada capturada: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\n2. PRUEBA: Validación del límite de 160 caracteres en NotificacionSMS:");
    string textoLargo = new string('A', 165);
    try
    {
        _ = new NotificacionSMS("+1-809-555-9999", textoLargo, Prioridad.Alta);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✓ Excepción esperada capturada: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine("\n3. ENCAPSULAMIENTO DEL ESTADO:");
    Console.WriteLine("La propiedad 'Estado' de Notificacion tiene 'protected set'.");
    Console.WriteLine("Un usuario del objeto no puede hacer: notificacion.Estado = EstadoNotificacion.Confirmada;");
    Console.WriteLine("Solo las operaciones autorizadas de la jerarquía (Enviar, ConfirmarEntrega) pueden cambiar el estado.");
}

// =============================================================================
// 6. EXTENSIBILIDAD: AGREGAR NOTIFICACIONWHATSAPP
// =============================================================================
void DemostrarExtensibilidadWhatsApp()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("--- DEMOSTRACIÓN: EXTENSIBILIDAD SIN MODIFICAR EL GESTOR ---");
    Console.ResetColor();

    var gestor = new GestorNotificaciones();

    Console.WriteLine("Agregando una nueva NotificacionWhatsApp al GestorNotificaciones...");
    var whatsapp = new NotificacionWhatsApp(
        "+1-829-999-0011",
        "Su paquete ha sido entregado en la sucursal de Santo Domingo.",
        Prioridad.Alta
    );
    gestor.Agregar(whatsapp);

    Console.WriteLine("\nEjecutando gestor.ProcesarTodas():");
    Console.WriteLine("Note que GestorNotificaciones NO tuvo que cambiar ni una sola línea de código.");
    gestor.ProcesarTodas();
}

void MostrarEncabezado()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("================================================================================");
    Console.WriteLine("                   UNIVERSIDAD AUTÓNOMA DE SANTO DOMINGO (UASD)                 ");
    Console.WriteLine("                  FACULTAD DE CIENCIAS - ESCUELA DE INFORMÁTICA                 ");
    Console.WriteLine("           INF-512 PROGRAMACIÓN II | RETO DE DISEÑO POLIMÓRFICO                 ");
    Console.WriteLine("================================================================================");
    Console.ResetColor();
}

