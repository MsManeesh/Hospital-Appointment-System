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
    public class PatientDbHandler
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
        // **************** ADD NEW Patient *********************
        public bool AddPatient(Patient patient)
        {
            patient.PatientId = $"ABCPAT00{GetPatientsTotalCount()}";
            connection();
            SqlCommand cmd = new SqlCommand("AddNewPatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
            cmd.Parameters.AddWithValue("@Name", patient.Name);
            cmd.Parameters.AddWithValue("@Dob", patient.Dob);
            cmd.Parameters.AddWithValue("@Gender", patient.Gender);
            cmd.Parameters.AddWithValue("@Address", patient.Address);
            cmd.Parameters.AddWithValue("@PhoneNo", patient.PhoneNo);
            cmd.Parameters.AddWithValue("@Email", patient.Email);
            cmd.Parameters.AddWithValue("@CreatedBy", patient.CreatedBy);
            cmd.Parameters.AddWithValue("@Deleted", 0);
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Get User Count
        private int GetPatientsTotalCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetPatientsCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@TCount"].Value);
            return tempInt;

        }
        public Patient GetPatient(int Id)
        {
            connection();
            Patient patient= new Patient();

            SqlCommand cmd = new SqlCommand("GetSinglePatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@Id", Id);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            if (dt.Rows.Count != 0)
            {
                patient.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                patient.PatientId = Convert.ToString(dt.Rows[0]["PatientId"]);
                patient.Name = Convert.ToString(dt.Rows[0]["Name"]);
                patient.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                patient.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                patient.Address = Convert.ToString(dt.Rows[0]["Address"]);
                patient.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                patient.Email = Convert.ToString(dt.Rows[0]["Email"]);
                patient.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);

                dt.Dispose();
                return patient;
            }
            dt.Dispose();
            return null;

        }

        public bool UpdatePatient(Patient patient)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdatePatient", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", patient.Id);
            cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
            cmd.Parameters.AddWithValue("@Name", patient.Name);
            cmd.Parameters.AddWithValue("@Dob", patient.Dob);
            cmd.Parameters.AddWithValue("@Gender", patient.Gender);
            cmd.Parameters.AddWithValue("@Address", patient.Address);
            cmd.Parameters.AddWithValue("@PhoneNo", patient.PhoneNo);
            cmd.Parameters.AddWithValue("@Email", patient.Email);
            cmd.Parameters.AddWithValue("@CreatedBy", patient.CreatedBy);
            cmd.Parameters.AddWithValue("@Deleted", 0);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool DeletePatient(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeletePatient", con);
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

        public List<Patient> GetPatients(int pageStart, int rowsOfPage, string search, string orderColumn, string orderdir)
        {
            connection();
            List<Patient> patients = new List<Patient>();

            SqlCommand cmd = new SqlCommand("GetPatientsRecord", con);
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
                patients.Add(
                    new Patient
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        PatientId = Convert.ToString(dr["PatientId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Dob = Convert.ToDateTime(dr["Dob"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Address = Convert.ToString(dr["Address"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Email = Convert.ToString(dr["Email"]),
                        CreatedBy = Convert.ToInt32(dr["CreatedBy"])

                    });
            };
            dt.Dispose();
            return patients;
        }
        public int GetPatientsCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetPatientsCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@Count"].Value);
            return tempInt;

        }
    }
}