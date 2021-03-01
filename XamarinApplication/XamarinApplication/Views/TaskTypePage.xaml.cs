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
    public partial class TaskTypePage : ContentPage
    {
        public TaskTypePage()
        {
            InitializeComponent();
            BindingContext = new TaskTypeViewModel();
        }
        private async void Add_TaskType(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new NewTaskTypePage());
        }
        private async void Update_TaskType(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var taskType = mi.CommandParameter as TaskType;
            await PopupNavigation.Instance.PushAsync(new UpdateTaskTypePage(taskType));
        }
    }
}