using CourseApp.Controllers;
using System;

namespace CourseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var studentController = new StudentController();
            var groupController = new GroupController();

            while (true)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    // Group Management
                    case "1":
                        groupController.CreateGroup();
                        break;
                    case "2":
                        groupController.ShowAllGroups();
                        break;
                    case "3":
                        groupController.DeleteGroup();
                        break;
                    case "4":
                        groupController.GetGroupById();
                        break;
                    case "5":
                        groupController.GetGroupsByTeacher();
                        break;
                    case "6":
                        groupController.GetGroupsByRoom();
                        break;
                    case "7":
                        groupController.GetGroupByName();
                        break;
                    case "8":
                        groupController.Update();
                        break;

                    // Student Management
                    case "9":
                        studentController.CreateStudent();
                        break;
                    case "10":
                        studentController.ShowAllStudents();
                        break;
                    case "11":
                        studentController.DeleteStudent();
                        break;
                    case "12":
                        studentController.GetStudentById();
                        break;
                    case "13":
                        studentController.UpdateStudent();
                        break;
                    case "14":
                        studentController.GetStudentsByGroup();
                        break;
                    case "15":
                        studentController.SearchStudentsByNameOrSurname();
                        break;

                    // Exit Option
                    case "0":
                        Console.WriteLine("Exiting the application. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("Welcome to CourseApp!");
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Create Group");
            Console.WriteLine("2. Show All Groups");
            Console.WriteLine("3. Delete Group");
            Console.WriteLine("4. Get Group By ID");
            Console.WriteLine("5. Get Groups By Teacher");
            Console.WriteLine("6. Get Groups By Room");
            Console.WriteLine("7. Get Group By Name");
            Console.WriteLine("8. Update Group");
            Console.WriteLine("9. Create Student");
            Console.WriteLine("10. Show All Students");
            Console.WriteLine("11. Delete Student");
            Console.WriteLine("12. Get Student By ID");
            Console.WriteLine("13. Update Student");
            Console.WriteLine("14. Get Students By Group");
            Console.WriteLine("15. Get Student By Name");
            Console.WriteLine("0. Exit");
        }
    }
}
