using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;

//Планировщик задач
//(необходима базовая функциональность – добавить задачу, редактировать задачу,
//отметить задачу выполненной,
//установить задаче доп.описание и дедлайн, индикация просроченных задач).

namespace TaskPlanner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int GridRowIndex = 0;
        public bool IsEditable = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Template

        void SomeClick() { }

        void SomeFunc() { }

        void SomeViewUpdate() { }

        #endregion

        void CreateNewTask(string name, string description, string data)
        {
            UpdateNewTaskView(name, description, data);
            ClearInput();
            CheckDeadline();

            GridRowIndex++;
        }


        void MakeTaskEditable()
        {
            ChangeTasksForEdit(IsEditable);

            IsEditable = !IsEditable;
        }

        void ChangeTasksForEdit(bool isEditable)
        {
            foreach (StackPanel panel in grTasks.Children.OfType<StackPanel>().ToList())
            {
                foreach (TextBox textBox in panel.Children.OfType<TextBox>().ToList())
                {
                    ChangeEditability(textBox, isEditable);
                }
            }
            string message = IsEditable ? "Can't edit!" : "Can edit!";
            MessageBox.Show(message);
        }


        DateTime GetCurrentDate()
        {
            return DateTime.Today.Date;
        }
        
        bool IsDeadlinePass(DateTime date)
        {
            return date < GetCurrentDate();
        }


        bool IsCheckBoxChecked(CheckBox checkBox)
        {
            return (bool)checkBox.IsChecked;
        }

        void ChangeForegroundColour(TextBox textBox, Brush brush)
        {
            textBox.Foreground = brush;
        }







        void UpdateNewTaskView(string name, string description, string data)
        {
            RowDefinition row = new RowDefinition() { MaxHeight = 70 };
            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal };
            CheckBox checkBox = new CheckBox() { Width = 100, HorizontalAlignment = HorizontalAlignment.Left };
            checkBox.Checked += checkBox_Checked;
            checkBox.Unchecked += checkBox_Unchecked;
            grTasks.RowDefinitions.Add(row);

            Grid.SetRow(stack, GridRowIndex);

            stack.Children.Add(checkBox);
            stack.Children.Add(CreateTextBox(name, true, 250));
            stack.Children.Add(CreateTextBox(description, true, 280));
            stack.Children.Add(CreateTextBox(data, true, 100));

            grTasks.Children.Add(stack);
        }

        void CheckDeadline()
        {
            foreach (StackPanel panel in grTasks.Children.OfType<StackPanel>().ToList())
            {
                TextBox tbdate = (TextBox)panel.Children[3];
                CheckBox checkBox = (CheckBox)panel.Children[0];
                string sdate = tbdate.Text;
                DateTime date = DateTime.Parse(sdate);

                if (IsDeadlinePass(date) && !IsCheckBoxChecked(checkBox))
                    ChangeForegroundColour(tbdate, Brushes.Red);
                else
                    ChangeForegroundColour(tbdate, Brushes.Black);

            }
        }

        void ChangeEditability(TextBox textBox, bool isEditable)
        {
            textBox.IsReadOnly = isEditable;
        }

        TextBox CreateTextBox(string text, bool isReadOnly, int width)
        {
            return new TextBox() { Text = text, IsReadOnly = isReadOnly, Width = width };
        }

        void ClearInput()
        {
            tbTaskName.Text = "";
            tbTaskDes.Text = "";
            dpTaskDate.Text = "";
        }


        void ColorComplitedTask(object checkBox)
        {
            CheckBox cbComplited = (CheckBox)checkBox;
            StackPanel spComlited = (StackPanel)cbComplited.Parent;

            if((bool)cbComplited.IsChecked)
                spComlited.Background = Brushes.LightGreen;
            else
                spComlited.Background = Brushes.White;
        }







        private void btAddTask_Click(object sender, RoutedEventArgs e)
        {
            string taskName = tbTaskName.Text;
            string taskDes = tbTaskDes.Text;
            string taskDate = dpTaskDate.ToString().Trim();

            if(taskName == "" || taskDes == "" || taskDate == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                taskDate = taskDate.Substring(0, 10);
                CreateNewTask(taskName, taskDes, taskDate);
            }
        }

        private void btEditTasks_Click(object sender, RoutedEventArgs e)
        {
            MakeTaskEditable();
            CheckDeadline();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckDeadline();
            ColorComplitedTask(sender);
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckDeadline();
            ColorComplitedTask(sender);
        }
    }
}
