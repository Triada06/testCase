using Repository.Repositories.Interfaces;
using Domain.Entities;
using Repository.Data;
using System;
using System.Collections.Generic;
using Repository.Helpers.Exceptions;
using Repository.Helpers.ExceptionMessages;

namespace Repository.Repositories
{
    public class GroupRepository : BaseRepository<EntitiesGroup>, IGroupRepository
    {
        public List<EntitiesGroup> GetAll()
        {
            return AppDbContext<EntitiesGroup>.Entity;

        }

        public void Delete(int id)
        {
            EntitiesGroup group = new();

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Id == id)
                {
                    group = item;
                    AppDbContext<EntitiesGroup>.Entity.Remove(group);
                    return;
                }
            }
        }

        public List<EntitiesGroup> GetGroupsByRoom(string room)
        {
            List<EntitiesGroup> datas = new List<EntitiesGroup>();

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Room.ToLower().Trim() == room.ToLower().Trim())
                {
                    datas.Add(item);
                }
            }

            return datas;
        }

        public List<EntitiesGroup> GetGroupsByTeacher(string teacher)
        {
            List<EntitiesGroup> datas = new List<EntitiesGroup>();

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if(item.Teacher.ToLower().Trim() == teacher.ToLower().Trim())
                {
                    datas.Add(item);
                }
            }

            return datas;
        }

        public EntitiesGroup SearchGroupByName(string name)
        {
            int succesCount = 0;

            EntitiesGroup group = new();

            foreach (var item in AppDbContext<EntitiesGroup>.Entity) 
            {
                if (item.Name.ToLower().Trim() == name.ToLower().Trim())
                {
                    succesCount++;
                    group = item;
                    break;
                }
            }
            if (succesCount == 0) throw new CustomNotFoundException(CustomNotFoundMessage.message);
            else
            {
                return group;

            }
        }

        public void Update(EntitiesGroup group)
        {
            EntitiesGroup newGroup = new();
            int succesCount = 0;

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if(item.Id == group.Id)
                {
                    newGroup = item;
                    succesCount++;
                    break;
                }
            }

            if (succesCount == 0) throw new CustomNotFoundException(CustomNotFoundMessage.message);
            else 
            {
                newGroup.Name = group.Name;
                newGroup.Room = group.Room;
                newGroup.Teacher = group.Teacher;
            }

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Id == group.Id)
                {
                    AppDbContext<EntitiesGroup>.Entity.Remove(item);
                    AppDbContext<EntitiesGroup>.Entity.Add(group);
                    return;
                }
            }

        }
    }
}
