# Explicación del Sistema y Justificación de la Implementación (N-Capas MVC C#)

Este documento detalla el funcionamiento del sistema de **Citas Médicas** desarrollado bajo una arquitectura orientada al dominio (DDD/N-Capas) usando **ASP.NET MVC 5** (.NET Framework 4.7.2), y justifica las decisiones tomadas para la implementación de la historia de usuario **HU - Gestionar Paciente**.

---

## 1. Arquitectura del Sistema
El sistema se ha estructurado en 4 capas lógicas con responsabilidades bien diferenciadas. Esto desacopla el diseño visual del almacenamiento de datos y las reglas del negocio, facilitando el mantenimiento y escalabilidad futura.

```mermaid
graph TD
    subgraph Capa 1: Presentación (Web MVC)
        C1[GestionarPacienteController] --> V1[GestionarPaciente.cshtml]
    end
    subgraph Capa 2: Aplicación (Servicios)
        C2[GestionarPacienteServicio]
    end
    subgraph Capa 3: Dominio (Entidades)
        C3[Paciente.cs]
    end
    subgraph Capa 4: Persistencia (Acceso a Datos)
        C4[PacienteSQL.cs] --> DB[(SQL Server)]
    end

    C1 --> C2
    C2 --> C3
    C2 --> C4
    C4 --> C3
```

---

## 2. Capa de Dominio (`Capa3_Dominio.ModuloPrincipal`)
Representa el corazón del sistema. Contiene las entidades y las reglas más puras del negocio independientes de bases de datos o interfaces visuales.

### Clase: `Paciente.cs`
* **Campos y Propiedades**: Define la información del paciente (`Nombres`, `Apellidos`, `Tipo_documento`, `Documento_paciente`, `Sexo`, `Fecha_nacimiento`, `Telefono`, `Correo`, `Direccion` y `Observaciones`).
  * *Justificación*: Se agregó el campo y propiedad faltante `Documento_paciente` ya que, aunque la base de datos y la vista lo requerían, la entidad original no lo contemplaba, imposibilitando compilar la persistencia.
* **Constructor Vacío (`Paciente()`)**:
  * *Justificación*: Permite al motor de serialización de ASP.NET MVC mapear las solicitudes JSON que llegan desde el cliente directamente a objetos de tipo `Paciente` de forma automática.
* **Método `ValidarCamposObligatorios()`**:
  * *Funcionamiento*: Verifica que ningún campo requerido (RN-03) contenga valores nulos, cadenas vacías o valores por defecto.
  * *Justificación*: Es responsabilidad directa de la entidad velar por su propia integridad estructural mínima antes de ser procesada por servicios o persistida.
* **Método `ValidarFormatos()`**:
  * *Funcionamiento*: Evalúa que `correo` tenga una estructura de email válida (`usuario@dominio.com`) y que `telefono` contenga exactamente 9 dígitos numéricos mediante **Expresiones Regulares** (Regex) (RN-06).
  * *Justificación*: Asegura la calidad y validez legal de los datos antes de guardarlos en el servidor.

---

## 3. Capa de Persistencia (`Capa4_Persistencia.SqlServer.ModuloPrincipal`)
Responsable de realizar la comunicación directa con la base de datos SQL Server.

### Procedimientos Almacenados (`CitasMedicas_BaseDeDatos.sql`)
* *Justificación*: El uso de procedimientos almacenados en lugar de consultas SQL crudas en C# protege al sistema contra ataques de **Inyección SQL**, encapsula el modelo físico de la base de datos y mejora el rendimiento de ejecución.
* **`sp_ListarPacientes`**: Consulta directa de todas las columnas de la tabla `Paciente`.
* **`sp_RegistrarPaciente` / `sp_ModificarPaciente`**: Reciben los parámetros mapeados para insertar o actualizar las columnas físicas. El campo `direccion` y `observaciones` reciben `NULL` si el usuario no los llena (RN-03).
* **`sp_EliminarPaciente`**: Elimina físicamente el registro por su clave primaria (`id_paciente`).
* **`sp_ExisteDocumentoPaciente`**: Retorna `1` (true) o `0` (false) si un número de documento ya está asociado a un paciente, con soporte para excluir un ID de paciente en específico (utilizado en la edición).
* **`sp_TieneCitasProgramadas`**: Cuenta cuántas citas tiene el paciente cuyo estado sea `'Programada'`.

### Clase: `PacienteSQL.cs`
* **Método `ListarPacientes()`**:
  * *Funcionamiento*: Abre una conexión SQL, ejecuta `sp_ListarPacientes`, y mediante un `SqlDataReader` itera los registros convirtiendo cada fila en un objeto `Paciente` para poblar una lista.
* **Métodos `RegistrarPaciente()` / `ModificarPaciente()` / `EliminarPaciente()`**:
  * *Funcionamiento*: Asocian las propiedades de los objetos de C# a parámetros del procedimiento almacenado y ejecutan `ExecuteNonQuery()`, devolviendo `true` si se afectó al menos una fila.
* **Método `ExisteDocumentoPaciente(documento, idPacienteExcluir)`**:
  * *Funcionamiento*: Ejecuta el stored procedure enviando el DNI a validar. En caso de modificación, envía el ID del paciente actual para excluirlo de la búsqueda y no considerarlo como "duplicado" de sí mismo.
* **Método `TieneCitasProgramadas(idPaciente)`**:
  * *Funcionamiento*: Consulta la existencia de citas activas del paciente. Retorna un valor booleano.

---

## 4. Capa de Aplicación (`Capa2_Aplicacion.ModuloPrincipal`)
Actúa como mediador entre la capa de presentación y la base de datos. Orquesta las reglas de negocio complejas que involucran llamadas a múltiples entidades o persistencia.

### Clase: `GestionarPacienteServicio.cs`
* **Método `RegistrarPaciente(paciente)`**:
  * *Funcionamiento*:
    1. Ejecuta `paciente.ValidarCamposObligatorios()` (RN-03). Si falla, interrumpe y lanza una excepción.
    2. Ejecuta `paciente.ValidarFormatos()` (RN-06). Si falla, interrumpe.
    3. Llama a `ExisteDocumentoPaciente()` (RN-02). Si da positivo, arroja un error indicando documento duplicado.
    4. Procede al registro en la base de datos.
* **Método `ModificarPaciente(paciente)`**:
  * *Funcionamiento*: Realiza las mismas validaciones de campos obligatorios y formatos, pero realiza la comprobación de documento duplicado excluyendo el ID del paciente actual para permitir guardar los cambios sin colisión (RN-04).
* **Método `EliminarPaciente(idPaciente)`**:
  * *Funcionamiento*: Consulta a la base de datos si el paciente tiene citas programadas (`TieneCitasProgramadas()`). Si es verdadero, arroja una excepción impidiendo la eliminación (RN-05). En caso contrario, procede a eliminar.

---

## 5. Capa de Presentación Web MVC (`Capa1_Presentacion`)
Provee la interfaz visual del usuario final e interpreta las acciones de este para interactuar con el sistema.

### Controlador: `GestionarPacienteController.cs`
* **`PaginaGestionarPaciente()` (GET)**:
  * *Funcionamiento*: Renderiza y carga la vista HTML principal.
* **`ListarPacientes()` (GET)**:
  * *Funcionamiento*: Retorna toda la lista de pacientes registrados en formato JSON.
* **`RegistrarPaciente()` / `ModificarPaciente()` / `EliminarPaciente()` (POST)**:
  * *Funcionamiento*: Capturan los objetos enviados por AJAX, ejecutan el servicio (`GestionarPacienteServicio`) encapsulado en un bloque `try-catch`. Si ocurre un error de validación, la excepción es atrapada y devuelta al navegador en un campo `mensajeError` con `estadoCorrecto = false` para que la pantalla del usuario lo notifique amigablemente.

### Vista: `GestionarPaciente.cshtml` y Layout
* **Navegación**:
  * Se modificó [\_Layout.cshtml](file:///d:/Proyectos/Antigravity/citas-medicas/Consultar_Citas/Capa1_Presentacion.Web.AspNet.ModuloPrincipal/Views/Shared/_Layout.cshtml) para incluir enlaces responsivos que interconecten Inicio, Consulta de Citas y Gestión de Pacientes.
* **Formulario y Listado AJAX**:
  * *Funcionamiento*: Al cargar la página, se realiza un llamado AJAX a `ListarPacientes` para pintar dinámicamente las filas de la tabla.
  * *Modal de Registro/Edición*: Cuando se hace clic en "+ Nuevo Paciente" o "Modificar", se despliega una interfaz modal estilizada mediante CSS de manera instantánea. Al enviar el formulario, AJAX serializa el objeto paciente y lo envía por POST. Si el backend retorna éxito, la tabla se recarga asíncronamente (sin parpadeos de pantalla), de lo contrario, salta un aviso informando el motivo exacto del rechazo.
  * *Función `escapeHtml()`*: Sanitiza los strings que se inyectan en los eventos jQuery del modal, evitando fallos de script si el paciente tiene nombres con comillas simples (ej. O'Connor).
