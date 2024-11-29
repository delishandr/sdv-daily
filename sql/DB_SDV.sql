CREATE DATABASE DB_SDV_Daily
GO

USE DB_SDV_Daily
GO

CREATE TABLE season (
	id INT IDENTITY(1,1) PRIMARY KEY,
	name VARCHAR(15) NOT NULL,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT  NOT NULL DEFAULT (0)
)
GO

INSERT INTO season (name) VALUES ('Spring'), ('Summer'), ('Fall'), ('Winter');

CREATE TABLE crop_category (
	id INT IDENTITY(1,1) PRIMARY KEY,
	name VARCHAR(20) NOT NULL,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
GO

INSERT INTO crop_category (name) VALUES ('Fruit'), ('Vegetable'), ('Flower');

CREATE TABLE crop (
	id INT IDENTITY(1,1) PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	categoryId INT,
	growthTime INT NOT NULL,
	regrowthTime INT,
	unirrigated INT,
	isWalkable BIT NOT NULL DEFAULT (1),
	startYear INT NOT NULL DEFAULT (1),
	sellPrice INT,
	img TEXT,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
GO

INSERT INTO crop (name, categoryId, growthTime, regrowthTime, sellPrice)
VALUES
	('Blue Jazz', 3, 7, null, 50),
	('Carrot', 2, 3, null, 35),
	('Cauliflower', 2, 12, null, 175),
	('Coffee Bean', null, 10, 2, 15),
	('Garlic', 2, 4, null, 60),
	('Green Bean', 2, 10, 3, 40),
	('Kale', 2, 6, null, 110),
	('Parsnip', 2, 4, null, 35),
	('Potato', 2, 6, null, 80),
	('Rhubarb', 1, 4, null, 220),
	('Strawberry', 1, 8, 4, 120),
	('Tulip', 3, 6, null, 30),
	('Unmilled Rice', 2, 6, null, 30);

CREATE TABLE crop_season (
	id INT IDENTITY(1,1) PRIMARY KEY,
	cropId INT NOT NULL,
	seasonId INT NOT NULL,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
GO

INSERT INTO crop_season (cropId, seasonId)
VALUES
	(1, 1), (2, 1), (3, 1), (4, 1), (4, 2),
	(5, 1), (6, 1), (7, 1), (8, 1), (9, 1),
	(10, 1), (11, 1), (12, 1), (13, 1);

INSERT INTO crop (name, categoryId, growthTime, regrowthTime, sellPrice)
VALUES
	('Blueberry', 1, 13, 4, 50),
	('Corn', 2, 14, 4, 50),
	('Hops', 2, 11, 1, 25),
	('Hot Pepper', 1, 5, 3, 40),
	('Melon', 1, 12, null, 250),
	('Poppy', 3, 7, null, 140),
	('Radish', 2, 6, null, 90),
	('Red Cabbage', 2, 9, null, 260),
	('Starfruit', 1, 13, null, 750),
	('Summer Spangle', 3, 8, null, 90),
	('Summer Squash', 2, 6, 3, 45),
	('Sunflower', 3, 8, null, 80),
	('Tomato', 2, 11, 4, 60),
	('Wheat', 2, 4, null, 25);

INSERT INTO crop_season (cropId, seasonId)
VALUES
	(14, 2), (15, 2), (15, 3), (16, 2), (17, 2),
	(18, 2), (19, 2), (20, 2), (21, 2), (22, 2),
	(23, 2), (24, 2), (25, 2), (25, 3), (26, 2),
	(27, 2), (27, 3);

INSERT INTO crop (name, categoryId, growthTime, regrowthTime, sellPrice)
VALUES
	('Amaranth', 2, 7, null, 150),
	('Artichoke', 2, 8, null, 160),
	('Beet', 2, 6, null, 100),
	('Bok Choy', 2, 4, null, 80),
	('Broccoli', 2, 8, 4, 70),
	('Cranberries', 1, 7, 5, 75),
	('Eggplant', 2, 5, 5, 60),
	('Fairy Rose', 3, 12, null, 290),
	('Grape', 1, 10, 3, 80),
	('Pumpkin', 2, 13, null, 320),
	('Yam', 2, 10, null, 160);

INSERT INTO crop_season (cropId, seasonId)
VALUES
	(28, 3), (29, 3), (30, 3), (31, 3), (32, 3), (33, 3),
	(34, 3), (35, 3), (36, 3), (37, 3), (38, 3);

INSERT INTO crop (name, categoryId, growthTime, regrowthTime, sellPrice)
VALUES
	('Powdermelon', 1, 7, null, 60),
	('Ancient Fruit', 1, 28, 7, 550),
	('Cactus Fruit', 1, 12, 3, 75),
	('Fiber', null, 7, null, 1),
	('Pineapple', 1, 14, 7, 300),
	('Taro Root', 2, 7, null, 100),
	('Sweet Gem Berry', null, 24, null, 3000),
	('Tea Leaves', null, 20, 1, 50),
	('Wild Seeds', null, 7, null, null);

INSERT INTO crop_season (cropId, seasonId)
VALUES
	(39, 4), (40, 1), (40, 2), (40, 3),
	(41, 1), (41, 2), (41, 3), (41, 4),
	(42, 1), (42, 2), (42, 3), (42, 4),
	(43, 2), (44, 2), (45, 3),
	(46, 1), (46, 2), (46, 3),
	(47, 1), (47, 2), (47, 3), (47, 4);

CREATE TABLE villager (
	id INT IDENTITY(1,1) PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	birthDay INT,
	birthMonth INT,
	lovedGifts TEXT,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
GO

INSERT INTO villager (name, birthDay, birthMonth)
VALUES
	('Kent', 4, 1),
	('Lewis', 7, 1),
	('Vincent', 10, 1),
	('Haley', 14, 1),
	('Pam', 18, 1),
	('Shane', 20, 1),
	('Pierre', 26, 1),
	('Emily', 27, 1),
	('Jas', 4, 2),
	('Gus', 8, 2),
	('Maru', 10, 2),
	('Alex', 13, 2),
	('Sam', 17, 2),
	('Demetrius', 19, 2),
	('Dwarf', 22, 2),
	('Willy', 24, 2),
	('Leo', 26, 2),
	('Penny', 2, 3),
	('Elliott', 5, 3),
	('Jodi', 11, 3),
	('Abigail', 13, 3),
	('Sandy', 15, 3),
	('Marnie', 18, 3),
	('Robin', 21, 3),
	('George', 24, 3),
	('Krobus', 1, 4),
	('Linus', 3, 4),
	('Caroline', 7, 4),
	('Sebastian', 10, 4),
	('Harvey', 14, 4),
	('Wizard', 17, 4),
	('Evelyn', 20, 4),
	('Leah', 23, 4),
	('Clint', 26, 4);

update crop
set isWalkable = 0
where id = 6 or id = 16 or id = 36 or id = 46;

update crop
set startYear = 2
where id = 5 or id = 21 or id = 29;

update crop
set unirrigated = 8
where id = 13;

update crop
set unirrigated = 10
where id = 44;

create table event (
	id INT IDENTITY(1,1) PRIMARY KEY,
	name VARCHAR(50) NOT NULL,
	type VARCHAR(1) NOT NULL,
	location VARCHAR(20),
	startTime VARCHAR(10),
	endTime VARCHAR(10),
	preparation TEXT,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT  NOT NULL DEFAULT (0)
)
go

insert into event (name, type, location, startTime, endTime)
values 
	('Egg Festival', 'F', 'Pelican Town', '9am', '2pm'),
	('Desert Festival', 'F', 'The Desert', '10am', '2am'),
	('Salmonberry Season', 'S', null, null, null),
	('Flower Dance', 'F', 'Cindersap Forest', '9am', '2pm'),
	('Luau', 'F', 'The Beach', '9am', '2pm'),
	('Extra Forageables', 'S', 'The Beach', null, null),
	('Trout Derby', 'F', 'Cindersap Forest', '6:10am', '2am'),
	('Dance of the Moonlight Jellies', 'F', 'The Beach', '10pm', '12am'),
	('Blackberry Season', 'S', null, null, null),
	('Stardew Valley Fair', 'F', 'Pelican Town', '9am', '3pm'),
	('Spirit''s Eve', 'F', 'Pelican Town', '10pm', '11:50pm'),
	('Festival of Ice', 'F', 'Cindersap Forest', '9am', '2pm'),
	('SquidFest', 'F', 'The Beach', '6:10am', '2am'),
	('Night Market', 'F', 'The Beach', '5pm', '2am'),
	('Feast of the Winter Star', 'F', 'Pelican Town', '9am', '2pm');

create table event_day (
	id INT IDENTITY(1,1) PRIMARY KEY,
	eventId INT NOT NULL,
	day INT,
	season INT,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
go

insert into event_day (eventId, day, season)
values
	(1, 13, 1), (2, 15, 1), (2, 16, 1), (2, 17, 1),
	(3, 15, 1), (3, 16, 1), (3, 17, 1), (3, 18, 1),
	(4, 24, 1), (5, 11, 2), (6, 12, 2), (6, 13, 2),
	(6, 14, 2), (7, 20, 2), (7, 21, 2), (8, 28, 2),
	(9, 8, 3), (9, 9, 3), (9, 10, 3), (9, 11, 3),
	(10, 16, 3), (11, 27, 3), (12, 8, 4), (13, 12, 4),
	(13, 13, 4), (14, 15, 4), (14, 16, 4), (14, 17, 4),
	(15, 25, 4);

create table save_file (
	id INT IDENTITY(1,1) PRIMARY KEY,
	userId INT NOT NULL,
	name VARCHAR(30) NOT NULL,
	hasPet BIT NOT NULL DEFAULT(0),
	hasFarmAnimals BIT NOT NULL DEFAULT(0),
	isAgriculturist BIT NOT NULL DEFAULT(0),
	day INT,
	season INT,
	year INT,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
go

create table growing_crop (
	id INT IDENTITY(1,1) PRIMARY KEY,
	saveId INT NOT NULL,
	cropId INT NOT NULL,
	nextHarvest INT NOT NULL,
	nextHarvestSeason INT NOT NULL,
	amount INT NOT NULL,
	isOnGinger BIT NOT NULL DEFAULT(0),
	isIndoors BIT NOT NULL DEFAULT(0),
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
go

create table [user] (
	id INT IDENTITY(1,1) PRIMARY KEY,
	username VARCHAR(50) NOT NULL,
	email VARCHAR(100) NOT NULL,
	password VARCHAR(255) NOT NULL,
	roleId INT NOT NULL,
	lastSave INT,
	loginAttempt INT,
	lastLogin DATETIME,
	createdAt DATETIME NOT NULL DEFAULT (GETDATE()),
	updatedAt DATETIME,
	isDeleted BIT NOT NULL DEFAULT (0)
)
go
