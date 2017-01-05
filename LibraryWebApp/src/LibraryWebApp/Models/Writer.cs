using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models
{
    public class Writer
    {
        public Guid WriterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string AboutWriter { get; set; }
        public Guid UserId { get; set; }

        public List<Book> WritersBooks { get; set; }

        public Writer()
        {
            
        }

        public Writer(string firstName, string lastName, DateTime birthday, Guid userId)
        {
            WriterId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            UserId = userId;

        }

        public override bool Equals(object obj)
        {
            var newObj = obj as Writer;

            if (newObj == null)
            {
                return false;
            }
            else
            {
                return WriterId.Equals(newObj.WriterId);
            }

        }

        public override int GetHashCode()
        {
            return WriterId.GetHashCode();
        }

    }
}
