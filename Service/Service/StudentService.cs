
using Domain.Entities;
using Repository.Data;
using Repository.Repositories;
using Repository.Repositories.Interfaces;
using Service.Service.Interfaces;

namespace Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService()
        {
             _studentRepository = new StudentRepository();
        }

        public void Create(Student data)
        {
            _studentRepository.Create(data);
        }

        public void Delete(int data)
        {
            _studentRepository.Delete(data);
        }

        public List<Student> GetAllStudentsByGroupId(int groupId)
        {
            return _studentRepository.GetAllStudentsByGroupId(groupId);
        }

        public Student GetById(int id)
        {
            return _studentRepository.GetById(id);
        }

        public List<Student> GetStudentsByAge(int age)
        {
            return _studentRepository.GetStudentsByAGe(age);
        }

        public List<Student> SearchByNameOrSurname(string line)
        {
            return _studentRepository.SearchByNameOrSurname(line);
        }

        public void Update(Student data)
        {
            _studentRepository.Update(data);
        }

        public List<Student> GetAllStudents()
        {
           return _studentRepository.GetAllStudents();
        }
    }
}
