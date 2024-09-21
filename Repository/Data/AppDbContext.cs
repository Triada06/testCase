using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    
namespace Repository.Data
{
    public static class AppDbContext  <T>  where T : BaseEntity
    {
        public static List<T> Entity;


        static AppDbContext()   
        {
            Entity = new List<T>();
        }

    }
}
