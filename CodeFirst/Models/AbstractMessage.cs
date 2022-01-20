using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeFirst.Models
{
    public abstract class AbstractMessage
    {
        public abstract void Add(Message message);
        public abstract Message GetMessageById(int id);
        public abstract List<Message> GetMessages();
        public abstract List<Message> GetMessages(string username);
        public abstract List<Message> GetMessages(DateTime from, DateTime to);
        
    }

    public class MessageModel : AbstractMessage
    {
        private readonly DataContext db;

        public MessageModel(DataContext db)
        {
            this.db = db;
        }

        public override void Add(Message message)
        {
            message.date = DateTime.Now;
            
                db.AddAsync(message);
                db.SaveChangesAsync();
            
        }

        public override Message GetMessageById(int id)
        {
            
                return db.Messages.FirstOrDefault(a=>a.Id==id);
            
        }

        public override List<Message> GetMessages()
        {
            
                return db.Messages.ToList();
            
        }

        public override List<Message> GetMessages(string username)
        {
            
                return db.Messages.Where(a => a.author.ToLower().Contains(username.ToLower())).ToList();
            
        }

        public override List<Message> GetMessages(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }
    }
}
