using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
         public string FirstName { get; set; }
         public string LastName { get; set; }
         public string Street { get; set; }
        public string City { get; set; }
         public string Country { get; set; }
        #region For User Relation
        public string UserId { get; set; }
        public User User { get; set; }
        #endregion


    }
}
