-- Creacion de la base de datos.
create database prueba_tecnica_db;

-- Se accede a la base de datos.
use prueba_tecnica_db;

-- Se crea la entidad personas.
create table personas2 (
    Id int auto_increment primary key,
    nombres varchar(50),
    apellidos varchar(50),
    num_identificacion varchar(15),
    email varchar(100),
    tipo_identificacion varchar(20),
    fecha_creacion timestamp default current_timestamp,
    identificacion varchar(35) generated always as (concat(num_identificacion, tipo_identificacion)),
    nombre_completo varchar(100) generated always as (concat(nombres, ' ', apellidos))
);

-- Se crea la entidad usuarios.
create table usuario (
    Id int auto_increment primary key,
    usuario varchar(50) not null,
    password varchar(100) not null,
    fecha_creacion timestamp default current_timestamp
);

-- Se crea el Store Procedure para la consulta de personas creadas.
create procedure sp_consultar_personas()
begin
    select * from personas;
end;