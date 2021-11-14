using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPCCrimeData.Models;

namespace WPCCrimeData.Interfaces.Services
{
    public interface ICrimeDataService
    {
        Task<List<Crime>> GetCrimesAsync(decimal lat, decimal lng, DateTime date);
        Task<List<CrimeCategory>> GetCrimeCategoriesAsync(DateTime date);
        Task<LastUpdated> GetLastUpdatedAsync();
        Task<List<MonthData>> GetMonthListAsync();
    }
}
