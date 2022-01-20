using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFirst.Models
{
    public class UserModel
    {
        private readonly DataContext db;

        public UserModel(DataContext db)
        {
            this.db = db;
        }

        public List<User> GetUsers() 
        {


            
                return db.Users.ToList();
            
        }

        public void AddUser(string username) 
        {
           
                if (db.Users.FirstOrDefault(a => a.name == username) == null)
                {
                    db.Users.Add(new User { name = username });
                    db.SaveChangesAsync();
                }
            
        }
        public List<Chat> GetChats(string username) 
        {
           

              return db.Users.Include(x => x.Chats).FirstOrDefault(a => a.name == username).Chats;
            
        }
        public List<string> GetChatsNames(string username)
        {
            

                return db.Users.Include(x => x.Chats)
                    .FirstOrDefault(a => a.name == username)
                    .Chats.Select(x => x.Name).ToList();
            
        }
    


        public void migrate(List<string> usernames) 
        {

            foreach (var item in usernames)
            {
                AddUser(item);
            }
        }
        public void Subscribe(string chat, string username) 
        {
            
                
                db.Users.FirstOrDefault(a => a.name == username).Chats.Add(db.Chats.FirstOrDefault(a => a.Name == chat));
                db.SaveChangesAsync();
            

        }
        
    }
}
