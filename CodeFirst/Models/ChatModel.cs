using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFirst.Models
{
    public class ChatModel
    {
        private readonly DataContext db;

        public ChatModel(DataContext _db)
        {
            db = _db;
        }

        public void CreateChat(string chat) 
        {
           
                if (db.Chats.FirstOrDefault(a => a.Name == chat) == null)
                {
                    db.Add(new Chat() { Name = chat });
                    db.SaveChanges();
                }
            
        }
        public Chat FindChatByName(string name) 
        {
            
                return db.Chats
                    .FirstOrDefault(
                    a => a.Name == name
                    );                             
            

        }
        public List<Chat> GetChatList()
        {
           
                return db.Chats.ToList();
            
        }

        public List<User> GetUsers(string chat) 
        {
            return FindChatByName(chat).Users;          
        }

        public void AddMessage(Message message,string chat) {

            
                message.ChatId = FindChatByName(chat).Id;
                message.date = DateTime.Now;
              
                db.Messages.Add(message);
                           
                db.SaveChanges();                                    
            
        }
        public List<Message> GetMessages(string chat) 
        {
            

                return db.Chats.Include(m=>m.Messages).FirstOrDefault(a=>a.Name==chat).Messages ?? new List<Message>(); 
            
              
            
        }

      









        
    }
}
