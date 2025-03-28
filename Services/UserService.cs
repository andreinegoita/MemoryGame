using MemoryGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MemoryGame.Services
{
    public static class UserService
    {
        private static readonly string FilePath = "users.json";

        public static void SaveUsers(ObservableCollection<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static ObservableCollection<User> LoadUsers()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<ObservableCollection<User>>(json) ?? new ObservableCollection<User>();
            }
            return new ObservableCollection<User>();
        }
    }
}
