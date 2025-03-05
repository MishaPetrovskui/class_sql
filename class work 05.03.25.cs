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
                bool stoop = false;
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


                    /*DataBase.AddDoctor("Ніколаєнко Василь", 200, 1500);
                    
                    //Doctor last_doctor = doctors.Last();

                    Console.ReadLine();
                    DataBase.EditDoctor(last_doctor.ID, new Doctor(last_doctor.ID, "Григоренко Василь", 250, 1500));
                    doctors = DataBase.GetDoctors();
                    ShowDoctors(doctors);
                    last_doctor = doctors.Last();

                    Console.ReadLine();
                    DataBase.RemoveDoctor(last_doctor.ID);
                    doctors = DataBase.GetDoctors();
                    ShowDoctors(doctors);*/
                    connection.Close();
                } while (!stoop);
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




//drugoe


using Hospital;
using MySql.Data.MySqlClient;
using Mysqlx.Datatypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            string DoctorsQuery =
                "CREATE TABLE IF NOT EXISTS Doctors (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)," +
                "   Premium DOUBLE," +
                "   Salary DOUBLE" +
                ");";
            using (MySqlCommand command = new MySqlCommand(DoctorsQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string SpecializationsQuery =
                "CREATE TABLE IF NOT EXISTS Specializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   Name VARCHAR(100)" +
                ");";
            using (MySqlCommand command = new MySqlCommand(SpecializationsQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            string DoctorsSpecializationsQuery =
                "CREATE TABLE IF NOT EXISTS DoctorsSpecializations (" +
                "   ID INT AUTO_INCREMENT PRIMARY KEY," +
                "   DoctorID INT," +
                "   SpecializationID INT" +
                ");";
            using (MySqlCommand command = new MySqlCommand(DoctorsSpecializationsQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void AddDoctor(string name, double premium, double salary)
        {
            string query =
                "INSERT INTO Doctors (Name, Premium, Salary) VALUES (@name, @premium, @salary)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@premium", premium);
                command.Parameters.AddWithValue("@salary", salary);
                command.ExecuteNonQuery();
            }
        }

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

        public void AddDoctorsSpecialization(int doctorId, int specializationId)
        {
            string query =
                "INSERT INTO DoctorsSpecializations (DoctorID, SpecializationID) VALUES (@doctorId, @specializationId)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@doctorId", doctorId);
                command.Parameters.AddWithValue("@specializationId", specializationId);
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
                while (reader.Read())
                {
                    doctors.Add(new Doctor(
                            (int)reader["ID"],
                            (string)reader["Name"],
                            Convert.ToDecimal((double)reader["Premium"]),
                            Convert.ToDecimal((double)reader["Salary"])
                        ));
                }
                return doctors;
            }
        }

        public List<Specializations> GetSpecialization()
        {
            string query = "SELECT * FROM Specializations";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<Specializations> doctors = new List<Specializations>();
                while (reader.Read())
                {
                    doctors.Add(new Specializations(
                            (int)reader["ID"],
                            (string)reader["Name"]
                            ));
                }
                return doctors;
            }
        }

        public List<DoctorsSpecializations> GetDoctorsSpecialization()
        {
            string query = "SELECT * FROM DoctorsSpecializations";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                List<DoctorsSpecializations> doctors = new List<DoctorsSpecializations>();
                while (reader.Read())
                {
                    doctors.Add(new DoctorsSpecializations(
                            (int)reader["ID"],
                            (int)reader["DoctorID"],
                            (int)reader["SpecializationID"]
                            ));
                }
                return doctors;
            }
        }

        public void RemoveDoctor(int id)
        {
            string query = "DELETE FROM Doctors WHERE ID=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void EditDoctor(int id, Doctor update)
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
                command.ExecuteNonQuery();
            }
        }

        public void TestZapolnenie(int how_many)
        {
            List<string> names = new List<string> { 
                "User_1", "User_2", "User_3", "User_4", "User_5", "NoName", "Nothing", "None",
                "Abdul", "Vadim", "Domenic De-Cocu", "Jotaro Tokiyskiy", "Joskiy", "Krutoi",
                "Altair ibn la Ahad", "Vasya", "Viktor", "Gesiy", "Djamil", "Denis", "Swatic", "Volodya"
            };
            Random rnd = new();
            for (int i = 0; i < how_many; i++)
                AddDoctor(names[rnd.Next(0, 22+1)], rnd.Next(0, 2000), rnd.Next(0, 10000));
        }
    }

    class Doctor
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Premium { get; set; }
        public decimal Salary { get; set; }

        public Doctor(int id, string name,
            decimal premium, decimal salary)
        {
            ID = id;
            Name = name;
            Premium = premium;
            Salary = salary;
        }
        public Doctor(string name,
            decimal premium, decimal salary)
            : this(0, name, premium, salary) { }
        public Doctor() : this(0, "", 0, 0) { }
    }

    class Specializations
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Specializations(int id, string name)
        {
            ID = id;
            Name = name;
        }
        public Specializations(string name,
            decimal premium, decimal salary)
            : this(0, name) { }
        public Specializations() : this(0, "") { }
    }

    class DoctorsSpecializations
    {
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public int SpecializationID { get; set; }

        public DoctorsSpecializations(int id, int doctorID, int specializationID)
        {
            this.ID = id;
            this.DoctorID = doctorID;
            this.SpecializationID = specializationID;
        }
        public DoctorsSpecializations(int doctorID, int specializationID)
            : this() { }
        public DoctorsSpecializations() : this(0, 0, 0) { }
    }
}
