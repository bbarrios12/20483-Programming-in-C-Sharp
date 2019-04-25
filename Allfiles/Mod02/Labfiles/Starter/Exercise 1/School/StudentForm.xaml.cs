using System;
using System.Globalization;
using System.Windows;

namespace School
{
    /// <summary>
    /// Interaction logic for StudentForm.xaml
    /// </summary>
    public partial class StudentForm : Window
    {
        #region Predefined code

        public StudentForm()
        {
            InitializeComponent();
        }

        #endregion

        // If the user clicks OK to save the Student details, validate the information that the user has provided
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            firstName.Text = firstName.Text.Trim();
            lastName.Text = lastName.Text.Trim();
            dateOfBirth.Text = dateOfBirth.Text.Trim();

            if (String.IsNullOrEmpty(this.firstName.Text))
            {
                MessageBox.Show("El estudiante debe tener un nombre", "Error");
                return;
            }
            if (String.IsNullOrEmpty(this.lastName.Text))
            {
                MessageBox.Show("El estudiante debe tener un apellido", "Error");
                return;
            }

            if (String.IsNullOrEmpty(this.dateOfBirth.Text))
            {
                MessageBox.Show("El estudiante debe tener una fecha valida", "Error");
                return;
            }

            if (!String.IsNullOrEmpty(this.dateOfBirth.Text) && GetStudentYears() < 5)
            {
                MessageBox.Show("El estudiante debe tener al menos 5 años", "Error");
                return;
            }
            // Indicate that the data is valid
            this.DialogResult = true;
        }

        private int GetStudentYears()
        {
            try
            {
                var birthDatetime = DateTime.Parse(dateOfBirth.Text, CultureInfo.CurrentCulture);
                var years = (int)(DateTime.Now.Subtract(birthDatetime).Days / 365.25);
                return years;
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}
