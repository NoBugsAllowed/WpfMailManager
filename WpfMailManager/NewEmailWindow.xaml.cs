using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfTask1;

namespace WpfMailManager
{
    /// <summary>
    /// Interaction logic for NewEmailWindow.xaml
    /// </summary>
    public partial class NewEmailWindow : Window, INotifyPropertyChanged
    {
        private bool dataValid;

        public EmailMessage Email { get; set; }
        public bool DataValid
        {
            get { return dataValid; }
            set
            {
                dataValid = value;
                OnPropertyChanged("DataValid");
            }
        }

        public NewEmailWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Email = new EmailMessage();
            DataValid = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void btnSendClick(object sender, RoutedEventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            Email.Date = currentTime.Day + "-" + currentTime.Month + "-" + currentTime.Year;
            this.Close();
        }

        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if (toBox.Text.Length > 0 && titleBox.Text.Length > 0 && messageBox.Text.Length > 0)
            {
                if (Validation.GetHasError(toBox) || Validation.GetHasError(titleBox) || Validation.GetHasError(messageBox))
                    DataValid = false;
                else
                    DataValid = true;
                return;
            }
            DataValid = false;
        }
    }

    public class ToBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value as string;
            if(text.Length == 0)
                return new ValidationResult(false, (string)Application.Current.FindResource("fieldEmpty"));
            else if(!Regex.IsMatch(text, @"^([^@]+@([^@\.]+\.)+[^@\.]{2,})$"))
                return new ValidationResult(false, (string)Application.Current.FindResource("emailIncorrect"));

            return new ValidationResult(true, null);         
        }
    }

    public class TitleBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value as string;
            bool isEmpty = true;
            for (int i = 0; i < text.Length; i++)
                if (text[i] != ' ')
                {
                    isEmpty = false;
                    break;
                }

            if (isEmpty)
                return new ValidationResult(false, (string)Application.Current.FindResource("fieldEmpty"));
            return new ValidationResult(true, null);
        }
    }

    public class MessageBoxValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value as string;
            bool isEmpty = true;

            for(int i=0;i<text.Length;i++)
                if(text[i]!=' ')
                {
                    isEmpty = false;
                    break;
                }

            if(isEmpty)
                return new ValidationResult(false, (string)Application.Current.FindResource("fieldEmpty"));
            else if (text.Length<10)
                return new ValidationResult(false, (string)Application.Current.FindResource("messageToShort"));

            return new ValidationResult(true, null);
        }
    }
}
