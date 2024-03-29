﻿using System.Threading.Tasks;
using WebScrapingData.Model;

namespace WebScrapingWorker.Service.Interfaces
{
    public interface IScrapingService
    {
        Task GetProductsDataFromAmazonWebPage();
    }
}