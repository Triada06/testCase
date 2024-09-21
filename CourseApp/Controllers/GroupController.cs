using CourseApp.Helpers.Extensions;
using Domain.Entities;
using Repository.Data;
using Repository.Helpers.ExceptionMessages;
using Repository.Helpers.Exceptions;
using Service.Service;
using Service.Service.Interfaces;

namespace CourseApp.Controllers
{
    public class GroupController
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;

        public GroupController()
        {
            _studentService = new StudentService();
            _groupService = new GroupService();
        }

        public void CreateGroup()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter group's data:");

            var groupName = PromptForValidGroupName();
            if (GroupExists(groupName))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Group with this name already exists, choose a new one.");
                groupName = PromptForValidGroupName();
            }

            var teacher = PromptForNonEmptyInput("Enter group's teacher:");
            var room = PromptForNonEmptyInput("Enter group's room name:");

            var group = new EntitiesGroup { Name = groupName, Teacher = teacher, Room = room };
            _groupService.Create(group);
            ColorfullPrint.Print(ConsoleColor.Green, "Data successfully created!");
        }

        public void ShowAllGroups()
        {
            var groups = _groupService.GetAll();
            if (!groups.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data to show :(");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"Group's ID: {group.Id}, Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");
            }
        }

        public void DeleteGroup()
        {
            var id = PromptForValidId("Enter group's ID:");
            var groupExists = AppDbContext<EntitiesGroup>.Entity.Any(item => item.Id == id);

            if (!groupExists)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;
            }

            var studentsInGroup = _studentService.GetAllStudents().Where(student => student.Group.Id == id).ToList();
            foreach (var student in studentsInGroup)
            {
                _studentService.Delete(student.Id);
            }

            _groupService.Delete(id);
            ColorfullPrint.Print(ConsoleColor.Green, "Group successfully deleted!");
        }

        public void GetGroupById()
        {
            var id = PromptForValidId("Enter group's ID:");
            var group = _groupService.GetById(id);

            if (group == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;
            }

            Console.WriteLine($"Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");
        }

        public void GetGroupsByTeacher()
        {
            var teacher = PromptForNonEmptyInput("Enter Teacher's name:");
            var groups = _groupService.GetGroupsByTeacher(teacher);

            if (!groups.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "There's no group with this teacher.");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");
            }
        }

        public void GetGroupsByRoom()
        {
            var room = PromptForNonEmptyInput("Enter room's name:");
            var groups = _groupService.GetGroupsByRoom(room);

            if (!groups.Any())
            {
                ColorfullPrint.Print(ConsoleColor.Red, "There's no group with this room.");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");
            }
        }

        public void GetGroupByName()
        {
            var name = PromptForNonEmptyInput("Enter group name:");
            try
            {
                var group = _groupService.SearchGroupByName(name);
                if (group == null || string.IsNullOrEmpty(group.Name))
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "No data found.");
                    return;
                }

                Console.WriteLine($"ID: {group.Id}, Name: {group.Name}, Teacher: {group.Teacher}, Room: {group.Room}");
            }
            catch (CustomNotFoundException)
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
            }
        }

        public void Update()
        {
            var id = PromptForValidId("Enter group's ID:");
            var group = AppDbContext<EntitiesGroup>.Entity.FirstOrDefault(item => item.Id == id);

            if (group == null)
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
                return;
            }

            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter the group's details if you want to update them; otherwise, they won't be changed:");
            var newName = PromptForOptionalValidGroupName("Enter the group name:");
            var newRoom = PromptForOptionalInput("Enter the group's room name:");
            var newTeacher = PromptForOptionalInput("Enter the group teacher's name:");

            if (!string.IsNullOrEmpty(newName)) group.Name = newName;
            if (!string.IsNullOrEmpty(newRoom)) group.Room = newRoom;
            if (!string.IsNullOrEmpty(newTeacher)) group.Teacher = newTeacher;

            try
            {
                _groupService.Update(group);
                ColorfullPrint.Print(ConsoleColor.Green, "Group successfully updated!");
            }
            catch (CustomNotFoundException)
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
            }
        }

        // Helper Methods

        private string PromptForValidGroupName()
        {
            while (true)
            {
                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name (format: XX-nnn):");
                var name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name) && ValidateGroupName(name))
                {
                    return name.Trim();
                }

                ColorfullPrint.Print(ConsoleColor.Red, "Invalid format. Group name must be like XX-nnn, where XX are uppercase letters, and nnn is a 3-digit number.");
            }
        }

        private bool ValidateGroupName(string name)
        {
            return name.Length == 6 &&
                   char.IsUpper(name[0]) &&
                   char.IsUpper(name[1]) &&
                   name[2] == '-' &&
                   char.IsDigit(name[3]) &&
                   char.IsDigit(name[4]) &&
                   char.IsDigit(name[5]);
        }

        private bool GroupExists(string name)
        {
            return AppDbContext<EntitiesGroup>.Entity.Any(item => item.Name == name);
        }

        private string PromptForNonEmptyInput(string prompt)
        {
            while (true)
            {
                ColorfullPrint.Print(ConsoleColor.DarkMagenta, prompt);
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input?.Trim()))
                {
                    return input.Trim();
                }

                ColorfullPrint.Print(ConsoleColor.DarkRed, "Input cannot be empty, please try again.");
            }
        }

        private string PromptForOptionalInput(string prompt)
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, prompt);
            return Console.ReadLine()?.Trim();
        }

        private int PromptForValidId(string prompt)
        {
            while (true)
            {
                ColorfullPrint.Print(ConsoleColor.DarkMagenta, prompt);
                var input = Console.ReadLine();

                if (int.TryParse(input, out var id))
                {
                    return id;
                }

                ColorfullPrint.Print(ConsoleColor.Red, "Invalid ID, please enter a valid number.");
            }
        }

        private string PromptForOptionalValidGroupName(string prompt)
        {
            var name = PromptForOptionalInput(prompt);
            return string.IsNullOrEmpty(name) || ValidateGroupName(name) ? name : PromptForOptionalValidGroupName(prompt);
        }
    }
}
