using Microsoft.Data.SqlClient;
using StudentManagementSystem.Helpers;
using StudentManagementSystem.Models;
using System.Data;

namespace StudentManagementSystem.Services
{
    public class StudentService
    {
        private readonly DatabaseHelper _databaseHelper;

        public StudentService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<List<StudentDto>> GetAllStudents()
        {
            var students = new List<StudentDto>();
            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_GetAllStudents", connection);
            command.CommandType = CommandType.StoredProcedure;

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                students.Add(new StudentDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Course = reader.GetString(4),
                    CreatedDate = reader.GetDateTime(5)
                });
            }
            return students;
        }

        public async Task<StudentDto?> GetStudentById(int id)
        {
            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_GetStudentById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new StudentDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Course = reader.GetString(4),
                    CreatedDate = reader.GetDateTime(5)
                };
            }
            return null;
        }

        public async Task<int> CreateStudent(CreateStudentDto student)
        {
            // Check if email exists
            if (await EmailExists(student.Email))
                throw new InvalidOperationException("Email already exists");

            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_CreateStudent", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@Course", student.Course);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateStudent(int id, UpdateStudentDto student)
        {
            var existing = await GetStudentById(id);
            if (existing == null)
                return false;

            // Check if email exists for another student
            if (await EmailExists(student.Email, id))
                throw new InvalidOperationException("Email already exists");

            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_UpdateStudent", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.Parameters.AddWithValue("@Course", student.Course);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_DeleteStudent", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Id", id);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        private async Task<bool> EmailExists(string email, int? excludeId = null)
        {
            using var connection = _databaseHelper.GetConnection();
            using var command = new SqlCommand("sp_CheckEmailExists", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@ExcludeId", excludeId ?? (object)DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
    }
}