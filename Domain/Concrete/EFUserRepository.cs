using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFUserRepository : IUserRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<User> Users
        {
            get { return context.Users; }
        }

        public User SaveUser(User user)
        {
            user.Ticket = Guid.NewGuid().ToString();
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }


        public bool FindTicket(string ticket)
        {
            User dbEntry = context.Users.FirstOrDefault(u => (u.Ticket == ticket));

            if(dbEntry != null)
            {
                dbEntry.Confirm = true;
                dbEntry.Ticket  = null;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
