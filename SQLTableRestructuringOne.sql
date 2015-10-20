use SpeedyChef2

GO

DROP TABLE Tool;

DROP TABLE Member_Group;

CREATE TABLE Member_Group
(
	Groupid int PRIMARY KEY CLUSTERED,
	Groupadmin varchar(30) REFERENCES Member(Memname),
	Groupname varchar(50)
)

CREATE TABLE Tool
(
	Toolid int PRIMARY KEY CLUSTERED,
	Toolname varchar(50),
	Tooldesc varchar(255)
)

CREATE TABLE Stove
(
	Toolid int REFERENCES Tool(Toolid) PRIMARY KEY CLUSTERED,
	Pwrtype varchar(50),
	Burnnum int
)

CREATE TABLE Group_Tool
(
	Toolid int REFERENCES Tool(Toolid),
	Groupid int REFERENCES Member_Group(Groupid),
	PRIMARY KEY CLUSTERED (Toolid, Groupid)
)

CREATE TABLE Group_Member
(
	Memname varchar(30) REFERENCES Member(Memname),
	Groupid int REFERENCES Member_Group(Groupid),
	PRIMARY KEY CLUSTERED (Memname, Groupid)
)