using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace ToDoList
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow:Window
	{
		ObservableCollection<Task> tasks = new();

		public MainWindow()
		{
			InitializeComponent();	
			
			ToDoListBox.DisplayMemberPath = "Name";
		}

		private void ToDoListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (ToDoListBox.SelectedItem is Task selected)
			{
				MessageBox.Show(selected.Description);
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			NewTaskWindow window = new();
			window.Owner = this;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			if (window.ShowDialog() == true)
			{
				Task newTask = window.Result;
				tasks.Add(newTask);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			int index = ToDoListBox.SelectedIndex;
			if (index != -1)
				tasks.RemoveAt(index);
		}

		private void CompleteButton_Click(object sender, RoutedEventArgs e)
		{
			int index = ToDoListBox.SelectedIndex;
			if (index != -1)
				tasks[index].IsCompleted = true;
		}

		private void AllRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			CompleteButton.IsEnabled = true;			

			ToDoListBox.ItemsSource = tasks;
		}


		private void CompletedRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			CompleteButton.IsEnabled = false;

			ObservableCollection<Task> filtered = new(tasks.Where(x=>x.IsCompleted));
			
			ToDoListBox.ItemsSource = filtered;
		}

		private void NotCompletedRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			CompleteButton.IsEnabled = true;

			ObservableCollection<Task> filtered = new(tasks.Where(x => !x.IsCompleted));

		ToDoListBox.ItemsSource = filtered;
		}

		string fileName = "data.bin";
		private void Window_Closed(object sender, EventArgs e)
		{
			#pragma warning disable SYSLIB0011
			BinaryFormatter formatter = new();
			Stream file = File.OpenWrite(fileName);
			formatter.Serialize(file, tasks);
			file.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (File.Exists(fileName))
			{
				BinaryFormatter formatter = new();
				Stream file = File.OpenRead(fileName);
				tasks = (ObservableCollection<Task>) formatter.Deserialize(file);	
				file.Close();
				ToDoListBox.ItemsSource = tasks;
			}
		}
	}
}