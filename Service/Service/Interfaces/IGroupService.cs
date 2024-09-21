
using Domain.Entities;
using System.Text.RegularExpressions;


namespace Service.Service.Interfaces
{
    public interface IGroupService 
    {
        public void Create(EntitiesGroup data);
        public void Update(EntitiesGroup group);
        public void Delete(int id);
        public EntitiesGroup GetById(int id);
        public List<EntitiesGroup> GetGroupsByTeacher(string teacher);
        public List<EntitiesGroup> GetGroupsByRoom(string room);
        public List<EntitiesGroup> GetAll();
        public EntitiesGroup SearchGroupByName(string name);

    }
}
