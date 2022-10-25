
CREATE TABLE Appointments (
	Id int IDENTITY(1,1) PRIMARY KEY,
	AppointmentId varchar(255),
   	PatientId int,
	Address varchar(255),
	StartDate varchar(255),
	EndDate varchar(255),
	PhoneNo varchar(255),
	DoctorId int,
	Description varchar(Max),
	CreatedBy int,
	Deleted bit,
	Attented bit,
	ResetPasswordCode varchar(max)
);

GO

CREATE TABLE Users (
	Id int IDENTITY(1,1) PRIMARY KEY,
	EmployeeId varchar(255),
   	Name varchar(255) NOT NULL,
   	Dob date,
	Gender varchar(255),
	Address varchar(255),
	Department varchar(255),
	Designation varchar(255),
	DateOfJoining date,
	PhoneNo varchar(255),
	Email varchar(255) NOT NULL,
	[Password] varchar(255) NOT NULL,
	Deleted bit,
	Admin bit
	
);

Go

CREATE TABLE Patients (
	Id int IDENTITY(1,1) PRIMARY KEY,
	PatientId varchar(255),
   	Name varchar(255) NOT NULL,
   	Dob date,
	Gender varchar(255),
	Address varchar(255),
	PhoneNo varchar(255),
	Email varchar(255),
	Deleted bit	
);

GO

Create procedure [dbo].[AddNewUser]  
(  
   @EmployeeId varchar(255),
   	@Name varchar(255),
   	@Dob date,
	@Gender varchar(255),
	@Address varchar(255),
	@Department varchar(255),
	@Designation varchar(255),
	@DateOfJoining date,
	@PhoneNo varchar(255),
	@Email varchar(255),
	@Password varchar(255),
	@Deleted bit,
	@Admin bit
)  
as  
begin  
   Insert into Users(EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,Email,[Password],Deleted,Admin) values(@EmployeeId,@Name,@Dob,@Gender,@Address,@Department,@Designation,@DateOfJoining,@PhoneNo,@Email,@Password,@Deleted,@Admin)  
End  
 
 GO

 Create Procedure [dbo].[GetSingleUser]  
(
    @Id int
)
as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin FROM Users WHERE Id=@Id 
End


Go

Create procedure [dbo].[UpdateUser]  
(  @Id int,
   @EmployeeId varchar(255),
   	@Name varchar(255),
   	@Dob date,
	@Gender varchar(255),
	@Address varchar(255),
	@Department varchar(255),
	@Designation varchar(255),
	@DateOfJoining date,
	@PhoneNo varchar(255),
	@Email varchar(255),
	@Password varchar(255),
	@Deleted bit,
	@Admin bit
)
as  
begin  
   Update Users  
   set 
   EmployeeId=@EmployeeId,
   Name =@Name,
   Dob=@Dob,
   Gender=@Gender,
   Address=@Address,
   Department=@Department,
   Designation=@Designation,
   DateOfJoining=@DateOfJoining,
   PhoneNo = @PhoneNo,
   [Password]= @Password,
   Email=@Email,
   Deleted =0,
   Admin =@Admin
   where Id=@Id  
End  

Go

Create PROCEDURE [dbo].[GetUsersCount]
 @Count INT OUTPUT,
 @TCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @Count = COUNT(Id)
    FROM Users WHERE ([Deleted]=0);
	SELECT @TCount = COUNT(Id)
    FROM Users;
END

Go


Create Procedure [dbo].[GetUsersRecord]  
(
   @PageNumber int,
   @RowsOfPage int,
   @search varchar (50),
   @orderColumn varchar (50),
   @orderdir varchar (50)

)
as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin FROM Users WHERE ([Deleted]=0) AND ((Name LIKE '%'+@search+'%') OR (Address LIKE '%'+@search+'%') OR (Email LIKE '%'+@search+'%') OR (Address LIKE '%'+@search+'%') OR (EmployeeId LIKE '%'+@search+'%') OR (PhoneNo LIKE '%'+@search+'%'))  
ORDER BY
        CASE WHEN @orderColumn = '0' AND @orderdir = 'desc' THEN EmployeeId END DESC,    
        CASE WHEN @orderColumn = '0' AND @orderdir = 'asc' THEN EmployeeId END ASC,    
        CASE WHEN @orderColumn = '1' AND @orderdir = 'desc' THEN Name END DESC,
        CASE WHEN @orderColumn = '1' AND @orderdir = 'asc' THEN Name END ASC,
        CASE WHEN @orderColumn = '4' AND @orderdir = 'desc' THEN Address END DESC,
        CASE WHEN @orderColumn = '4' AND @orderdir = 'asc' THEN Address END ASC,
		CASE WHEN @orderColumn = '8' AND @orderdir = 'desc' THEN PhoneNo END DESC,
        CASE WHEN @orderColumn = '8' AND @orderdir = 'asc' THEN PhoneNo END ASC,
        CASE WHEN @orderColumn = '10' AND @orderdir = 'desc' THEN Email END DESC,
        CASE WHEN @orderColumn = '10' AND @orderdir = 'asc' THEN Email END ASC
OFFSET @PageNumber ROWS
FETCH NEXT @RowsOfPage ROWS ONLY
End

Go



Create procedure [dbo].[DeleteUser]  
(  
   @Id int
)
as  
begin  
   Update Users  
   set [Deleted]=1
   where Id=@Id  
End 


Go

Create Procedure [dbo].[GetSingleUserDetails]  
(
    @Email varchar(50)
)
as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin FROM Users WHERE Email=@Email AND Deleted=0
End

Go

Create Procedure [dbo].[GetDoctors]  

as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin FROM Users WHERE Designation='Doctor' AND Deleted=0
End

Go


Create procedure [dbo].[AddNewPatient]  
(  
   @PatientId varchar(255),
   	@Name varchar(255),
   	@Dob date,
	@Gender varchar(255),
	@Address varchar(255),
	@PhoneNo varchar(255),
	@Email varchar(255)= null,
	@CreatedBy int,
	@Deleted bit
	
)
as  
begin  
   Insert into Patients(PatientId,Name,Dob,Gender,Address,PhoneNo,Email,CreatedBy,Deleted) values(@PatientId,@Name,@Dob,@Gender,@Address,@PhoneNo,@Email,@CreatedBy,@Deleted)  
End

go


Create PROCEDURE [dbo].[GetPatientsCount]
 @Count INT OUTPUT,
 @TCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @Count = COUNT(Id)
    FROM Patients WHERE ([Deleted]=0);
	SELECT @TCount = COUNT(Id)
    FROM Patients;
END

Go

Create Procedure [dbo].[GetSinglePatient]  
(
    @Id int
)
as  
begin  
   SELECT Id,PatientId,Name,Dob,Gender,Address,PhoneNo,Email,CreatedBy FROM Patients WHERE Id=@Id AND Deleted=0
End


Go

Create procedure [dbo].[UpdatePatient]  
(  @Id int,
   @PatientId varchar(255),
   	@Name varchar(255),
   	@Dob date,
	@Gender varchar(255),
	@Address varchar(255),
	@PhoneNo varchar(255),
	@Email varchar(255)= null,
	@CreatedBy int,
	@Deleted bit
)
as  
begin  
   Update Patients  
   set 
   PatientId=@PatientId,
   Name =@Name,
   Dob=@Dob,
   Gender=@Gender,
   Address=@Address,
   PhoneNo = @PhoneNo,
   Email=@Email,
   CreatedBy=@CreatedBy,
   Deleted =0
   where Id=@Id  
End  

Go

Create procedure [dbo].[DeletePatient]  
(  
   @Id int
)
as  
begin  
   Update Patients  
   set [Deleted]=1
   where Id=@Id  
End 


Go

Create Procedure [dbo].[GetPatientsRecord]  
(
   @PageNumber int,
   @RowsOfPage int,
   @search varchar (50),
   @orderColumn varchar (50),
   @orderdir varchar (50)

)
as  
begin  
   SELECT Id,PatientId,Name,Dob,Gender,Address,PhoneNo,Email,CreatedBy FROM Patients WHERE ([Deleted]=0) AND ((Name LIKE '%'+@search+'%') OR(Email LIKE '%'+@search+'%') OR (Address LIKE '%'+@search+'%') OR (PatientId LIKE '%'+@search+'%') OR (PhoneNo LIKE '%'+@search+'%'))  
ORDER BY
        CASE WHEN @orderColumn = '0' AND @orderdir = 'desc' THEN PatientId END DESC,    
        CASE WHEN @orderColumn = '0' AND @orderdir = 'asc' THEN PatientId END ASC,    
        CASE WHEN @orderColumn = '1' AND @orderdir = 'desc' THEN Name END DESC,
        CASE WHEN @orderColumn = '1' AND @orderdir = 'asc' THEN Name END ASC,
        CASE WHEN @orderColumn = '4' AND @orderdir = 'desc' THEN Address END DESC,
        CASE WHEN @orderColumn = '4' AND @orderdir = 'asc' THEN Address END ASC,
		CASE WHEN @orderColumn = '5' AND @orderdir = 'desc' THEN PhoneNo END DESC,
        CASE WHEN @orderColumn = '5' AND @orderdir = 'asc' THEN PhoneNo END ASC,
        CASE WHEN @orderColumn = '6' AND @orderdir = 'desc' THEN Email END DESC,
        CASE WHEN @orderColumn = '6' AND @orderdir = 'asc' THEN Email END ASC
OFFSET @PageNumber ROWS
FETCH NEXT @RowsOfPage ROWS ONLY
End

GO

CREATE procedure [dbo].[AddNewAppointment]  
(  
   
	@AppointmentId varchar(255),
   	@PatientId int,
	@Address varchar(255),
	@StartDate varchar(255),
	@EndDate varchar(255),
	@PhoneNo varchar(255)=null,
	@DoctorId int,
	@Description varchar(Max)=null,
	@CreatedBy int,
	@Attented bit,
	@Deleted bit
	
)
as  
begin  
   Insert into Appointments(AppointmentId,PatientId,Address,StartDate,EndDate,PhoneNo,DoctorId,Description,CreatedBy,Deleted,Attented) values(@AppointmentId,@PatientId,@Address,@StartDate,@EndDate,@PhoneNo,@DoctorId,@Description,@CreatedBy,@Deleted,@Attented)  
End


Go

Create PROCEDURE [dbo].[GetAppointmentCount]
 @Count INT OUTPUT,
 @TCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @Count = COUNT(Id)
    FROM Appointments WHERE ([Deleted]=0);
	SELECT @TCount = COUNT(Id)
    FROM Appointments;
END

Go

Create Procedure [dbo].[GetAppointments]  

as  
begin  
   SELECT Id,AppointmentId,PatientId,Address,StartDate,EndDate,PhoneNo,DoctorId,Description,CreatedBy,Attented FROM Appointments WHERE  Deleted=0
End

Go
Create procedure [dbo].[UpdateAppointment]  
(  @Id int,
   @AppointmentId varchar(255),
   	@PatientId int,
	@Address varchar(255),
	@StartDate varchar(255),
	@EndDate varchar(255),
	@PhoneNo varchar(255)=null,
	@DoctorId int,
	@Description varchar(Max)=null,
	@CreatedBy int,
	@Attented bit
)
as  
begin  
   Update Appointments  
   set 
   AppointmentId=@AppointmentId,
   	@PatientId=@PatientId,
	Address=@Address,
	StartDate =@StartDate,
	EndDate =@EndDate,
	PhoneNo=@PhoneNo,
	DoctorId=@DoctorId,
	Description =@Description,
	CreatedBy =@CreatedBy,
	Attented =@Attented

   where Id=@Id  
End  

Go

Create Procedure [dbo].[GetAppointmentsRecord]  
(
   @PageNumber int,
   @RowsOfPage int,
   @search varchar (50),
   @orderColumn varchar (50),
   @orderdir varchar (50)

)
as  
begin  
	
   SELECT Id,AppointmentId,PatientId,Address,StartDate,EndDate,PhoneNo,DoctorId,Description,CreatedBy,Attented FROM Appointments WHERE ([Deleted]=0) AND ((AppointmentId LIKE '%'+@search+'%')OR (Address LIKE '%'+@search+'%') OR (PatientId in (Select Id from Patients where[Deleted]=0 and Name like '%'+@search+'%' )) OR (DoctorId in (Select Id from Users where[Deleted]=0 and Name like '%'+@search+'%' )))  
ORDER BY
        CASE WHEN @orderColumn = '0' AND @orderdir = 'desc' THEN AppointmentId END DESC,    
        CASE WHEN @orderColumn = '0' AND @orderdir = 'asc' THEN AppointmentId END ASC,    
        CASE WHEN @orderColumn = '2' AND @orderdir = 'desc' THEN StartDate END DESC,
        CASE WHEN @orderColumn = '2' AND @orderdir = 'asc' THEN StartDate END ASC,
        CASE WHEN @orderColumn = '3' AND @orderdir = 'desc' THEN EndDate END DESC,
        CASE WHEN @orderColumn = '3' AND @orderdir = 'asc' THEN EndDate END ASC
		
OFFSET @PageNumber ROWS
FETCH NEXT @RowsOfPage ROWS ONLY
End

Go

Create Procedure [dbo].[GetSingleAppointment]  
(
    @Id int
)
as  
begin  
   SELECT Id,AppointmentId,PatientId,Address,StartDate,EndDate,PhoneNo,DoctorId,Description,CreatedBy,Attented FROM Appointments WHERE Id=@Id AND Deleted=0
End

Go

Create procedure [dbo].[DeleteAppointment]  
(  
   @Id int
)
as  
begin  
   Update Appointments  
   set [Deleted]=1
   where Id=@Id  
End 

Create procedure [dbo].[DeleteAppointmentForPatients]  
(  
   @Id int
)
as  
begin  
   Update Appointments  
   set [Deleted]=1
   where patientId=@Id  
End 

Go

CREATE procedure [dbo].[AddResetPasswordCode]  
(  
   @Id int,
	@ResetPasswordCode varchar(max)
   
)
as  
begin  
  Update Users  
   set ResetPasswordCode=@ResetPasswordCode
   where Id=@Id  
End

Go

Create Procedure [dbo].[GetUsersByResetPasswordCode]  
	@ResetPasswordCode varchar(max)
as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin,ResetPasswordCode FROM  Users WHERE  ResetPasswordCode=@ResetPasswordCode
End

go

CREATE procedure [dbo].[UpdatePassword]  
(  
	@Id int,
	@Password varchar(255)
   
)
as  
begin  
  Update Users  
   set Password=@Password
   where Id=@Id  
End


