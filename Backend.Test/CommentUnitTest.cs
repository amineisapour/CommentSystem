using System;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Data;
using Backend.Entities;
using Backend.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Test
{
    public class CommentUnitTest
    {

        private async Task<DataContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new DataContext(options);
            await databaseContext.Database.EnsureCreatedAsync();
            if (await databaseContext.Comments.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Comments.Add(new Comment()
                    {
                        Id = i,
                        Username = $"UserTest{i}",
                        RegisterDateTime = DateTime.Now,
                        Level = 0,
                        Text = $"This is a test{i} for UserTest{i}",
                        ParentId = null
                    });
                    await databaseContext.SaveChangesAsync();
                }
            }

            return databaseContext;
        }

        [Fact]
        public async Task TestSuccessful()
        {
            var dbContext = await GetDatabaseContext();
            var ctrl = new CommentController(dbContext);

            var comments = await ctrl.GetComments();
            Assert.NotNull(comments);
            Assert.True(comments.IsSuccess);
            Assert.NotNull(comments.Value);
        }

        [Fact]
        public async Task TestFailed()
        {
            DataContext dbContext = null;
            var ctrl = new CommentController(dbContext);

            var comments = await ctrl.GetComments();
            Assert.NotNull(comments);
            Assert.True(comments.IsFailed);
            Assert.Equal(expected: 1, actual: comments.Errors.Count);
        }

        [Fact]
        public async Task TestRegisterSuccessful()
        {
            var dbContext = await GetDatabaseContext();
            var ctrl = new CommentController(dbContext);

            var result = await ctrl.Register(new CommentViewModel
            {
                Level = 0,
                Text = "This is a test",
                ParentId = 0,
                Username = "User Test"
            });
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Value > 0);
        }

        [Fact]
        public async Task TestRegisterFailed()
        {
            var dbContext = await GetDatabaseContext();
            var ctrl = new CommentController(dbContext);

            var result = await ctrl.Register(new CommentViewModel
            {
                Level = 0,
                Text = null,
                ParentId = 0,
                Username = null
            });
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Equal(expected: 2, actual: result.Errors.Count);
        }

    }
}
