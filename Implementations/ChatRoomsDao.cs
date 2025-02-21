using ChatRoomApp.Interfaces;
using ChatRoomApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ChatRoomApp.Implementations
{
    public class ChatRoomsDao : IChatRoomsDao
    {
        private readonly string _connectionString;
        public ChatRoomsDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddChatroomAsync(chatrooms chatroom)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = @"INSERT INTO Chatrooms (room_name, room_description, room_type, room_code, user_name)
                          VALUES (@RoomName, @RoomDescription, @RoomType, @RoomCode, @UserName);
                          SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parameters = new
                {
                    RoomName = chatroom.room_name,
                    RoomDescription = chatroom.room_description,
                    RoomType = chatroom.room_type,
                    RoomCode = await GenerateUniqueRoomCodeAsync(chatroom.room_type),
                    UserName = chatroom.user_name
                };

                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<bool> CheckRoomCodeUnique(string roomCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM Chatrooms WHERE room_code = @RoomCode";
                var count = await connection.ExecuteScalarAsync<int>(query, new { RoomCode = roomCode });

                return count == 0;
            }
        }

        public async Task<string> GenerateUniqueRoomCodeAsync(string RoomType)
        {
            if (RoomType == "Private")
            {
                bool isUnique = false;
                while (!isUnique)
                {
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    StringBuilder roomCode = new StringBuilder();
                    Random random = new Random();

                    for (int i = 0; i < 5; i++)
                    {
                        roomCode.Append(chars[random.Next(chars.Length)]);
                    }
                    isUnique = await CheckRoomCodeUnique(roomCode.ToString());
                    return roomCode.ToString();
                }
                return "Private";
            }
            else
            {
                return "Public";
            }

        }
    }
}
