
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository <EntitiesGroup>
    {
        public void Delete(int id);
        public void Update(EntitiesGroup group);       
        public List<EntitiesGroup> GetGroupsByTeacher(string teacher);
        public List<EntitiesGroup> GetGroupsByRoom(string room);
        public List<EntitiesGroup> GetAll();
        public EntitiesGroup SearchGroupByName(string name);

    }
}
