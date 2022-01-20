using CodeFirst;
using CodeFirst.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    { 
       private readonly ChatModel _chatmodel;
       private readonly UserModel _usermodel;
       private readonly MessageModel _messagemodel;

        public MessageController(ChatModel chatmodel, UserModel usermodel, MessageModel messagemodel)
        {
            _chatmodel = chatmodel;
            _usermodel = usermodel;
            _messagemodel = messagemodel;
        }

        [Authorize]
        [Route("api/subscribe")]
        [HttpPost]
        public void Subcribe(string chat)
        {
            _usermodel.Subscribe(chat, User.Identity.Name);
        }


       [Authorize]
        [Route("api/chats")]
        [HttpGet]
        public IEnumerable<string> GetChats()
        {
            
            var chats= _usermodel.GetChatsNames(User.Identity.Name);

            return chats;
        }

        [Authorize]
        [Route("api/addchat")]
        [HttpPost]
        public void AddChat(string chat)
        {
            _chatmodel.CreateChat(chat);
        }



        // GET: api/<MessageController>
        [HttpGet]
        public IEnumerable<Message> Get(string chatname)
        {


            if (chatname == null) chatname = "aaa";   //chatname=models.UserModel.GetChatsNames(User.Identity.Name).First();
            return _chatmodel.GetMessages(chatname);
           
        }

        // GET api/<MessageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MessageController>
        [Route("sendmsg")]
        [Authorize]
        [HttpPost]
        public void Post(MyMessageModel _message)
        {
            Message message = new Message();
            message.author = User.Identity.Name;
            message.body = _message.Body;
            _chatmodel.AddMessage(message,_message.ChatName);
        }


        // DELETE api/<MessageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

  
   

}
