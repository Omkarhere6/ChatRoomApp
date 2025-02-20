using ChatRoomApp.Implementations;
using ChatRoomApp.Interfaces;

namespace ChatRoomApp.Helpers
{
    public static class Configuration
    {
        public static void AddChatRoomApp(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IUserDao, UserDao>(provider =>
            {
                return new UserDao(connectionString);
            });
        }
    }
}
