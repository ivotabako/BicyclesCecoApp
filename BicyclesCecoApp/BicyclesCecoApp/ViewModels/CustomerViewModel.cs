﻿using BicyclesCecoApp.Models;
using BicyclesCecoApp.Views;
using Realms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace BicyclesCecoApp.ViewModels
{
    class CustomerViewModel : INotifyPropertyChanged
    {
        Realm _getRealmInstance = Realm.GetInstance();

        public CustomerViewModel()
        {
            // supply the public ListOfCustomerDetails with the retrieved list of details
            ListOfCustomerDetails = _getRealmInstance.All<CustomerDetails>().ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<CustomerDetails> _listOfCustomerDetails;

        public List<CustomerDetails> ListOfCustomerDetails
        {
            get { return _listOfCustomerDetails; }
            set
            {
                _listOfCustomerDetails = value;
                OnPropertyChanged(); // Added the OnPropertyChanged Method
            }
        }

        private CustomerDetails _customerDetails = new CustomerDetails();

        public CustomerDetails CustomerDetails
        {
            get { return _customerDetails; }
            set
            {
                _customerDetails = value;
                OnPropertyChanged(); // Add the OnPropertyChanged();
            }
        }

        // Add this inside your view model class
        public Command CreateCommand // for ADD
        {
            get
            {
                return new Command(() => {
                    // for auto increment the id upon adding
                    _customerDetails.CustomerId = _getRealmInstance.All<CustomerDetails>().Count() + 1;
                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(_customerDetails); // Add the whole set of details
                    });
                });
            }
        }

        public Command UpdateCommand // For UPDATE
        {
            get
            {
                return new Command(() =>
                {
                    // instantiate to supply the new set of details
                    var customerDetailsUpdate = new CustomerDetails
                    {
                        CustomerId = _customerDetails.CustomerId,
                        CustomerName = _customerDetails.CustomerName,
                        CustomerAge = _customerDetails.CustomerAge
                    };

                    _getRealmInstance.Write(() =>
                    {
                        // when there's id match, the details will be replaced except by primary key
                        _getRealmInstance.Add(customerDetailsUpdate, update: true);
                    });
                });
            }
        }

        public Command RemoveCommand
        {
            get
            {
                return new Command(() =>
                {
                    // get the details with specific id
                    var getAllCustomerDetailsById = _getRealmInstance.All<CustomerDetails>().First(x => x.CustomerId == _customerDetails.CustomerId);

                    using (var transaction = _getRealmInstance.BeginWrite())
                    {
                        // remove all details
                        _getRealmInstance.Remove(getAllCustomerDetailsById);
                        transaction.Commit();
                    };
                });
            }
        }

        // For Navigation Page
        public Command NavToListCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new ListOfCustomers());
                });
            }
        }

        public Command NavToCreateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CreateCustomer());
                });
            }
        }

        public Command NavToUpdateDeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new UpdateOrDeleteCustomer());
                });
            }
        }
    }
}
