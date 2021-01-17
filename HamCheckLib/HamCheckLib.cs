using System;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Data.SQLite;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace HamCheckLib
{
    public static class HamCheckLib
    {
        public static string zipFileUrl = "ftp://wirelessftp.fcc.gov/pub/uls/complete/l_amat.zip";
        public static string tempDirPath = Path.Combine(Path.GetTempPath(), "hamcheck");
        public static string zipFilePath = Path.Combine(tempDirPath, "l_amat.zip");
        public static string sqlFilePath = Path.Combine(tempDirPath, "hamcheck.sqlite");
        public static string csvFilePath = Path.Combine(tempDirPath, "EN.dat");
        public static SQLiteConnection dbConn = null;
        public static bool isInitialized = false;
        public static DataSet ds = null;
        public static SQLiteDataAdapter sqlAdapter = null;

        // from: https://www.fcc.gov/sites/default/files/public_access_database_definitions_sql_v3_2.txt
        public static string sqlCreateTable = @"
            create table if not exists PUBACC_EN
            (
                  record_type               char(2)              not null,
                  unique_system_identifier  numeric(9,0)         not null,
                  uls_file_number           char(14)             null,
                  ebf_number                varchar(30)          null,
                  call_sign                 char(10)             null,
                  entity_type               char(2)              null,
                  licensee_id               char(9)              null,
                  entity_name               varchar(200)         null,
                  first_name                varchar(20)          null collate nocase,
                  mi                        char(1)              null,
                  last_name                 varchar(20)          null collate nocase,
                  suffix                    char(3)              null,
                  phone                     char(10)             null,
                  fax                       char(10)             null,
                  email                     varchar(50)          null,
                  street_address            varchar(60)          null,
                  city                      varchar(20)          null collate nocase,
                  state                     char(2)              null collate nocase,
                  zip_code                  char(9)              null,
                  po_box                    varchar(20)          null,
                  attention_line            varchar(35)          null,
                  sgin                      char(3)              null,
                  frn                       char(10)             null,
                  applicant_type_code       char(1)              null,
                  applicant_type_other      char(40)             null,
                  status_code               char(1)		         null,
                  status_date               varchar(30)          null
            )";

        public static void Init()
        {
            if (!isInitialized)
            {
                PrepTempDir();
                DownloadUlsData();
                ExtractUlsData();
                CreateDatabase();
                OpenDatabase();
                CreateTable();
                BulkInsertCsv();
                isInitialized = true;
            }
        }

        public static void Close()
        {
            dbConn.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void PrepTempTablePeople(List<Person> people)
        {
            // drop temp table if it exists
            string sql = "DROP TABLE IF EXISTS TMP_PEOPLE";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteScalar();
            }

            // create temp table 
            sql = @"create table if not exists TMP_PEOPLE
                (first_name varchar(20)          null collate nocase,
                  last_name varchar(20)          null collate nocase)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteScalar();
            }

            using (var transaction = dbConn.BeginTransaction())
            {
                // insert all people into temp table 
                var command = dbConn.CreateCommand();
                command.CommandText = @" INSERT INTO TMP_PEOPLE VALUES ($first_name, $last_name)";
                
                var param_first_name = command.CreateParameter();
                param_first_name.ParameterName = "$first_name";
                command.Parameters.Add(param_first_name);

                var param_last_name = command.CreateParameter();
                param_last_name.ParameterName = "$last_name";
                command.Parameters.Add(param_last_name);

                foreach (Person p in people)
                {
                    if (string.IsNullOrWhiteSpace(p.firstName)) p.firstName = "";
                    param_first_name.Value = p.firstName;
                    param_last_name.Value = p.lastName;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public static void PrepTempTableCities(List<City> cities)
        {
            // drop temp table if it exists
            string sql = "DROP TABLE IF EXISTS TMP_CITIES";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteScalar();
            }

            // create temp table 
            sql = @"create table if not exists TMP_CITIES
                (city varchar(20)          null collate nocase,
                  state                     char(2)              null collate nocase)";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                cmd.ExecuteScalar();
            }

            using (var transaction = dbConn.BeginTransaction())
            {
                // insert all cities into temp table 
                var command = dbConn.CreateCommand();
                command.CommandText = @" INSERT INTO TMP_CITIES VALUES ($city, $state)";

                var param_city = command.CreateParameter();
                param_city.ParameterName = "$city";
                command.Parameters.Add(param_city);

                var param_state = command.CreateParameter();
                param_state.ParameterName = "$state";
                command.Parameters.Add(param_state);

                foreach (City c in cities)
                {
                    param_city.Value = c.cityName;
                    param_state.Value = c.stateName;
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public static void GetResults(string people, string cities)
        {
            // Including more than 1000 names + cities in a single query exceeds SQLite expression tree limits
            // Prepare temporary tables instead, allowing the actual query used for comparison to be smaller...
            List<Person> personList = GetNames(people);
            PrepTempTablePeople(personList);
            if (personList.Count < 1) return;

            List<City> cityList = GetCities(cities);
            PrepTempTableCities(cityList);

            string sqlStatement;
            sqlStatement = "select a.call_sign,a.last_name,a.first_name,a.street_address,a.city,a.state from PUBACC_EN as A, TMP_PEOPLE as P WHERE A.last_name = P.last_name AND A.first_name LIKE (P.first_name || '%')";
            if (cityList.Count>0) sqlStatement += " AND A.city in (SELECT city from TMP_CITIES)";
            
            sqlAdapter = new SQLiteDataAdapter(sqlStatement, dbConn);
            ds = new DataSet();
            sqlAdapter.Fill(ds);
        }

        private static bool IsTablePopulated()
        {
            string sql = "SELECT count(*) FROM PUBACC_EN";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
                int count;
                if (Int32.TryParse(cmd.ExecuteScalar().ToString(),out count))
                {
                    return (count > 0);
                }
                else
                {
                    return false;
                }
            }
        }

        private static void BulkInsertCsv()
        {
            // only insert if the table isn't already populated 
            if (IsTablePopulated()) return;
            
            // bulk insert:  https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/bulk-insert
            using (var transaction = dbConn.BeginTransaction())
            {
                var command = dbConn.CreateCommand();
                command.CommandText = @" INSERT INTO PUBACC_EN VALUES ($record_type,$unique_system_identifier,$uls_file_number,$ebf_number,$call_sign,$entity_type,$licensee_id,$entity_name,$first_name,$mi,$last_name,$suffix,$phone,$fax,$email,$street_address,$city,$state,$zip_code,$po_box,$attention_line,$sgin,$frn,$applicant_type_code,$applicant_type_other,$status_code,$status_date)";

                var param_record_type = command.CreateParameter();
                param_record_type.ParameterName = "$record_type";
                command.Parameters.Add(param_record_type);

                var param_unique_system_identifier = command.CreateParameter();
                param_unique_system_identifier.ParameterName = "$unique_system_identifier";
                command.Parameters.Add(param_unique_system_identifier);

                var param_uls_file_number = command.CreateParameter();
                param_uls_file_number.ParameterName = "$uls_file_number";
                command.Parameters.Add(param_uls_file_number);

                var param_ebf_number = command.CreateParameter();
                param_ebf_number.ParameterName = "$ebf_number";
                command.Parameters.Add(param_ebf_number);

                var param_call_sign = command.CreateParameter();
                param_call_sign.ParameterName = "$call_sign";
                command.Parameters.Add(param_call_sign);

                var param_entity_type = command.CreateParameter();
                param_entity_type.ParameterName = "$entity_type";
                command.Parameters.Add(param_entity_type);

                var param_licensee_id = command.CreateParameter();
                param_licensee_id.ParameterName = "$licensee_id";
                command.Parameters.Add(param_licensee_id);

                var param_entity_name = command.CreateParameter();
                param_entity_name.ParameterName = "$entity_name";
                command.Parameters.Add(param_entity_name);

                var param_first_name = command.CreateParameter();
                param_first_name.ParameterName = "$first_name";
                command.Parameters.Add(param_first_name);

                var param_mi = command.CreateParameter();
                param_mi.ParameterName = "$mi";
                command.Parameters.Add(param_mi);

                var param_last_name = command.CreateParameter();
                param_last_name.ParameterName = "$last_name";
                command.Parameters.Add(param_last_name);

                var param_suffix = command.CreateParameter();
                param_suffix.ParameterName = "$suffix";
                command.Parameters.Add(param_suffix);

                var param_phone = command.CreateParameter();
                param_phone.ParameterName = "$phone";
                command.Parameters.Add(param_phone);

                var param_fax = command.CreateParameter();
                param_fax.ParameterName = "$fax";
                command.Parameters.Add(param_fax);

                var param_email = command.CreateParameter();
                param_email.ParameterName = "$email";
                command.Parameters.Add(param_email);

                var param_street_address = command.CreateParameter();
                param_street_address.ParameterName = "$street_address";
                command.Parameters.Add(param_street_address);

                var param_city = command.CreateParameter();
                param_city.ParameterName = "$city";
                command.Parameters.Add(param_city);

                var param_state = command.CreateParameter();
                param_state.ParameterName = "$state";
                command.Parameters.Add(param_state);

                var param_zip_code = command.CreateParameter();
                param_zip_code.ParameterName = "$zip_code";
                command.Parameters.Add(param_zip_code);

                var param_po_box = command.CreateParameter();
                param_po_box.ParameterName = "$po_box";
                command.Parameters.Add(param_po_box);

                var param_attention_line = command.CreateParameter();
                param_attention_line.ParameterName = "$attention_line";
                command.Parameters.Add(param_attention_line);

                var param_sgin = command.CreateParameter();
                param_sgin.ParameterName = "$sgin";
                command.Parameters.Add(param_sgin);

                var param_frn = command.CreateParameter();
                param_frn.ParameterName = "$frn";
                command.Parameters.Add(param_frn);

                var param_applicant_type_code = command.CreateParameter();
                param_applicant_type_code.ParameterName = "$applicant_type_code";
                command.Parameters.Add(param_applicant_type_code);

                var param_applicant_type_other = command.CreateParameter();
                param_applicant_type_other.ParameterName = "$applicant_type_other";
                command.Parameters.Add(param_applicant_type_other);

                var param_status_code = command.CreateParameter();
                param_status_code.ParameterName = "$status_code";
                command.Parameters.Add(param_status_code);

                var param_status_date = command.CreateParameter();
                param_status_date.ParameterName = "$status_date";
                command.Parameters.Add(param_status_date);

                // read CSV data and insert each line
                using (StreamReader sr = new StreamReader(csvFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // values are '|' delimited
                        string[] l = line.Split('|');

                        param_record_type.Value = l[00];
                        param_unique_system_identifier.Value = l[01];
                        param_uls_file_number.Value = l[02];
                        param_ebf_number.Value = l[03];
                        param_call_sign.Value = l[04];
                        param_entity_type.Value = l[05];
                        param_licensee_id.Value = l[06];
                        param_entity_name.Value = l[07];
                        param_first_name.Value = l[08];
                        param_mi.Value = l[09];
                        param_last_name.Value = l[10];
                        param_suffix.Value = l[11];
                        param_phone.Value = l[12];
                        param_fax.Value = l[13];
                        param_email.Value = l[14];
                        param_street_address.Value = l[15];
                        param_city.Value = l[16];
                        param_state.Value = l[17];
                        param_zip_code.Value = l[18];
                        param_po_box.Value = l[19];
                        param_attention_line.Value = l[20];
                        param_sgin.Value = l[21];
                        param_frn.Value = l[22];
                        param_applicant_type_code.Value = l[23];
                        param_applicant_type_other.Value = l[24];
                        param_status_code.Value = l[25];
                        param_status_date.Value = l[26];
                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            } 
        }

        public static void CreateTable()
        {
            using (var cmd = new SQLiteCommand(sqlCreateTable, dbConn))
            {
                cmd.ExecuteScalar();
            }
        }

        private static void PrepTempDir()
        {
            if (!Directory.Exists(tempDirPath)) { 
                Directory.CreateDirectory(tempDirPath); 
            } 
        }

        private static void DownloadUlsData()
        {
            if (!File.Exists(zipFilePath))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(zipFileUrl, zipFilePath);
                }
            }
        }

        private static void ExtractUlsData()
        {
            if (!File.Exists(csvFilePath))
            {
                // could optimize by extracting just the EN.dat file
                ZipFile.ExtractToDirectory(zipFilePath, tempDirPath);
            }
        }

        private static void CreateDatabase()
        {
            if (!File.Exists(sqlFilePath))
            {
                SQLiteConnection.CreateFile(sqlFilePath);
            }
        }

        private static void OpenDatabase()
        {
            if (dbConn == null)
            {
                dbConn = new SQLiteConnection("Data Source=" + sqlFilePath + ";Version=3");
                dbConn.Open();
            }
        }

        public static string GetVersion()
        {
            string sql = "SELECT SQLITE_VERSION()";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
            {
               return cmd.ExecuteScalar().ToString();
            }
        }

        public static List<City> GetCities(string stringToParse)
        {
            List<City> list = new List<City>();

            using (StringReader sr = new StringReader(stringToParse))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // skip any blank lines 
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] parts = line.Trim().Split(',');
                        string city = parts[0].Trim();
                        if (parts.Length>1)
                        {
                            list.Add(new City(city, parts[1].Trim()));
                        } else
                        {
                            list.Add(new City(city));
                        }                           
                    }
                }
            }
            return list;
        }

        public static List<Person> GetNames(string stringToParse)
        {
            List<Person> list = new List<Person>();

            // read raw data and construct a list of names
            // data is expected to be in the format: 
            // Lastname, FirstName1 MiddleName1 & FirstName2 MiddleName2

            using (StringReader sr = new StringReader(stringToParse))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // skip any blank lines, or lines with only one letter
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // values are ',' delimited, 
                        string[] last = line.Split(',');

                        string lastName = last[0].Trim();                    
                        if (last.Length > 1)
                        {
                            // get first name(s) if any
                            // split on ampersand - if it exists, there were two family members with same last name, so add them both
                            string[] fam = last[1].Split('&');

                            // add first family member                                
                            string[] f1 = fam[0].Trim().Split(' ');
                            list.Add(new Person(lastName, f1[0]));

                            // add second family member, if applicable
                            if (fam.Length>1)
                            {
                                string[] f2 = fam[1].Trim().Split(' ');
                                list.Add(new Person(lastName, f2[0]));
                            }
                        }
                        else
                        {
                            if (lastName.Length>1) { list.Add(new Person(lastName)); }
                        }
                    }
                }
            }
            return list;
        }

    }
}
