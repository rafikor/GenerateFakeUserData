using System.ComponentModel;

namespace ReactTest.Models
{
    public class UserDataModel
    {
        [DisplayName("Number")]
        public int number { get; set; }

        [DisplayName("Random ID")]
        public string randomId { get; set; }

        [DisplayName("Adress")]
        public string adress { get; set; }

        [DisplayName("Full name")]
        public string fullName { get; set; }

        [DisplayName("Phone")]
        public string phone { get; set; }
    }
}
