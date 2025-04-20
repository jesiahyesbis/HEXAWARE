using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualArtGalleryNew.Entities;
namespace VirtualArtGalleryNew.DAO.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool RemoveUser(int userId);
        User GetUserById(int userId);
        List<User> SearchUsers(string keyword);
        List<User> ListAllUsers();
    }
}
