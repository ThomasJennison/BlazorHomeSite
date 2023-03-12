﻿using BlazorHomeSite.Data;
using BlazorHomeSite.Data.Domain;
using BlazorHomeSite.Data.Interfaces;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Schema
{
    public class GivenAlbum_WhenAddedToSetWithPhotos_ThenCanReadBack : SqliteInMemoryDbTestBase
    {
        [Fact]
        public void Test()
        {
            using var _homeSiteDbContext = GetContext();

            var name = "coolest album evaaarrrr";
            var albumDescription = "Cool Album";
            var userLevel = UserLevel.Max;
            var photoAlbum = new PhotoAlbum(name, albumDescription, userLevel);
            _homeSiteDbContext.PhotoAlbums.Add(photoAlbum);
            _homeSiteDbContext.SaveChanges();

            var photos = new List<Photo>()
            {
                new("a/b/c.webp", "c/g/a.webp", DateTime.Now, photoAlbum.Id),
                new("g/z/a.webp", "c/z/z.webp", DateTime.Now.AddDays(-1), photoAlbum.Id)
            };

            _homeSiteDbContext.Photos.AddRange(photos);
            _homeSiteDbContext.SaveChanges();

            var album = _homeSiteDbContext
                                .PhotoAlbums
                                .Include(x => x.Photos)
                                .AsSplitQuery()
                                .FirstOrDefault(x => x.Id == photoAlbum.Id);

            Assert.NotNull(album);
            album.Description.Should().Be(albumDescription);
            album.UserLevel.Should().Be(userLevel);

            album.Photos.Should().NotBeNull();
            album.Photos.Should().HaveCount(2);
            album.Photos.Should().BeEquivalentTo(photoAlbum.Photos);
        }
    }
}