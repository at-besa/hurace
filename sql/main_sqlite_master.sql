INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'sqlite_sequence', 'sqlite_sequence', 9, 'CREATE TABLE sqlite_sequence(name,seq)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'racedata', 'racedata', 2, 'CREATE TABLE "racedata"
(
	id INTEGER not null
		primary key autoincrement,
	raceId INTEGER not null
		constraint FK_racedata_race
			references race
				on update cascade on delete cascade,
	skierId INTEGER not null
		constraint FK_racedata_skier
			references skier
				on update cascade on delete cascade,
	disqualified INTEGER not null
)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'racedata_id_uindex', 'racedata', 7, 'CREATE UNIQUE INDEX racedata_id_uindex
	on racedata (id)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'racetype', 'racetype', 13, 'CREATE TABLE "racetype"
(
	id INTEGER not null
		primary key autoincrement,
	type TEXT not null
, [numberOfRuns] INTEGER DEFAULT (1) NOT NULL)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'racetype_id_uindex', 'racetype', 3, 'CREATE UNIQUE INDEX racetype_id_uindex
	on racetype (id)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'skier', 'skier', 14, 'CREATE TABLE "skier"
(
	id INTEGER not null
		primary key autoincrement,
	firstname TEXT not null,
	lastname TEXT not null,
	dateofbirth TEXT not null,
	nation TEXT(3) not null,
	profileimage BLOB
, [weight] TEXT NULL, [height] TEXT NULL, [sex] TEXT NULL)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'skier_id_uindex', 'skier', 6, 'CREATE UNIQUE INDEX skier_id_uindex
	on skier (id)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'race', 'race', 10, 'CREATE TABLE "race"
(
	id INTEGER not null
		primary key autoincrement,
	typeId INTEGER not null
		constraint FK_race_racetype
			references racetype
				on update cascade on delete cascade,
	date TEXT not null,
	name TEXT not null,
	location TEXT not null,
	splittimes INTEGER not null,
	sex TEXT
)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'race_id_uindex', 'race', 5, 'CREATE UNIQUE INDEX race_id_uindex
	on race (id)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'splittime', 'splittime', 43, 'CREATE TABLE "splittime" (
"racedataId"  INTEGER NOT NULL,
"runNo"  INTEGER NOT NULL,
"splittimeNo"  INTEGER NOT NULL,
"splittime"  TEXT NOT NULL,
PRIMARY KEY ("racedataId" ASC, "runNo" ASC, "splittimeNo" ASC),
CONSTRAINT "FK_splittime_racedata" FOREIGN KEY ("racedataId") REFERENCES "racedata" ("id") ON DELETE CASCADE ON UPDATE CASCADE,
CONSTRAINT "PK_splittime" UNIQUE ("racedataId" ASC, "runNo" ASC, "splittimeNo" ASC),
CONSTRAINT "runNo" CHECK (runNo >= 1 AND runNo <= 2),
CONSTRAINT "splittimeNo" CHECK (splittimeNo >0)
)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'sqlite_autoindex_splittime_1', 'splittime', 44, null);
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('table', 'startlist', 'startlist', 95, 'CREATE TABLE "startlist"
(
	raceId INTEGER not null
		constraint FK_startlist_race
			references race
				on update cascade on delete cascade,
	skierId INTEGER not null
		references skier
			on update cascade on delete cascade,
	startpos INTEGER not null,
	constraint startlist_pk
		primary key (raceId, skierId, startpos)
)');
INSERT INTO sqlite_master (type, name, tbl_name, rootpage, sql) VALUES ('index', 'sqlite_autoindex_startlist_1', 'startlist', 96, null);