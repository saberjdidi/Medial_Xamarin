﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApplication.ViewModels;

namespace XamarinApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttachmentsPage : ContentPage
    {
        public AttachmentsPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            (this.BindingContext as AttachmentsViewModel).GetAttachments();
        }
    }
}