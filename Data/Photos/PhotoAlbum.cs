namespace BlazorHomeSite.Data;

public class PhotoAlbum
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public List<Photo>? Photos { get; set; }
}