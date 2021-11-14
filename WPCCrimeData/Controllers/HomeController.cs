using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WPCCrimeData.Interfaces.Services;
using WPCCrimeData.ViewModels;

namespace WPCCrimeData.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICrimeDataService _service;

        public HomeController(ICrimeDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [ActionName("Index")]
        public async  Task<IActionResult> IndexAsync()
        {
            var lastUpdated = await _service.GetLastUpdatedAsync();
            var dates = await _service.GetMonthListAsync();

            var model = new HomeViewModel()
            {
                LastUpdated = lastUpdated.Date,
                DateList = dates
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync(string lat, string lng, DateTime mnt)
        {
            var errorList = new List<string>();

            if (!Decimal.TryParse(lat, out decimal latVal)) errorList.Add("Latitude is not valid");
            if (!Decimal.TryParse(lng, out decimal lngVal)) errorList.Add("Longitude is not valid");

            HomeViewModel model;

            if (errorList.Count > 0)
            {
                var lastUpdated = await _service.GetLastUpdatedAsync();
                var dates = await _service.GetMonthListAsync();

                model = new HomeViewModel()
                {
                    Errors = errorList,
                    DateList = dates,
                    LastUpdated = lastUpdated.Date,
                    ShowSplash = false
                };
            }
            else
            {
                var lastUpdated = await _service.GetLastUpdatedAsync();
                var crimes = await _service.GetCrimesAsync(latVal, lngVal, mnt);
                var dates = await _service.GetMonthListAsync();

                model = new HomeViewModel()
                {
                    Crimes = crimes,
                    LastUpdated = lastUpdated.Date,
                    DateList = dates,
                    ShowSplash = false
                };
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
