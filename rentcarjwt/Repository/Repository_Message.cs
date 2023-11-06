using rentcarjwt.Model.Data;
using rentcarjwt.Model.Data.Entity;
using rentcarjwt.Model.MessageModel;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using Azure;


namespace rentcarjwt.Repository
{
    public class Repository_Message: IRepository_Message
    {
        private readonly DataContext _context;
        private readonly IRepository_Car repository_Car;
        private readonly IRepository_User repository_User;
        public Repository_Message(DataContext context, IRepository_Car _repository_Car, IRepository_User _repository_User)
        {
            _context = context;
            repository_Car = _repository_Car;
            repository_User = _repository_User;
        }
        public async Task<MessageResponse> sendMessage2(string emailSender, string idCar, string txt, string? userLessorId, string? userTenantId)
        {
            MessageResponse response = new MessageResponse();
            Car car = await repository_Car.getCar(new Guid(idCar));
            if (car == null)
            {
                return null;
            }

            User userSender = await repository_User.CheckEmail(emailSender);
            User userListener = new User();
            User userTenant=new User();
            if (userSender.Id == new Guid( userTenantId))
            {  
                 userListener = await repository_User.GetUserId(userLessorId);
                  userTenant = await repository_User.GetUserId(userTenantId);
            }
            else
            {
                 userListener = await repository_User.GetUserId(userTenantId);
                 userTenant = await repository_User.GetUserId(userLessorId);
            }


            Messages messages = new Messages();
            messages.Id = Guid.NewGuid();
            messages.Dt = DateTime.Now;
            messages.txt = txt;
            messages.UserLessor = userListener; //Тей хто отримує 
            messages.UserTenant = userTenant;   // Тей хто пише 
            messages.Car = car;
            messages.read = false;

            _context.Add(messages);
            await _context.SaveChangesAsync();

            response.date = messages.Dt;
            response.name = userTenant.firstName;
            response.txt = messages.txt;
            response.emailUser = userTenant.email;
            response.emailUser2 = userListener.email;

            return response;
        }

        public async  Task<MessageResponse> sendMessage(string emailSender, string idCar, string txt)
        {
            MessageResponse response=new MessageResponse();

            Car car= await repository_Car.getCar(new Guid(idCar));
            if (car==null)
            {
                return null;
            }
            User userListener= await repository_User.CheckEmail(car.UserEmail);
            User userSender = await repository_User.CheckEmail(emailSender);


            Messages messages=new Messages();
            messages.Id= Guid.NewGuid();
            messages.Dt= DateTime.Now;
            messages.txt= txt;
            messages.UserLessor = userListener; //Тей хто отримує 
            messages.UserTenant = userSender;   // Тей хто пише 
            messages.Car = car;
            messages.read = false;

            _context.Add(messages);
            await _context.SaveChangesAsync();

            response.date = messages.Dt;
            response.name = userSender.firstName;
            response.txt = messages.txt;
            response.emailUser = userSender.email;
            

            return response;
        }


        // Спрацьовує зі сторінки Tenant
        public async Task<List<MessageResponse>> getAllMessageIdCar(string emailSender, string idCar)
        {
            List<MessageResponse> result=new List<MessageResponse>();

            Car car = await repository_Car.getCar(new Guid(idCar));
            
            if (car == null)
            {
                return null;
            }
            User userListener = await repository_User.CheckEmail(car.UserEmail);
            User userSender = await repository_User.CheckEmail(emailSender);
            List<Messages> messages = await _context.Messages
                .Where(m => m.CarId == car.Id && m.UserLessorId == userListener.Id && m.UserTenantId == userSender.Id||
                m.CarId == car.Id && m.UserLessorId == userSender.Id && m.UserTenantId == userListener.Id)
                .OrderBy(m => m.Dt)
                .ToListAsync();
            for (int i = 0; i < messages.Count; i++)
            {
                MessageResponse temp=new MessageResponse();

                temp.date = messages[i].Dt;
                temp.name = messages[i].UserTenant.firstName;
                temp.txt = messages[i].txt;
                temp.emailUser = messages[i].UserTenant.email;
                result.Add(temp);
            }

            return result;
        }

        // Спрацьовує зі сторінки messagelist
        public async Task<List<MessageResponse>> getAllMessageIdCarTL(string IdUser1, string IdUser2, string idCar, string emailSender)
        {
            // писав IdUser2 (потрібно  первірити чи отримувач читав)
            // отримував IdUser1
            List<MessageResponse> result = new List<MessageResponse>();

            Car car = await repository_Car.getCar(new Guid(idCar));

            if (car == null)
            {
                return null;
            }
            User userListener = await repository_User.GetUserId(IdUser1);
            User userTenant = await repository_User.GetUserId(IdUser2);
            User userSender = await repository_User.CheckEmail(emailSender);
            if (userSender == null || userTenant == null || userListener == null)
            {
                return null;
            }
            List<Messages> messages = await _context.Messages
                .Include(u => u.UserLessor)
                .Include(u => u.UserTenant)
                .Where(m => m.CarId == car.Id && m.UserLessorId == userListener.Id && m.UserTenantId == userTenant.Id||
                            m.CarId == car.Id && m.UserLessorId == userTenant.Id && m.UserTenantId == userListener.Id)
                .OrderBy(m => m.Dt)
                .ToListAsync();
            for (int i = 0; i < messages.Count; i++)
            {
                MessageResponse temp = new MessageResponse();

                temp.date = messages[i].Dt;
                temp.name = messages[i].UserTenant.firstName;
                temp.txt = messages[i].txt;
                temp.emailUser = messages[i].UserTenant.email;
                if (messages[i].UserLessor.Id== userSender.Id)
                {
                    messages[i].read= true;
                }
                result.Add(temp);
                await _context.SaveChangesAsync();
            }

            return result;
        }



        public async  Task <int> getCountNoRaedMessage(string myEmail)
        {
            User userSender = await repository_User.CheckEmail(myEmail);
            int unreadMessagesCount = 0;
            if (userSender!=null) 
            {   unreadMessagesCount = _context.Messages
                 .Where(m => m.UserLessorId == userSender.Id && !m.read )
                 .Count();
            }
    
            return unreadMessagesCount;
        }

        public async Task<List<MessageList>> getLatestMessages(string myEmail)
        {
            List<MessageList> results  = new List<MessageList>();
            User userSender = await repository_User.CheckEmail(myEmail);
            if(userSender == null) { return null; }
            List<Messages> lastMessages= new List<Messages>();

            try
            {

                lastMessages = _context.Messages
                 .Include(u => u.Car)               // Включаю в вибірку об'єкт Car
                 .Include(u => u.UserLessor)        // Включаю в вибірку об'єкт UserLessor
                 .Include(u => u.UserTenant)         // Включаю в вибірку об'єкт UserTenant
                 .Where(m => m.UserLessorId == userSender.Id || m.UserTenantId == userSender.Id)
                 .GroupBy(m => new { m.CarId, m.UserTenantId,m.UserLessorId }) // Групування за авто і користувача
                 .AsEnumerable()
                 .Select(g => g.OrderByDescending(m => m.Dt).FirstOrDefault())//Відсортувати по даті OrderByDescending та FirstOrDefault отримую перший елемент (тобто повідомлення яке було написане останнє)
                 .ToList();


                for (int i = 0; i <= lastMessages.Count-1; i++)
                {// Повідомлень може виявитись більше одного
                    MessageList temp = new MessageList();
                    temp.foto = lastMessages[i].Car.foto;
                    temp.price = lastMessages[i].Car.price;
                    temp.brand = lastMessages[i].Car.brand;
                    temp.model = lastMessages[i].Car.model;
                    temp.idCar = lastMessages[i].Car.Id;
                    temp.txt = lastMessages[i].txt;
                    temp.date = lastMessages[i].Dt;
                    temp.date = lastMessages[i].Dt;
                    temp.read = lastMessages[i].read;
                    temp.userLessorId = lastMessages[i].UserLessorId;
                    temp.userTenantId = lastMessages[i].UserTenantId;
                    temp.TenantEmai = lastMessages[i].UserTenant.email;
                    temp.LessorEmai = lastMessages[i].UserLessor.email;
                    temp.TenantName = lastMessages[i].UserTenant.firstName;
                    temp.LessorName = lastMessages[i].UserLessor.firstName;


                    MessageList existingMessage = results.FirstOrDefault(m =>// Перевіряю чи повідомлення було вже в масиві
                    (m.userLessorId == temp.userLessorId && m.userTenantId == temp.userTenantId && m.idCar == temp.idCar) ||
                    (m.userLessorId == temp.userTenantId && m.userTenantId == temp.userLessorId && m.idCar == temp.idCar));

                    if (existingMessage != null)
                    {
                        // Якщо повідомлення  знайшлось перевіряю дату та додаю його до списку
                        if (existingMessage.date < temp.date)
                        {
                            results.Remove(existingMessage);
                            results.Add(temp);
                        }
                    }
                    else
                    {
                        // Якщо повідомлення не знайшлось додаю його до списку
                        results.Add(temp);
                    }
               
                   


                }
    

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return results;
        }
    }
}
