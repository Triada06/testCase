using CourseApp.Helpers.Extensions;
using Domain.Entities;
using Repository.Data;
using Repository.Helpers.ExceptionMessages;
using Repository.Helpers.Exceptions;
using Service.Service;
using Service.Service.Interfaces;

namespace CourseApp.Controllers
{
    public class StudentController
    {
        private IStudentService _studentService;
        private IGroupService _groupService;

        public StudentController()
        {
            _studentService = new StudentService();
            _groupService = new GroupService();
        }

        public void CreateStudent()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter student's data:");

        EnterName:
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.DarkRed, "Name can't be empty, please try again!");
                goto EnterName;
            }

        EnterGroupId:
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's ID: ");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Invalid group ID, please try again.");
                goto EnterGroupId;
            }

            var group = _groupService.GetById(groupId);
            if (group == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Group not found, please try again.");
                goto EnterGroupId;
            }

            var student = new Student
            {
                Name = name,
                Group = group
            };

            _studentService.Create(student);
            ColorfullPrint.Print(ConsoleColor.Green, "Student created successfully!");
        }

        public void ShowAllStudents()
        {
            var students = _studentService.GetAllStudents();
            if (!students.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No students to show.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Group ID: {student.Group.Id}");
            }
        }

        public void DeleteStudent()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Invalid ID, please try again.");
                return;
            }

            var student = _studentService.GetById(id);
            if (student == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Student not found.");
                return;
            }

            _studentService.Delete(id);
            ColorfullPrint.Print(ConsoleColor.Green, "Student deleted successfully!");
        }

        public void GetStudentById()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Invalid ID, please try again.");
                return;
            }

            var student = _studentService.GetById(id);
            if (student == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Student not found.");
                return;
            }

            Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Group ID: {student.Group.Id}");
        }

        public void UpdateStudent()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Invalid ID, please try again.");
                return;
            }

            var student = _studentService.GetById(id);
            if (student == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Student not found.");
                return;
            }

            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter new student name (leave blank to keep current): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                student.Name = name;
            }

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter new group ID (leave blank to keep current): ");
            string groupIdStr = Console.ReadLine();
            if (int.TryParse(groupIdStr, out int groupId))
            {
                var group = _groupService.GetById(groupId);
                if (group != null)
                {
                    student.Group = group;
                }
                else
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Group not found. Keeping current group.");
                }
            }

            _studentService.Update(student);
            ColorfullPrint.Print(ConsoleColor.Green, "Student updated successfully!");
        }

        public void GetStudentsByGroup()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group ID: ");
            if (!int.TryParse(Console.ReadLine(), out int groupId))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Invalid group ID, please try again.");
                return;
            }

            var students = _studentService.GetAllStudentsByGroupId(groupId);
            if (!students.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No students found in this group.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}");
            }
        }

        public void SearchStudentsByNameOrSurname()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's name or surname to search: ");
            string searchTerm = Console.ReadLine();

            var students = _studentService.SearchByNameOrSurname(searchTerm);
            if (!students.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No students found with that name or surname.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Group ID: {student.Group.Id}");
            }
        }
    }

}
