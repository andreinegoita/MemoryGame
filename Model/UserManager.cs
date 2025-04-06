using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MemoryGame.Model
{
    public class UserManager
    {
        private static readonly string UsersFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "UserData", "users.json");

        private static List<User> _users = new List<User>();

        static UserManager()
        {
            LoadUsers();
        }

        public static List<User> GetAllUsers()
        {
            return _users;
        }

        public static User GetUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Name == name);
        }

        public static User GetOrCreateUser(string name)
        {
            var user = GetUserByName(name);
            if (user == null)
            {
                user = new User(name);
                _users.Add(user);
                SaveUsers();
            }
            return user;
        }

        public static void UpdateUserStatistics(string userName, bool won)
        {
            var user = GetOrCreateUser(userName);
            user.Statistics.AddGame(won);
            SaveUsers();
        }

        private static void LoadUsers()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(UsersFilePath));

                if (File.Exists(UsersFilePath))
                {
                    string json = File.ReadAllText(UsersFilePath);
                    _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading users: {ex.Message}");
                _users = new List<User>();
            }
        }

        private static void SaveUsers()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(UsersFilePath));
                string json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(UsersFilePath, json);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving users: {ex.Message}");
            }
        }
    }
}