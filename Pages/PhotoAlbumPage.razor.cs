using BlazorHomeSite.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorHomeSite.Pages;

public partial class PhotoAlbumPage
{
    private const int PhotosPerPage = 50;
    protected string albumDescription = string.Empty;
    protected List<Photo>? allPhotos;

    protected int currentPage = 1;
    protected int numberOfPages;
    protected List<Photo>? pagePhotos;

    [Parameter] public string? AlbumId { get; set; }

    public string GetPhotoNavigaitonParams(int photoId)
    {
        return $"photo/{int.Parse(AlbumId ?? "-1")}/{photoId}";
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        allPhotos = await GetAllPhotosAsync();
        numberOfPages = (int)double.Round(allPhotos.Count / PhotosPerPage, 0);
        StateHasChanged();
    }

    private async Task<List<Photo>> GetAllPhotosAsync()
    {
        await using var context = await DbFactory.CreateDbContextAsync();

        if (context != null && context.PhotoAlbums != null && int.TryParse(AlbumId, out var parsedAlbumId))
        {
            var album = context.PhotoAlbums.FirstOrDefault(x => x.Id == parsedAlbumId);

            if (album != null) albumDescription = album.Description ?? "";

            return await context.Photos.Where(x => x.Album != null && x.Album.Id == parsedAlbumId)
                .OrderBy(x => x.CaptureTime).ToListAsync();
        }

        return new List<Photo>();
    }
}