

using Domain.Base;

namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository <T> where T : BaseEntity
    {
        public void Create(T data);
        public T GetById(int id);
    }
}
