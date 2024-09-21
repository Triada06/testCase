

using Domain.Base;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        public void Create(T data)
        {
            AppDbContext<T>.Entity.Add(data);
        }

        public T GetById(int id)
        {
            var data = AppDbContext<T>.Entity.FirstOrDefault(x => x.Id == id);

            return data;
        }

       
    }
}
