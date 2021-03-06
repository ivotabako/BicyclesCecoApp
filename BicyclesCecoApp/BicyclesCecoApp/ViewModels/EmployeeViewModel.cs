﻿using BicyclesCecoApp.Models;
using BicyclesCecoApp.Views;
using Realms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace BicyclesCecoApp.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private List<Employee> _listOfEmployees;
        private Employee _employee = new Employee();
        private Realm _getRealmInstance = Realm.GetInstance();

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeeViewModel()
        {
            // supply the public ListOfEmployee with the retrieved list of details
            ListOfEmployees = _getRealmInstance.All<Employee>().ToList();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Employee> ListOfEmployees
        {
            get { return _listOfEmployees; }
            set
            {
                _listOfEmployees = value;
                OnPropertyChanged(); // Added the OnPropertyChanged Method
            }
        }

        public Employee Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
                OnPropertyChanged(); // Add the OnPropertyChanged();
            }
        }

        // Add this inside your view model class
        public Command CreateCommand // for ADD
        {
            get
            {
                return new Command(() =>
                {
                    // for auto increment the id upon adding
                    _employee.ID = _getRealmInstance.All<Employee>().Count() + 1;
                    _getRealmInstance.Write(() =>
                    {
                        _getRealmInstance.Add(_employee); // Add the whole set of details
                    });
                    App.Current.MainPage.Navigation.PushAsync(new ListOfEmployees());
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
                    var EmployeeUpdate = new Employee
                    {
                        ID = _employee.ID,
                        BicycleId = _employee.BicycleId,
                        Deposit = _employee.Deposit,
                        FirstName = _employee.FirstName,
                        LastName = _employee.LastName,
                        LockerCode = _employee.LockerCode,
                        CardNumber = _employee.CardNumber,
                        LockUnlockMessage = _employee.LockUnlockMessage,
                        HasReceivedConfirmation = _employee.HasReceivedConfirmation,
                        IsLocked = _employee.IsLocked,
                        IsInUse = _employee.IsInUse,
                        Manual = _employee.Manual,
                        MessageId = _employee.MessageId,
                        PaymentLastWeek = _employee.PaymentLastWeek,
                        PaymentThisWeek = _employee.PaymentThisWeek,
                        PaymentThreeWeeksAgo = _employee.PaymentThreeWeeksAgo,
                        PaymentTwoWeeksAgo = _employee.PaymentTwoWeeksAgo,
                        Shift = _employee.Shift
                    };

                    _getRealmInstance.Write(() =>
                    {
                        // when there's id match, the details will be replaced except by primary key
                        _getRealmInstance.Add(EmployeeUpdate, update: true);
                    });
                    App.Current.MainPage.Navigation.PushAsync(new ListOfEmployees());
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
                    var getAllEmployeeById = _getRealmInstance.All<Employee>().First(x => x.ID == _employee.ID);

                    using (var transaction = _getRealmInstance.BeginWrite())
                    {
                        // remove all details
                        _getRealmInstance.Remove(getAllEmployeeById);
                        transaction.Commit();
                    };
                    App.Current.MainPage.Navigation.PushAsync(new ListOfEmployees());
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
                    await App.Current.MainPage.Navigation.PushAsync(new ListOfEmployees());
                });
            }
        }

        public Command NavToCreateCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new CreateEmployee());
                });
            }
        }

        public Command NavToUpdateDeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await App.Current.MainPage.Navigation.PushAsync(new UpdateOrDeleteEmployee());
                });
            }
        }

        public Command ForceLockUnlockCommand
        {
            get
            {
                return new Command(() =>
                {
                    _getRealmInstance.Write(() =>
                    {
                        _employee.Manual = true;
                        _getRealmInstance.Add(_employee, update: true);
                    });

                    if(!_employee.HasReceivedConfirmation.HasValue ||
                        (_employee.HasReceivedConfirmation.HasValue && _employee.HasReceivedConfirmation.Value)
                    )
                    {
                        _getRealmInstance.Write(() =>
                        {
                            var mng = new SmsManager();
                            mng.Send(_employee);
                            _getRealmInstance.Add(_employee, update: true);
                        });
                    }                   
                });
            }
        }
    }
}
