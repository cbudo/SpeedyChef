CREATE TABLE Recipe
(
Recid int PRIMARY KEY CLUSTERED,
Recname varchar(255),
Recdesc varchar(255));

CREATE TABLE Task
(
Taskrec int REFERENCES Recipe(Recid),
Taskid int PRIMARY KEY CLUSTERED,
Taskname varchar(50),
Taskdesc varchar(255),
Tasktime int);

CREATE TABLE Food_Item
(
Foodname varchar(50) PRIMARY KEY CLUSTERED);

CREATE TABLE Member
(
Memname varchar(30) PRIMARY KEY CLUSTERED,
Mempass varchar(255));

CREATE TABLE Agenda
(
Agowner varchar(30) REFERENCES Member(Memname) unique,
Agname varchar(50),
Agstartdate date,
Agenddate date
PRIMARY KEY CLUSTERED (Agowner, Agname));

CREATE TABLE Member_Group
(
Groupadmin varchar(30) REFERENCES Member(Memname),
Groupname varchar(50) unique,
PRIMARY KEY CLUSTERED (Groupadmin, Groupname));

CREATE TABLE Tool
(
Toolname varchar(50) unique,
Groupname varchar(50) REFERENCES Member_Group(Groupname),
Tooldesc varchar(255),
PRIMARY KEY (Toolname, Groupname));

CREATE TABLE Oven
(
Toolname varchar(50) REFERENCES Tool(Toolname),
Groupname varchar(50) REFERENCES Member_Group(Groupname),
Ovenpwrtype varchar(255),
Ovenburnnum int,
PRIMARY KEY (Toolname, Groupname));






CREATE TABLE Recipe_Tasks
(
Recid int REFERENCES Recipe(Recid),
Taskid int REFERENCES Task(Taskid),
PRIMARY KEY (Recid, Taskid));

CREATE TABLE Task_Food_Items
(
Taskid int REFERENCES Task(Taskid),
Foodname varchar(50) REFERENCES Food_Item(Foodname),
PRIMARY KEY (Taskid, Foodname));

CREATE TABLE Member_Allergens
(
Memname varchar(30) REFERENCES Member(Memname),
Foodname varchar(50) REFERENCES Food_Item(Foodname),
Substitution varchar(50) REFERENCES Food_Item(Foodname),
PRIMARY KEY (Memname, Foodname));

CREATE TABLE Agenda_Recipes
(
Agowner varchar(30) REFERENCES Agenda(Agowner),
Recid int REFERENCES Recipe(Recid),
Recstart datetime,
Recend datetime,
Partysize int,
PRIMARY KEY (Agowner, Recid));