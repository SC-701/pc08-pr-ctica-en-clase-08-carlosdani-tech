[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/W4gCEiT5)
# Práctica en Clase 08: Implementación de Seguridad en API y Web de Productos

## Información General
- **Curso:** Desarrollo de Aplicaciones Web
- **Práctica:** #8
- **Rubro:** Caso práctico 2
- **Valor:** 6%
- **Semana:** 12
- **Duración:** 1 hora

---

## 📋 Objetivo
Aplicar seguridad (autenticación y autorización) al proyecto de Productos desarrollado en la Práctica en Clase 07, replicando la misma implementación de seguridad vista en la **PP4** (videos Semana 09).

## Objetivos de Aprendizaje
Al finalizar esta práctica, el estudiante será capaz de:
- Implementar autenticación JWT en una API ASP.NET Core
- Configurar el paquete de autorización en el API
- Proteger endpoints con atributos de autorización
- Implementar login y manejo de token en la aplicación Web
- Configurar seguridad en Azure (App Service)
- Aplicar seguridad de extremo a extremo en una solución API + Web

---

## Requisitos Previos
- Tener completada la Práctica en Clase 07 (Web con Razor Pages para Productos)
- Haber revisado los videos de la PP4 (Semana 09 - Seguridad)

---

## 🏗️ Cambios en el Proyecto

Aplicar seguridad sobre el proyecto existente `Producto.WEB` + `Producto.API`, siguiendo los mismos pasos de `Vehiculos.WEB` (Semana 09):

### API (`Producto.API`)
- Agregar configuración JWT en `appsettings.json`
- Instalar y configurar el paquete de autorización
- Proteger los endpoints con `[Authorize]`
- Configurar seguridad en `Program.cs`

### Web (`Producto.WEB`)
- Implementar páginas de Cuenta (Login / Logout)
- Manejar el token JWT recibido del API
- Enviar el token en las solicitudes a endpoints protegidos
- Redirigir al login cuando no hay sesión activa

---

## 📝 Instrucciones

- Copiar el código fuente de la Práctica en Clase 07 como punto de partida
- Seguir los mismos pasos implementados en los videos de la Semana 09 (PP4):
  - **01-SA-Conceptos** y **02-SA-Conceptos**: Revisar la teoría aplicada
  - **03-SA-Paquete Autorización**: Instalar y configurar el paquete en el API
  - **05-SA-API Con Seguridad**: Aplicar seguridad al `Producto.API`
  - **06-SA-Web Con Seguridad**: Aplicar seguridad al `Producto.WEB`
- Ejecutar ambos proyectos y verificar que el CRUD requiere autenticación
- Subir el código al repositorio