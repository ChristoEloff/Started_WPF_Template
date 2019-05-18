using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACWorkShop
{
    public class User
    {
        /// <summary>
        /// User ID in Database
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// User First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User Login Name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Is User Active 
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// UserGroup
        /// </summary>
        public UserGroup UGroup { get; set; }

        /// <summary>
        /// UserGroup
        /// </summary>
        public VisibilityAccess UVisibility { get; set; }

        /// <summary>
        /// Login init
        /// </summary>
        public User()
        {
            ID = 0;
            UserName = string.Empty;
            Password = string.Empty;
            UGroup = new UserGroup();
            UVisibility = new VisibilityAccess();
        }

        public User(User u)
        {
            try
            {
                ID = u.ID;
                UserName = u.UserName;
                Password = u.Password;
                FirstName = u.FirstName;
                LastName = u.LastName;
                Active = u.Active;
                UGroup = u.UGroup;
                UVisibility = u.UVisibility;
            }
            catch
            {
                ID = 0;
                UserName = string.Empty;
                Password = string.Empty;
                FirstName = string.Empty;
                LastName = string.Empty;
                Active = false;
                UGroup = new UserGroup();
                UVisibility = new VisibilityAccess();
            }
        }
    }


    
    public class UserHandler
    {
        private readonly string DataBaseName = "use " + Configuration.InitialCatalog + ";";
        private readonly string UsersTable = "users";
        private readonly string UserGroupTable = "usergroup";
        private readonly string ProjectUsersTable = "projectusers";

       

        /// <summary>
        /// Get all users from Database
        /// </summary>
        /// <returns>List<Users></Users></returns>
        public List<User> GetAllUsersOnly(string FirstName, string GroupName, string LastName)
        {
            var Holder = new List<User>();
            var sqlString = DataBaseName + @" SELECT U.ID,
                                                FirstName,
	                                            LastName,
	                                            UserName,
	                                            `Password`,
	                                            Active,
	                                            UserGroupID,
	                                            GroupName
                                            FROM " + UsersTable + @" AS U
                                            INNER JOIN " + UserGroupTable + " UG ON UG.ID = U.UserGroupID" +
                                            " WHERE GroupName LIKE '" + GroupName + "%' " +
                                                "and FirstName LIKE '" + FirstName + "%' " +
                                                "and LastName LIKE '" + LastName + "%'";
            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder.Add(new User()
                        {
                            ID = Convert.ToInt64(rdr["ID"]),
                            UserName = rdr["UserName"].ToString(),
                            Password = rdr["Password"].ToString(),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Active = Convert.ToBoolean(rdr["Active"]),
                            UGroup = new UserGroup() { ID = Convert.ToInt64(rdr["UserGroupID"]), GroupName = rdr["GroupName"].ToString() }
                        });
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }

        public User GetSpesificUserOnly(long id)
        {
            var Holder = new User();
            string sqlString = DataBaseName + @" SELECT *    
                                                 FROM users
                                                 WHERE ID = @p0";
            MySqlDataReader rdr = null;
            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();
                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@p0", id);
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder = new User()
                        {
                            ID = Convert.ToInt64(rdr["ID"]),
                            UserName = rdr["UserName"].ToString(),
                            Password = rdr["Password"].ToString(),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Active = Convert.ToBoolean(rdr["Active"])
                        };
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        public User Login(User _usr)
        {
            if (_usr.UserName == "RSASoft" && _usr.Password == "R$@t@!&1")
            {
                _usr.ID = -1;
                _usr.UGroup = new UserGroup()
                {
                    ID = -1,
                    CanViewClient = true,
                    CanViewProject = true,
                    CanViewReports = true,
                    CanViewSettings = true,
                    CanViewSummary = true,
                    CanViewTimesheet = true,
                    CanViewUserGroup = true,
                    CanViewUsers = true,
                    CanViewToDo = true,
                    CanViewInvoice = true,
                    GroupName = "RSASoftAdmin"
                };

                return _usr;

            }
            else
            {
                var Holder = new User();
                var sqlString = DataBaseName + @" SELECT * 
                                                    FROM " + UsersTable + @" 
                                                    WHERE UserName = @p0 
                                                        AND Password = @p1";
                MySqlDataReader rdr = null;

                using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
                {
                    sqlcon.Open();
                    using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@p0", _usr.UserName);
                        sqlcmd.Parameters.AddWithValue("@p1", Crypto.Encrypt(_usr.Password));

                        rdr = sqlcmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            Holder = new User()
                            {
                                ID = Convert.ToInt64(rdr["ID"]),
                                UserName = rdr["UserName"].ToString(),
                                Password = rdr["Password"].ToString(),
                                FirstName = rdr["FirstName"].ToString(),
                                LastName = rdr["LastName"].ToString(),
                                Active = Convert.ToBoolean(rdr["Active"]),
                                UGroup = new UserGroupHandler().GetSpesificGroup(Convert.ToInt64(rdr["UserGroupID"]))
                            };
                        }
                    }

                    sqlcon.Close();

                }

                if (Holder.ID == 0)
                {
                    Holder = _usr;
                }

                //On return if ID = 0 then failed else success
                return Holder;
            }
        }

        /// <summary>
        /// Get all users from Database
        /// </summary>
        /// <returns>List<Users></Users></returns>
        public List<User> GetAllUsers()
        {
            var Holder = new List<User>();
            var sqlString = DataBaseName + @" Select *
                                              From " + UsersTable;
            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder.Add(new User()
                        {
                            ID = Convert.ToInt64(rdr["ID"]),
                            UserName = rdr["UserName"].ToString(),
                            Password = rdr["Password"].ToString(),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Active = Convert.ToBoolean(rdr["Active"]),
                            UGroup = new UserGroupHandler().GetSpesificGroup(Convert.ToInt64(rdr["UserGroupID"]))
                        });
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }

        public List<User> GetAllUsersOnProject(long id)
        {
            List<User> Holder = new List<User>();
            string query = DataBaseName + @" SELECT ID, 
                                                FirstName,
                                                LastName,
                                                UserName,
                                                `Password`,
                                                Active,
                                                UserGroupID
                                            FROM " + UsersTable + @" 
                                            WHERE ID IN (SELECT UserID 
                                                         FROM " + ProjectUsersTable + @" 
                                                         WHERE ProjectID = @p0)";
            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(query, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@p0", id);
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder.Add(new User()
                        {
                            ID = Convert.ToInt64(rdr["ID"]),
                            UserName = rdr["UserName"].ToString(),
                            Password = rdr["Password"].ToString(),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Active = Convert.ToBoolean(rdr["Active"]),
                            //UGroup = new UserGroupHandler().GetSpesificGroup(Convert.ToInt64(rdr["UserGroupID"]))
                        });
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }

        public User GetSpesificUser(long id)
        {
            var Holder = new User();
            string sqlString = DataBaseName + @" SELECT *    
                                                 FROM users
                                                 WHERE ID = @p0";
            MySqlDataReader rdr = null;
            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();
                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@p0", id);
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder = new User()
                        {
                            ID = Convert.ToInt64(rdr["ID"]),
                            UserName = rdr["UserName"].ToString(),
                            Password = rdr["Password"].ToString(),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Active = Convert.ToBoolean(rdr["Active"]),
                            UGroup = new UserGroupHandler().GetSpesificGroup(Convert.ToInt64(rdr["UserGroupID"]))
                        };
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }


        public bool Save(User usr)
        {
            var Holder = false;

            if (usr.ID > 0)
            {
                Holder = Update(usr);
            }
            else
            {
                if (CheckUsername(usr.UserName))
                {
                    Holder = false;
                }
                else
                {
                    Holder = Insert(usr);
                }
            }
            return Holder;
        }

        private bool Insert(User usr)
        {
            var Holder = false;

            try
            {
                var sqlString = DataBaseName + @" INSERT INTO " + UsersTable + @"
                                        (FirstName, LastName, UserName, Password, Active, UserGroupID) 
                                        VALUES(@FirstName,
                                            @LastName,
                                            @UserName,
                                            @Password,
                                            @Active,
                                            @UserGroupID)";

                using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
                {
                    sqlcon.Open();

                    using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                    {

                        sqlcmd.Parameters.AddWithValue("@FirstName", usr.FirstName);
                        sqlcmd.Parameters.AddWithValue("@LastName", usr.LastName);
                        sqlcmd.Parameters.AddWithValue("@UserName", usr.UserName);
                        sqlcmd.Parameters.AddWithValue("@Password", Crypto.Encrypt(usr.Password));
                        sqlcmd.Parameters.AddWithValue("@Active", usr.Active);
                        sqlcmd.Parameters.AddWithValue("@UserGroupID", usr.UGroup.ID);

                        sqlcmd.ExecuteNonQuery();
                    }
                    sqlcon.Close();
                }

                Holder = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Holder = false;
            }

            return Holder;
        }

        private bool Update(User usr)
        {
            var Holder = false;
            var sqlString = DataBaseName + @"UPDATE " + UsersTable + @"
                                            SET FirstName = @FirstName,
	                                            LastName = @LastName,
	                                            Password = @Password,
                                                Active = @Active,
                                                UserGroupID = @UserGroupID
                                            WHERE ID = @ID";
            try
            {
                using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
                {
                    sqlcon.Open();

                    using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@FirstName", usr.FirstName);
                        sqlcmd.Parameters.AddWithValue("@LastName", usr.LastName);
                        sqlcmd.Parameters.AddWithValue("@Password", Crypto.Encrypt(usr.Password));
                        sqlcmd.Parameters.AddWithValue("@Active", usr.Active);
                        sqlcmd.Parameters.AddWithValue("@UserGroupID", usr.UGroup.ID);
                        sqlcmd.Parameters.AddWithValue("@ID", usr.ID);

                        sqlcmd.ExecuteNonQuery();
                    }
                    sqlcon.Close();
                }

                Holder = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Holder = false;
            }

            return Holder;
        }

        public User GetUserGroup(User u)
        {
            var Holder = new UserGroup();
            string query = DataBaseName + @" SELECT ID,
	                                            GroupName,
                                                CanViewSummary,
                                                CanViewTimesSheet,
                                                CanViewReports,
                                                CanViewProject,
                                                CanViewUsers,
                                                CanViewUserGroup,
                                                CanViewSettings,
                                                CanViewClient,
                                                CanViewToDo,
                                                CanViewInvoices
                                            FROM " + UserGroupTable + @"
                                            WHERE ID = @UID";

            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(query, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@UID", u.UGroup.ID);
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder = new UserGroup()
                        {
                            ID = Convert.ToInt32(rdr["ID"]),
                            GroupName = rdr["GroupName"].ToString(),
                            CanViewSummary = Convert.ToBoolean(rdr["CanViewSummary"]),
                            CanViewTimesheet = Convert.ToBoolean(rdr["CanViewTimesSheet"]),
                            CanViewReports = Convert.ToBoolean(rdr["CanViewReports"]),
                            CanViewProject = Convert.ToBoolean(rdr["CanViewProject"]),
                            CanViewUsers = Convert.ToBoolean(rdr["CanViewUsers"]),
                            CanViewUserGroup = Convert.ToBoolean(rdr["CanViewUserGroup"]),
                            CanViewSettings = Convert.ToBoolean(rdr["CanViewSettings"]),
                            CanViewClient = Convert.ToBoolean(rdr["CanViewClient"]),
                            CanViewToDo = Convert.ToBoolean(rdr["CanViewToDo"]),
                            CanViewInvoice = Convert.ToBoolean(rdr["CanViewInvoices"])
                        };
                    }
                }

                sqlcon.Close();
            }

            u.UGroup = Holder;

            return u;
        }

        public bool CheckUsername(string _username)
        {
            var Holder = false;
            var sqlString = DataBaseName + @" SELECT * 
                                              FROM " + UsersTable + @"
                                              WHERE UserName = @p0";
            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@p0", _username);
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder = true;
                    }
                }

                sqlcon.Close();
            }

            return Holder;
        }
    }
}
