using BicyclesCecoApp.Models;
using BicyclesCecoApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BicyclesCecoApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateOrDeleteEmployee : ContentPage
	{
        EmployeeViewModel employeeViewModel;
        public UpdateOrDeleteEmployee ()
		{
			InitializeComponent ();
		}

        public UpdateOrDeleteEmployee(Employee post)
        {
            InitializeComponent();
            employeeViewModel = new EmployeeViewModel();
            employeeViewModel.Employee = post;
            this.BindingContext = employeeViewModel;
        }
    }
}