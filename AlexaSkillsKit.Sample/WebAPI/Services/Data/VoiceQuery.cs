using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace Sample.WebAPI.Services.Data
{
    public class VoiceQuery
    {
        private string _connectionString;

        public VoiceQuery()
        {
            _connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<CourseResponse> GetCoursesForBody(string body)
        {
            var connection = new SqlConnection(_connectionString);
            var sqlCommand = new SqlCommand();
            sqlCommand.CommandText = $"SELECT Id,Body,Paper,Loc AS Location,Room,Tutor,Type,StartTime,EndTime FROM BPPVoice WHERE body like '%{body}%'";
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = connection;

            connection.Open();

            var reader = sqlCommand.ExecuteReader();

            var courses = new List<CourseResponse>();
            while (reader.Read())
            {
                var course = new CourseResponse();
                course.Id = reader.GetInt32(0);
                course.Body = reader.GetString(1);
                course.Paper = reader.GetString(2);
                course.Location = reader.GetString(3);
                course.Room = reader.GetString(4);
                course.Tutor = reader.GetString(5);
                course.Type = reader.GetString(6);
                course.StartTime = reader.GetDateTime(7);
                course.EndTime = reader.GetDateTime(8);
                courses.Add(course);
            }

            connection.Close();

            return courses;
        }
    }
}