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
    public class AppointmentDbHandler
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
        // **************** ADD NEW Appointment *********************
        public bool AddAppointment(Appointment appointment)
        {
            appointment.AppointmentId = $"ABC0APP{GetTotalAppointmentCount()}";
            connection();
            SqlCommand cmd = new SqlCommand("AddNewAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
            cmd.Parameters.AddWithValue("@PatientId", appointment.patientNo);
            cmd.Parameters.AddWithValue("@Address", appointment.Address);
            cmd.Parameters.AddWithValue("@startDate", appointment.startDate);
            cmd.Parameters.AddWithValue("@EndDate", appointment.EndDate);
            cmd.Parameters.AddWithValue("@PhoneNo", appointment.PhoneNo);
            cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
            cmd.Parameters.AddWithValue("@Description", appointment.Description);
            cmd.Parameters.AddWithValue("@CreatedBy", appointment.CreatedBy);
            cmd.Parameters.AddWithValue("@Deleted", 0);
            cmd.Parameters.AddWithValue("@Attented", 0);
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }

        private int GetTotalAppointmentCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetAppointmentCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@TCount"].Value);
            return tempInt;

        }

        public List<Appointment> GetAppointments()
        {
            connection();
            List<Appointment> appointments = new List<Appointment>();

            SqlCommand cmd = new SqlCommand("GetAppointments", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            foreach (DataRow dr in dt.Rows)
            {
                appointments.Add(
                    new Appointment
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        AppointmentId = Convert.ToString(dr["AppointmentId"]),
                        patientNo = Convert.ToInt32(dr["PatientId"]),
                        Address = Convert.ToString(dr["Address"]),
                        startDate = Convert.ToString(dr["startDate"]),
                        EndDate = Convert.ToString(dr["EndDate"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        DoctorId = Convert.ToInt32(dr["DoctorId"]),
                        Description = Convert.ToString(dr["Description"]),
                        CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                        Attented = Convert.ToBoolean(dr["Attented"])

                    });
            };
            dt.Dispose();
            return appointments;
        }

        public List<Appointment> GetAppointmentsDatatable(int pageStart, int rowsOfPage, string search, string orderColumn, string orderdir)
        {
            connection();
            List<Appointment> appointments = new List<Appointment>();

            SqlCommand cmd = new SqlCommand("GetAppointmentsRecord", con);
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
                appointments.Add(
                new Appointment
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    AppointmentId = Convert.ToString(dr["AppointmentId"]),
                    patientNo = Convert.ToInt32(dr["PatientId"]),
                    Address = Convert.ToString(dr["Address"]),
                    startDate = Convert.ToString(dr["startDate"]),
                    EndDate = Convert.ToString(dr["EndDate"]),
                    PhoneNo = Convert.ToString(dr["PhoneNo"]),
                    DoctorId = Convert.ToInt32(dr["DoctorId"]),
                    Description = Convert.ToString(dr["Description"]),
                    CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                    Attented = Convert.ToBoolean(dr["Attented"])

                });
            };
            dt.Dispose();
            return appointments;
        }

        public int GetAppointmentCount()
        {
            int tempInt = 0;
            connection();

            SqlCommand cmd = new SqlCommand("GetAppointmentCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Count", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@TCount", SqlDbType.Int).Direction = ParameterDirection.Output;
            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();
            tempInt = Convert.ToInt32(cmd.Parameters["@Count"].Value);
            return tempInt;

        }

        public Appointment GetAppointment(int Id)
        {
            connection();
            Appointment appointment = new Appointment();

            SqlCommand cmd = new SqlCommand("GetSingleAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@Id", Id);
            DataTable dt = new DataTable();
            connectionManage();
            sd.Fill(dt);
            connectionManage();
            if (dt.Rows.Count != 0)
            {
                appointment.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                appointment.AppointmentId = Convert.ToString(dt.Rows[0]["AppointmentId"]);
                appointment.patientNo = Convert.ToInt32(dt.Rows[0]["PatientId"]);
                appointment.Address = Convert.ToString(dt.Rows[0]["Address"]);
                appointment.startDate = Convert.ToString(dt.Rows[0]["startDate"]);
                appointment.EndDate = Convert.ToString(dt.Rows[0]["EndDate"]);
                appointment.PhoneNo = Convert.ToString(dt.Rows[0]["PhoneNo"]);
                appointment.DoctorId = Convert.ToInt32(dt.Rows[0]["DoctorId"]);
                appointment.Description = Convert.ToString(dt.Rows[0]["Description"]);
                appointment.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                appointment.Attented = Convert.ToBoolean(dt.Rows[0]["Attented"]);
            };

            dt.Dispose();
            return appointment;
        }

        public bool UpdateAppointment(Appointment appointment)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", appointment.Id);
            cmd.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
            cmd.Parameters.AddWithValue("@PatientId", appointment.patientNo);
            cmd.Parameters.AddWithValue("@Address", appointment.Address);
            cmd.Parameters.AddWithValue("@startDate", appointment.startDate);
            cmd.Parameters.AddWithValue("@EndDate", appointment.EndDate);
            cmd.Parameters.AddWithValue("@PhoneNo", appointment.PhoneNo);
            cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
            cmd.Parameters.AddWithValue("@Description", appointment.Description);
            cmd.Parameters.AddWithValue("@CreatedBy", appointment.CreatedBy);
            cmd.Parameters.AddWithValue("@Attented", appointment.Attented);

            connectionManage();
            int i = cmd.ExecuteNonQuery();
            connectionManage();

            if (i >= 1)
                return true;
            else
                return false;
        }


        public bool DeleteAppointment(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteAppointment", con);
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

        public bool DeleteAppointmenForPatients(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteAppointmentForPatients", con);
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
    }


}