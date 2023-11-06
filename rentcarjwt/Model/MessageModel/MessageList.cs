using rentcarjwt.Model.Car_;
using rentcarjwt.Model.Data.Entity;

namespace rentcarjwt.Model.MessageModel
{
    public class MessageList
    {
        public Guid idCar { get; set; }
        public string? foto { get; set; }
        public float? price { get; set; } = null!;                   //  ціна
        public string? brand { get; set; } = null!;                   //  марка
        public string? model { get; set; }                           //  модель
        public string? txt { get; set; } = null!;
        public DateTime? date { get; set; }                   //  дата створення
        public bool read { get; set; }
        public Guid userLessorId { get; set; } // Зовнішній ключ на таблицю User
        public Guid userTenantId { get; set; } // Зовнішній ключ на таблицю User
        public string? TenantEmai { get; set; }
        public string? LessorEmai { get; set; }
        public string? TenantName { get; set; }
        public string? LessorName { get; set; }
        public bool? online { get; set; }
        public string? showName { get; set; }
    }
}
