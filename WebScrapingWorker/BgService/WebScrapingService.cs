﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebScrapingWorker.Config;
using WebScrapingWorker.Hubs;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker.BgService
{
    public class WebScrapingService : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<WebScrapingService> _logger;
        private readonly IScrapingService _scrapingService;
        private readonly IHubContext<NotifHub> _notifHub;

        public WebScrapingService(IScrapingService scrapingService, AppConfig appConfig,
            ILogger<WebScrapingService> logger, IHubContext<NotifHub> notifHub)
        {
            _scrapingService = scrapingService;
            _appConfig = appConfig;
            _logger = logger;
            _notifHub = notifHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Test signal R
            //await _notifHub.Clients.All.SendCoreAsync("NewReviews", new object[]{"Init","123", DateTime.UtcNow}, stoppingToken);
            //await _notifHub.Clients.All.SendAsync("NewReviews", "Init2","1234", DateTime.UtcNow,stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
                try
                {
                    await _scrapingService.GetProductsDataFromAmazonWebPage();
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