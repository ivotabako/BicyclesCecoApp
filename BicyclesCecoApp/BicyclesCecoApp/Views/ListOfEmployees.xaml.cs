using BicyclesCecoApp.Models;
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
	public partial class ListOfEmployees : ContentPage
	{
		public ListOfEmployees ()
		{
			InitializeComponent ();
		}

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var postDetailPage = new UpdateOrDeleteEmployee(e.SelectedItem as Employee);
            await Navigation.PushAsync(postDetailPage);
        }
    }
}