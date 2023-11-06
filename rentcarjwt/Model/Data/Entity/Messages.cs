using System.ComponentModel.DataAnnotations.Schema;

namespace rentcarjwt.Model.Data.Entity
{
    public class Messages
    {
        public Guid Id { get; set; }

        public string txt { get; set; } 
        public DateTime Dt { get; set; }

        public Guid UserLessorId { get; set; } // Зовнішній ключ на таблицю User
        public User UserLessor { get; set; }   // здає в оренду 

        public Guid UserTenantId { get; set; } // Зовнішній ключ на таблицю User
        public User UserTenant { get; set; } // бере в оренду

        public Guid CarId { get; set; } // Зовнішній ключ на таблицю Car
        public Car Car { get; set; }

        public bool read { get; set; }
    }
}
