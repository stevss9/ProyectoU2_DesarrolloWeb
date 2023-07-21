Create Database registro_notas;

USE registro_notas;

CREATE TABLE roles (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50)
);

CREATE TABLE estudents (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  last_name VARCHAR(50),
  dni VARCHAR(50),
  email VARCHAR(50)
);


CREATE TABLE teachers (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  last_name VARCHAR(50),
  specialty VARCHAR(50),
  email VARCHAR(50)
);

CREATE TABLE subjects (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  description VARCHAR(100),
  id_teacher INT,
  FOREIGN KEY (id_teacher) REFERENCES Teachers(id)
);

CREATE TABLE courses (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  id_teacher INT,
  FOREIGN KEY (id_teacher) REFERENCES Teachers(id)
);

CREATE TABLE notes (
  id INT IDENTITY(1,1) PRIMARY KEY,
  note_1 DECIMAL(5, 2),
  note_2 DECIMAL(5, 2),
  note_3 DECIMAL(5, 2),
  note_4 DECIMAL(5, 2),
  id_student INT,
  id_course INT,
  FOREIGN KEY (id_student) REFERENCES estudents(id),
  FOREIGN KEY (id_course) REFERENCES courses(id)
);

CREATE TABLE users (
  id INT IDENTITY(1,1) PRIMARY KEY,
  name VARCHAR(50),
  password VARCHAR(50),
  id_role INT,
  id_student INT,
  id_teacher INT,
  FOREIGN KEY (id_role) REFERENCES roles(id),
  FOREIGN KEY (id_student) REFERENCES estudents(id),
  FOREIGN KEY (id_teacher) REFERENCES teachers(id)
);

-- Crear tabla students_has_subjects
CREATE TABLE students_has_subjects (
  id_student INT,
  id_subject INT,
  FOREIGN KEY (id_student) REFERENCES Estudents(id),
  FOREIGN KEY (id_subject) REFERENCES Subjects(id),
  PRIMARY KEY (id_student, id_subject)
);

-- Agregar roles
INSERT INTO roles (name) VALUES ('administrador');
INSERT INTO roles (name) VALUES ('estudiante');
INSERT INTO roles (name) VALUES ('docente');

SELECT * FROM roles

-- Agregar usuario administrador
INSERT INTO Users (name, password, id_role) VALUES ('Administrador', 'contrase�a_admin', (SELECT id FROM Roles WHERE name = 'administrador'));

-- Agregar estudiante
INSERT INTO Users (name, password, id_role) VALUES ('Estudiante', 'contrase�a_estudiante', (SELECT id FROM Roles WHERE name = 'estudiante'));

-- Agregar profesor
INSERT INTO Users (name, password, id_role) VALUES ('Profesor', 'contrase�a_profesor', (SELECT id FROM Roles WHERE name = 'docente'));

SELECT * FROM users

-- Insertar datos en Teachers
INSERT INTO teachers (name, last_name, specialty, email) VALUES
('Mariela', 'Rosales', 'Matematicas', 'juan.perez@example.com'),
('Gaby', 'Perez', 'Historia', 'maria.gonzalez@example.com'),
('Pedro', 'Fomez', 'Fisica', 'pedro.sanchez@example.com');

-- Insertar datos en Estudents
INSERT INTO Estudents (name, last_name, dni, email) VALUES
('Ana', 'Lopez', '2350081010', 'ana.lopez@example.com'),
('Luis', 'Garcia', '2350081010', 'luis.garcia@example.com'),
('Maria', 'Martinez', '2350081010', 'maria.martinez@example.com');

