using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using Hospital;
using System.Numerics;

class Program
{
    static void ShowDoctors(List<Doctor> doctors, List<Specialization> specializations, MySqlConnection connection)
    {
        Console.WriteLine($"{"ID",3} | {"Прізвище та ім`я",20} | " +
            $"{"Премія",10} | {"Зарплатня",11} | {"Спеціалізація", 12}");
        foreach (Doctor doctor in doctors)
        {
            DBManager DataBase = new DBManager(connection);
            Console.WriteLine($"{doctor.ID,3} | {doctor.Name,20} | " +
                        $"{doctor.Premium,5} грн. | {doctor.Salary,6} грн. | {DataBase.SpecializationInDoctor(doctors, specializations)} {doctor.SpecializationID}");
        }
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
                Console.WriteLine($"Підключення до бази даних {db_database}...");
                connection.Open();
                Console.WriteLine($"Успішно підключено до бази даних {db_database}");
                DBManager DataBase = new DBManager(connection);
                Console.WriteLine($"Оновлення таблиць...");
                DataBase.CreateTables();
                Console.WriteLine($"Таблиці оновлено!");
                Console.Write("Додати тестові записи у базу даних?\n (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    DataBase.AddTestData();
                }

                Console.Clear();
                List<Doctor> doctors = DataBase.GetDoctors();
                List<Specialization> specializations = DataBase.GetSpecializations();
                ShowDoctors(doctors, specializations, connection);

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
                    Console.WriteLine();
                }
            }

        }
    }
}








using Hospital;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    class DBManager
    {
        MySqlConnection connection;

        public DBManager(MySqlConnection connection)
        {
            this.connection = connection;
        }

        public void CreateTables()
        {
            string query =
                "CREATE TABLE IF NOT EXISTS Doctors (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)," +
                "   Premium DOUBLE," +
                "   Salary DOUBLE," +
                "   SpecializationID INT" +
                ");" +

                "CREATE TABLE IF NOT EXISTS Specializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)" +
                ");" +

                "CREATE TABLE IF NOT EXISTS DoctorsSpecializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   DoctorID INT," +
                "   SpecializationID INT," +
                "   FOREIGN KEY (DoctorID) REFERENCES Doctors(ID)," +
                "   FOREIGN KEY (SpecializationID) REFERENCES Specializations(ID)" +
                ");";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void AddTestData()
        {
            AddSpecialization("Хірург");
            AddSpecialization("Кардіолог");
            AddSpecialization("Педіатр");

            Random rnd = new Random();

            AddDoctor("Григоренко Василь", 230, 1540, rnd.Next(1,3));
            AddDoctor("Петров Дмитро", 160, 1300, rnd.Next(1, 3));
            AddDoctor("Ніколаєнко Захар", 200, 1250, rnd.Next(1, 3));
            AddDoctor("Дейнега Кирило", 90, 1800, rnd.Next(1, 3));


            List<Specialization> specializations = GetSpecializations();
            List<Doctor> doctors = GetDoctors();

            foreach (Doctor doctor in doctors)
            {
                for (int i = 0; i < rnd.Next(1, 3); i++)
                {
                    int specId = specializations[rnd.Next(0, specializations.Count)].ID;
                    if (!IsDoctorHaveSpecialization(doctor.ID, specId))
                        AddDoctorSpecialization(doctor.ID, specId);
                }
            }
        }

        // == SPECIALIZATIONS

        public void AddSpecialization(string name)
        {
            string query =
                "INSERT INTO Specializations (Name) VALUES (@name)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
        }

        public int RemoveSpecialization(int id)
        {
            string query = "DELETE FROM Specializations WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
        }

        public List<Specialization> GetSpecializations()
        {
            string query = "SELECT * FROM Specializations";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Specialization> specializations = new List<Specialization>();
                while (reader.Read())
                {
                    specializations.Add(
                        new Specialization(
                            (int)reader["ID"],
                            (string)reader["Name"]
                        ));
                }
                return specializations;
            }
        }

        // == DOCTORS SPECIALIZATIONS

        public void AddDoctorSpecialization(int doctorID, int specializationID)
        {
            string query =
                "INSERT INTO DoctorsSpecializations (DoctorID, SpecializationID)" +
                " VALUES (@doctorID, @specializationID)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                command.ExecuteNonQuery();
            }
        }

        public int RemoveDoctorSpecialization(int doctorID, int specializationID)
        {
            string query = "DELETE FROM DoctorsSpecializations" +
                " WHERE DoctorID=@doctorID AND SpecializationID=@specializationID";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                return command.ExecuteNonQuery();
            }
        }

        public bool IsDoctorHaveSpecialization(int doctorID, int specializationID)
        {
            string query = "SELECT ID FROM DoctorsSpecializations" +
                " WHERE DoctorID=@doctorID AND SpecializationID=@specializationID";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorID", doctorID);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        // == DOCTORS

        public void AddDoctor(string name, double premium, double salary, int specializationID)
        {
            string query =
                "INSERT INTO Doctors (Name, Premium, Salary, SpecializationID) VALUES (@name, @premium, @salary, @specializationID)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@premium", premium);
                command.Parameters.AddWithValue("@salary", salary);
                command.Parameters.AddWithValue("@specializationID", specializationID);
                command.ExecuteNonQuery();
            }
        }

        public List<Doctor> GetDoctors()
        {
            string query = "SELECT * FROM Doctors";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Doctor> doctors = new List<Doctor>();
                List<Specialization> specializations = new List<Specialization>();
                while (reader.Read())
                {
                    doctors.Add(new Doctor(
                            (int)reader["ID"],
                            (string)reader["Name"],
                            Convert.ToDecimal((double)reader["Premium"]),
                            Convert.ToDecimal((double)reader["Salary"]),
                            (int)(reader["SpecializationID"]),
                            (string)(SpecializationInDoctor(doctors, specializations))
                        ));
                }
                return doctors;
            }
        }

        public string SpecializationInDoctor(List<Doctor> doctors, List<Specialization> Specialization)
        {
            string Name = "";

            foreach (var doc in doctors)
            {
                for (int i = 0; i < Specialization.Count(); i++)
                    if (Specialization[i].ID == doc.SpecializationID)
                    {
                        Name = Specialization[i].Name;
                        doc.NameOfSpecialization = Specialization[i].Name;
                    }


                //return doc.NameOfSpecialization;
            }
            return Name;
        }

        public int RemoveDoctor(int id)
        {
            string query = "DELETE FROM Doctors WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery();
            }
        }

        public int EditDoctor(int id, Doctor update)
        {
            string query = "UPDATE Doctors " +
                "SET ID=@id, Name=@name, Premium=@premium, Salary=@salary " +
                "WHERE ID=@source_id;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@source_id", id);
                command.Parameters.AddWithValue("@id", update.ID);
                command.Parameters.AddWithValue("@name", update.Name);
                command.Parameters.AddWithValue("@premium", update.Premium);
                command.Parameters.AddWithValue("@salary", update.Salary);
                return command.ExecuteNonQuery();
            }
        }
    }

    class Doctor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Premium { get; set; }
        public decimal Salary { get; set; }
        public int SpecializationID { get; set; }
        public string NameOfSpecialization { get; set; }

        public Doctor(int id, string name,
            decimal premium, decimal salary, int specializationID, string nameOfSpecialization)
        {
            ID = id;
            Name = name;
            Premium = premium;
            Salary = salary;
            SpecializationID = specializationID;
            NameOfSpecialization = nameOfSpecialization;
        }
        public Doctor(string name,
            decimal premium, decimal salary, int specializationID, string nameOfSpecialization)
            : this(0, name, premium, salary, specializationID, nameOfSpecialization) { }
        public Doctor() : this(0, "", 0, 0, 0, "") { }
    }

    class Specialization
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Specialization(int id, string name)
        {
            ID = id;
            Name = name;
        }
        public Specialization(string name)
            : this(0, name) { }
        public Specialization() : this(0, "") { }
    }
}

/*
    1. Створити клас Doctor, в якому зберігається
        інформація про лікаря
    2. Змінити метод GetDoctors таким чином, 
        щоб він не виводив таблицю з лікарями, а
        повертав список об`єктів класу Doctor.
        Список лікарів — List<Doctor>
    3. Створити функцію ShowDoctors, яка приймає
        список лікарів та виводить його на екран
        у вигляді таблиці. Реалізувати також
        шапку таблиці.

*/
