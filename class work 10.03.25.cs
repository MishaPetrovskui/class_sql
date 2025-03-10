using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using Hospital;
using System.Numerics;

class Program
{
    static void ShowDoctors(IEnumerable<Doctor> doctors)
    {
        Console.WriteLine($"{"ID",3} | {"Прізвище та ім`я",20} | " +
            $"{"Премія",10} | {"Зарплатня",11}");
        foreach (Doctor doctor in doctors)
            Console.WriteLine($"{doctor.ID,3} | {doctor.Name,20} | " +
                        $"{doctor.Premium,5} грн. | {doctor.Salary,6} грн.");
    }

    public static void Main(string[] args)
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;
        Console.InputEncoding = UTF8Encoding.UTF8;
        string db_host = "localhost";
        string db_database = "hospital";
        string db_user = "root";
        string db_password = "";

        string connectionString = $"Server={db_host};Database={db_database};" +
            $"User ID={db_user};Password={db_password};";

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                DBManager DataBase = new DBManager(connection);
                DataBase.CreateTables();
                DataBase.AddSpecialization("Хірург");
                DataBase.AddSpecialization("Кардіолог");
                DataBase.AddSpecialization("Педіатр");
                /*bool stoop = false;
                do
                {
                    connection.Open();

                    DBManager DataBase = new DBManager(connection);

                    DataBase.CreateTables();
                    List<Doctor> doctors = DataBase.GetDoctors();
                    Doctor last_doctor = doctors.Last();
                    ShowDoctors(doctors);
                    Console.Write("1)Add doctor 2)Edit doctor 3)Delete doctor 4)Autocomplete 0)Exit: ");
                    int action = int.Parse(Console.ReadLine());
                    if (action == 0)
                        stoop = true;
                    else if (action == 1)
                    {
                        Console.WriteLine();
                        Console.Write("Enter doctors Name: ");
                        string NewName = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Enter doctors Premium: ");
                        double NewPremium = double.Parse(Console.ReadLine());
                        Console.WriteLine();
                        Console.Write("Enter doctors Salary: ");
                        double NewSalary = double.Parse(Console.ReadLine());
                        DataBase.AddDoctor(NewName, NewPremium, NewSalary);
                        doctors = DataBase.GetDoctors();
                        last_doctor = doctors.Last();
                    }

                    else if (action == 2)
                    {
                        int a;
                        do
                        {
                            Console.WriteLine();
                            Console.Write("Enter doctors ID: ");
                            a = int.Parse(Console.ReadLine());
                        }while(a >= last_doctor.ID + 1);
                        Console.WriteLine();
                        Console.Write("Enter doctors Name: ");
                        string NewName = Console.ReadLine();
                        Console.WriteLine();
                        Console.Write("Enter doctors Premium: ");
                        double NewPremium = double.Parse(Console.ReadLine());
                        Console.WriteLine();
                        Console.Write("Enter doctors Salary: ");
                        double NewSalary = double.Parse(Console.ReadLine());
                        last_doctor.ID = a;
                        DataBase.EditDoctor(last_doctor.ID, new Doctor(last_doctor.ID, NewName, (decimal)NewPremium, (decimal)NewSalary));
                        doctors = DataBase.GetDoctors();
                    }

                    else if (action == 3)
                    {
                        int a;
                        do
                        {
                            Console.WriteLine();
                            Console.Write("Enter doctors ID: ");
                            a = int.Parse(Console.ReadLine());
                        } while (a >= last_doctor.ID + 1);
                        DataBase.RemoveDoctor(a);
                        doctors = DataBase.GetDoctors();
                    }

                    else if (action == 4)
                    {
                        Console.WriteLine();
                        Console.Write("Enter quantity: ");
                        int a = int.Parse(Console.ReadLine());
                        DataBase.TestZapolnenie(a);
                        doctors = DataBase.GetDoctors();
                    }

                    else
                    {
                        Console.WriteLine("NET");
                    }


                    *//*DataBase.AddDoctor("Ніколаєнко Василь", 200, 1500);
                    
                    //Doctor last_doctor = doctors.Last();

                    Console.ReadLine();
                    DataBase.EditDoctor(last_doctor.ID, new Doctor(last_doctor.ID, "Григоренко Василь", 250, 1500));
                    doctors = DataBase.GetDoctors();
                    ShowDoctors(doctors);
                    last_doctor = doctors.Last();

                    Console.ReadLine();
                    DataBase.RemoveDoctor(last_doctor.ID);
                    doctors = DataBase.GetDoctors();
                    ShowDoctors(doctors);*//*
                    connection.Close();
                } while (!stoop);*/
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.Message == "Sequence contains no elements")
                {
                    DBManager DataBase = new DBManager(connection);
                    DataBase.TestZapolnenie(1);
                }
            }

        }
    }
}
