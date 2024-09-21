

using CourseApp.Controllers;
using CourseApp.Helpers;
using CourseApp.Helpers.Extensions;
using Domain.Entities;
using Repository.Data;
using Repository.Helpers.ExceptionMessages;
using Repository.Helpers.Exceptions;
using Service.Service;
using Service.Service.Interfaces;
using System.Threading.Channels;
using System.Xml.Linq;

namespace CourseApp.StudentConstructor
{
    public class StudentControllers
    {
        private readonly IStudentService _studentService;

        public StudentControllers()
        {
            _studentService = new StudentService();
        }


        public void GetAll()
        {
            var data = _studentService.GetAllStudents();

            if(data.Count != 0)
            {
                foreach (var item in data)
                {
                    Console.WriteLine($"Studetn's id- {item.Id}, Student's name- {item.Name}, Student's surname- {item.Surname}, Studetn's mail- {item.Email}, Student's age- {item.Age}, student's group name- {item.Group.Name}");
                }

            }
            else
            {
                ColorfullPrint.Print(ConsoleColor.Red, "There's no student to show");
            }
        }

        public void CreateStudent()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter Student's details: ");

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's name: ");

            addName: string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Name can't be empty, add again: ");
                goto addName;
            }

            foreach (char c in name)
            {
                if(!(c>= 65 && c <= 122))
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Name only must contain letters, enter again!");
                    goto addName;
                }
            }


            
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's surname: ");

            addSurname: string surname = Console.ReadLine();


            if (string.IsNullOrEmpty(surname.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Surname can't be empty, add again: ");
                goto addSurname;
            }

            foreach (char c in surname)
            {
                if (!(c >= 65 && c <= 122))
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Surname only must contain letters, enter again!");
                    goto addSurname;
                }
            }

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's age: ");

            addAge:  string strAge = Console.ReadLine();


            if (string.IsNullOrWhiteSpace(strAge))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Age can't be empty, enter again !");
                goto addAge;

            }

            if (!(int.TryParse(strAge, out int age)))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Wrong age format or unacceptable age, enter again!");

                goto addAge;
            }

            if(!(age >= 15 && age <= 65))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Unacceptable age, age must be between 15 and 65 to sign up for the academy!\r\n");

                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter age again: ");
                goto addAge;
            }

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's email: ");

            ColorfullPrint.Print(ConsoleColor.Yellow, "Email format example : example@address.com");

            addMail: string email = Console.ReadLine();

            if (string.IsNullOrEmpty(email.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Surname can't be empty, add again: ");
                goto addMail;
            }

            if (!(MailService.IsValidEmail(email)))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Wrong mail format, enter again: ");
                ColorfullPrint.Print(ConsoleColor.Yellow, "Email format example : example@address.com");
                goto addMail;
            }


            ColorfullPrint.Print(ConsoleColor.Yellow, "To complete creating a new student, choose the group where the student will be added, if there is no free group for the student, u can create a new one.\r\n");
            ColorfullPrint.Print(ConsoleColor.Cyan, @"
            1.Add a student to an existing group
            2.Create a new group for  students
                ");

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

            IGroupService _groupService = new GroupService();

            switch (option)
            {
                case 1:

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group name: ");

                    enterGroupName: string groupName = Console.ReadLine();

                    if (string.IsNullOrEmpty(name.Trim()))
                    {
                        ColorfullPrint.Print(ConsoleColor.Red, "Group name cant be empty, enter again!");
                        goto enterGroupName;
                    }

                    

                    try
                    {
                        

                        var group = _groupService.SearchGroupByName(groupName);

                        if (group.Name == string.Empty)
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group name: ");

                            goto enterGroupName;
                        }

                        _studentService.Create(new Student { Name = name, Surname = surname, Email = email, Age = age, Group = group });

                        ColorfullPrint.Print(ConsoleColor.Green, "Student successfully created");
                        break;
                    }

               
                    catch (CustomNotFoundException)
                    {
                        ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);

                        ColorfullPrint.Print(ConsoleColor.Yellow, @"If there's no such a group yet, u can create one, or finish adding student process:
                                                1.Create a group to add students
                                                2.Finish the process");

                        EnterNewLine: string newLine = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Choosing an option is required, it can't be empty, please select an option again");
                            goto EnterNewLine;
                        }

                        if (!int.TryParse(newLine, out int newOption))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Invalid option type, select again: ");
                            goto EnterNewLine;
                        }


                        if (newOption == 1) goto createAStudent;
                        if(newOption == 2) return;
                        else
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "There's no such an option, enter again!");
                                   goto EnterNewLine;
                        }

                       

                    }
                case 2:

                createAStudent: ColorfullPrint.Print(ConsoleColor.Yellow, "Enter group's data: ");

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name: ");
                    EnterName: string newGroupName = Console.ReadLine();


                    if (string.IsNullOrEmpty(newGroupName.Trim()))
                    {
                        ColorfullPrint.Print(ConsoleColor.DarkRed, "Name can't be empty, enter again! ");
                        goto EnterName;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        if (newGroupName[i] < 65 || newGroupName[i] > 90)
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are upper letters and nnn is 3 digit number");
                            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                            goto EnterName;
                        }
                    }


                    if (newGroupName[2] != '-')
                    {
                        ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                        ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                        goto EnterName;
                    }

                    for (int i = 3; i <= 5; i++)
                    {


                        if (char.IsLetter(newGroupName[i]))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                            goto EnterName;
                        }
                    }

                    EntitiesGroup newGroup = new EntitiesGroup();
                    newGroup.Name = newGroupName;



                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's teacher: ");

                    EnterTeacher: string teacher = Console.ReadLine();
                    if (string.IsNullOrEmpty(teacher.Trim()))
                    {
                        ColorfullPrint.Print(ConsoleColor.DarkRed, "Teacher's name  can't be empty, enter again! ");
                        goto EnterTeacher;
                    }
                    newGroup.Teacher = teacher;


                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's room name: ");

                    RoomName: string room = Console.ReadLine();

                    if (string.IsNullOrEmpty(room.Trim()))
                    {
                        ColorfullPrint.Print(ConsoleColor.DarkRed, "Room's name  can't be empty, enter again! ");
                        goto RoomName;
                    }

                    newGroup.Room = room;


                    _groupService.Create(newGroup);
                    ColorfullPrint.Print(ConsoleColor.Green, "New group successfully created! \r\n ");

                    _studentService.Create(new Student { Name = name, Surname = surname, Email = email, Age = age, Group = newGroup });
                    ColorfullPrint.Print(ConsoleColor.Green, "Student successfully created");
                    break;


                default:
                    ColorfullPrint.Print(ConsoleColor.Red, "There's no such an option, enter again!");
                    goto EnterLine;
            }
        }

        public void GetAllStudentsByGroupId()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter id: ");

            EnterID:  string strId = Console.ReadLine();

            if (string.IsNullOrEmpty(strId))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again! ");
                goto EnterID;
            }

            if(!int.TryParse(strId, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's formad is invalid, enter again! ");
                goto EnterID;
            }

            var datas = _studentService.GetAllStudentsByGroupId(id);

            if(datas.Count == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;
            }

            foreach (var student in _studentService.GetAllStudentsByGroupId(id))
            {
                Console.WriteLine($"Student's id - {student.Id}, Student's name -  {student.Name}, Student's surname -  {student.Surname}, student's mail address - {student.Email}");
            }



        }


        public void DeleteStudent()
        {
            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter id: ");

            EnterID: string strId = Console.ReadLine();

            if (string.IsNullOrEmpty(strId))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again! ");
                goto EnterID;
            }

            if (!int.TryParse(strId, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's formad is invalid, enter again! ");
                goto EnterID;
            }


            try
            {
                _studentService.Delete(id);
                ColorfullPrint.Print(ConsoleColor.Green, "Student deleted!");
            }
            catch (CustomNotFoundException)
            {
                ColorfullPrint.Print(ConsoleColor.Red,CustomNotFoundMessage.message);
            }
            


        }



        public void GetStudetnById()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter id: ");

            EnterID: string strId = Console.ReadLine();

            if (string.IsNullOrEmpty(strId))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id can't be empty, enter again! ");
                goto EnterID;
            }

            if (!int.TryParse(strId, out int id))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Id's formad is invalid, enter again! ");
                goto EnterID;
            }


            foreach (var student in AppDbContext<Student>.Entity)
            {
                if(student.Id == id)
                {
                    Console.WriteLine($"Student's id - {student.Id}, Student's name -  {student.Name}, Student's surname -  {student.Surname}, student's mail address - {student.Email}");
                    return;
                }
                
            }
            ColorfullPrint.Print(ConsoleColor.Red, "No student found");

        }

        public void GetStudentsByAge()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter age: ");

            EnterAge: string strAge = Console.ReadLine();

            if (string.IsNullOrEmpty(strAge))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Age can't be empty, enter again! ");
                goto EnterAge;
            }

            if (!int.TryParse(strAge, out int age))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Age's formad is invalid, enter again! ");
                goto EnterAge;
            }

            var datas = _studentService.GetStudentsByAge(age);

            if(datas.Count() == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data found");
                return;
            }


            foreach (var student in datas )
            {
                Console.WriteLine($"Student's id - {student.Id}, Student's name -  {student.Name}, Student's surname -  {student.Surname}, student's mail address - {student.Email}");

            }
        }


        public void SearchStudentByNameOrSurname()
        {
            ColorfullPrint.Print(ConsoleColor.Yellow, "Enter student's name or surname: ");

            EnterName: string line = Console.ReadLine();

            if (string.IsNullOrEmpty(line.Trim()))
            {
                ColorfullPrint.Print(ConsoleColor.Red, "Name or surname can't be empty, enter again: ");
                goto EnterName;
            }

            foreach (char c in line)
            {
                if (!(c >= 65 && c <= 122))
                {
                    ColorfullPrint.Print(ConsoleColor.Red, "Name or surname only must contain letters, enter again!");
                    goto EnterName;
                }
            }


            var data = _studentService.SearchByNameOrSurname(line);

            if(data.Count() == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data has been found");
                return ;
            }

            foreach (var student in data)
            {
                Console.WriteLine($"Student's id - {student.Id}, Student's name -  {student.Name}, Student's surname -  {student.Surname}, student's mail address - {student.Email}");

            }

        }




        public void StudentUpdate()
        {

            if(_studentService.GetAllStudents().Count == 0)
            {
                ColorfullPrint.Print(ConsoleColor.Red, "No data to update");
                return ;
            }

            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's id: ");

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
            Student student = new();

            foreach (var item in AppDbContext<Student>.Entity)
            {
                if (item.Id == id)
                {
                    isExist = true;
                    student = item;
                    break;
                }
            }

            if (!isExist)
            {
                ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
                return;
            }

            else
            {
                try
                {
                    ColorfullPrint.Print(ConsoleColor.Yellow, "Enter the students's details if you wanna update'em, otherwise they won't be changed: \r\n");


                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter student's name : ");
                    enterName: string name = Console.ReadLine();

                    if (!(string.IsNullOrEmpty(name.Trim())))
                    {
                        foreach (var item in name)
                        {
                            if (char.IsDigit(item))
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Student's name must only contain letters");
                                goto enterName;
                            }
                            
                        }
                        student.Name = name;
                    }

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter  student's  surname : ");
                    EnterSurName: string surname = Console.ReadLine();

                    if (!(string.IsNullOrEmpty(surname.Trim())))
                    {
                        foreach (var item in surname)
                        {
                            if (char.IsDigit(item))
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Student's surname must only contain letters");
                                goto EnterSurName;
                            }

                        }
                        student.Surname = surname;
                    }

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter  student's mail address : ");
                    EnterMail: string mail = Console.ReadLine();


                    if (!(string.IsNullOrEmpty(mail.Trim())))
                    {
                        if (MailService.IsValidEmail(mail))
                        {
                            student.Email = mail;
                        }
                        else
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Wrong mail format, mail example: example@test.com , enter again: ");
                            goto EnterMail;
                        }
                    }

                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter  student's age : ");

                    EnterAge: string strAge = Console.ReadLine();

                    if (!(string.IsNullOrEmpty(strAge)))
                    {
                        if (!(int.TryParse(strAge, out int age)))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Wrong age format or unacceptable age, enter again!");

                            goto EnterAge;
                        }

                        if (!(age >= 15 && age <= 65))
                        {
                            ColorfullPrint.Print(ConsoleColor.Red, "Unacceptable age, age must be between 15 and 65 to sign up for the academy!\r\n");

                            ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter age again: ");
                            goto EnterAge;
                        }

                        student.Age = age;
                    }


                    ColorfullPrint.Print(ConsoleColor.Yellow, "Do u want to change student's groub data also? (y/n)");

                    enterChange: string change = Console.ReadLine();

                    if(change == "y")
                    {
                        goto enterGroupData;
                    }
                    if(change == "n")
                    {
                        ColorfullPrint.Print(ConsoleColor.Green, "Student updating is finished");
                        return;
                    }
                    else
                    {
                        ColorfullPrint.Print(ConsoleColor.Red, "Enter correct option (y/n)");
                        goto enterChange;
                    }


                #region Update Student's group



                enterGroupData: ColorfullPrint.Print(ConsoleColor.Yellow, "Enter group name  to update student's group or leave it empty if u dont wanna change it");
                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter a new group name  to update, new group name should already exist : ");


                    EntitiesGroup newStudentGroup = new();
                    newStudentGroup = student.Group;

                    EnterGroupName: string strGroupName = Console.ReadLine();
                    if (!(string.IsNullOrEmpty(strGroupName)))
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (strGroupName[i] <= 65 || strGroupName[i] >= 90)
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are upper letters and nnn is 3 digit number");
                                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                                goto EnterGroupName;
                            }
                        }


                        if (strGroupName[2] != '-')
                        {
                                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                           ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                          goto EnterGroupName;
                            }

                        for (int i = 3; i <= 5; i++)
                        {


                            if (char.IsLetter(strGroupName[i]))
                            {
                                ColorfullPrint.Print(ConsoleColor.Red, "Wrong name format, group name must be like XX-nnn, where XX are letters and nnn is 3 digit number");
                                ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter group's name again: ");
                                goto EnterGroupName;
                            }
                        }

                            //Checks if group with name that user entered exist in DB


                            bool ifIsExist = false;

                            foreach (var item in AppDbContext<EntitiesGroup>.Entity)
                            {
                                if (item.Name == strGroupName && student.Group != item)
                                {
                                    ifIsExist = true;
                                student.Group = item;
                                _studentService.Update(student);
                                ColorfullPrint.Print(ConsoleColor.Green, "Student updated succesfully");

                                    return;
                                }
                                else if (item.Name == strGroupName && student.Group == item)
                                 {
                                     {
                                        ifIsExist = true;
                                        ColorfullPrint.Print(ConsoleColor.Red, "The group name that u entered is the same as the old one, enter again  or stop updating group.");
                                        ColorfullPrint.Print(ConsoleColor.Yellow, @"
                                     To enter name again press 1
                                     To finish updating press 2 ");

                                    ColorfullPrint.Print(ConsoleColor.DarkMagenta, "Enter again: ");
                                    EnterGroupOption: string strNewOptionForGroupupdate = Console.ReadLine();

                                   if (string.IsNullOrEmpty(strNewOptionForGroupupdate.Trim()))
                                    {
                                        ColorfullPrint.Print(ConsoleColor.Red, "Option cant be empty, enter again: ");
                                        goto EnterGroupOption;
                                    }
                                    if (!(int.TryParse(strNewOptionForGroupupdate, out int newOptionForGroupupdate)))
                                    {
                                        ColorfullPrint.Print(ConsoleColor.Red, "Wrong option format, enter again: ");
                                        goto EnterGroupOption;
                                    }

                                    switch (newOptionForGroupupdate)
                                    {
                                        case 1:
                                            goto EnterGroupName;
                                        case 2:
                                            return;

                                        default:
                                            ColorfullPrint.Print(ConsoleColor.Red, "There's no such an option yet, enter again: ");
                                            goto EnterGroupOption;
                                    }

                                    }
                                }
                                if (ifIsExist)
                                {
                                    ColorfullPrint.Print(ConsoleColor.Red, "There's no group with this name, enter again: ");
                                    goto EnterGroupName;
                                }
                            
                            


                            }

                    }


                    
                }
                catch (CustomNotFoundException)
                {

                    ColorfullPrint.Print(ConsoleColor.Red, CustomNotFoundMessage.message);
                }

                #endregion

            }

        }


    }
}
