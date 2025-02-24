using ChatRoomApp.Interfaces;
using ChatRoomApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatRoomApp.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly IChatRoomsDao _chatRoomsDao;
        public ChatRoomsController(IChatRoomsDao chatRoomsDao)
        {
            _chatRoomsDao = chatRoomsDao;
        }

        public IActionResult ChatRooms()
        {
            return View();
        }

        public IActionResult createChatRoom(chatrooms chatRoomModel)
        {
            try
            {
                var result = _chatRoomsDao.AddChatroomAsync(chatRoomModel);
                return Json(new { data = "True" });
            }
            catch (Exception ex)
            {
                return Json(new { data = "False" });
            }
        }

        public IActionResult getPublicChatRooms()
        {
            try
            {
                return Json(new { data = _chatRoomsDao.getPublicChatRooms() });
            }
            catch (Exception ex)
            {
                return Json(new { data = "False" });
            }
        }
    }
}
