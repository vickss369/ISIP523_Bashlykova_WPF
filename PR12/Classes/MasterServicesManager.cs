using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR12.Classes
{
    public static class MasterServicesManager
    {
        private static List<int> selectedServiceIDs = new List<int>();

        public static List<Service> GetMyServices()
        {
            if (selectedServiceIDs.Count == 0) return new List<Service>();

            return Core.Context.Service.Where(s => selectedServiceIDs.Contains(s.ID)).ToList();
        }

        public static List<Service> GetAvailableServices()
        {
            if (User.currentUser == null) return new List<Service>();

            var masterTypeLink = Core.Context.MasterServiceType.FirstOrDefault(m => m.MasterID == User.currentUser.ID);
            if (masterTypeLink == null) return new List<Service>();

            return Core.Context.Service.Where(s => s.ServiceTypeID == masterTypeLink.ServiceTypeID).ToList();
        }

        public static void ToggleService(int serviceID, bool add)
        {
            if (add)
            {
                if (!selectedServiceIDs.Contains(serviceID)) selectedServiceIDs.Add(serviceID);
            }
            else
            {
                selectedServiceIDs.Remove(serviceID);
            }
        }

        public static void SetSelectedServices(List<int> newList)
        {
            selectedServiceIDs.Clear();
            if (newList != null) selectedServiceIDs.AddRange(newList);
        }
    }
}
