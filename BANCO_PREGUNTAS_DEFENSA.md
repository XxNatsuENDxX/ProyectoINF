# Guía Maestra de Defensa Sincrónica y Banco de Preguntas Resuelto
**Asignatura:** Programación II (INF-512) | **Docente:** Prof. Víctor Núñez  
**Instrumento:** Banco de Preguntas para Defensa Sincrónica — Reto de Diseño Polimórfico  

Esta guía contiene la preparación exhaustiva para que **cada integrante del equipo** obtenga la máxima calificación (Nivel 2: Respuesta Sólida) en la defensa oral individual.

---

## 1. Protocolo de la Defensa y Guion de Demostración (60–90 Segundos)

El docente iniciará la evaluación pidiendo:  
> *"Por favor, realicen una demostración rápida de 60 a 90 segundos de su sistema antes de comenzar las preguntas."*

### Guion Recomendado Paso a Paso:
1. **Ejecutar la aplicación:** Abrir la terminal y escribir `dotnet run`.
2. **Seleccionar Opción 1:** *"Ejecutar Demostración Sincrónica Oficial (60-90 segundos)"*.
3. **Explicación verbal (mientras corre la consola):**
   > *"Profesor, nuestro sistema administra un conjunto heterogéneo de notificaciones en un único contenedor polimórfico `List<Notificacion>` ubicado en `GestorNotificaciones`. Demostramos dos sobrecargas de `Agregar`: una para objetos individuales y otra para colecciones en lote.*  
   > *A continuación, invocamos la capacidad opcional de `IProgramable` solo en los canales compatibles mediante coincidencia de patrones. Luego, el gestor ejecuta `ProcesarTodas()`, aplicando la regla de negocio de ordenar por prioridad: las notificaciones Críticas se despachan primero. En cada iteración del bucle, se ejecuta el despacho virtual polimórfico de `Enviar()`, donde cada canal (Correo, SMS, Push y WhatsApp) corre su comportamiento especializado.*  
   > *Finalmente, se confirma la entrega con `IConfirmable` en los canales que soportan acuse de recibo, y se emite el reporte consolidado."*
4. **Tiempo estimado:** 70 segundos. Claridad absoluta, vocabulario técnico preciso y demostración funcional.

---

## 2. Escala de Calificación Docente

| Nivel | Puntos | Criterio del Profesor |
| :---: | :---: | :--- |
| **Sólida** | **2** | Explica con precisión, relaciona el concepto con líneas concretas de su código y puede anticipar consecuencias de cambios. |
| **Parcial** | **1** | Reconoce el concepto, pero la justificación es incompleta o requiere pistas del profesor. |
| **Insuficiente** | **0** | No identifica el mecanismo, responde por memoria/intuición o contradice el código. |

---

## 3. Preguntas Conceptuales Fundamentales

### Pregunta 3.1: ¿Dónde ocurre exactamente el polimorfismo dinámico en su programa?
- **Respuesta Sólida:**  
  *"Ocurre en la clase `GestorNotificaciones.cs`, dentro del método `ProcesarTodas()` (líneas 63 a 80). Específicamente en la línea donde se declara la variable de iteración `foreach (Notificacion n in listaOrdenada)` y se invoca `n.Enviar()`.*  
  *Allí, `n` es una referencia del tipo base abstracto `Notificacion`, pero cada objeto real en memoria es una instancia de `NotificacionSMS`, `NotificacionCorreo`, `NotificacionPush` o `NotificacionWhatsApp`. En tiempo de ejecución, el despachador dinámico del CLR consulta la tabla de métodos virtuales (vtable) del objeto real y ejecuta la versión `override` correspondiente."*

---

### Pregunta 3.2: ¿Cuál es la diferencia entre el tipo declarado y el tipo real de una variable de su colección?
- **Respuesta Sólida:**  
  *"Tomemos como ejemplo la línea:*  
  ```csharp
  Notificacion n = new NotificacionSMS("+1-809-555-0199", "Mensaje", Prioridad.Alta);
  ```  
  *• **Tipo Declarado (`Notificacion`):** Es el tipo con el que se declara la variable ante el compilador. Determina qué miembros y métodos son directamente accesibles en tiempo de compilación. Por ejemplo, a través de `n` solo puedo invocar los métodos definidos en `Notificacion` (como `n.Enviar()` o `n.Resumen()`), pero no puedo acceder directamente a miembros exclusivos de SMS a menos que haga un cast.*  
  *• **Tipo Real (`NotificacionSMS`):** Es el tipo del objeto efectivamente instanciado en la memoria heap en tiempo de ejecución. Determina la implementación concreta que se ejecutará cuando se invoque un miembro virtual o abstracto mediante despacho dinámico."*

---

### Pregunta 3.3: ¿Por qué `Enviar()` es `abstract`/`virtual` y no un método normal idéntico para todas las clases?
- **Respuesta Sólida:**  
  *"Porque un canal de notificación no tiene una implementación universal común de transmisión física: enviar un SMS requiere comunicarse con una pasarela de telefonía celular con límite de caracteres, enviar un correo requiere protocolo SMTP con asunto, y enviar una alerta push requiere contactar a servicios como FCM o APNs.*  
  *Definir `Enviar()` como `abstract` en la clase base obliga a cada subclase a suministrar su propia implementación especializada, mientras que permite al consumidor (`GestorNotificaciones`) invocar a todas uniformemente a través del contrato común de la clase base sin acoplarse a detalles técnicos de cada canal."*

---

### Pregunta 3.4: ¿Qué diferencia existe entre `override` y `overload` en su solución?
- **Respuesta Sólida:**  
  *"Son dos mecanismos totalmente distintos:*  
  *1. **Sobrescritura (`override`):** Reemplaza el comportamiento de un método heredado (`abstract` o `virtual`) que posee exactamente la misma firma. Es polimorfismo dinámico resuelto en tiempo de ejecución. En nuestra solución, `NotificacionSMS` y `NotificacionCorreo` hacen `override` de `Enviar()` y de `Resumen()`.*  
  *2. **Sobrecarga (`overload`):** Define dos o más métodos dentro del mismo ámbito con el **mismo nombre**, pero con **distinta lista de parámetros** (diferentes tipos, cantidad u orden). Es polimorfismo estático resuelto en tiempo de compilación. En nuestra solución, `GestorNotificaciones` tiene sobrecargas de `Agregar(Notificacion)` y `Agregar(IEnumerable<Notificacion>)`; y `NotificacionCorreo` sobrecarga `Resumen()` y `Resumen(bool incluirMensajeCompleto)`."*

---

### Pregunta 3.5: ¿Por qué no basta con cambiar el tipo de retorno para crear una sobrecarga?
- **Respuesta Sólida:**  
  *"Porque en la especificación del lenguaje C#, la **firma de un método** para efectos de sobrecarga está compuesta únicamente por su identificador (nombre) y el número, tipo y modificadores de sus parámetros. El tipo de retorno no forma parte de la firma.*  
  *Si el compilador permitiera métodos con idéntico nombre y parámetros pero distinto retorno (como `int Enviar(int c)` y `double Enviar(int c)`), cuando un programador invoque simplemente `Enviar(5);` sin asignar el valor resultante a una variable, el compilador no tendría ninguna forma de saber cuál de las dos implementaciones ejecutar, produciendo ambigüedad y el error de compilación CS0111."*

---

### Pregunta 3.6: ¿Qué ventaja concreta obtiene al usar `List<Notificacion>`?
- **Respuesta Sólida:**  
  *"Obtenemos tres ventajas fundamentales:*  
  *1. **Procesamiento uniforme:** Podemos iterar y operar sobre cualquier canal con un único algoritmo sin código duplicado.*  
  *2. **Bajo acoplamiento:** El gestor depende de la abstracción `Notificacion`, no de las clases concretas.*  
  *3. **Principio Abierto/Cerrado (OCP) y extensibilidad:** Si el día de mañana agregamos 10 canales nuevos (`NotificacionTelegram`, `NotificacionSlack`), `List<Notificacion>` y el bucle de procesamiento permanecen idénticos, sin necesidad de ser modificados ni recompilados."*

---

## 4. Herencia e Interfaces

### Pregunta 4.1: ¿Por qué `IProgramable` es una interfaz y no un método abstracto obligatorio de `Notificacion`?
- **Respuesta Sólida:**  
  *"Porque no todas las notificaciones del negocio tienen por qué soportar programación diferida. Por ejemplo, una alerta de emergencia o un SMS de verificación bancaria suelen ser de envío inmediato.*  
  *Las interfaces modelan **capacidades opcionales y transversales**. Si hubiéramos definido `Programar()` como método abstracto en `Notificacion`, habríamos forzado a canales como `NotificacionSMS` a implementar un método sin sentido que arroje `NotSupportedException` o quede vacío, violando abiertamente el **Principio de Segregación de Interfaces (ISP)** de SOLID."*

---

### Pregunta 4.2: ¿Qué problema de diseño aparecería si todas las clases implementaran todas las interfaces solo para cumplir la tarea?
- **Respuesta Sólida:**  
  *"Aparecerían contratos artificiales, métodos ficticios o vacíos ('dummy methods'), violación del principio de responsabilidad única, código propenso a errores en tiempo de ejecución (si lanzan excepciones de 'no soportado') y una pérdida total de fidelidad semántica en el modelo de dominio. Una clase solo debe implementar una interfaz si realmente puede satisfacer de manera natural el contrato que dicha interfaz promete."*

---

### Pregunta 4.3: Muestre una clase que implemente dos contratos. ¿Qué aporta cada contrato?
- **Respuesta Sólida:**  
  *"En nuestro proyecto, la clase `NotificacionSMS.cs` implementa dos contratos: `IConfirmable` e `IRegistrable` (y `NotificacionCorreo` implementa tres: `IProgramable`, `IConfirmable`, `IRegistrable`).*  
  *• `IConfirmable`: Aporta la capacidad de registrar un acuse de recibo cuando la operadora confirma la entrega (`ConfirmarEntrega()`), permitiendo transicionar el estado a `Confirmada`.*  
  *• `IRegistrable`: Aporta la capacidad de generar un registro estructurado (`ObtenerRegistro()`) para auditoría y trazabilidad del envío.*  
  *Cada contrato desacopla una responsabilidad específica: quien audita solo interactúa con `IRegistrable`, y quien verifica recepción interactúa con `IConfirmable`."*

---

### Pregunta 4.4: ¿Podemos almacenar un objeto únicamente tipado como `IConfirmable` dentro de `List<Notificacion>`?
- **Respuesta Sólida:**  
  *"No directamente. En C# el sistema de tipos es estático y estricto. Una referencia de tipo `IConfirmable` no tiene una relación de herencia con la clase `Notificacion`, por lo que el compilador no permite agregarla a `List<Notificacion>`.*  
  *Solo sería posible si se comprueba en tiempo de ejecución que el objeto concreto detrás de la referencia también deriva de `Notificacion` (por ejemplo mediante `if (obj is Notificacion n) lista.Add(n);`) o realizando un cast explícito."*

---

### Pregunta 4.5: ¿Cuándo preferiría una interfaz frente a una nueva clase base?
- **Respuesta Sólida:**  
  *"Preferiría una **interfaz** cuando:*  
  *1. Deseo modelar un comportamiento o capacidad transversal que puede ser compartido por clases en ramas completamente distintas de la jerarquía (o que no tienen parentesco alguno).*  
  *2. Se requiere implementar múltiples capacidades, ya que C# no soporta herencia múltiple de clases pero sí de interfaces.*  
  *3. Solo necesito definir un contrato de operaciones sin proveer estado ni implementación compartida.*  
  *En cambio, una **clase base** se prefiere cuando existe una relación estricta 'es-un' y hay atributos comunes (`Id`, `Destinatario`, `Mensaje`, `Estado`) y lógica de validación o inicialización que se deben reutilizar."*

---

## 5. Encapsulamiento, Estado y Reglas de Negocio

### Pregunta 5.1: ¿Por qué `Estado` puede tener `set` protegido en lugar de `set` público?
- **Respuesta Sólida:**  
  *"Porque el estado de una notificación debe seguir una máquina de estados consistente (`Pendiente` $\rightarrow$ `Programada` / `Enviada` $\rightarrow$ `Confirmada` / `Fallida`). Con `protected set`, únicamente la clase base `Notificacion` y sus clases derivadas pueden modificar el estado como consecuencia de operaciones formales del dominio (como ejecutar `Enviar()` o `ConfirmarEntrega()`).*  
  *Si tuviera `set` público, cualquier consumidor externo del sistema podría asignar arbitrariamente `notificacion.Estado = EstadoNotificacion.Confirmada;` sin que la notificación haya sido enviada ni confirmada por el canal real, corrompiendo la integridad del sistema."*

---

### Pregunta 5.2: ¿Dónde ubicaron la validación de los 160 caracteres del SMS y por qué?
- **Respuesta Sólida:**  
  *"La ubicamos en la clase `NotificacionSMS.cs` (líneas 16 a 33), mediante una constante privada `public const int MaximoCaracteres = 160;` y un método de validación ejecutado directamente en su constructor.*  
  *Pertenece estrictamente al tipo `NotificacionSMS` porque la restricción de 160 caracteres es una regla de negocio y técnica exclusiva del protocolo SMS celular. No debe estar en `Program/Main` (lo que violaría el encapsulamiento) ni en la clase base `Notificacion` (porque los correos y las alertas push no tienen ese límite de 160 caracteres)."*

---

### Pregunta 5.3: ¿Qué debe suceder si falla `Enviar()`?
- **Respuesta Sólida:**  
  *"El sistema debe garantizar coherencia y consistencia en el estado del objeto. Dentro del bloque `try-catch` de `Enviar()`, si ocurre una falla en la comunicación o una excepción, el estado de la notificación se transiciona inmediatamente a `EstadoNotificacion.Fallida`.*  
  *De esta manera, el objeto refleja con exactitud la realidad de lo sucedido, la excepción se registra o propaga ordenadamente, y se evita que una notificación errónea quede en estado 'Enviada' o 'Pendiente'."*

---

### Pregunta 5.4: ¿Quién debe ordenar las notificaciones por prioridad: cada notificación o el gestor?
- **Respuesta Sólida:**  
  *"El **gestor (`GestorNotificaciones`)**. Una notificación individual solo conoce sus propios atributos (su destinatario, su mensaje y su prioridad particular); no tiene conocimiento ni autoridad sobre el conjunto de notificaciones existentes en el sistema.*  
  *El `GestorNotificaciones`, como orquestador y servicio del contenedor, tiene la visión global de la colección y es el responsable legítimo de aplicar las políticas de despacho, ordenando las notificaciones mediante `OrderByDescending(n => n.Prioridad)` antes de procesarlas."*

---

### Pregunta 5.5: ¿Qué riesgo existe si `Destinatario` o `Mensaje` pueden quedar vacíos?
- **Respuesta Sólida:**  
  *"El riesgo es la existencia de 'objetos zombi' o inválidos en el sistema. Si se permite instanciar una notificación sin destinatario o sin mensaje, la falla ocurrirá tardíamente durante la ejecución del envío por red, desperdiciando recursos y provocando excepciones difíciles de rastrear.*  
  *Por eso aplicamos **validación temprana (Fail-Fast)** en el constructor de `Notificacion.cs`, lanzando un `ArgumentException` inmediato si cualquiera de los dos parámetros es nulo o contiene solo espacios en blanco."*

---

## 6. Lectura de Código y Predicción (Preguntas del Examen)

### Caso 1: Despacho Virtual
```csharp
Notificacion n = new NotificacionSMS("+1-809-555-0101", "Alerta", Prioridad.Alta);
n.Enviar();
```
- **Pregunta:** ¿Qué implementación se ejecuta y por qué?
- **Respuesta Sólida:**  
  *"Se ejecuta la implementación de `NotificacionSMS.Enviar()`. Aunque la variable `n` tiene tipo declarado `Notificacion` (la clase base), el objeto instanciado en tiempo de ejecución es de tipo real `NotificacionSMS`. Dado que el método `Enviar()` está declarado como `abstract` en la clase base y sobrescrito con `override` en la clase derivada, el despacho dinámico en tiempo de ejecución selecciona la implementación del tipo real."*

---

### Caso 2: Principio Abierto/Cerrado en el Bucle
```csharp
foreach (Notificacion n in notificaciones)
{
    n.Enviar();
}
```
- **Pregunta:** Si agregamos `NotificacionWhatsApp` correctamente a la lista, ¿hay que modificar este foreach?
- **Respuesta Sólida:**  
  *"**No, no hay que modificar absolutamente nada.** Ese es el beneficio central del diseño polimórfico y del Principio Abierto/Cerrado (OCP). El bucle opera contra el contrato abstracto `Notificacion`. Al ingresar una instancia de `NotificacionWhatsApp`, el mismo bucle invocará automáticamente su método `Enviar()` especializado mediante despacho virtual sin requerir ninguna modificación en el código consumidor."*

---

### Caso 3: Firma de Sobrecarga
```csharp
public int Enviar(int codigo) => codigo;
public double Enviar(int codigo) => codigo;
```
- **Pregunta:** ¿Esto representa una sobrecarga válida?
- **Respuesta Sólida:**  
  *"**No, no es una sobrecarga válida.** En C#, dos métodos en la misma clase no pueden diferir únicamente en el tipo de retorno si tienen el mismo identificador y la misma lista de tipos de parámetros. El compilador generará el error de compilación CS0111 porque sus firmas son idénticas para la resolución de sobrecarga."*

---

### Caso 4: Anti-Patrón de Verificación de Tipos
```csharp
if (n is NotificacionSMS)
    ((NotificacionSMS)n).Enviar();
else if (n is NotificacionCorreo)
    ((NotificacionCorreo)n).Enviar();
```
- **Pregunta:** ¿Por qué esta solución debilita el objetivo del ejercicio?
- **Respuesta Sólida:**  
  *"Porque destruye las ventajas del polimorfismo y genera **alto acoplamiento**. El código consumidor vuelve a depender de los tipos concretos; viola el Principio Abierto/Cerrado (OCP), ya que cada vez que agreguemos un canal nuevo (`NotificacionWhatsApp`, `NotificacionTelegram`) nos veríamos forzados a modificar este bloque `if/else`, introduciendo riesgo de errores y haciendo el sistema frágil e inmantenible."*

---

## 7. Preguntas de Extensibilidad y Cambio de Requisito

### Pregunta 7.1: Agregue conceptualmente `NotificacionWhatsApp`, que puede registrar y confirmar entrega, pero no programarse. ¿Qué debe crear o modificar?
- **Respuesta Sólida:**  
  *"1. Creo una nueva clase `public class NotificacionWhatsApp : Notificacion, IConfirmable, IRegistrable`.*  
  *2. En su constructor paso destinatario y mensaje al constructor base.*  
  *3. Hago `override` del método `Enviar()` con la lógica de la API de WhatsApp.*  
  *4. Implemento `ConfirmarEntrega()` (de `IConfirmable`) y `ObtenerRegistro()` (de `IRegistrable`).*  
  *5. Agrego instancias al `GestorNotificaciones`.  
  **Importante:** Ni `GestorNotificaciones` ni su bucle `foreach` sufren la más mínima modificación."*

---

### Pregunta 7.2: El cliente agrega `NotificacionTelegram`. ¿Qué parte de su diseño debería permanecer intacta?
- **Respuesta Sólida:**  
  *"Permanecen 100% intactos:*  
  *• La clase base `Notificacion`.*  
  *• Las interfaces (`IProgramable`, `IConfirmable`, etc.).*  
  *• Toda la clase `GestorNotificaciones` y su método `ProcesarTodas()`.*  
  *• Los demás canales existentes (`NotificacionCorreo`, `NotificacionSMS`, `NotificacionPush`).*  
  *Únicamente se crea el nuevo archivo `NotificacionTelegram.cs` heredando de `Notificacion` e implementando las interfaces que correspondan."*

---

### Pregunta 7.3: Ahora algunas notificaciones deben poder cancelarse. ¿Lo pondría en `Notificacion` o crearía `ICancelable`?
- **Respuesta Sólida:**  
  *"Crearía una interfaz `ICancelable` con el método `void Cancelar(string motivo);`.*  
  *Razón: La cancelación es una capacidad opcional que no necesariamente aplica a todos los canales ni en cualquier momento (por ejemplo, un SMS emitido instantáneamente no se puede cancelar una vez transmitido a la red celular). Si lo pusiéramos en la clase base, forzaríamos a todos los canales a lidiar con cancelaciones. Con la interfaz, solo los canales que efectivamente permitan cancelación antes del envío la implementan."*

---

### Pregunta 7.4: Se pide un nuevo canal que no puede confirmar entrega. ¿Qué evita tener que implementar un método sin sentido?
- **Respuesta Sólida:**  
  *"La segregación de capacidades mediante interfaces (`IConfirmable`). Como la confirmación de entrega no es un método de la clase base `Notificacion`, el nuevo canal simplemente no declara la interfaz `IConfirmable` en su definición. No tiene que implementar ningún método vacío ni lanzar `NotSupportedException`."*

---

### Pregunta 7.5: Se solicita procesar primero Crítica, luego Alta, Normal y Baja. ¿Qué objeto debería asumir esa política?
- **Respuesta Sólida:**  
  *"El **`GestorNotificaciones`**. La política de ordenamiento pertenece al servicio orquestador que administra la colección, utilizando `OrderByDescending(n => n.Prioridad)` antes de despacharlas. Las notificaciones individuales solo encapsulan sus propios datos y la lógica de su canal; no deben ordenar a sus pares."*

---

### Pregunta 7.6: Se quiere agregar una nueva clase sin recompilar el `foreach`. ¿Qué característica del diseño lo permite?
- **Respuesta Sólida:**  
  *"La programación contra abstracciones (el tipo base `Notificacion`) combinada con el despacho dinámico en tiempo de ejecución (despacho virtual polimórfico). El `foreach` está compilado para invocar `Notificacion.Enviar()`, cuya dirección de ejecución se resuelve dinámicamente en runtime consultando la vtable del objeto real."*

---

## 8. Preguntas sobre Decisiones de Diseño

### Pregunta 8.1: ¿Qué parte de su solución cambiaría si `Notificacion` dejara de ser abstracta?
- **Respuesta Sólida:**  
  *"Cambiarían tres cosas clave:*  
  *1. Se permitiría instanciar directamente `new Notificacion(...)`, lo cual carece de sentido de negocio porque no hay un canal físico asociado.*  
  *2. El método `Enviar()` ya no podría ser `abstract`; tendría que ser `virtual` y proporcionar una implementación base por defecto (o vacía).*  
  *3. Las subclases ya no estarían obligadas por el compilador a implementar `Enviar()`; si una clase olvida hacer `override`, compilaría igualmente pero usaría la lógica base, introduciendo bugs silenciosos."*

---

### Pregunta 8.2: ¿Qué comportamiento común sí tendría sentido implementar en la clase base?
- **Respuesta Sólida:**  
  *"Tiene sentido implementar:*  
  *1. Validación de campos universales obligatorios (`Destinatario`, `Mensaje`).*  
  *2. Inicialización común de propiedades (`Id`, `Prioridad`, `Estado = Pendiente`, `FechaCreacion`).*  
  *3. Verificación de transiciones de estado reutilizables, como `ValidarEnvioPendiente()` para impedir reenvíos accidentales.*  
  *4. Implementación base de `Resumen()` que los hijos pueden reutilizar mediante `base.Resumen()`.*  
  *Lo que NO tendría sentido es poner detalles específicos de protocolos (SMTP, pasarelas SMS o tokens push)."*

---

### Pregunta 8.3: ¿Su `GestorNotificaciones` está acoplado a los canales concretos? Muestre evidencia.
- **Respuesta Sólida:**  
  *"**No está acoplado.** Si abrimos `GestorNotificaciones.cs`:*  
  *• La colección interna es `List<Notificacion>`.*  
  *• Los métodos de agregar reciben `Notificacion` o `IEnumerable<Notificacion>`.*  
  *• En el método `ProcesarTodas()`, el bucle solo hace `n.Enviar()`.*  
  *No existe una sola referencia a `NotificacionSMS`, `NotificacionCorreo` ni `NotificacionPush`. El gestor ni siquiera necesita saber que esas clases existen para poder despacharlas."*

---

## 9. Preguntas sobre el Uso de Inteligencia Artificial

> **IMPORTANTE:** El mandato permite el uso de IA como herramienta de apoyo, pero exige criterio técnico propio, capacidad de justificación y comprensión causal línea por línea.

### Pregunta 9.1: ¿Qué recomendación de IA aceptaron y cómo verificaron que era correcta?
- **Respuesta Sólida:**  
  *"Aceptamos la recomendación de estructurar `GestorNotificaciones` exponiendo su lista interna como `IReadOnlyList<Notificacion>` mediante `.AsReadOnly()`. Verificamos su validez comprobando que protege la colección de manipulaciones externas accidentales sin impedir su lectura, aplicando el principio de menor privilegio y garantizando un encapsulamiento sólido."*

---

### Pregunta 9.2: ¿Qué recomendación de IA rechazaron o modificaron? ¿Por qué?
- **Respuesta Sólida:**  
  *"La IA inicialmente sugería implementar un patrón 'Template Method' donde `Enviar()` en la clase base era no virtual y llamaba internamente a un método protegido `protected abstract void EnviarPorCanal()`.*  
  *Rechazamos esa propuesta porque, según los requisitos de nuestro reto y el examen sincrónico de Programación II, el docente evalúa que `Enviar()` sea directamente el método virtual/abstracto sobrescrito con `override` por cada canal, de modo que `n.Enviar()` demuestre con total pureza el despacho dinámico polimórfico y el tipo declarado frente al tipo real."*

---

### Pregunta 9.3: Seleccione una parte sugerida por IA y explíquela línea por línea.
- **Respuesta Sólida:** (Tomar por ejemplo el constructor de `NotificacionSMS.cs`):  
  ```csharp
  public NotificacionSMS(string telefono, string mensaje, Prioridad prioridad)
      : base(telefono, ValidarMensajeSMS(mensaje), prioridad)
  {
  }
  ```  
  *• **Línea 1:** Declaración del constructor con sus tres parámetros obligatorios.*  
  *• **Línea 2:** Invocación del constructor de la clase base (`: base(...)`). El segundo argumento ejecuta el método estático privado `ValidarMensajeSMS(mensaje)` antes de ceder el control al constructor base. Si el mensaje supera 160 caracteres, la excepción se dispara antes de inicializar la clase base, garantizando que jamás existirá en memoria un objeto SMS inválido.*  
  *• **Línea 3-4:** Cuerpo del constructor vacío, ya que la inicialización completa y el encapsulamiento quedan garantizados."*

---

### Pregunta 9.4: Si quitamos una línea sugerida por IA, ¿qué efecto produce?
- **Respuesta Sólida:**  
  *"Por ejemplo, si en `NotificacionCorreo.cs` quitamos la palabra reservada `override` en `public override void Enviar()`:*  
  *El código **no compilará**, arrojando el error CS0534 indicando que `NotificacionCorreo` no implementa el miembro abstracto heredado `Notificacion.Enviar()`. Y si `Enviar()` fuera virtual con código base en lugar de abstracto, quitar `override` provocaría 'ocultamiento' accidental (shadowing), ejecutando el método de la clase base en lugar del especializado al invocarlo polimórficamente."*

---

### Pregunta 9.5: ¿Qué fuente o prueba utilizaron para resolver una discrepancia entre integrantes y la IA?
- **Respuesta Sólida:**  
  *"Utilizamos la **documentación oficial de Microsoft Learn sobre C#** (específicamente la especificación de tipos de firma de métodos, despacho virtual y reglas de sobrecarga CS0111) y la ejecución de pruebas unitarias y de consola con el compilador de .NET SDK para validar empíricamente el comportamiento exacto en runtime."*

---

## 10. Tutorial para Modificaciones en Vivo Sugeridas por el Docente

En el último minuto de la defensa, el profesor solicitará una modificación rápida en vivo. Aquí tienes el procedimiento exacto para cada una:

### Modificación 1: "Agregar `NotificacionWhatsApp` con `IRegistrable` e `IConfirmable`"
*(Dificultad: Media | Tiempo: 40 segundos)*
1. Mostrar el archivo `NotificacionWhatsApp.cs` ya estructurado en el proyecto:
   ```csharp
   public class NotificacionWhatsApp : Notificacion, IConfirmable, IRegistrable
   {
       public DateTime? FechaConfirmacion { get; private set; }
       public NotificacionWhatsApp(string numero, string mensaje, Prioridad prioridad)
           : base(numero, mensaje, prioridad) { }

       public override void Enviar() {
           ValidarEnvioPendiente();
           Console.WriteLine($"[WHATSAPP] A: {Destinatario} -> {Mensaje}");
           Estado = EstadoNotificacion.Enviada;
       }

       public void ConfirmarEntrega() {
           Estado = EstadoNotificacion.Confirmada;
           FechaConfirmacion = DateTime.Now;
       }

       public string ObtenerRegistro() => $"WHATSAPP | {Destinatario} | {Estado}";
   }
   ```
2. Explicar: *"Profesor, para integrarla simplemente instanciamos el objeto y llamamos a `gestor.Agregar(whatsapp)`. No hay que tocar ni una línea de `GestorNotificaciones` ni de su bucle de envío."*

---

### Modificación 2: "Hacer que SMS deje de ser programable (o quitar contrato)"
*(Dificultad: Baja | Tiempo: 20 segundos)*
1. Abrir `NotificacionSMS.cs`.
2. Mostrar la cabecera de la clase:
   ```csharp
   // Si tuviera IProgramable:
   // public class NotificacionSMS : Notificacion, IProgramable, IConfirmable, IRegistrable
   // Simplemente se retira ', IProgramable' y el método void Programar(...)
   public class NotificacionSMS : Notificacion, IConfirmable, IRegistrable
   ```
3. Explicar: *"Profesor, al ser una capacidad modelada por interfaces, retirarla consiste únicamente en remover `, IProgramable` de la firma de clase y su método `Programar()`. La jerarquía base `Notificacion` no se ve afectada ni se rompe el resto de los canales."*

---

### Modificación 3: "Cambiar el límite de SMS y mostrar dónde se modifica"
*(Dificultad: Baja | Tiempo: 15 segundos)*
1. Abrir `NotificacionSMS.cs`.
2. Señalar la línea 16:
   ```csharp
   public const int MaximoCaracteres = 160;
   ```
3. Cambiarla en vivo si el profesor pide 200:
   ```csharp
   public const int MaximoCaracteres = 200;
   ```
4. Explicar: *"Está encapsulada en una constante dentro de `NotificacionSMS`. Cambiarlo aquí actualiza de inmediato el constructor y la validación en todo el sistema sin afectar ningún otro archivo."*

---

### Modificación 4: "Agregar `ICancelable` solo a dos canales"
*(Dificultad: Media | Tiempo: 40 segundos)*
1. Abrir `Interfaces.cs` y mostrar `ICancelable`:
   ```csharp
   public interface ICancelable {
       bool EsCancelada { get; }
       void Cancelar(string motivo);
   }
   ```
2. Abrir `NotificacionCorreo.cs` y agregar al final de la clase:
   ```csharp
   public bool EsCancelada => Estado == EstadoNotificacion.Cancelada;
   public void Cancelar(string motivo) {
       if (Estado == EstadoNotificacion.Enviada)
           throw new InvalidOperationException("No se puede cancelar un correo ya enviado.");
       Estado = EstadoNotificacion.Cancelada;
       Console.WriteLine($"[CORREO CANCELADO] Motivo: {motivo}");
   }
   ```
3. Y añadir `, ICancelable` a la definición de la clase. Repetir en `NotificacionPush.cs`.

---

### Modificación 5: "Procesar primero las notificaciones críticas"
*(Dificultad: Media | Tiempo: 20 segundos)*
1. Abrir `GestorNotificaciones.cs` en `ProcesarTodas()`.
2. Señalar la línea:
   ```csharp
   var listaOrdenada = _notificaciones.OrderByDescending(n => n.Prioridad).ToList();
   ```
3. Explicar: *"Esta responsabilidad está asumida en `GestorNotificaciones` porque el gestor posee la colección y define la política de despacho sin delegar el orden a objetos individuales."*

---

### Modificación 6: "Agregar una nueva sobrecarga válida de una operación existente"
*(Dificultad: Media | Tiempo: 30 segundos)*
1. Abrir `GestorNotificaciones.cs` o `NotificacionCorreo.cs`.
2. En `NotificacionCorreo.cs`, mostrar:
   ```csharp
   // Operación original:
   public override void Enviar() { ... }

   // NUEVA SOBRECARGA VÁLIDA:
   public void Enviar(string remitentePersonalizado)
   {
       Console.WriteLine($"[CORREO] Enviando desde: {remitentePersonalizado}");
       Enviar();
   }
   ```
3. Explicar: *"Es una sobrecarga válida porque comparte el mismo identificador (`Enviar`), pero su lista de parámetros difiere (`string remitentePersonalizado` frente a sin parámetros). El compilador resuelve la invocación en tiempo de compilación según los argumentos suministrados."*

---

## 11. Señales de Comprensión Sólida vs. Alertas a Evitar

### Señales de Comprensión Sólida (Para ganar los 2 puntos por pregunta):
- Señalar líneas concretas de código y usar con precisión los términos: **tipo declarado**, **tipo real**, **despacho dinámico**, **override**, **overload**, **contrato** e **interfaz**.
- Explicar las consecuencias antes de ejecutar (`"Si quito override aquí, el compilador arrojará error CS0534..."`).
- Justificar técnicamente el uso de interfaces basándose en el **Principio de Segregación de Interfaces (ISP)**.
- Demostrar que el gestor no contiene condicionales por tipo (`if (n is NotificacionSMS)`).

### Señales de Alerta (¡Evitar a toda costa!):
- Confundir sobrecarga (`overload`) con sobrescritura (`override`).
- Afirmar que cambiar únicamente el tipo de retorno basta para sobrecargar un método.
- Decir que la IA escribió el código y no saber explicar una línea.
- Sugerir usar `if/else` o `switch` sobre los tipos concretos para enviar los mensajes.
- Decir que "no pasaría nada" si una clase abstracta se convierte en clase normal.

---
*¡Con esta preparación y el código del proyecto, el equipo tiene garantizada una defensa sincrónica sobresaliente!*

