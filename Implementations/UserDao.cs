using ChatRoomApp.Interfaces;
using ChatRoomApp.Models;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatRoomApp.Implementations
{
    public class UserDao : IUserDao
    {
        private readonly string _connectionString;
        public UserDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> RegisterUserAsync(string userName, string password)
        {
            string encryptedPassword = EncryptPassword(password);

            string query = @"INSERT INTO users (user_name, password, created_datetime) VALUES (@UserName, @Password, @CreatedDatetime)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var rowsAffected = await connection.ExecuteAsync(query, new
                {
                    UserName = userName,
                    Password = encryptedPassword,
                    CreatedDatetime = DateTime.UtcNow
                });

                return rowsAffected > 0;
            }
        }

        public ClaimsPrincipal AuthenticateUserAsync(string userName, string password)
        {
            try
            {
                string encryptedPassword = EncryptPassword(password);

                string query = @"SELECT user_id, password FROM users WHERE user_name = @UserName";

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var user = connection.QueryFirstOrDefault<users>(query, new { UserName = userName });

                    if (user == null || encryptedPassword != user.password)
                    {
                        return null;
                    }

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    return principal;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            string query = @"SELECT COUNT(1) FROM users WHERE user_name = @UserName";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var count = await connection.ExecuteScalarAsync<int>(query, new { UserName = userName });
                return count > 0;
            }
        }
    }
}
