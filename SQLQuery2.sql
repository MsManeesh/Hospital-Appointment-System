alter table users add ResetPasswordCode varchar(max)

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


Create Procedure [dbo].[GetUsersByResetPasswordCode]  
	@ResetPasswordCode varchar(max)
as  
begin  
   SELECT Id,EmployeeId,Name,Dob,Gender,Address,Department,Designation,DateOfJoining,PhoneNo,[Password],Email,Deleted,Admin,ResetPasswordCode FROM  Users WHERE  ResetPasswordCode=@ResetPasswordCode
End

Go

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