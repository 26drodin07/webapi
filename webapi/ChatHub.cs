
using CodeFirst.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.Models;
using CodeFirst;

namespace webapi
{
    [Authorize]
    public class ChatHub:Hub
    {

        private readonly ChatModel _chatmodel;
        private readonly UserModel _usermodel;
        private readonly MessageModel _messagemodel;

        public ChatHub(ChatModel chatmodel, UserModel usermodel, MessageModel messagemodel)
        {
            _chatmodel = chatmodel;
            _usermodel = usermodel;
            _messagemodel = messagemodel;
        }


        public async Task Send(MyMessageModel _message)
        {
            _message.Author= Context.User.Identity.Name;
            Message message = new Message();
            message.author = _message.Author;
            message.body = _message.Body;
            _chatmodel.AddMessage(message, _message.ChatName);
            await this.Clients.All.SendAsync("Echo",_message);


        }

    }
}
