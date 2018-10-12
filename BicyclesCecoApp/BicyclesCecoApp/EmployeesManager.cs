using BicyclesCecoApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BicyclesCecoApp
{
    public class EmployeesManager
    {
        public static bool ShouldLockDaily(Employee item)
        {
            if (!item.IsLocked &&
                item.IsInUse &&
                DateTime.UtcNow.Hour <= 6 && DateTime.UtcNow.Hour >= 20 &&
                item.Shift == "D")
            {
                return true;
            }

            return false;
        }

        public static bool ShouldUnlockDaily(Employee item)
        {
            if (item.IsLocked &&
                item.IsInUse &&
                DateTime.UtcNow.Hour >= 7 && DateTime.UtcNow.Hour <= 19 &&
                item.Shift == "D")
            {
                return true;
            }

            return false;
        }

        public static bool ShouldLockNightly(Employee item)
        {
            if (!item.IsLocked &&
                item.IsInUse &&
                DateTime.UtcNow.Hour >= 7 && DateTime.UtcNow.Hour <= 19 &&
                item.Shift == "N")
            {
                return true;
            }

            return false;
        }

        public static bool ShouldUnlockNightly(Employee item)
        {
            if (item.IsLocked &&
                item.IsInUse &&
                DateTime.UtcNow.Hour <= 6 && DateTime.UtcNow.Hour >= 20 &&
                item.Shift == "N")
            {
                return true;
            }

            return false;
        }

    }
}
