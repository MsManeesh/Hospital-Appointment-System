using Hospital_Appointment.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hospital_Appointment.DAL
{
    public class UserDbHandler
    {
        private SqlConnection con;
        private void connection()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString);
        }
        private void connectionManage()
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            else
                con.Open();
        }
        // **************** ADD NEW USER *********************
        public bool AddUser(User user)
        {
            user.EmployeeId = $"ABC00{GetTotalUserCount()}";
            connection();
            SqlCommand cmd = new SqlCommand("AddNewUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", user.EmployeeId);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Dob", user.Dob);
            cmd.Parameters.AddWithValue("@Gender", user.Gender);
            cmd.Parameters.AddWithValue("@Address", user.Address);
            cmd.Parameters.AddWithValue("@Department", user.Department);
            cmd.Parameters.AddWithValue("@Designation", user.Designation);
            cmd.Parameters.AddWithValue("@DateOfJoining", user.DateofJoining);
            cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Deleted", 0);
            cmd.Parameters.AddWithValue("@Admin", user.Admin);
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Get a single User with ID
        public User GetUser(int Id)
        {
            connection();
            User user = new User();

            SqlCommand cmd = new SqlCommand("GetSingleUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@Id", Id);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            if (dt.Rows.Count != 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.EmployeeId = Convert.ToString(dt.Rows[0]["EmployeeId"]);
                user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                user.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                user.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                user.Address = Convert.ToString(dt.Rows[0]["Address"]);
                user.Department = Convert.ToString(dt.Rows[0]["Department"]);
                user.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                user.DateofJoining = Convert.ToDateTime(dt.Rows[0]["DateofJoining"]);
                user.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                user.Admin = Convert.ToBoolean(dt.Rows[0]["Admin"]);

                dt.Dispose();
                return user;
            }
            dt.Dispose();
            return null;

        }

        //Update a user detais

        public bool UpdateUser(User user)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", user.Id);
            cmd.Parameters.AddWithValue("@EmployeeId", user.EmployeeId);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Dob", user.Dob);
            cmd.Parameters.AddWithValue("@Gender", user.Gender);
            cmd.Parameters.AddWithValue("@Address", user.Address);
            cmd.Parameters.AddWithValue("@Department", user.Department);
            cmd.Parameters.AddWithValue("@Designation", user.Designation);
            cmd.Parameters.AddWithValue("@DateOfJoining", user.DateofJoining);
            cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Deleted", 0);
            cmd.Parameters.AddWithValue("@Admin", user.Admin);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Get all Users
        public List<User> GetUsers(int pageStart, int rowsOfPage, string search, string orderColumn, string orderdir)
        {
            connection();
            List<User> users = new List<User>();

            SqlCommand cmd = new SqlCommand("GetUsersRecord", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@PageNumber", pageStart);
            cmd.Parameters.AddWithValue("@RowsOfPage", rowsOfPage);
            cmd.Parameters.AddWithValue("@search", search);
            cmd.Parameters.AddWithValue("@orderColumn", orderColumn);
            cmd.Parameters.AddWithValue("@orderdir", orderdir);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            foreach (DataRow dr in dt.Rows)
            {
                users.Add(
                    new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        EmployeeId = Convert.ToString(dr["EmployeeId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Dob = Convert.ToDateTime(dr["Dob"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Address = Convert.ToString(dr["Address"]),
                        Department = Convert.ToString(dr["Department"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        DateofJoining = Convert.ToDateTime(dr["DateofJoining"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Email = Convert.ToString(dr["Email"]),
                        Password = Convert.ToString(dr["Password"]),
                        Admin = Convert.ToBoolean(dr["Admin"])

                    });
            };
            dt.Dispose();
            return users;
        }
        //Get User Count
        public int GetUserCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetUsersCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@Count"].Value);
            return tempInt;

        }
        public bool DeleteUser(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }


        public User GetSingleUserDetails(string email)
        {
            connection();
            User user = new User();

            SqlCommand cmd = new SqlCommand("GetSingleUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@Email", email);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            if (dt.Rows.Count != 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.EmployeeId = Convert.ToString(dt.Rows[0]["EmployeeId"]);
                user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                user.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                user.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                user.Address = Convert.ToString(dt.Rows[0]["Address"]);
                user.Department = Convert.ToString(dt.Rows[0]["Department"]);
                user.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                user.DateofJoining = Convert.ToDateTime(dt.Rows[0]["DateofJoining"]);
                user.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                user.Admin = Convert.ToBoolean(dt.Rows[0]["Admin"]);

                dt.Dispose();
                return user;
            }
            dt.Dispose();
            return null;

        }
        private int GetTotalUserCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetUsersCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@TCount"].Value);
            return tempInt;

        }

        public List<User> GetDoctors()
        {
            connection();
            List<User> users = new List<User>();

            SqlCommand cmd = new SqlCommand("GetDoctors", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            foreach (DataRow dr in dt.Rows)
            {
                users.Add(
                    new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        EmployeeId = Convert.ToString(dr["EmployeeId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Dob = Convert.ToDateTime(dr["Dob"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Address = Convert.ToString(dr["Address"]),
                        Department = Convert.ToString(dr["Department"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        DateofJoining = Convert.ToDateTime(dr["DateofJoining"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Email = Convert.ToString(dr["Email"]),
                        Password = Convert.ToString(dr["Password"]),
                        Admin = Convert.ToBoolean(dr["Admin"])

                    });
            };
            dt.Dispose();
            return users;
        }

        public User GetUsersByResetPasswordCode(string code)
        {
            connection();
            User user = new User();

            SqlCommand cmd = new SqlCommand("GetUsersByResetPasswordCode", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@ResetPasswordCode", code);
            
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            if (dt.Rows.Count != 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.EmployeeId = Convert.ToString(dt.Rows[0]["EmployeeId"]);
                user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                user.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                user.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                user.Address = Convert.ToString(dt.Rows[0]["Address"]);
                user.Department = Convert.ToString(dt.Rows[0]["Department"]);
                user.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                user.DateofJoining = Convert.ToDateTime(dt.Rows[0]["DateofJoining"]);
                user.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                user.Admin = Convert.ToBoolean(dt.Rows[0]["Admin"]);

                dt.Dispose();
                return user;
            }
            dt.Dispose();
            return null;

        }


        public bool AddResetPasswordCode(string ResetPasswordCode,int Id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddResetPasswordCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ResetPasswordCode", ResetPasswordCode);
            cmd.Parameters.AddWithValue("@Id", Id);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }


        public bool UpdatePassword(string password, int Id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddResetPasswordCode", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Id", Id);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }

    }



}