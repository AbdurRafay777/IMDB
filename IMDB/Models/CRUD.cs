using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;


namespace IMDB.Models
{
    public class CRUD
    {

        public static string connectionString = "data source=localhost; Initial Catalog=IMDB;Integrated Security=true";
        public static int UserSignUpCheck(string userName, string email, DateTime DOB, string password)
        {

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 1;

            try
            {
                cmd = new SqlCommand("UserSignupCheck", con)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 20).Value = userName;
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email;
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = password;
                cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DOB;
                cmd.Parameters.Add("@isAdmin", SqlDbType.Int).Value = 0;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //0will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;

        }

        public static IMDB.Models.User UserLoginCheck(string email, string password)
        {
            IMDB.Models.User userDetail = new IMDB.Models.User();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 1;

            try
            {
                cmd = new SqlCommand("UserLoginCheck", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 50).Value = password;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@out_userName", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@out_userID", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@out_password", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@out_email", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@out_isAdmin", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@output"].Value);

                if (result > 0)
                {
                    userDetail.userID = Convert.ToInt32(cmd.Parameters["@out_userID"].Value);
                    userDetail.userName = Convert.ToString(cmd.Parameters["@out_userName"].Value);
                    userDetail.email = Convert.ToString(cmd.Parameters["@out_email"].Value);
                    userDetail.password = Convert.ToString(cmd.Parameters["@out_password"].Value);
                    userDetail.isAdmin = Convert.ToInt32(cmd.Parameters["@out_isAdmin"].Value);
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return userDetail;
        }

        public static int AddMovieToWatchList(int userID, int movieID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("AddMovieToWatchlist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@movieID", SqlDbType.Int).Value = movieID;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int DeleteMovieFromWatchList(int userID, int movieID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("DeleteMovieFromWatchlist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@movieID", SqlDbType.Int).Value = movieID;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int AddMovieToDB(string movieName, string rating, string description, DateTime releaseDate, string posterLink, string trailerLink)
        {

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 1;
            float m_rating = Convert.ToSingle(rating);
            try
            {
                cmd = new SqlCommand("AddMovieToDB", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@movieName", SqlDbType.VarChar, 50).Value = movieName;
                cmd.Parameters.Add("@rating", SqlDbType.Float).Value = rating;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 5000).Value = description;
                cmd.Parameters.Add("@releaseDate", SqlDbType.Date).Value = releaseDate;
                cmd.Parameters.Add("@posterLink", SqlDbType.VarChar, 1000).Value = posterLink;
                cmd.Parameters.Add("@trailerLink", SqlDbType.VarChar, 1000).Value = trailerLink;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //0will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static DataSet Search(string movie)
        {
            // SELECT TABLE CONTENTS FROM SQL !!

            string sqlStr = "select * From [Movie] where movieName LIKE '%" + movie + "%' ";

            // POPULATE DATASET OBJECT
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlStr, connectionString);
            da.SelectCommand.CommandType = CommandType.Text;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            da.Fill(ds);
            con.Close();
            return ds;
        }


        public static IMDB.Models.Movie SearchMovie(string inp_movieName)
        {
            IMDB.Models.Movie movieDetails = new IMDB.Models.Movie();

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 1;

            try
            {
                cmd = new SqlCommand("SearchMovie", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@inp_mov_name", SqlDbType.VarChar, 50).Value = inp_movieName;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@movieID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@movieName", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@rating", SqlDbType.Float).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@releaseDate", SqlDbType.Date).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@description", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@posterLink", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@trailerLink", SqlDbType.VarChar, 1000).Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);

                if (result > 0)
                {
                    movieDetails.m_ID = Convert.ToInt32(cmd.Parameters["@movieID"].Value);
                    movieDetails.m_name = Convert.ToString(cmd.Parameters["@movieName"].Value);
                    movieDetails.m_description = Convert.ToString(cmd.Parameters["@description"].Value);
                    movieDetails.m_rating = Convert.ToString(cmd.Parameters["@rating"].Value);
                    movieDetails.m_releaseDate = Convert.ToString(cmd.Parameters["@releaseDate"].Value);
                    movieDetails.m_poster = Convert.ToString(cmd.Parameters["@posterLink"].Value);
                    movieDetails.m_trailer = Convert.ToString(cmd.Parameters["@trailerLink"].Value);
                }
                else
                {
                    movieDetails.m_ID = -1;
                }
            }

            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //0will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }

            return movieDetails;
        }

        public static int AddUserRating(int userID, int movieID, int rating)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("AddUserRating", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@movieID", SqlDbType.Int).Value = movieID;
                cmd.Parameters.Add("@rating", SqlDbType.Int).Value = rating;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int AddUserReview(int userID, int movieID, string review)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("AddUserReview", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@movieID", SqlDbType.Int).Value = movieID;
                cmd.Parameters.Add("@review", SqlDbType.VarChar, 1000).Value = review;


                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = 0; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public static int UpdateUserName(int userID, string new_userName)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = -1;

            try
            {
                cmd = new SqlCommand("UpdateUserName", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@userName", SqlDbType.VarChar, 20).Value = new_userName;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int UpdateUserPassword(int userID, string new_password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            int result = -1;

            try
            {
                cmd = new SqlCommand("UpdateUserPassword", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = new_password;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table class = \"myTable\">";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public static DataTable GetMovieList()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            string query = "SELECT movieID, movieName, rating FROM Movie";
            DataTable dt = new DataTable();

            cmd = new SqlCommand(query, con);
            dt.Load(cmd.ExecuteReader());
            con.Close();
            return dt;
        }

        public static DataTable GetUserList()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            string query = "SELECT userName, email, DOB FROM [User]";
            DataTable dt = new DataTable();

            cmd = new SqlCommand(query, con);
            dt.Load(cmd.ExecuteReader());
            con.Close();
            return dt;
        }

        public static DataTable GetWatchList(int userID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            string query = "select M.movieName, M.rating From WatchedList W INNER JOIN Movie M ON W.movieID = M.movieID WHERE userID = " + userID;
            DataTable dt = new DataTable();

            cmd = new SqlCommand(query, con);
            dt.Load(cmd.ExecuteReader());
            con.Close();
            return dt;
        }

        public static int InWatchlist(int userID = 0, int movieID = 0)
        {
            int movieCount = 0;
            if (userID != 0)
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd;

                string query = "SELECT COUNT(*) FROM WatchedList WHERE userID = " + userID + "AND movieID = " + movieID;
                cmd = new SqlCommand(query, con);

                movieCount = (int)cmd.ExecuteScalar();
            }

            return movieCount;
        }
    }
}