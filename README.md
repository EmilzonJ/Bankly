![image](https://github.com/EmilzonJ/Bankly/assets/84218667/28d12fcc-f1ba-4b51-90c6-aacf10b71215)# Bankly

## Descripción del Proyecto

Bankly es una solución de software que permite la creación y gestión de clientes, cuentas de ahorro, depósitos, retiros y transferencias entre cuentas. Esta aplicación está desarrollada utilizando .NET 8 para el back-end y React para el front-end, con MongoDB como base de datos.

## Arquitectura de la Solución

### Diseño de la Arquitectura

La solución backend sigue los principios de la Clean Architecture, organizada en cinco capas principales:

- **Domain**: Contiene las entidades del dominio, enums y errores del dominio (`DomainErrors`).
- **Application**: Maneja la lógica de la aplicación con comandos y consultas organizados (`UseCases`), utilizando `Mediator.SourceGenerator` (Alternativa a `MediatR` pero con Source Generator) para una alta performance y `FluentValidation` para la validación de datos entrantes.
- **Infrastructure**: Implementa la persistencia de datos utilizando `MongoDB.Driver`, y sigue un enfoque de repositorios específicos en lugar de repositorios genéricos para evitar malas prácticas.
- **Web**: Proporciona la API, utilizando controllers para un código más estructurado y fácil de mantener, Minimal APIs es el futuro; pero faltan cosas por mejorar.
- **Shared**: Contiene componentes compartidos entre las diferentes capas de la aplicación, como clases para aplicar el patrón `Result Pattern`

### Comunicación entre Back-end y Front-end

El front-end se comunica con el back-end a través de una API RESTful. Las solicitudes HTTP se envían desde el front-end a los endpoints del back-end, que procesan las solicitudes y devuelven las respuestas correspondientes.

### Captura y Manejo de Excepciones

Se utiliza el patrón de resultados (Result Pattern) para manejar errores de dominio sin lanzar excepciones, mejorando así el rendimiento y manteniendo el código limpio y muy fácil de testear.

### Testing

Para las pruebas unitarias de la capa de `Application` (`UseCases`), se utiliza `XUnit` junto con `FluentAssertions` y `NSubstitute` para realizar mocks. Esto asegura una alta cobertura de pruebas y una mayor confiabilidad del código.

### Diagrama de Arquitectura

**Diagrama de Arquitectura General**

```plaintext
+-------------------+     +-------------------+     +-------------------+
|                   |     |                   |     |                   |
|   User Interface  |<--->|    REST API       |<--->|  Application      |
|                   |     |                   |     |  (Commands/Queries|
+-------------------+     +-------------------+     +-------------------+
                             |
                             |
                             V
                      +-------------------+
                      |    Domain         |
                      +-------------------+
                             |
                             |
                             V
                      +-------------------+
                      |  Infrastructure   |
                      |  (MongoDB.Driver) |
                      +-------------------+
                             |
                             |
                             V
                      +-------------------+
                      |    MongoDB        |
                      +-------------------+
```

## Repositorio de Datos

### Elección de la Tecnología de Almacenamiento de Datos

Para este proyecto, se eligió MongoDB como base de datos debido a su flexibilidad y capacidad para manejar grandes volúmenes de datos no estructurados. Dados lo requerimientos, esta base de datos se adapta muy bien para guardar documentos en collecciones que pueden cambiar con el tiempo en su definición, pero sobre todo, para este tipo se software es vital el mantener un historial de transacciones limpias sin echar de menos la integridad de las bases de datos SQL pero con la potencia de las NoSQL.

### Modelo de Datos y Relación entre Entidades

**Modelo de Datos**

Todas las entidades heredan de una clase base que contiene las siguientes propiedades:

  - `CreatedAt`: Fecha de creación.
  - `UpdatedAt`: Fecha de actualización.
  - `IsActive`: Para manejar SoftDelete.

- **Customer**: Representa a un cliente del banco.
  - `Id`: Identificador único.
  - `Name`: Nombre del cliente.
  - `Email`: Correo electrónico del cliente.

- **Account**: Representa una cuenta de ahorro.
  - `Id`: Identificador único.
  - `CustomerId`: Identificador del cliente propietario de la cuenta.
  - `Number`: Número de cuenta.
  - `Type`: Tipo de cuenta.

- **Transaction**: Representa una transacción bancaria.
  - `Id`: Identificador único.
  - `Type`: Tipo de transacción (Depósito, Retiro, Transferencia).
  - `Amount`: Monto de la transacción.
  - `Description`: Descripción de la transacción.
  - `SourceAccountId`: Identificador de la cuenta fuente.
  - `DestinationAccountId`: Identificador de la cuenta destino (opcional).

### Ejemplos de Consultas

**Consulta para Obtener Clientes paginados y filtrados**

```csharp
public async Task<Customer?> GetByIdAsync(ObjectId id)
{
    return await _customers.Find(c => c.Id == id).FirstOrDefaultAsync();
}

