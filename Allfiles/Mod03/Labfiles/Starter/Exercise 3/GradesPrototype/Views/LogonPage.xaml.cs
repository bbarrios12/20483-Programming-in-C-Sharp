using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradesPrototype.Data;
using GradesPrototype.Services;

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for LogonPage.xaml
    /// </summary>
    public partial class LogonPage : UserControl
    {
        public LogonPage()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler LogonSuccess;

        //  Exercise 3: Task 1a: Define LogonFailed event
        public event EventHandler LogonFailed;

        #endregion

        #region Logon Validation

        // Exercise 3: Task 1b: Validate the username and password against the Users collection in the MainWindow window
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            var teacher = (from Teacher t in DataSource.Teachers
                           where string.Compare(t.UserName, username.Text, true) == 0 &&
                                 string.Compare(t.Password, password.Password) == 0
                           select t).FirstOrDefault();

            if(!String.IsNullOrEmpty(teacher.UserName))
            {
                // Save the UserID and Role (teacher or student) and UserName in the global context
                SessionContext.UserID = teacher.TeacherID;
                SessionContext.UserRole = Role.Teacher;
                SessionContext.UserName = teacher.UserName;
                SessionContext.CurrentTeacher = teacher;

                LogonSuccess(this, null);
                return;
            }

            var student = (from Student t in DataSource.Students
                           where string.Compare(t.UserName, username.Text, true) == 0 &&
                                 string.Compare(t.Password, password.Password) == 0
                           select t).FirstOrDefault();

            if (!String.IsNullOrEmpty(student.UserName))
            {
                // Save the UserID and Role (teacher or student) and UserName in the global context
                SessionContext.UserID = student.StudentID;
                SessionContext.UserRole = Role.Student;
                SessionContext.UserName = student.UserName;
                SessionContext.CurrentStudent = student;

                LogonSuccess(this, null);
                return;
            }

            // If the credentials do not match those for a Teacher or for a Student then they must be invalid
            // Raise the LogonFailed event
            LogonFailed(this, null);
        }
        #endregion
    }
}
