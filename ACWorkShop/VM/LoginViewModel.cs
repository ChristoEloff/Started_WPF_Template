using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACWorkShop
{
    class LoginViewModel : ViewModelBase
    {
        #region Constructor
        public LoginViewModel()
        {
            CurrentUser = new User();
            ClickCommand = new RelayCommand((x) => ButtonClick(x));
            VisibilityAccess Visible = new VisibilityAccess();
        }
        #endregion

        #region Prop
        #region Private
        private User _currentUser { get; set; }

        private RelayCommand _clickCommand;
        #endregion

        #region Public
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                RaisePropertyChanged("CurrentUserName");
            }
        }

        public RelayCommand ClickCommand
        {
            get { return _clickCommand; }
            set
            {
                _clickCommand = value;
                RaisePropertyChanged("ClickCommand");
            }
        }
        #endregion
        #endregion

        #region Function
        public void ButtonClick(object p)
        {
            switch (p.ToString())
            {
                case "login":

                    //Application.Current.Windows[1].Close();
                    if (TryLogin())
                    {
                        MainWindow M = new MainWindow() { DataContext = new MainWindowViewModel(CurrentUser) };
                        M.Show();

                        this.CloseWindowFlag = true;
                        this.CloseWindow();
                    }

                    break;

                default:
                    break;
            }
        }

        public bool TryLogin()
        {
            CurrentUser.Password = CurrentUser.Password.Trim();
            CurrentUser.UserName = CurrentUser.UserName.Trim();
            UserHandler Hand = new UserHandler();
            CurrentUser = Hand.Login(CurrentUser);
            if (CurrentUser.ID == 0)
            {
                MessageBox.Show("Invalid Username and Password Combination");

                return false;
            }
            if (CheckUserActive(CurrentUser))
            {
                if (CurrentUser.UGroup.ID == -1)
                {
                    return true;
                }

                else
                {

                    //Get Usergroup permissions                    
                    CurrentUser.UGroup = CurrentUser.UGroup;
                    ViewHandeler vh = new ViewHandeler();
                    CurrentUser.UVisibility = vh.CheckPermissions(CurrentUser);
                    var ToLog = new Log()
                    {
                        Details = @"User " + CurrentUser.FirstName + " Loged In ",
                        Action = "Login"
                    };
                    Task tASK = Task.Factory.StartNew(() => LogAction(ToLog));
                    return true;

                }
            }
            else
            {
                MessageBox.Show("Your Current User is not Active Please Contack Your Database Adminstrator");
                return false;
            }
        }

        public bool CheckUserActive(User _usr)
        {
            if (_usr.UGroup.ID == -1)
            {
                return true;
            }
            else
            {
                if (_usr.Active == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        private void LogAction(Log lg)
        {
            var l = new LogHander().LogAction(lg);
        }

        #endregion
    }
}
