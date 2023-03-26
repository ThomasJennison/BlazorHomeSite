﻿using BlazorHomeSite.Data.Domain;

namespace BlazorHomeSite.Services.SiteSettings;

public interface ISiteSettingsService
{
    public const int SiteOwnerAlbumId = 999999;

    SiteOwner GetSiteOwner();

    Task<SiteOwner> GetSiteOwnerAsync();

    void UpdateOrCreateSiteOwner(SiteOwner siteOwner);
}