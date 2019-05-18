using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACWorkShop
{
    #region Model UserGroup
    public class UserGroup
    {
        public long ID { get; set; }
        public string GroupName { get; set; }
        public bool CanViewSummary { get; set; }
        public bool CanViewTimesheet { get; set; }
        public bool CanViewReports { get; set; }
        public bool CanViewProject { get; set; }
        public bool CanViewUsers { get; set; }
        public bool CanViewUserGroup { get; set; }
        public bool CanViewSettings { get; set; }
        public bool CanViewClient { get; set; }
        public bool CanViewToDo { get; set; }
        public bool CanViewInvoice { get; set; }
        public bool CanViewQuote { get; set; }



        public UserGroup()
        {
            ID = 0;
            GroupName = string.Empty;
            CanViewSummary = false;
            CanViewTimesheet = false;
            CanViewReports = false;
            CanViewProject = false;
            CanViewUsers = false;
            CanViewUserGroup = false;
            CanViewSettings = false;
            CanViewToDo = false;
            CanViewInvoice = false;
            CanViewQuote = false;


        }
    }
    #endregion

    #region ViewModel UserGroup
    public class UserGroupHandler
    {
        private readonly string DataBaseName = "use " + Configuration.InitialCatalog + ";";
        private readonly string UsersTable = "users";
        private readonly string UserGroupTable = "usergroup";

        /// <summary>
        /// Get all users from Database
        /// </summary>
        /// <returns>List<Users></Users></returns>
        public List<UserGroup> GetAll(string Filter)
        {
            var Holder = new List<UserGroup>();
            var sqlString = DataBaseName + @" SELECT ID
                                                    ,GroupName
                                                    ,CanViewSummary
                                                    ,CanViewTimesSheet
                                                    ,CanViewReports
                                                    ,CanViewProject
                                                    ,CanViewUsers
                                                    ,CanViewUserGroup
                                                    ,CanViewSettings
                                                    ,CanViewClient
                                                    ,CanViewToDo  
                                                    ,CanViewInvoice
                                                    ,CanViewQuote
                                            FROM " + UserGroupTable + @" WHERE GroupName LIKE '" + Filter + "%'";
            ;
            MySqlDataReader rdr = null;

            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    rdr = sqlcmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Holder.Add(new UserGroup()
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
                            CanViewInvoice = Convert.ToBoolean(rdr["CanViewInvoice"]),
                            CanViewQuote = Convert.ToBoolean(rdr["CanViewQuote"])
                        });
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
        public UserGroup GetUserGroup(User _usr)
        {
            var Holder = new UserGroup();
            var sqlString = DataBaseName + @" SELECT ug.ID                        
                                                    ,GroupName
                                                    ,CanViewSummary
                                                    ,CanViewTimesSheet
                                                    ,CanViewReports
                                                    ,CanViewProject
                                                    ,CanViewUsers
                                                    ,CanViewUserGroup
                                                    ,CanViewSettings
                                                    ,CanViewClient
                                                    ,CanViewToDo 
                                                    ,CanViewInvoice
                                                    ,CanViewQuote
                                                  FROM " + UserGroupTable + @" as ug
                                                  INNER JOIN " + UsersTable + @" as u on u.UserGroupID = ug.ID
                                                  where u.ID = @UID";
            MySqlDataReader rdr = null;
            using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
            {
                sqlcon.Open();

                using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                {
                    sqlcmd.Parameters.AddWithValue("@UID", _usr.ID);
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
                            CanViewInvoice = Convert.ToBoolean(rdr["CanViewInvoice"]),
                            CanViewQuote = Convert.ToBoolean(rdr["CanViewQuote"])
                        };
                    }
                }

                sqlcon.Close();
            }

            //On return if ID = 0 then failed else success
            return Holder;

        }

        public UserGroup GetSpesificGroup(long id)
        {
            var Holder = new UserGroup();
            var sqlString = DataBaseName + @" SELECT ID                        
                                                    ,GroupName
                                                    ,CanViewSummary
                                                    ,CanViewTimesSheet
                                                    ,CanViewReports
                                                    ,CanViewProject
                                                    ,CanViewUsers
                                                    ,CanViewUserGroup
                                                    ,CanViewSettings
                                                    ,CanViewClient 
                                                    ,CanViewToDo
                                                    ,CanViewInvoice
                                                    ,CanViewQuote
                                               FROM " + UserGroupTable + @" 
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
                            CanViewInvoice = Convert.ToBoolean(rdr["CanViewInvoice"]),
                            CanViewQuote = Convert.ToBoolean(rdr["CanViewQuote"])
                        };
                    }
                }

                sqlcon.Close();
            }

            //On return if ID = 0 then failed else success
            return Holder;

        }

        public bool Save(UserGroup usrg)
        {
            var Holder = false;

            if (usrg.ID > 0)
            {
                Holder = Update(usrg);
            }
            else
            {
                Holder = Insert(usrg);
            }

            return Holder;
        }

        private bool Insert(UserGroup usrg)
        {
            var Holder = false;
            var sqlString = DataBaseName + @" INSERT INTO " + UserGroupTable + @"
                (GroupName, CanViewSummary, CanViewTimesSheet, CanViewReports, CanViewProject, CanViewUsers, CanViewUserGroup, CanViewSettings, CanViewClient, CanViewToDo)
                                VALUES
                                (@GroupName,
                                @CanViewSummary,
                                @CanViewTimesSheet,
                                @CanViewReports,
                                @CanViewProject,
                                @CanViewUsers,
                                @CanViewUserGroup,
                                @CanViewSettings,
                                @CanViewClient,
                                @CanViewToDo,
                                @CanViewInvoice,
                                @CanViewQuote);";
            try
            {
                using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
                {
                    sqlcon.Open();

                    using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                    {

                        sqlcmd.Parameters.AddWithValue("@GroupName", usrg.GroupName);
                        sqlcmd.Parameters.AddWithValue("@CanViewSummary", usrg.CanViewSummary);
                        sqlcmd.Parameters.AddWithValue("@CanViewTimesSheet", usrg.CanViewTimesheet);
                        sqlcmd.Parameters.AddWithValue("@CanViewReports", usrg.CanViewReports);
                        sqlcmd.Parameters.AddWithValue("@CanViewProject", usrg.CanViewProject);
                        sqlcmd.Parameters.AddWithValue("@CanViewUsers", usrg.CanViewUsers);
                        sqlcmd.Parameters.AddWithValue("@CanViewUserGroup", usrg.CanViewUserGroup);
                        sqlcmd.Parameters.AddWithValue("@CanViewSettings", usrg.CanViewSettings);
                        sqlcmd.Parameters.AddWithValue("@CanViewClient", usrg.CanViewClient);
                        sqlcmd.Parameters.AddWithValue("@CanViewToDo", usrg.CanViewToDo);
                        sqlcmd.Parameters.AddWithValue("@CanViewInvoice", usrg.CanViewToDo);
                        sqlcmd.Parameters.AddWithValue("@CanViewQuote", usrg.CanViewToDo);
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

        private bool Update(UserGroup usrg)
        {
            var Holder = false;

            try
            {

                var sqlString = DataBaseName + @" UPDATE " + UserGroupTable + @"
                                SET
	                                GroupName = @GroupName,
	                                CanViewSummary = @CanViewSummary,
	                                CanViewTimesSheet = @CanViewTimesSheet,
	                                CanViewReports = @CanViewReports,
                                    CanViewProject   = @CanViewProject,
                                    CanViewUsers= @CanViewUsers,
									CanViewUserGroup= @CanViewUserGroup,
									CanViewSettings= @CanViewSettings,
									CanViewClient= @CanViewClient,
                                    CanViewToDo= @CanViewToDo,
                                    CanViewInvoice= @CanViewInvoice,
                                    CanViewQuote= @CanViewQuote
                                WHERE ID = @ID";

                using (MySqlConnection sqlcon = new MySqlConnection(Configuration.ConnectionString))
                {
                    sqlcon.Open();

                    using (MySqlCommand sqlcmd = new MySqlCommand(sqlString, sqlcon))
                    {
                        sqlcmd.Parameters.AddWithValue("@GroupName", usrg.GroupName);
                        sqlcmd.Parameters.AddWithValue("@CanViewSummary", usrg.CanViewSummary);
                        sqlcmd.Parameters.AddWithValue("@CanViewTimesSheet", usrg.CanViewTimesheet);
                        sqlcmd.Parameters.AddWithValue("@CanViewReports", usrg.CanViewReports);
                        sqlcmd.Parameters.AddWithValue("@CanViewProject", usrg.CanViewProject);
                        sqlcmd.Parameters.AddWithValue("@CanViewUsers", usrg.CanViewUsers);
                        sqlcmd.Parameters.AddWithValue("@CanViewUserGroup", usrg.CanViewUserGroup);
                        sqlcmd.Parameters.AddWithValue("@CanViewSettings", usrg.CanViewSettings);
                        sqlcmd.Parameters.AddWithValue("@CanViewClient", usrg.CanViewClient);
                        sqlcmd.Parameters.AddWithValue("@CanViewToDo", usrg.CanViewToDo);
                        sqlcmd.Parameters.AddWithValue("@CanViewInvoice", usrg.CanViewToDo);
                        sqlcmd.Parameters.AddWithValue("@CanViewQuote", usrg.CanViewToDo);
                        sqlcmd.Parameters.AddWithValue("@ID", usrg.ID);

                        sqlcmd.ExecuteNonQuery();
                    }
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


    }
    #endregion
}
