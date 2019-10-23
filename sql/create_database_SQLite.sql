create table racetype
(
    id   INTEGER not null
        primary key autoincrement,
    type TEXT    not null
);

create table race
(
    id          INTEGER not null
        primary key autoincrement,
    typeId      INTEGER not null
        references racetype
            on update cascade on delete cascade,
    date        TEXT    not null,
    name        TEXT    not null,
    location    TEXT    not null,
    splittimes  INTEGER not null,
    description TEXT
);

create unique index race_id_uindex
    on race (id);

create unique index racetype_id_uindex
    on racetype (id);

create table skier
(
    id           INTEGER not null
        primary key autoincrement,
    firstname    TEXT    not null,
    lastname     TEXT    not null,
    dateofbirth  TEXT    not null,
    nation       TEXT(3) not null,
    profileimage BLOB
);

create table racedata
(
    id           INTEGER not null
        primary key autoincrement,
    raceId       INTEGER not null
        references race
            on update cascade on delete cascade,
    skierId      INTEGER not null
        references skier
            on update cascade on delete cascade,
    disqualified INTEGER not null
);

create unique index racedata_id_uindex
    on racedata (id);

create unique index skier_id_uindex
    on skier (id);

create table splittime
(
    racedataId  INTEGER not null
        constraint FK_splittime_racedata
            references racedata
            on update cascade on delete cascade,
    runNo       INTEGER not null,
    splittimeNo INTEGER not null,
    splittime   REAL    not null,
    constraint PK_splittime
        unique (racedataId, runNo, splittimeNo),
    constraint runNo
        check (runNo >= 1 AND runNo <= 2),
    constraint splittimeNo
        check (splittimeNo > 0)
);

create table startlist
(
    raceId   INTEGER not null
        constraint FK_startlist_race
            references race
            on update cascade on delete cascade,
    skierId  INTEGER not null
        constraint FK_startlist_skier
            references skier
            on update cascade on delete cascade,
    startpos INTEGER not null
);

