using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACWorkShop
{
    class DBDConfigBuilderViewModel:ViewModelBase
    {
        #region Constructor
        public DBDConfigBuilderViewModel()
        {
            TestSaveCommand = new RelayCommand(TestSave);
        }
        #endregion

        #region Prop

        #region Private
        private string _dataSource;

        private RelayCommand _testSaveCommand;

        private string _initialCatalog;

        private bool _integratedSecurity;

        private string _username;

        private string _password;
        #endregion

        #region Public

        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                if (!string.IsNullOrWhiteSpace(_dataSource))
                {
                    Configuration.DataSource = _dataSource;
                }
                RaisePropertyChanged("DataSource");
            }
        }

        public RelayCommand TestSaveCommand
        {
            get { return _testSaveCommand; }
            set
            {
                _testSaveCommand = value;
                RaisePropertyChanged("TestSaveCommand");
            }
        }

        public string InitialCatalog
        {
            get { return _initialCatalog; }
            set
            {
                _initialCatalog = value;
                if (!string.IsNullOrWhiteSpace(_initialCatalog))
                {
                    Configuration.InitialCatalog = InitialCatalog;
                }
                RaisePropertyChanged("InitialCatalog");
            }
        }

        public bool IntegratedSecurity
        {
            get { return _integratedSecurity; }
            set
            {
                _integratedSecurity = value;
                Configuration.IntegratedSecurity = value;
                RaisePropertyChanged("IntegratedSecurity");
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                if (!string.IsNullOrWhiteSpace(_username))
                {
                    Configuration.UserID = _username;
                }
                RaisePropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                if (!string.IsNullOrWhiteSpace(_password))
                {
                    Configuration.Password = _password;
                }
                RaisePropertyChanged("Password");
            }
        }



        #endregion

        #endregion

        #region Function

        void TestSave(object parameter)
        {

            if (DataSource == null || InitialCatalog == null)
            {
                MessageBox.Show("Please Enter Valid Values");
            }
            else
            {
                if (IntegratedSecurity)
                {
                    Username = "";
                    Password = "";
                }

                if (Configuration.TestCon())
                {
                    if (Configuration.SaveDetails())
                    {
                    }
                    //UsersHandler UH = new UsersHandler(Config);
                    //Globals.Users = UH.GetAllUser();
                    var win = new LoginView { DataContext = new LoginViewModel() };
                    win.Show();
                    this.CloseWindowFlag = true;
                    this.CloseWindow();
                }
                else
                {
                    MessageBox.Show("Data Base connections are incorrect!", "DB Connection Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        #endregion
    }
}
