using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service.Interfaces
{
    public interface IStudentService
    {
        public void Create(Student data);
        public void Update(Student data);
        public void Delete(int data);
        public Student GetById(int id);
        public List<Student> GetStudentsByAge(int age);
        public List<Student> GetAllStudentsByGroupId(int groupId);
        public List<Student> SearchByNameOrSurname(string line);
        public List<Student> GetAllStudents();
    }
}
