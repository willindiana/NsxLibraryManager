﻿using Microsoft.AspNetCore.Components;
using NsxLibraryManager.Services;
using Radzen;

namespace NsxLibraryManager.Pages.Components;
#nullable disable

public partial class RefreshTitleDbProgressDialog : IDisposable
{
    public string DownloadingInfo { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }
    [Inject]
    protected ITitleDbService TitleDbService { get; set; }
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await DoWork();
        }
    }
    
    private async Task DoWork()
    {
        await InvokeAsync(
                async () =>
                {
                    var regions = TitleDbService.GetRegionsToImport();
                    foreach (var region in regions)
                    {
                        DownloadingInfo = $"region {region}";
                        StateHasChanged();
                        await TitleDbService.ImportRegionAsync(region);
                    }
                    DownloadingInfo = "cnmts";
                    StateHasChanged();
                    await TitleDbService.ImportCnmtsAsync();
                });
        DialogService.Close();
    }
    
    public void Dispose()
    {
        
    }
}