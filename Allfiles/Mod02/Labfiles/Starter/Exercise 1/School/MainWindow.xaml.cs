using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using School.Data;


namespace School
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Connection to the School database
        private SchoolDBEntities schoolContext = null;

        // Field for tracking the currently selected teacher
        private Teacher teacher = null;

        // List for tracking the students assigned to the teacher's class
        private IList studentsInfo = null;

        string eventLog = "Application";
        string eventSource = "School";

        #region Predefined code

        public MainWindow()
        {
            if (!EventLog.SourceExists(eventSource))
                EventLog.CreateEventSource(eventSource, eventLog);
            EventLog.WriteEntry(eventSource, "Iniciando Aplicación",EventLogEntryType.Information);
            InitializeComponent();
            EventLog.WriteEntry(eventSource, "Ventana Principal Creada", EventLogEntryType.Information);
        }

        // Connect to the database and display the list of teachers when the window appears
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EventLog.WriteEntry(eventSource, "Conectando a la base de datos", EventLogEntryType.Information);
            this.schoolContext = new SchoolDBEntities();
            teachersList.DataContext = this.schoolContext.Teachers;
            EventLog.WriteEntry(eventSource, "Datos de maestros cargados", EventLogEntryType.Information);
        }

        // When the user selects a different teacher, fetch and display the students for that teacher
        private void teachersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            // Find the teacher that has been selected
            this.teacher = teachersList.SelectedItem as Teacher;
            EventLog.WriteEntry(eventSource, "Cambiando Profesor " + teacher.FirstName + " " + teacher.LastName, EventLogEntryType.Information);
            EventLog.WriteEntry(eventSource, "Cargando Alumnos", EventLogEntryType.Information);
            this.schoolContext.LoadProperty<Teacher>(this.teacher, s => s.Students);
            // Find the students for this teacher
            this.studentsInfo = ((IListSource)teacher.Students).GetList();
            EventLog.WriteEntry(eventSource, "Finalizo la carga de alumnos", EventLogEntryType.Information);
            // Use databinding to display these students
            studentsList.DataContext = this.studentsInfo;
            EventLog.WriteEntry(eventSource, "Actualizando listado de alumnos", EventLogEntryType.Information);
        }

        #endregion

        // When the user presses a key, determine whether to add a new student to a class, remove a student from a class, or modify the details of a student
        private void studentsList_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            switch (e.Key)
            {
                // If the user pressed Enter, edit the details for the currently selected student
                case Key.Enter:
                    editStudent(this.studentsList.SelectedItem as Student);
                    break;
                // If the user pressed Insert, add a new student
                case Key.Insert:
                    addNewStudent();
                    break;
                // If the user pressed Delete, remove the currently selected student
                case Key.Delete:
                    deleteStudent(this.studentsList.SelectedItem as Student);
                    break;
            }
        }

        // TODO: Exercise 1: Task 3c: create Edit student method
        void editStudent(Student student)
        {
            EventLog.WriteEntry(eventSource, "Editando Estudiante", EventLogEntryType.Information);
            // TODO: Exercise 1: Task 1a: Copy code for editing the details for that student

            // TODO: Exercise 1: Task 3d: Refactor as the EditStudent method

            // Use the StudentsForm to display and edit the details of the student
            StudentForm sf = new StudentForm();

            // Set the title of the form and populate the fields on the form with the details of the student           
            sf.Title = Properties.Resources.TituloEditarEstudiante;
            sf.firstName.Text = student.FirstName;
            sf.lastName.Text = student.LastName;
            sf.dateOfBirth.Text = student.DateOfBirth.ToString("d", CultureInfo.CurrentCulture); // Format the date to omit the time element

            // Display the form
            if (sf.ShowDialog().Value)
            {
                // When the user closes the form, copy the details back to the student
                student.FirstName = sf.firstName.Text;
                student.LastName = sf.lastName.Text;
                student.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text, CultureInfo.CurrentCulture);

                // Enable saving (changes are not made permanent until they are written back to the database)
                saveChanges.IsEnabled = true;
            }
        }

        void addNewStudent()
        {
            // TODO: Exercise 1: Task 3a: Refactor as the addNewStudent method
            EventLog.WriteEntry(eventSource, "Agregando Estudiante", EventLogEntryType.Information);
            // Use the StudentsForm to get the details of the student from the user
            var sf = new StudentForm();

            // Set the title of the form to indicate which class the student will be added to (the class for the currently selected teacher)
            sf.Title = "Agregar Nuevo estudiantes a la clase " + teacher.Class;

            // Display the form and get the details of the new student
            if (sf.ShowDialog().Value)
            {
                // When the user closes the form, retrieve the details of the student from the form
                // and use them to create a new Student object
                Student newStudent = new Student();
                newStudent.FirstName = sf.firstName.Text;
                newStudent.LastName = sf.lastName.Text;
                newStudent.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text, CultureInfo.CurrentCulture);

                // Assign the new student to the current teacher
                this.teacher.Students.Add(newStudent);

                // Add the student to the list displayed on the form
                this.studentsInfo.Add(newStudent);

                // Enable saving (changes are not made permanent until they are written back to the database)
                saveChanges.IsEnabled = true;
            }
        }

        void deleteStudent(Student student)
        {
            // TODO: Exercise 1: Task 3b: Refactor as the removeStudent method
            EventLog.WriteEntry(eventSource, "Borrando Estudiante", EventLogEntryType.Information);
            // Prompt the user to confirm that the student should be removed
            MessageBoxResult response = MessageBox.Show(
                string.Format(CultureInfo.CurrentCulture, Properties.Resources.EliminarEstudiante, student.FirstName + " " + student.LastName),
                Properties.Resources.ConfirmarAccion, MessageBoxButton.YesNo, MessageBoxImage.Question,
                MessageBoxResult.No);

            // If the user clicked Yes, remove the student from the database
            if (response == MessageBoxResult.Yes)
            {
                this.schoolContext.Students.DeleteObject(student);

                // Enable saving (changes are not made permanent until they are written back to the database)
                saveChanges.IsEnabled = true;
            }
        }

        // TODO: Exercise 1: Task 1b: If the user double-clicks a student, edit the details for that student
        private void studentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editStudent(this.studentsList.SelectedItem as Student);
        }

        // Save changes back to the database and make them permanent
        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            EventLog.WriteEntry(eventSource, "Guardando Cambios " + schoolContext, EventLogEntryType.Information);
            try
            {
                // Save the changes
                this.schoolContext.SaveChanges();

                // Disable the Save button (it will be enabled if the user makes more changes)
                saveChanges.IsEnabled = false;
            }
            catch (OptimisticConcurrencyException)
            {
                this.schoolContext.Refresh(RefreshMode.ClientWins, schoolContext.Students);
                this.schoolContext.SaveChanges();
                throw;
            }
            catch(UpdateException uEx)
            {
                MessageBox.Show(uEx.InnerException.Message, "Error al guardar la información");
                this.schoolContext.Refresh(RefreshMode.StoreWins, schoolContext.Students);
            }
            catch (Exception ex)
            {
                // If some other exception occurs, report it to the user
                MessageBox.Show(ex.Message, "Error al guardar la información");
                this.schoolContext.Refresh(RefreshMode.ClientWins, schoolContext.Students);
            }
        }
    }

    [ValueConversion(typeof(string), typeof(Decimal))]
    class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            // Convert the date of birth provided in the value parameter and convert to the age of the student in years
            if (value != null)
            {
                DateTime studentDateOfBirth = (DateTime)value;
                TimeSpan difference = DateTime.Now.Subtract(studentDateOfBirth);
                int ageInYears = (int)(difference.Days / 365.25);
                return ageInYears.ToString(CultureInfo.CurrentCulture);
            }
            else
            {
                return "";
            }
        }

        #region Predefined code

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
