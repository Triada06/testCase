using CourseApp.Controllers;
using CourseApp.Helpers;
using CourseApp.Helpers.Extensions;
using CourseApp.StudentConstructor;

ColorfullPrint.Print(ConsoleColor.Cyan, @"
    Welcome, select an option:

    0.Exit
    
    Student services:

        1. Create a student
        2. Update a student
        3. Delete a student
        4. Get all students by group id
        5. Get a student by id
        6. Get students by their age
        7. Search by name or surname
        8. Get all students
        



    Group services :
        
        9. Create a group
        10. Update the group
        11. Delete Group
        12. Get a group by it's id
        13. Get groups by the teacher
        14. Get groups by room
        15. Get all groups
        16. Search a group by it's name
");


GroupController groupController = new GroupController();
StudentControllers studentControllers = new StudentControllers();


bool  isWorking = true;

while (isWorking)
{
    ColorfullPrint.Print(ConsoleColor.Yellow, "Select a new option: ");
    EnterLine: string line = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(line))
    {
        ColorfullPrint.Print(ConsoleColor.Red, "Choosing an option is required, it can't be empty, please select an option again");
        goto EnterLine;
    }

    if (!int.TryParse(line, out int option))
    {
        ColorfullPrint.Print(ConsoleColor.Red, "Invalid option type, select again: ");
        goto EnterLine;
    }



    switch (option)
    {
        case (int)EnumMethods.Exit:
            ColorfullPrint.Print(ConsoleColor.Yellow, "Programm is finished");
            isWorking = false;
            break;
        case (int)EnumMethods.CreateStudent:
            studentControllers.CreateStudent();
            break;
        case (int)EnumMethods.UpdateStudent:
            studentControllers.StudentUpdate();
            break;
        case (int)EnumMethods.DeleteStudent:
            studentControllers.DeleteStudent();
            break;
        case (int)EnumMethods.GetAllStudentsByGroupId:
            studentControllers.GetAllStudentsByGroupId();
            break;
        case (int)EnumMethods.GetStudentById:
            studentControllers.GetStudetnById();
            break;
        case (int)EnumMethods.GetStudentsByAge:
            studentControllers.GetStudentsByAge();
            break;
        case (int)EnumMethods.SearchForStudentByNameOrSurname:
            studentControllers.SearchStudentByNameOrSurname();
            break;
        case (int)EnumMethods.GetAllStudents:
            studentControllers.GetAll();
            break;
        case (int)EnumMethods.CreateGroup:
            groupController.CreateGroup();
            break;
        case (int)EnumMethods.UpdateGroup:
            groupController.Update();
            break;
        case (int)EnumMethods.DeleteGroup:
            groupController.DeleteGroup();
            break;
        case (int)EnumMethods.GetGroupById:
            groupController.GetGroupById();
            break;
        case (int)EnumMethods.GetGroupsByTeacher:
            groupController.GetGroupsByTeacher();
            break;
        case (int)EnumMethods.GetGruopsByRoom:
            groupController.GetGroupsByRoom();
            break;
        case (int)EnumMethods.GetAllGroups:
            groupController.ShowAllGroups();
            break;
        case (int)EnumMethods.SearchGroupByName:
            groupController.GetGroupByName();
            break;
        default:
            ColorfullPrint.Print(ConsoleColor.Red, "There is no such an option yet, select again or finish the program");
            goto EnterLine;
    }
}

