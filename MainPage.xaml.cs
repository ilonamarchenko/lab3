using Microsoft.Maui.Controls;
using System.Text.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;



namespace lab3
{
    public partial class MainPage : ContentPage
    {
        private FileManager fileManager;
        private Schelude fileObject;
        public ObservableCollection<Lesson> LessonList { get; set; }

        private bool mainPageLocker = false;

        public MainPage()
        {
            InitializeComponent();

            fileManager = new FileManager();
            fileObject = Schelude.GetInstance();

            UpdateArticlesView();
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            if (mainPageLocker)
            {
                fileManager.SaveFile();
            }
        }

        private void OpenWindow(Page view)
        {
            Window editWindow = new Window(view);
            Application.Current.OpenWindow(editWindow);
        }

        private void EditClicked(object sender, EventArgs e)
        {
            UpdateArticlesView();
            if (mainPageLocker)
            {
                OpenWindow(new EditView());
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Error", "Cant edit empty file", "OK");
            }
        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            if (mainPageLocker)
            {
                fileObject.DeleteLesson();
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Error", "No elements left", "OK");
            }
        }

        private void UpdateArticlesView()
        {
            this.BindingContext = fileObject.Data;
            if (fileObject.Data.Count > 0)
            {
                OnPropertyChanged(nameof(fileObject.Data));
                mainPageLocker = true;
            }
            else
            {
                mainPageLocker = false;
            }
        }

        private void AddClicked(object sender, EventArgs e)
        {
            if (mainPageLocker || this.fileObject.IsOpened())
            {
                OpenWindow(new AddView());
                UpdateArticlesView();
            }
            else
            {
                DisplayAlert("Error", "Open file first", "OK");
            }
        }

        private void AboutClicked(object sender, EventArgs e)
        {
            OpenWindow(new AboutView());
        }

        private void OpenJsonFileClicked(object sender, EventArgs e)
        {
            try
            {
                if (fileManager.OpenFile("C:\\Users\\Home\\Downloads\\Илона\\lab3\\lab3\\json.json"))
                {
                    DisplayAlert("Success", "File opened successfully", "OK");
                    UpdateArticlesView();
                    mainPageLocker = true;
                }
                else
                {
                    DisplayAlert("Error", "Error reading file. Choose another one", "OK");
                    mainPageLocker = false;
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", "Cannot open file: " + ex.Message, "OK");
            }
        }

        private void SearchBackClicked(object sender, EventArgs e)
        {
            fileObject.UpdateDataToBuffer();
            UpdateArticlesView();
        }

        private void SearchClicked(object sender, EventArgs e)
        {

            this.BindingContext = fileObject.Search(searchPicker.SelectedItem?.ToString(), searchEntry.Text);
        }

    }
}