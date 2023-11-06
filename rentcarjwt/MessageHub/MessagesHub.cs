using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using rentcarjwt.Model.MessageModel;
using rentcarjwt.Repository;
using rentcarjwt.Services.JwtToken;

namespace rentcarjwt.MessageHub
{
    /*
   Ключевой сущностью в SignalR, через которую клиенты обмениваются сообщеними 
   с сервером и между собой, является хаб (hub). 
   Хаб представляет некоторый класс, который унаследован от абстрактного класса 
   Microsoft.AspNetCore.SignalR.Hub.
   */
    public class MessagesHub : Hub
    {
        static List<UserMessage> userMessage = new();
        static List<string> Users = new();
        private readonly IRepository_Message repository_Message;
        private readonly IJwtToken jwtToken;
        public MessagesHub(IRepository_Message _repository_Message, IJwtToken _jwtToken)
        {
            repository_Message = _repository_Message;
            jwtToken = _jwtToken;
        }

        public override async Task OnConnectedAsync() //підключення клієнта
        {
            try
            {
                await Clients.Caller.SendAsync("UserConnected");
            }
            catch { }
        }


        //Якщо  підключення нового користувача вдале користувач викличе цей метод
        public async Task Connect(string token)
        {
            try { 
            conncted(token);
              await Clients.Caller.SendAsync("conectionListener_");
            }
            catch { }
        } 

        // перевіряю чи вже був доданий користувач 
        private void conncted(string token)
        {
           try { 
            string email = jwtToken.getEmailUser(token);

            var item = userMessage.FirstOrDefault(x => x.email == email);
            var id = Context.ConnectionId;
  

            if (item==null)
            {
                UserMessage user_message = new UserMessage();
                user_message.ConnectionId = id;
                user_message.email = email;
                Users.Add(id);
                userMessage.Add(user_message);
            }
           }
           catch { }
        }

        //Отримати кількість не прочитаних повідомлень
        public async Task getCountMessageNotRead(string token)
        {
            try
            { 
                         conncted(token);
                         string email = jwtToken.getEmailUser(token);
                         int count = await repository_Message.getCountNoRaedMessage(email);

                         await Clients.Caller.SendAsync("setCountMessageNotRead_", count);
            }
            catch { }
        }

        //Отримати останні повідомлення с конкретним користувачем та конкретним авто
        public async Task getLatestMessages(string token)
        {
            try
            { 
            conncted(token);
           string email = jwtToken.getEmailUser(token);
           List< MessageList> messageList= await repository_Message.getLatestMessages(email);
            for(int i = 0;i < messageList.Count; i++) 
            {
                if(email== messageList[i].TenantEmai)
                {
                    messageList[i].showName = messageList[i].LessorName;
                    for (int j = 0; j < userMessage.Count(); j++)
                    {
                        if (messageList[i].LessorEmai == userMessage[j].email)
                        {
                            messageList[i].online = true;
                        }

                    }
                }
                else if (email == messageList[i].LessorEmai)
                { 
                    messageList[i].showName = messageList[i].TenantName;
                    for (int j = 0; j < userMessage.Count(); j++)
                    {
                        if (messageList[i].TenantEmai == userMessage[j].email)
                        {
                            messageList[i].online = true;
                        }

                    }
                }

            }
           await Clients.Clients(this.Context.ConnectionId).SendAsync("setMessageList_", messageList);
            }
            catch { }
        }

        //Отримати  повідомлення с конкретним користувачем та конкретним авто
        public async Task getAllMessageIdCar( string idCar, string token)
        {
            try
            { 
            conncted(token);
            string emailSender = jwtToken.getEmailUser(token);
            List<MessageResponse> messageResponse = await repository_Message.getAllMessageIdCar(emailSender, idCar);
            await Clients.Clients(this.Context.ConnectionId).SendAsync("setMessageIdCarIdTenant_", messageResponse);
            }
            catch { }
        }

        public async Task getAllMessageIdCarTL(string idCar, string idUser1, string idUser2, string token)
        {
            try
            { 
            conncted(token);
            string emailSender = jwtToken.getEmailUser(token);
            List<MessageResponse> messageResponse = await repository_Message.getAllMessageIdCarTL(idUser1, idUser2, idCar,emailSender);
            await Clients.Clients(this.Context.ConnectionId).SendAsync("setMessageIdCarIdTenant_", messageResponse);
            }
            catch { }
        }

        public async Task sendMessage([FromQuery] string idCar, string token, string txt, string? userLessorId, string? userTenantId)
        {
            try { 
            MessageResponse messageResponse=new MessageResponse();
            string emailSender = jwtToken.getEmailUser(token);
            if(userLessorId!=null&& userTenantId != null)
            {
                messageResponse = await repository_Message.sendMessage2(emailSender, idCar, txt, userLessorId, userTenantId);
            }
            else
            {
                messageResponse = await repository_Message.sendMessage(emailSender, idCar, txt);
            }
            ResponseUsersMessage responseUsersMessage = getRum( messageResponse);
            if(responseUsersMessage.IdUser!= null) 
            {
                await Clients.Clients(responseUsersMessage.IdUser).SendAsync("addMessage_", messageResponse);
            }
            if (responseUsersMessage.IdUser2 != null)
            {
                await Clients.Clients(responseUsersMessage.IdUser2).SendAsync("addMessage_", messageResponse);
            }

            }
            catch { }
        }



        // OnDisconnectedAsync срабатывает при отключении клиента.
        // В качестве параметра передается сообщение об ошибке, которая описывает,
        // почему произошло отключение.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var item = Users.FirstOrDefault(x => x == Context.ConnectionId);
            var item2 = userMessage.FirstOrDefault(x => x.ConnectionId == item);
            if (item != null)
            { 
              
                Users.Remove(item);
                 userMessage.Remove(item2);
                var id = Context.ConnectionId;
                // Вызов метода UserDisconnected на всех клиентах
                //await Clients.All.SendAsync("UserDisconnected", id);
                await Clients.Clients(this.Context.ConnectionId).SendAsync("UserDisconnected_");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public  async Task OnDisconnected(string token)
        {
            try
            { 
            string email = jwtToken.getEmailUser(token);
          
            var item1 = userMessage.FirstOrDefault(x => x.email == email);
            if (item1 != null) 
            {
                var item2 = Users.FirstOrDefault(x => x == item1.ConnectionId);
                if (item2 != null)
                {

                    Users.Remove(item2);
                    userMessage.Remove(item1);
                    var id = Context.ConnectionId;

                }
            }
            }
            catch { }
        }

        private ResponseUsersMessage getRum(MessageResponse messageResponse)
        {
            ResponseUsersMessage responseUsersMessage =new ResponseUsersMessage();
            try
            {

                for (int i = 0; i < userMessage.Count(); i++)
            {
                if (userMessage[i].email== messageResponse.emailUser)
                {
                    responseUsersMessage.IdUser = userMessage[i].ConnectionId;
                }
                else if(userMessage[i].email == messageResponse.emailUser2)
                {
                    responseUsersMessage.IdUser2 = userMessage[i].ConnectionId;
                }

            }
           
          }
          catch { }
            return responseUsersMessage;
        }
    }

    public class ResponseUsersMessage
    {
        public string IdUser { get; set; } = null!;
        public string IdUser2 { get; set; } = null!;
    }
}
