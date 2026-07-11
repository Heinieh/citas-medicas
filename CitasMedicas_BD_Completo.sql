-- ==========================================
-- SCRIPT GENERAL UNIFICADO DE BASE DE DATOS
-- PROYECTO: Sistema de Citas Médicas
-- BASE DE DATOS: BD_Clinica
-- ==========================================

USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'BD_Clinica')
BEGIN
    ALTER DATABASE [BD_Clinica] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BD_Clinica;
END
GO

CREATE DATABASE BD_Clinica;
GO

USE BD_Clinica;
GO


-- ==========================================
-- SECCIÓN: CitasMedicas-ScriptTablas.sql
-- ==========================================
/* ---------------------------------------------------- */
/* Script Adaptado para: Sistema de Citas Médicas      */
/* DBMS       : SQL Server                             */
/* Estructura : Idempotente (Drop & Create)            */
/* ---------------------------------------------------- */

USE [BD_Clinica]
GO

/* ==================================================== */
/* Drop Foreign Key Constraints                         */
/* ==================================================== */

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Usuario_Medico]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Usuario] DROP CONSTRAINT [FK_Usuario_Medico]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Medico_Especialidad]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Medico] DROP CONSTRAINT [FK_Medico_Especialidad]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Cita_Paciente]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Cita] DROP CONSTRAINT [FK_Cita_Paciente]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[FK_Cita_Medico]') AND OBJECTPROPERTY(id, N'IsForeignKey') = 1) 
ALTER TABLE [Cita] DROP CONSTRAINT [FK_Cita_Medico]
GO

/* ==================================================== */
/* Drop Tables                                          */
/* ==================================================== */

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Cita]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Cita]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Medico]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Medico]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Paciente]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Paciente]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Usuario]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Usuario]
GO

IF EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[Especialidad]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
DROP TABLE [Especialidad]
GO

/* ==================================================== */
/* Create Tables                                        */
/* ==================================================== */

CREATE TABLE [Especialidad]
(
    [Id_especialidad] int IDENTITY(1,1) NOT NULL,
    [Nombre] varchar(100) NOT NULL,
    [Descripcion] varchar(255) NULL
)
GO

CREATE TABLE [Medico]
(
    [Id_medico] int IDENTITY(1,1) NOT NULL,
    [Nombres] varchar(100) NOT NULL,
    [Apellidos] varchar(100) NOT NULL,
    [Colegiatura] varchar(50) NULL,
    [Telefono] varchar(20) NULL,
    [Id_especialidad] int NOT NULL
)
GO

CREATE TABLE [Paciente]
(
    [Id_paciente] int IDENTITY(1,1) NOT NULL,
    [Tipo_documento] varchar(20) NOT NULL,
    [Numero_documento] varchar(20) NOT NULL,
    [Nombres] varchar(100) NOT NULL,
    [Apellidos] varchar(100) NOT NULL,
    [Sexo] char(1) NOT NULL,
    [Fecha_nacimiento] date NOT NULL,
    [Direccion] varchar(200) NULL,
    [Telefono] varchar(20) NULL,
    [Correo] varchar(100) NULL,
    [Observaciones] varchar(MAX) NULL,
    [Tipo_sangre] varchar(20) NOT NULL CONSTRAINT [DF_Paciente_Tipo_sangre] DEFAULT ('No especificado')
)
GO

CREATE TABLE [Cita]
(
    [Id_cita] int IDENTITY(1,1) NOT NULL,
    [Id_paciente] int NOT NULL,
    [Id_medico] int NOT NULL,
    [Fecha_cita] date NOT NULL,
    [Hora_cita] time NOT NULL,
    [Motivo] varchar(255) NOT NULL,
    [Estado] varchar(50) NOT NULL,
    [Observaciones] varchar(MAX) NULL
)
GO

CREATE TABLE [Usuario]
(
    [Id_usuario] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(50) NOT NULL,
    [Password] varchar(50) NOT NULL,
    [Rol] varchar(50) NOT NULL,
    [Id_medico] int NULL
)
GO

/* ==================================================== */
/* Create Primary Keys, Indexes, Uniques, Checks        */
/* ==================================================== */

ALTER TABLE [Especialidad] 
 ADD CONSTRAINT [PK_Especialidad]
    PRIMARY KEY CLUSTERED ([Id_especialidad] ASC)
GO

ALTER TABLE [Medico] 
 ADD CONSTRAINT [PK_Medico]
    PRIMARY KEY CLUSTERED ([Id_medico] ASC)
GO

-- Índice para búsquedas rápidas de médicos por especialidad
CREATE NONCLUSTERED INDEX [IXFK_Medico_Especialidad] 
 ON [Medico] ([Id_especialidad] ASC)
GO

ALTER TABLE [Paciente] 
 ADD CONSTRAINT [PK_Paciente]
    PRIMARY KEY CLUSTERED ([Id_paciente] ASC)
GO

-- Restricción UNIQUE para que no existan dos pacientes con el mismo documento
ALTER TABLE [Paciente] 
 ADD CONSTRAINT [UQ_Paciente_Documento]
    UNIQUE ([Numero_documento])
GO

ALTER TABLE [Cita] 
 ADD CONSTRAINT [PK_Cita]
    PRIMARY KEY CLUSTERED ([Id_cita] ASC)
GO

-- Índices para mejorar la velocidad al buscar las citas de un paciente o de un médico
CREATE NONCLUSTERED INDEX [IXFK_Cita_Paciente] 
 ON [Cita] ([Id_paciente] ASC)
GO

CREATE NONCLUSTERED INDEX [IXFK_Cita_Medico] 
 ON [Cita] ([Id_medico] ASC)
GO

/* ==================================================== */
/* Create Foreign Key Constraints                       */
/* ==================================================== */

ALTER TABLE [Medico] ADD CONSTRAINT [FK_Medico_Especialidad]
    FOREIGN KEY ([Id_especialidad]) REFERENCES [Especialidad] ([Id_especialidad]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Cita] ADD CONSTRAINT [FK_Cita_Paciente]
    FOREIGN KEY ([Id_paciente]) REFERENCES [Paciente] ([Id_paciente]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Cita] ADD CONSTRAINT [FK_Cita_Medico]
    FOREIGN KEY ([Id_medico]) REFERENCES [Medico] ([Id_medico]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [Usuario] 
 ADD CONSTRAINT [PK_Usuario]
    PRIMARY KEY CLUSTERED ([Id_usuario] ASC)
GO

ALTER TABLE [Usuario] 
 ADD CONSTRAINT [UQ_Usuario_Username]
    UNIQUE ([Username])
GO

ALTER TABLE [Usuario] ADD CONSTRAINT [FK_Usuario_Medico]
    FOREIGN KEY ([Id_medico]) REFERENCES [Medico] ([Id_medico]) ON DELETE No Action ON UPDATE No Action
GO

-- ==========================================
-- SECCIÓN: CitasMedicas-ScriptProcedimientosAlmacenados.sql
-- ==========================================
USE [BD_Clinica]
GO

/* ============================================================================================== */
/* ================================== 1. TABLA ESPECIALIDAD ===================================== */
/* ============================================================================================== */

IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_especialidades' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_especialidades
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_id_especialidad' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_id_especialidad
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='ins_crear_especialidad' AND xtype = 'P') 
	DROP PROCEDURE dbo.ins_crear_especialidad
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='upd_modificar_especialidad' AND xtype = 'P') 
	DROP PROCEDURE dbo.upd_modificar_especialidad
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='del_eliminar_especialidad' AND xtype = 'P') 
	DROP PROCEDURE dbo.del_eliminar_especialidad
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_especialidades]
AS  
SET NOCOUNT ON 

SELECT	Id_especialidad, Nombre, Descripcion 
FROM	Especialidad 
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_id_especialidad]
@pIdEspecialidad INT
AS  
SET NOCOUNT ON 

SELECT	Id_especialidad, Nombre, Descripcion 
FROM	Especialidad 
WHERE	Id_especialidad = @pIdEspecialidad
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[ins_crear_especialidad]
@pNombre		VARCHAR(100)
,@pDescripcion	VARCHAR(255)
AS  
SET NOCOUNT ON 

INSERT INTO Especialidad (Nombre, Descripcion)
VALUES (@pNombre, @pDescripcion)
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[upd_modificar_especialidad]
@pIdEspecialidad INT
,@pNombre		VARCHAR(100)
,@pDescripcion	VARCHAR(255)
AS  
SET NOCOUNT ON 

UPDATE	Especialidad 
SET		Nombre = @pNombre
		,Descripcion = @pDescripcion
WHERE	Id_especialidad = @pIdEspecialidad
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[del_eliminar_especialidad]
@pIdEspecialidad INT
AS  
SET NOCOUNT ON 

DELETE FROM	Especialidad 
WHERE		Id_especialidad = @pIdEspecialidad
GO


/* ============================================================================================== */
/* ======================================= 2. TABLA MEDICO ====================================== */
/* ============================================================================================== */

IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_medicos' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_medicos
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_id_medico' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_id_medico
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='ins_crear_medico' AND xtype = 'P') 
	DROP PROCEDURE dbo.ins_crear_medico
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='upd_modificar_medico' AND xtype = 'P') 
	DROP PROCEDURE dbo.upd_modificar_medico
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='del_eliminar_medico' AND xtype = 'P') 
	DROP PROCEDURE dbo.del_eliminar_medico
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_medicos]
AS  
SET NOCOUNT ON 

SELECT	Id_medico, Nombres, Apellidos, Colegiatura, Telefono, Id_especialidad 
FROM	Medico 
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_id_medico]
@pIdMedico INT
AS  
SET NOCOUNT ON 

SELECT	Id_medico, Nombres, Apellidos, Colegiatura, Telefono, Id_especialidad 
FROM	Medico 
WHERE	Id_medico = @pIdMedico
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[ins_crear_medico]
@pNombres			VARCHAR(100)
,@pApellidos		VARCHAR(100)
,@pColegiatura		VARCHAR(50)
,@pTelefono			VARCHAR(20)
,@pIdEspecialidad	INT
AS  
SET NOCOUNT ON 

INSERT INTO Medico (Nombres, Apellidos, Colegiatura, Telefono, Id_especialidad)
VALUES (@pNombres, @pApellidos, @pColegiatura, @pTelefono, @pIdEspecialidad)
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[upd_modificar_medico]
@pIdMedico			INT
,@pNombres			VARCHAR(100)
,@pApellidos		VARCHAR(100)
,@pColegiatura		VARCHAR(50)
,@pTelefono			VARCHAR(20)
,@pIdEspecialidad	INT
AS  
SET NOCOUNT ON 

UPDATE	Medico 
SET		Nombres = @pNombres
		,Apellidos = @pApellidos
		,Colegiatura = @pColegiatura
		,Telefono = @pTelefono
		,Id_especialidad = @pIdEspecialidad
WHERE	Id_medico = @pIdMedico
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[del_eliminar_medico]
@pIdMedico INT
AS  
SET NOCOUNT ON 

DELETE FROM	Medico 
WHERE		Id_medico = @pIdMedico
GO


/* ============================================================================================== */
/* ===================================== 3. TABLA PACIENTE ====================================== */
/* ============================================================================================== */

IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_pacientes_filtro' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_pacientes_filtro
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_id_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_id_paciente
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_documento_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_documento_paciente
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='ins_crear_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.ins_crear_paciente
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='upd_modificar_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.upd_modificar_paciente
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='del_eliminar_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.del_eliminar_paciente
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_pacientes_filtro]
@pCriterio VARCHAR(100)
AS  
SET NOCOUNT ON 

SELECT	Id_paciente, Tipo_documento, Numero_documento, Nombres, Apellidos, Sexo, Fecha_nacimiento, Direccion, Telefono, Correo, Observaciones, Tipo_sangre
FROM	Paciente 
WHERE	Numero_documento LIKE '%' + @pCriterio + '%' 
   OR   Nombres LIKE '%' + @pCriterio + '%' 
   OR   Apellidos LIKE '%' + @pCriterio + '%'
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_id_paciente]
@pIdPaciente INT
AS  
SET NOCOUNT ON 

SELECT	Id_paciente, Tipo_documento, Numero_documento, Nombres, Apellidos, Sexo, Fecha_nacimiento, Direccion, Telefono, Correo, Observaciones, Tipo_sangre
FROM	Paciente 
WHERE	Id_paciente = @pIdPaciente
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_documento_paciente]
@pDocumento VARCHAR(20)
AS  
SET NOCOUNT ON 

SELECT	Id_paciente, Tipo_documento, Numero_documento, Nombres, Apellidos, Sexo, Fecha_nacimiento, Direccion, Telefono, Correo, Observaciones, Tipo_sangre
FROM	Paciente 
WHERE	Numero_documento = @pDocumento
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[ins_crear_paciente]
@pTipoDocumento		VARCHAR(20)
,@pNumeroDocumento	VARCHAR(20)
,@pNombres			VARCHAR(100)
,@pApellidos		VARCHAR(100)
,@pSexo				CHAR(1)
,@pFechaNacimiento	DATE
,@pDireccion		VARCHAR(200)
,@pTelefono			VARCHAR(20)
,@pCorreo			VARCHAR(100)
,@pObservaciones	VARCHAR(MAX)
,@pTipoSangre		VARCHAR(20)
AS  
SET NOCOUNT ON 

INSERT INTO Paciente (Tipo_documento, Numero_documento, Nombres, Apellidos, Sexo, Fecha_nacimiento, Direccion, Telefono, Correo, Observaciones, Tipo_sangre)
VALUES (@pTipoDocumento, @pNumeroDocumento, @pNombres, @pApellidos, @pSexo, @pFechaNacimiento, @pDireccion, @pTelefono, @pCorreo, @pObservaciones, @pTipoSangre)
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[upd_modificar_paciente]
@pIdPaciente		INT
,@pTipoDocumento	VARCHAR(20)
,@pNumeroDocumento	VARCHAR(20)
,@pNombres			VARCHAR(100)
,@pApellidos		VARCHAR(100)
,@pSexo				CHAR(1)
,@pFechaNacimiento	DATE
,@pDireccion		VARCHAR(200)
,@pTelefono			VARCHAR(20)
,@pCorreo			VARCHAR(100)
,@pObservaciones	VARCHAR(MAX)
,@pTipoSangre		VARCHAR(20)
AS  
SET NOCOUNT ON 

UPDATE	Paciente 
SET		Tipo_documento = @pTipoDocumento
		,Numero_documento = @pNumeroDocumento
		,Nombres = @pNombres
		,Apellidos = @pApellidos
		,Sexo = @pSexo
		,Fecha_nacimiento = @pFechaNacimiento
		,Direccion = @pDireccion
		,Telefono = @pTelefono
		,Correo = @pCorreo
		,Observaciones = @pObservaciones
		,Tipo_sangre = @pTipoSangre
WHERE	Id_paciente = @pIdPaciente
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[del_eliminar_paciente]
@pIdPaciente INT
AS  
SET NOCOUNT ON 

DELETE FROM	Paciente 
WHERE		Id_paciente = @pIdPaciente
GO


/* ============================================================================================== */
/* ======================================= 4. TABLA CITA ======================================== */
/* ============================================================================================== */

IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_citas_por_paciente' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_citas_por_paciente
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_citas_por_medico' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_citas_por_medico
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_cita_medico_fecha_hora' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_cita_medico_fecha_hora
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_consultar_citas_filtros' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_consultar_citas_filtros
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='ins_crear_cita' AND xtype = 'P') 
	DROP PROCEDURE dbo.ins_crear_cita
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='upd_modificar_cita' AND xtype = 'P') 
	DROP PROCEDURE dbo.upd_modificar_cita
GO
IF EXISTS (SELECT * FROM sysobjects WHERE name ='del_eliminar_cita' AND xtype = 'P') 
	DROP PROCEDURE dbo.del_eliminar_cita
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_citas_por_paciente]
@pIdPaciente INT
AS  
SET NOCOUNT ON 

SELECT	Id_cita, Id_paciente, Id_medico, Fecha_cita, Hora_cita, Motivo, Estado, Observaciones
FROM	Cita 
WHERE	Id_paciente = @pIdPaciente
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_citas_por_medico]
@pIdMedico INT
AS  
SET NOCOUNT ON 

SELECT	Id_cita, Id_paciente, Id_medico, Fecha_cita, Hora_cita, Motivo, Estado, Observaciones
FROM	Cita 
WHERE	Id_medico = @pIdMedico
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_cita_medico_fecha_hora]
@pIdMedico		INT
,@pFechaCita	DATE
,@pHoraCita		TIME
AS  
SET NOCOUNT ON 

SELECT	Id_cita, Id_paciente, Id_medico, Fecha_cita, Hora_cita, Motivo, Estado, Observaciones
FROM	Cita 
WHERE	Id_medico = @pIdMedico 
  AND	Fecha_cita = @pFechaCita 
  AND	Hora_cita = @pHoraCita
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[sel_consultar_citas_filtros]
@pIdMedico			INT
,@pFechaEspecifica	DATE = NULL
,@pFechaInicio		DATE = NULL
,@pFechaFin			DATE = NULL
,@pEstadoCita		VARCHAR(50) = NULL
AS  
SET NOCOUNT ON 

SELECT	Id_cita, Id_paciente, Id_medico, Fecha_cita, Hora_cita, Motivo, Estado, Observaciones
FROM	Cita 
WHERE	Id_medico = @pIdMedico
  AND	(@pFechaEspecifica IS NULL OR Fecha_cita = @pFechaEspecifica)
  AND	(@pFechaInicio IS NULL OR Fecha_cita >= @pFechaInicio)
  AND	(@pFechaFin IS NULL OR Fecha_cita <= @pFechaFin)
  AND	(@pEstadoCita IS NULL OR Estado = @pEstadoCita)
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[ins_crear_cita]
@pIdPaciente	INT
,@pIdMedico		INT
,@pFechaCita	DATE
,@pHoraCita		TIME
,@pMotivo		VARCHAR(255)
,@pEstado		VARCHAR(50)
,@pObservaciones VARCHAR(MAX)
AS  
SET NOCOUNT ON 

INSERT INTO Cita (Id_paciente, Id_medico, Fecha_cita, Hora_cita, Motivo, Estado, Observaciones)
VALUES (@pIdPaciente, @pIdMedico, @pFechaCita, @pHoraCita, @pMotivo, @pEstado, @pObservaciones)
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[upd_modificar_cita]
@pIdCita		INT
,@pFechaCita	DATE
,@pHoraCita		TIME
,@pEstado		VARCHAR(50)
,@pObservaciones VARCHAR(MAX)
AS  
SET NOCOUNT ON 

UPDATE	Cita 
SET		Fecha_cita = @pFechaCita
		,Hora_cita = @pHoraCita
		,Estado = @pEstado
		,Observaciones = @pObservaciones
WHERE	Id_cita = @pIdCita
GO

--==============================================================================================

CREATE PROCEDURE [dbo].[del_eliminar_cita]
@pIdCita INT
AS  
SET NOCOUNT ON 

DELETE FROM	Cita 
WHERE		Id_cita = @pIdCita
GO

/* ============================================================================================== */
/* ======================================= 5. TABLA USUARIO ===================================== */
/* ============================================================================================== */

IF EXISTS (SELECT * FROM sysobjects WHERE name ='sel_validar_usuario' AND xtype = 'P') 
	DROP PROCEDURE dbo.sel_validar_usuario
GO

CREATE PROCEDURE [dbo].[sel_validar_usuario]
@pUsername VARCHAR(50)
,@pPassword VARCHAR(50)
AS  
SET NOCOUNT ON 

SELECT	Id_usuario, Username, Password, Rol, Id_medico
FROM	Usuario 
WHERE	Username = @pUsername AND Password = @pPassword
GO

-- ==========================================
-- SECCIÓN: Insert_Especialidad.sql
-- ==========================================
USE [BD_Clinica]
GO

INSERT INTO [dbo].[Especialidad]
           ([Nombre]
           ,[Descripcion])
     VALUES
           ('Medicina General', 'Atención médica integral y diagnósticos primarios.'),
           ('Cardiología', 'Especialidad médica encargada del corazón y sistema circulatorio.'),
           ('Pediatría', 'Atención médica integral enfocada en niños y adolescentes.')
GO

select * from Especialidad

-- ==========================================
-- SECCIÓN: Insert_Medico.sql
-- ==========================================
USE [BD_Clinica]
GO

INSERT INTO [dbo].[Medico]
           ([Nombres]
           ,[Apellidos]
           ,[Colegiatura]
           ,[Telefono]
           ,[Id_especialidad])
     VALUES
           ('Carlos', 'Mendoza Ruiz', 'CMP-12345', '987654321', 1),
           ('Lucía', 'Fernández Soto', 'CMP-54321', '912345678', 2),
           ('Roberto', 'Gómez Bolaños', 'CMP-99887', '999888777', 3)
GO

select * from Medico

-- ==========================================
-- SECCIÓN: Insert_Paciente.sql
-- ==========================================
USE [BD_Clinica]
GO

INSERT INTO [dbo].[Paciente]
           ([Tipo_documento]
           ,[Numero_documento]
           ,[Nombres]
           ,[Apellidos]
           ,[Sexo]
           ,[Fecha_nacimiento]
           ,[Direccion]
           ,[Telefono]
           ,[Correo]
           ,[Observaciones]
           ,[Tipo_sangre])
     VALUES
           ('DNI', '71234567', 'Ana', 'Pérez Silva', 'F', '1990-05-15', 'Av. Larco 123', '955444333', 'ana.perez@email.com', 'Alérgica a la penicilina.', 'O+'),
           ('DNI', '78901234', 'Juan', 'Carlos Torres', 'M', '1985-11-20', 'Calle Las Flores 456', '911222333', 'juan.carlos@email.com', NULL, 'A+'),
           ('DNI', '74455667', 'Sofía', 'López Vega', 'F', '2015-08-10', 'Urb. El Recreo 789', '944555666', 'contacto.mama@email.com', 'Paciente pediátrico.', 'O-'),
           ('DNI', '44182725', 'Heinieh', 'Gonzalez', 'M', '1991-01-24', 'Avenida José Gabriel Condorcanqui 1985', '970900986', 'heinieh@hotmail.com', 'ninguna', 'O+')
GO

select * from Paciente

-- ==========================================
-- SECCIÓN: Insert_Cita.sql
-- ==========================================
USE [BD_Clinica]
GO

INSERT INTO [dbo].[Cita]
           ([Id_paciente]
           ,[Id_medico]
           ,[Fecha_cita]
           ,[Hora_cita]
           ,[Motivo]
           ,[Estado]
           ,[Observaciones])
     VALUES
           (1, 1, '2026-07-15', '09:00:00', 'Chequeo general anual', 'Programada', NULL),
           (2, 2, '2026-07-16', '10:30:00', 'Taquicardia y dolor leve en el pecho', 'Cancelada', 'El paciente debe traer electros anteriores.'),
           (3, 3, '2026-07-17', '15:00:00', 'Control de crecimiento', 'Programada', 'Viene acompañada de su madre.'),
           (4, 1, '2026-07-12', '08:00:00', 'presión alta', 'Programada', NULL)
GO

-- ==========================================
-- SECCIÓN: Insert_Usuario.sql
-- ==========================================
USE [BD_Clinica]
GO

INSERT INTO [dbo].[Usuario]
           ([Username]
           ,[Password]
           ,[Rol]
           ,[Id_medico])
     VALUES
           ('admin1', 'admin1', 'Recepcionista', NULL),
           ('admin2', 'admin2', 'Medico', 1),
           ('admin3', 'admin3', 'Medico', 2)
GO

select * from Usuario