
 -------      17L-4015, 17L-4014, 16L-4390, 17L-4370

use master
drop database IMDB
create database IMDB
go
use IMDB

create table [User]
(
userID		int primary key identity (1, 1),
userName	varchar(50) not null,
email		varchar(50) unique not null CHECK(email LIKE '%.com'),
DOB			date not null CHECK(DOB < getDate()),
[password]	varchar(50) not null CHECK(len([password])>=8),
isAdmin		int not null CHECK (isAdmin in (0,1)),
)

go

create table Movie
(
movieID			int primary key,
movieName		varchar(50) not null unique,
rating			float CHECK(rating >= 0 AND rating <=10),
[description]	varchar(5000),
releaseDate		Date,
trailerLink		varchar(1000),
posterLink		varchar(1000)
)

go

create table UserRating
(
userID		int foreign key references [User](userID),
movieID		int foreign key references Movie(movieID),
userRating	int CHECK(userRating >= 0 AND userRating <=10),
primary key(userID, movieID)
)

go

create table UserReview
(
userID		int foreign key references [User](userID),
movieID		int foreign key references Movie(movieID),
review		varchar(1000)
primary key(userID, movieID)
)

go

create table WatchedList
(
userID		int foreign key references [User](userID),
movieID		int foreign key references Movie(movieID),
primary key(userID, movieID)
)
go




--------Procedure Part

go
Create Procedure ViewUsers 
As
Begin
Select userID, [password], email  From [User]
End
go


--Procedure 1
create procedure UserSignupCheck
@userName varchar(20), 
@email varchar(50), 
@DOB Date, 
@password varchar(50), 
@isAdmin char, 
@output int OUTPUT
As
Begin
	if exists (select * From [User] where userName=@userName)
	Begin
	set @output=0
	return
	End

    Insert into [User] (userName, email, DOB, [password], isAdmin) values (@userName, @email, @DOB, @password, @isAdmin)
	set @output=1
End
go


--Procedure 2
create Procedure UserLoginCheck 
@email varchar(50), 
@password varchar(50), 
@output int OUTPUT,
@out_userName varchar(20) OUTPUT,
@out_userID int OUTPUT,
@out_password varchar(50) OUTPUT,
@out_email varchar(50) OUTPUT,
@out_isAdmin int OUTPUT
As
Begin
	if  not exists (select * From [User] where email=@email AND [password]=@password)
	Begin
	set @output=0
	return
	End

	set @output=1
	select @out_userName = userName, 
		   @out_userID = userID,
		   @out_password = [password],
		   @out_email = email,
		   @out_isAdmin = isAdmin
		   from [User]
		   where email=@email AND [password]=@password
End

go



--Procedure 3
create procedure AddMovieToWatchList 
@userID int, 
@movieID int, 
@output int OUTPUT
As
Begin

	if exists (select * From WatchedList where userID=@userID AND movieID = @movieID)
	Begin
		set @output=0
	return
	End

    Insert into WatchedList values (@userID, @movieID)
	set @output=1 
End


go


--Procedure 4
create procedure DeleteMovieFromWatchList 
@userID int, 
@movieID int, 
@output int OUTPUT
As
Begin
	if exists (select * From WatchedList where userID=@userID AND movieID = @movieID)
	Begin
	delete from WatchedList where userID=@userID AND movieID = @movieID
	set @output=1
	return
	End

	set @output=0
End


go


--Procedure 5
create procedure AddMovieToDB
@movieID int, 
@movieName varchar(50), 
@rating float, 
@description varchar(5000), 
@releaseDate Date,
@trailerLink varchar(1000),
@posterLink  varchar(1000),
@output int OUTPUT
As
Begin

	if exists (select * From Movie where movieName = @movieName)
	Begin
	set @output=0
	return
	End

    Insert into Movie values (@movieId, @movieName, @rating, @description, @releaseDate, @trailerLink, @posterLink)
	set @output=1
End


go


--Procedure 6
create procedure SearchMovie 
@inp_mov_name varchar(50), 
@movieID int OUTPUT,
@movieName varchar(50) OUTPUT, 
@rating float OUTPUT, 
@description varchar(5000) OUTPUT, 
@releaseDate Date OUTPUT, 
@posterLink varchar(1000) OUTPUT,
@trailerLink varchar(1000) OUTPUT,
@output int OUTPUT
As
Begin
	if exists (select * From Movie where movieName LIKE @inp_mov_name)
	Begin
		select @movieID = movieID,
			   @movieName = movieName,
			   @description = [description],
			   @rating = rating,
			   @releaseDate = releaseDate,
			   @posterLink = posterLink,
			   @trailerLink = trailerLink
		from Movie
		where movieName LIKE @inp_mov_name
	
		set @output=1
	return
	End

	set @output=0 
End

go


--Procedure 7
create procedure AddUserRating
@userID int,
@movieID int, 
@rating int,
@output int OUTPUT
As
Begin
	set @output = 0
	if exists (select * From UserRating where userID=@userID AND movieID = @movieID)
	Begin
		UPDATE [UserRating] 
		SET userRating = @rating
		WHERE userID = @userID AND movieID = @movieID
		set @output=1 
	return
	End

    Insert into UserRating values (@userID, @movieID, @rating)
	set @output=2
End

go

--Procedure 9 
create procedure UpdateUserName
@userID int,
@userName varchar(20),
@output int OUTPUT
As
Begin
	set @output = 0
	if exists (select * From [User] where userID=@userID)
	Begin
	 Update [User] 
	 set userName = @userName
	 where userID = @userID
	 set @output = 1
	End
End 

go


--Procedure 10
create procedure UpdateUserPassword
@userID int,
@password varchar(50),
@output int OUTPUT
As
Begin
	set @output = 0
	if exists (select * From [User] where userID=@userID)
	Begin
	 Update [User] 
	 set [password] = @password
	 where userID = @userID
	 set @output = 1
	End
End 


go

--Procedure 11
CREATE TRIGGER CalculateMovieRating
ON UserRating
AFTER INSERT,
UPDATE 
AS
	DECLARE @m_rating INT;
	SET @m_rating = (SELECT AVG(userRating) from UserRating WHERE movieID = (SELECT movieID FROM inserted)
					GROUP BY movieID)
	UPDATE Movie
	SET rating = @m_rating 
	WHERE movieID = (SELECT movieID FROM inserted)


select * from [User]
select * from Movie
select * from WatchedList
select * from UserRating

DELETE FROM [User]
DELETE FROM Movie
DELETE FROM WatchedList

insert into [User] values ('admin', 'admin@gmail.com', '12-12-1990','admin123', 1)

Insert into Movie values (1, 'The Godfather', 9.2, 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.', '3-24-1972', 'https://www.youtube.com/watch?v=sY1S34973zA' ,'https://i.imgur.com/3l3Cdf0.jpg')
Insert into Movie values (2, 'Genocidal Organ', 6.4, 'Set in a time when Sarajevo was obliterated by a homemade nuclear device, the story reflects a world inundated with genocide. An American man by the name of John Paul seems to be responsible for all of this and intelligence agent Clavis Shepherd treks across the wasteland of the world to find him and the eponymous "genocidal organ."', '02-3-2017', 'https://www.youtube.com/watch?v=7KDYEjGecZc', 'https://i.imgur.com/d75ZUvz.jpg')
Insert into Movie values (3, 'Blade Runner 2049', 8.0, 'Young Blade Runner K discovery of a long-buried secret leads him to track down former Blade Runner Rick Deckard, who been missing for thirty years.'  ,'10-6-2017', 'https://www.youtube.com/watch?v=gCcx85zbxz4','https://i.imgur.com/BAaaITD.jpg')
Insert into Movie values (4, 'Monty Python and the Holy Grail', 8.2, 'King Arthur (Graham Chapman) and his Knights of the Round Table embark on a surreal, low-budget search for the Holy Grail, encountering many, very silly obstacles.', '5-25-1975', 'https://www.youtube.com/watch?v=urRkGvhXc8w' ,'https://i.imgur.com/ZxBoI1y.jpg')
