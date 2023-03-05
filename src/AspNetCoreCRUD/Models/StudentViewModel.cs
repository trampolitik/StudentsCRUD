using System.ComponentModel;

namespace AspNetCoreCRUD.Models
{    
    
    /// <summary>Класс для хранения информации, которую мы вводим</summar>
    public class StudentViewModel
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
    }
}
