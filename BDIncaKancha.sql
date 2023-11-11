CREATE DATABASE IncaKancha
go

use IncaKancha
go

create table ROL(
IdRol int primary key,
Descripcion varchar(50)
)
go

insert into ROL(IdRol,Descripcion) values(1,'Administrador')
insert into ROL(IdRol,Descripcion) values(2,'Empleado')

go

CREATE TABLE USUARIOS(
     id INT IDENTITY(1,1) PRIMARY KEY,
     Nombres varchar(50) null,
	 Apellidos varchar(50) null,
     Correo varchar(50) null,
     clave varchar(50) null,
	 Dni varchar(8) null,
     IdRol INT, 
     FOREIGN KEY (IdRol) REFERENCES ROL(IdRol) 
);

INSERT INTO USUARIOS (Nombres,Apellidos, Correo, clave,Dni, IdRol) VALUES ('Luis Felipe','Saldaña Chuquiviguel', 'luis@gmail.com', 'luis8057','75477270', 1);
insert into USUARIOS (Nombres,Apellidos, Correo, clave,Dni, IdRol) values ('Patrick Alexander', 'Montes de Oca','pk@gmail.com','polluelo123456', '81561821', 2)

--Store Procedure:
CREATE PROCEDURE PListarUsuarios
	AS
	BEGIN
		SELECT id, Nombres,Apellidos, Correo, clave,Dni, IdRol
		FROM USUARIOS;
END;

CREATE TABLE Categoria (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    NombreCategoria VARCHAR(255) NOT NULL,
)
DROP TABle Productos
CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    NombreProducto VARCHAR(255) NOT NULL,
    Descripcion VARCHAR(255),
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
	Stock int NOT NULL,
    IdCategoria INT,
	IdProveedor INT,
    FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria),
	FOREIGN KEY (IdProveedor) REFERENCES Proveedor(IdProveedor)
)

CREATE TABLE Proveedor (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    NombreProveedor NVARCHAR(255) NOT NULL,
	direccion VARCHAR(250) NOT NULL,
	Telefono VARCHAR(24) NOT NULL
)

CREATE PROCEDURE sp_listar_categoria
AS
BEGIN
    SELECT IdCategoria,  NombreCategoria
    FROM Categoria;
END;

create procedure sp_insertar_categoria
@NombreCategoria varchar(255)
as begin
	insert into Categoria values(@NombreCategoria)
end

create procedure sp_buscar_categoria
@IdCategoria int
as begin
	select*from Categorias
	where IdCategoria = @IdCategoria
end

create procedure sp_actualizar_categoria
    @IdCategoria int,
    @NombreCategoria varchar(255)
as begin
    update Categoria
    set NombreCategoria = @NombreCategoria
    where IdCategoria = @IdCategoria;
end

CREATE PROCEDURE sp_eliminar_categoria
    @IdCategoria INT
AS
BEGIN
    DELETE FROM Categoria
    WHERE IdCategoria = @IdCategoria;
END

CREATE PROCEDURE sp_listar_proveedores
AS
BEGIN
    SELECT IdProveedor, NombreProveedor , direccion , Telefono
    FROM Proveedor;
END;

CREATE PROCEDURE sp_insertar_proveedor
@NombreProveedor VARCHAR(255),
@Direccion VARCHAR(255),
@Telefono VARCHAR(20)
AS
BEGIN
    INSERT INTO Proveedor (NombreProveedor, Direccion, Telefono)
    VALUES (@NombreProveedor, @Direccion, @Telefono);
END

CREATE PROCEDURE sp_actualizar_proveedor
@IdProveedor INT,
@NombreProveedor VARCHAR(255),
@Direccion VARCHAR(255),
@Telefono VARCHAR(20)
AS
BEGIN
    UPDATE Proveedor
    SET NombreProveedor = @NombreProveedor, Direccion = @Direccion, Telefono = @Telefono
    WHERE IdProveedor = @IdProveedor;
END

CREATE PROCEDURE usp_EliminarProveedor
    @IdProveedor INT
AS
BEGIN
    DELETE FROM Proveedor
    WHERE IdProveedor = @IdProveedor;
END;

drop procedure sp_actualizar_producto
CREATE PROCEDURE sp_listar_productos
AS
BEGIN
    SELECT P.IdProducto, P.NombreProducto, P.Descripcion, P.PrecioUnitario, P.Stock, C.NombreCategoria, C.IdCategoria, PR.NombreProveedor, PR.IdProveedor
    FROM Productos P
    INNER JOIN Categoria C ON C.IdCategoria = P.IdCategoria
    INNER JOIN Proveedor PR ON PR.IdProveedor = P.IdProveedor;
END

CREATE PROCEDURE sp_insertar_producto
@NombreProducto VARCHAR(255),
@Descripcion VARCHAR(255),
@PrecioUnitario DECIMAL(10, 2),
@Stock INT,
@IdCategoria INT,
@IdProveedor INT
AS
BEGIN
    INSERT INTO Productos (NombreProducto, Descripcion, PrecioUnitario, Stock, IdCategoria, IdProveedor)
    VALUES (@NombreProducto, @Descripcion, @PrecioUnitario, @Stock, @IdCategoria, @IdProveedor);
END

CREATE PROCEDURE sp_actualizar_producto
@IdProducto INT,
@NombreProducto VARCHAR(255),
@Descripcion VARCHAR(255),
@PrecioUnitario DECIMAL(10, 2),
@Stock INT,
@IdCategoria INT,
@IdProveedor INT
AS
BEGIN
    UPDATE Productos
    SET NombreProducto = @NombreProducto, Descripcion = @Descripcion,
        PrecioUnitario = @PrecioUnitario, Stock = @Stock, IdCategoria = @IdCategoria, IdProveedor = @IdProveedor
    WHERE IdProducto = @IdProducto;
END

CREATE PROCEDURE sp_eliminar_producto
@IdProducto INT
AS
BEGIN
    DELETE FROM Productos
    WHERE IdProducto = @IdProducto;
END

create proc usp_categoria
as
	select IdCategoria, NombreCategoria
	from Categorias

create proc usp_proveedor
as
	select IdProveedor, NombreProveedor
	from Proveedores


create table Venta(
	idVenta int primary key identity(1,1),
	idUsuario int references USUARIOS(id),
	documentoCliente varchar(40),
	nombreCliente varchar(40),
	subTotal decimal(10,2),
	impuestoTotal decimal(10,2),
	total decimal(10,2),
)

-- Tabla de Detalles de Venta
create table DetalleVenta(
	idDetalleVenta int primary key identity(1,1),
	idVenta int references Venta(idVenta),
	idProducto int references Productos(idProducto),
	cantidad int,
	precio decimal(10,2),
	total decimal(10,2)
)

CREATE PROCEDURE sp_RegistrarVenta(
    @documentoCliente VARCHAR(40),
    @nombreCliente VARCHAR(40),
    @idUsuario INT,
    @subTotal DECIMAL(10,2),
    @impuestoTotal DECIMAL(10,2),
    @total DECIMAL(10,2),
    @productos XML,
    @nroDocumento VARCHAR(6) OUTPUT
)
AS
BEGIN
    DECLARE @nrodocgenerado VARCHAR(6)
    DECLARE @nro INT
    DECLARE @idventa INT

    DECLARE @tbproductos TABLE (
        IdProducto INT,
        Cantidad INT,
        Precio DECIMAL(10,2),
        Total DECIMAL(10,2)
    )

    BEGIN TRY
        BEGIN TRANSACTION

        INSERT INTO @tbproductos(IdProducto, Cantidad, Precio, Total)
        SELECT 
            nodo.elemento.value('IdProducto[1]', 'INT') AS IdProducto,
            nodo.elemento.value('Cantidad[1]', 'INT') AS Cantidad,
            nodo.elemento.value('Precio[1]', 'DECIMAL(10,2)') AS Precio,
            nodo.elemento.value('Total[1]', 'DECIMAL(10,2)') AS Total
        FROM @productos.nodes('Productos/Item') nodo(elemento)

        UPDATE NumeroDocumento 
        SET @nro = id = id + 1

        SET @nrodocgenerado = RIGHT('000000' + CONVERT(VARCHAR(MAX), @nro), 6)

        INSERT INTO Venta(
            numeroDocumento,
            idUsuario,
            documentoCliente,
            nombreCliente,
            subTotal,
            impuestoTotal,
            total
        ) 
        VALUES (
            @nrodocgenerado,
            @idUsuario,
            @documentoCliente,
            @nombreCliente,
            @subTotal,
            @impuestoTotal,
            @total
        )

        SET @idventa = SCOPE_IDENTITY()

        INSERT INTO DetalleVenta(
            idVenta,
            idProducto,
            cantidad,
            precio,
            total
        ) 
        SELECT 
            @idventa,
            IdProducto,
            Cantidad,
            Precio,
            Total
        FROM @tbproductos

        UPDATE p 
        SET p.Stock = p.Stock - dv.Cantidad 
        FROM PRODUCTO p
        INNER JOIN @tbproductos dv ON dv.IdProducto = p.IdProducto

        COMMIT
        SET @nroDocumento = @nrodocgenerado
    END TRY
    BEGIN CATCH
        ROLLBACK
        SET @nroDocumento = ''
    END CATCH
END
