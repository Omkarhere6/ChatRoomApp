using ChatRoomApp.Models;

namespace ChatRoomApp.Interfaces
{
    public interface IChatRoomsDao
    {
        Task<int> AddChatroomAsync(chatrooms chatroom);
    }
}
