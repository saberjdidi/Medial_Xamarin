using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.Models;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskStatusPage : ContentPage
    {
        public TaskStatusPage()
        {
            InitializeComponent();
            BindingContext = new TaskStatusViewModel();
        }
        private async void Add_TaskStatus(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewTaskStatusPage());
        }
        private async void Update_TaskStatus(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var taskStatus = mi.CommandParameter as TaskStatuss;
            await PopupNavigation.Instance.PushAsync(new UpdateTaskStatusPage(taskStatus));
        }
    }
}