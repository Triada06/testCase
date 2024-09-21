using Domain.Entities;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;


        public GroupService()
        {
            _groupRepository = new GroupRepository();
        }

        public void Create(EntitiesGroup data)
        {
            _groupRepository.Create(data);
        }

        public void Delete(int id)
        {
            _groupRepository.Delete(id);
        }

        public List<EntitiesGroup> GetAll()
        {
           return _groupRepository.GetAll();
        }

        public EntitiesGroup GetById(int id)
        {
            return _groupRepository.GetById(id);
        }

        public List<EntitiesGroup> GetGroupsByRoom(string room)
        {
            return _groupRepository.GetGroupsByRoom(room);
        }

        public List<EntitiesGroup> GetGroupsByTeacher(string teacher)
        {
            return _groupRepository.GetGroupsByTeacher(teacher);
        }

        public EntitiesGroup SearchGroupByName(string name)
        {
           return _groupRepository.SearchGroupByName(name);
        }

        public void Update(EntitiesGroup group)
        {
            _groupRepository.Update(group);
        }
    }
}
