using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WPCCrimeData.Interfaces.Services;
using WPCCrimeData.Models;

// Should we use the  same HttpClient for  each request?
// check for 404 not found, last updated date etc.

namespace WPCCrimeData.Services
{
    public class CrimeDataService : ICrimeDataService
    {
        private string CrimeDataAPI { get; init; }

        public CrimeDataService(GlobalOptions globals)
        {
            CrimeDataAPI = globals.CrimeDataAPI;
        }

        public async Task<List<Crime>> GetCrimesAsync(decimal lat, decimal lng, DateTime date)
        {
            var crimes = new List<Crime>();

            var crimeCategories = await GetCrimeCategoriesAsync(date);

            var url = $"{CrimeDataAPI}crimes-street/all-crime?lat={lat:#0.00000;-#0.00000}&lng={lng:#0.00000;-#0.00000}&date={date:yyyy-MM}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            var responseStr = await response.Content.ReadAsStringAsync();

            if (!String.IsNullOrEmpty(responseStr)) {
                crimes = JsonSerializer.Deserialize<List<Crime>>(responseStr,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                crimes.ForEach(c =>
                    c.CrimeCategory = crimeCategories.SingleOrDefault(cc =>
                        cc.URL == c.Category
                    )
                );
            }

            return crimes;
        }

        public async Task<List<CrimeCategory>> GetCrimeCategoriesAsync(DateTime date)
        {
            var url = $"{CrimeDataAPI}crime-categories?{date:yyyy-MM}";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            var responseData = await response.Content.ReadAsStringAsync();

            var crimeCategories = JsonSerializer.Deserialize<List<CrimeCategory>>(responseData,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return crimeCategories;
        }

        public async Task<LastUpdated> GetLastUpdatedAsync()
        {
            var url = $"{CrimeDataAPI}crime-last-updated";

            using var client = new HttpClient();
            using var response = await client.GetAsync(url);

            var responseData = await response.Content.ReadAsStringAsync();

            var date = JsonSerializer.Deserialize<LastUpdated>(responseData,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return date;
        }

        public async Task<List<MonthData>> GetMonthListAsync()
        {
            var monthList = new List<MonthData>();
            var updated = await this.GetLastUpdatedAsync();

            for (int i = 1; i < 13; i++)
            {
                var date = updated.Date.AddMonths(-i);
                var month = DateTimeFormatInfo.CurrentInfo.GetMonthName(date.Month);

                monthList.Add(new MonthData(month + " " + date.Year, date));
            }

            return monthList;            
        }
    }
}