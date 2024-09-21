
using Domain.Entities;
using Repository.Data;
using Repository.Helpers.ExceptionMessages;
using Repository.Helpers.Exceptions;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {

        public void Delete(int id)
        {
            if (AppDbContext<Student>.Entity.Count == 0) throw new CustomNotFoundException(CustomNotFoundMessage.message);

            foreach (var item in AppDbContext<Student>.Entity)
            {
                if (item.Id == id)
                {
                    AppDbContext<Student>.Entity.Remove(item);
                    return;
                }

            }
        }

        public List<Student> GetAllStudentsByGroupId(int groupId)
        {
            List<Student> students = new List<Student>();

            foreach (var item in AppDbContext<Student>.Entity)
            {
                if(item.Group.Id == groupId)
                {
                    students.Add(item);
                }
            }
            return students;
        }

        public List<Student> GetStudentsByAGe(int age)
        {
            return AppDbContext<Student>.Entity.Where(m=>m.Age == age).ToList();
        }

        public List<Student> SearchByNameOrSurname(string line)
        {
            return AppDbContext<Student>.Entity.Where(m => m.Name.Trim().ToLower() == line || m.Surname.Trim().ToLower() == line).ToList();

        }

        public void Update(Student data)
        {
            foreach (var item in AppDbContext<Student>.Entity)
            {
                if(item.Id == data.Id)
                {
                    AppDbContext<Student>.Entity.Remove(item);
                    AppDbContext<Student>.Entity.Add(data);
                    return;
                }
            }

            
        }

        public List<Student> GetAllStudents()
        {
            return AppDbContext<Student>.Entity;
        }
    }
}
