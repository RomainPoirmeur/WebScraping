﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Config;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker.BgService
{
    public class WebScrapingService : BackgroundService
    {
        private readonly IScrapingService _scrapingService;
        private readonly AppConfig _appConfig;
        private readonly ILogger<WebScrapingService> _logger;
        
        public WebScrapingService(IScrapingService scrapingService, AppConfig appConfig, ILogger<WebScrapingService> logger)
        {
            _scrapingService = scrapingService;
            _appConfig = appConfig;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try
                {
                    var product = new Product
                    {
                        IdProduct = "ABCDEF",
                        ProductName = "Smartphone"
                    };
                    await _scrapingService.AddNewProduct(product);
                    
                    _logger.LogInformation(
                        $"Success running background service {typeof(WebScrapingService).FullName} at {DateTime.UtcNow}");
                }
                catch (Exception e)
                {

                    
                    _logger.LogError(
                        $"Failed running background service {typeof(WebScrapingService).FullName} at {DateTime.UtcNow} : {e.Message}");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(_appConfig.BackgroundServiceCycleInSecond), stoppingToken);
                }
        }
    }
}