using BlazorHomeSite.Data;
using BlazorHomeSite.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nextended.Core.DeepClone;

namespace BlazorHomeSite.Shared;

public partial class Navigation
{
    [Inject] private IDbContextFactory<HomeSiteDbContext>? DbFactory { get; set; }
    [Inject] private IOptions<AppAdminOptions>? Options { get; set; }
    [Inject] private UserManager<IdentityUser> UserManager { get; set; } = null!;
    [Inject] private RoleManager<IdentityRole> RoleManager { get; set; } = null!;
    [Inject] private SignInManager<IdentityUser> SignInManager { get; set; } = null!;

    private static string GetAlbumRoute(int id)
    {
        return $"/photoAlbum/{id}";
    }

    private async Task InitAdmin()
    {
        var users = await UserManager.GetUsersInRoleAsync("Admin");

        if (users.Count == 0)
        {
            var adminEmail = Options.Value.FromEmailAddress;
            await RoleManager.CreateAsync(new IdentityRole("Admin"));
            var userWithAdmin = await UserManager.FindByEmailAsync(adminEmail);
            if (userWithAdmin != null)
            {
                await UserManager.AddToRoleAsync(userWithAdmin, "Admin");
            }
        }
    }

    private string? GetThumbnailForAlbum(int id)
    {
        if (DbFactory != null)
        {
            using var context = DbFactory.CreateDbContext();
            var cover = context.Photos.FirstOrDefault(x => x.AlbumId == id && x.IsAlbumCover);
            return cover?.ThumbnailPath;
        }

        return string.Empty;
    }

    private List<PhotoAlbum> GetAllAlbums()
    {
        if (DbFactory == null) return new List<PhotoAlbum>();

        using var context = DbFactory.CreateDbContext();
        var albums = context.PhotoAlbums.Where(x => x.Description != "All The Rest").ToList();
        return albums;
    }
}