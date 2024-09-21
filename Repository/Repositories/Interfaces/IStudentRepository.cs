using Domain.Entities;


namespace Repository.Repositories.Interfaces
{
    public interface IStudentRepository  : IBaseRepository <Student>
    {
        public List<Student> GetStudentsByAGe(int age);
        public List<Student> GetAllStudentsByGroupId(int groupId);
        public List<Student> SearchByNameOrSurname(string line);
        public void Update(Student student);
        public List<Student> GetAllStudents();
        public void Delete(int id);

    }
}
