

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
        private IGroupService _groupService;
        private IStudentService _studentService;

        public GroupController()
        {
            _studentService = new StudentService();
            _groupService = new GroupService();
        }

        public void CreateGroup()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter group's data: ");

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name: ");
            EnterName:  string name = Console.ReadLine();




            if(string.IsNullOrEmpty(name.Trim()) )
            {
                ColorfullPrint.Print(ConsoleColor.DarkRed, "Name can't be empty, enter again! ");
                goto EnterName;
            }


            for (int i = 0; i < 2; i++)
            {
                if (name[i] < 65 || name[i] > 90)
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are upper letters and nnn is 3 digit number");
                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                    goto EnterName;
                }
            }


            if (name[2] != '-')
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                goto EnterName;
            }

            for (int i = 3; i <= 5; i++)
            {
                

                if (char.IsLetter(name[i]))
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                    goto EnterName;
                }
            }

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if(item.Name == name)
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Group with this name already exist, choose a new one: ");
                    goto EnterName;
                }
            }

            EntitiesGroup group = new EntitiesGroup();
            group.Name = name;



            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's teacher: ");

            EnterTeacher:  string teacher = Console.ReadLine();
            if (string.IsNullOrEmpty(teacher.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.DarkRed, "Teacher's name  can't be empty, enter again! ");
                goto EnterTeacher;
            }
            group.Teacher = teacher;


            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's room name: ");

            RoomName: string room = Console.ReadLine();

            if (string.IsNullOrEmpty(room.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.DarkRed, "Room's name  can't be empty, enter again! ");
                goto RoomName;
            }

            group.Room = room;


            _groupService.Create(group);
            ColorfullPrint.Print(ConsoleColor.Green, "Data successfully created! ");
        }
        public void ShowAllGroups()
        {
           var data =  _groupService.GetAll();

            if (data.Count == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data to show :( ");
                return;
            }

            foreach (var entity in data)
            {

                Console.WriteLine($"Group's id - {entity.Id}, Group's name - {entity.Name}, Group's teacher - {entity.Teacher}, Room's name - {entity.Room}");
            }
        }
        public void DeleteGroup()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's id: ");

            EnterId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again!");
                goto EnterId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's type is invalid, enter again!");
                goto EnterId ;
            }

            bool isExist = false;

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Id == id) isExist = true;
            }

            var datas = _studentService.GetAllStudents().Where(x => x.Group.Id == id).ToList();

            foreach (var item in datas)
            {
                 _studentService.Delete(item.Id);
            }

            if(isExist) _groupService.Delete(id);

            
            else
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;

            }


            ColorfullPrint.Print(ConsoleColor.Green, "Group successfully deleted!");

        }
        public void GetGroupById()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's id: ");

            EnterId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again!");
                goto EnterId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's type is invalid, enter again!");
                goto EnterId;
            }


            bool isExist = false;

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Id == id) isExist = true;
            }

            if (isExist)
            {
                var data = _groupService.GetById(id);

                Console.WriteLine($"Name - {data.Name}, Teacher - {data.Teacher}, Room - {data.Room}");
            }

            else
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;

            }

        }
        public void GetGroupsByTeacher()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter Teahcer's name: ");

            enterTeacher:  string teacher = Console.ReadLine();

            if (string.IsNullOrEmpty(teacher.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Name can't be empty, enter again: ");
                goto enterTeacher;
            }

            var groups =  _groupService.GetGroupsByTeacher(teacher);

            if(groups.Count == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "There's no group with this teacher");
                return ;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"Id - {group.Id}, Name - {group.Name}, Teacher - {group.Teacher}, Room - {group.Room}");

            }
        }

        public void GetGroupsByRoom()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter room's name: ");

            enterRoom: string room = Console.ReadLine();

            if (string.IsNullOrEmpty(room.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Name can't be empty, enter again: ");
                goto enterRoom;
            }

            var groups = _groupService.GetGroupsByRoom(room);

            if (groups.Count == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "There's no group with this room");
                return;
            }

            foreach (var group in groups)
            {
                Console.WriteLine($"Id - {group.Id}, Name - {group.Name}, Teacher - {group.Teacher}, Room - {group.Room}");

            }
        }


        public void GetGroupByName()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter group name:");

            enterName:  string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Group name cant be empty, enter again!");
                goto enterName;
            }

            try
            {
                EntitiesGroup group = new EntitiesGroup();

                group = _groupService.SearchGroupByName(name);

                if (group.Name == string.Empty)
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                    return;
                }

                Console.WriteLine($"Id - {group.Id}, Name - {group.Name}, Teacher - {group.Teacher}, Room - {group.Room}");
            }
            catch (CustomNotFoundException )
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
            }


        }


        public void Update()
        {

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's id: ");

            EnterId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again!");
                goto EnterId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's type is invalid, enter again!");
                goto EnterId;
            }


            bool isExist = false;
            EntitiesGroup group = new();

            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
            {
                if (item.Id == id)
                {
                    isExist = true;
                    group = item;
                    
                }
            }

            if(!isExist)
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
                return;

            }

            else
            {
                try
                {
                    ColorfullPrint.Print(ConsoleColor.Yellow, "Enter the groups' details if you wanna update'em, otherwise they won't be changed: \r\n");

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter the group name : ");
                    EnterName: string name = Console.ReadLine();



                    if (!string.IsNullOrEmpty(name.Trim()))
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (name[i] < 65 || name[i] > 90)
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are upper letters and nnn is 3 digit number");
                                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                                goto EnterName;
                            }
                        }


                        if ((name[2] != '-'))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                            goto EnterName;
                        }

                        for (int i = 3; i <= 5; i++)
                        {


                            if (char.IsLetter(name[i]))
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                                goto EnterName;
                            }
                        }
                        group.Name = name;
                    }

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter the group's room name : ");
                    string room = Console.ReadLine();

                    if (!string.IsNullOrEmpty(room.Trim())) group.Room = room;

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter the group teacher's name : ");
                    enterTeacherName: string teacher = Console.ReadLine();

                    if (!string.IsNullOrEmpty(teacher.Trim()))
                    {
                        foreach (var item in teacher)
                        {
                            if (char.IsDigit(item))
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Teacher's name must only contain letters");
                                goto enterTeacherName;
                            }
                        }
                        group.Teacher= teacher;
                    }

                    _groupService.Update(group);

                    ColorfullPrint.Print(ConsoleColor.Green, "Group successfully updated! ");
                }
                catch (CustomNotFoundException)
                {
                    ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
                }
                


            }
        }
    }
}
