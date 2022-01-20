using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace CodeFirst
{
    public class Message
    {   
        public int Id { get; set; }
        public string author { get; set; }
        public DateTime date { get; set; }
        public string body { get; set; }

        [JsonIgnore]
        public int ChatId { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }

    }
    public class Chat
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Message> Messages {get;set;}= new List<Message>();
        [JsonIgnore]
        public List<User> Users { get; set; }= new List<User>();

        

    }
    public class User
    {
        
        public int Id { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }




}
