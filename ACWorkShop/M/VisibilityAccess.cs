using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ACWorkShop
{
    #region Model VisibilityAccess
    public class VisibilityAccess
    {
        public Visibility CanViewSummary { get; set; }
        public Visibility CanViewTimesheet { get; set; }
        public Visibility CanViewReports { get; set; }
        public Visibility CanViewProject { get; set; }
        public Visibility CanViewUsers { get; set; }
        public Visibility CanViewGroupUser { get; set; }
        public Visibility CanViewSettings { get; set; }
        public Visibility CanViewClient { get; set; }
        public Visibility CanViewToDo { get; set; }
        public Visibility CanViewInvoice { get; set; }
        public Visibility CanViewQuote { get; set; }



    }
    #endregion

    #region ViewModel VisibilityAccess
    public class ViewHandeler
    {

        public VisibilityAccess CheckPermissions(User CurentUser)
        {
            var holder = new VisibilityAccess
            {
                //Summary
                CanViewSummary = CurentUser.UGroup.CanViewSummary ? Visibility.Visible : Visibility.Hidden,
                //TimeSheet
                CanViewTimesheet = CurentUser.UGroup.CanViewTimesheet ? Visibility.Visible : Visibility.Hidden,
                //Report
                CanViewReports = CurentUser.UGroup.CanViewReports ? Visibility.Visible : Visibility.Hidden,
                //Projects
                CanViewProject = CurentUser.UGroup.CanViewProject ? Visibility.Visible : Visibility.Hidden,
                //User
                CanViewUsers = CurentUser.UGroup.CanViewUsers ? Visibility.Visible : Visibility.Hidden,
                //UserGroup
                CanViewGroupUser = CurentUser.UGroup.CanViewUserGroup ? Visibility.Visible : Visibility.Hidden,
                //Settings
                CanViewSettings = CurentUser.UGroup.CanViewSettings ? Visibility.Visible : Visibility.Hidden,
                //User
                CanViewClient = CurentUser.UGroup.CanViewClient ? Visibility.Visible : Visibility.Hidden,
                //ToDoView
                CanViewToDo = CurentUser.UGroup.CanViewToDo ? Visibility.Visible : Visibility.Hidden,
                //Invoice
                CanViewInvoice = CurentUser.UGroup.CanViewInvoice ? Visibility.Visible : Visibility.Hidden,
                //Quote
                CanViewQuote = CurentUser.UGroup.CanViewQuote ? Visibility.Visible : Visibility.Hidden
            };

            return holder;
        }
    }
    #endregion
}
