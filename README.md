# Bankly

## Características de la aplicación
- **Clientes**: Lista, Detalle, Filtrar, Crear, Actualizar y Eliminar.
- **Cuentas**: Listar, Detalle, Crear, Actualizar y Eliminar. (Varias cuentas por cliente, de tipo Ahorro por defecto)
- **Transacciones**: Lista, Detalle, Filtrar, Crear y Eliminar. (Depósitos, Retiros y Tranferencias)

Esta aplicación está desarrollada utilizando .NET 8 para el back-end y React para el front-end, con MongoDB como base de datos.

## Arquitectura de la Solución

### Backend
La arquitectura de la solución sigue los principios de Clean Architecture y está organizada en varios proyectos para separar las responsabilidades y facilitar el mantenimiento y la escalabilidad.

- **Web**
  - Contiene la API y los controladores.
  - Gestiona las solicitudes HTTP y coordina las respuestas.
  - Proporciona puntos de entrada a la aplicación.

- **Infrastructure**
  - Implementa la persistencia de datos utilizando MongoDB.
  - Contiene los repositorios específicos para cada entidad del dominio.
  - Gestiona la configuración y el acceso a la base de datos.

- **Application**
  - Maneja la lógica de la aplicación.
  - Organiza la lógica de negocio en comandos (`Commands`) y consultas (`Queries`).
  - Utiliza `Mediator.SourceGenerator` para la mediación y `FluentValidation` para la validación.

- **Domain**
  - Define las entidades del dominio y sus relaciones.
  - Contiene los enums y los errores del dominio (`DomainErrors`).
  - Representa el núcleo de la lógica de negocio.

- **Shared**
  - Contiene componentes y utilidades compartidos entre las diferentes capas de la aplicación.
  - Proporciona soporte común que puede ser reutilizado a través de los distintos proyectos.

### Principios de la Arquitectura

- **Separación de Responsabilidades**: Cada proyecto tiene una responsabilidad clara y específica, lo que facilita el mantenimiento y la escalabilidad.
- **Independencia de la Infraestructura**: La lógica de la aplicación y del dominio no dependen de detalles de implementación de la infraestructura, como la base de datos.
- **Testabilidad**: La estructura facilita la escritura de pruebas unitarias y de integración, mejorando la calidad del código.
- **Flexibilidad y Extensibilidad**: La organización modular permite agregar nuevas funcionalidades y adaptarse a cambios de requisitos de manera eficiente.

### Librerias y Herramientas Utilizadas

- **.NET 8**: Framework principal para el desarrollo del back-end.
- **MongoDB**: Base de datos NoSQL utilizada para almacenar los datos de la aplicación.
- **Mediator.SourceGenerator**: Librería utilizada para la mediación, proporcionando alta performance.
- **FluentValidation**: Librería utilizada para la validación de comandos y consultas.
- **CSvHelper**: Para obtener datos de un csv y crear seeder de datos.
- **Serilog**: Para tener logs estructurados. (Potencialmente combinado con OpenTelemtry para Observabilidad, Trazabilidad y Métricas)
- **XUnit**: Para TDD.
- **FluentAssertions**: Para facilitar los Asserts en los tests.
- **NSubstitute*: Mi libreria favorita para mock, mejor que Moq. 😉 


## Repositorio de Datos

### Beneficios de MongoDB para los Requerimientos del Proyecto

- **Flexibilidad en la Evolución del Esquema**: Dado que los esquemas de datos pueden cambiar a lo largo del tiempo, MongoDB ofrece la flexibilidad necesaria para adaptarse a estos cambios sin requerir migraciones de base de datos complejas. Esto es particularmente beneficioso en proyectos ágiles donde los requisitos pueden evolucionar rápidamente.

- **Escalabilidad para Manejar Cargas Elevadas**: Con la capacidad de escalar horizontalmente, MongoDB puede manejar eficientemente un creciente número de usuarios y transacciones. Esto asegura que la aplicación puede crecer sin enfrentar problemas de rendimiento.

- **Desempeño Óptimo en Consultas y Escrituras**: MongoDB está diseñado para soportar operaciones de lectura y escritura de alta velocidad, lo que es crucial para una aplicación bancaria que necesita procesar transacciones en tiempo real y responder rápidamente a las solicitudes de los usuarios.

- **Implementación de Consultas Complejas**: La capacidad de MongoDB para ejecutar consultas complejas de manera eficiente permite obtener información detallada y específica sobre clientes, cuentas y transacciones sin comprometer el rendimiento.

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
  - `Alias`: Alias definido por el usuario para identificar la cuenta.
  - `Balance`: Saldo de la cuenta que irá cambiando según las transacciones.
  - `Type`: Tipo de cuenta. (Limitado solo a ahorro)

- **Transaction**: Representa una transacción bancaria.
  - `Id`: Identificador único.
  - `Type`: Tipo de transacción (Depósito, Retiro, Transferencia entrante, Transferencia Saliente).
  - `Amount`: Monto de la transacción.
  - `Description`: Descripción de la transacción.
  - `SourceAccountId`: Identificador de la cuenta origen.
  - `DestinationAccountId`: Identificador de la cuenta destino (opcional).

