using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core;
using Backend.Data;
using Backend.Entities;
using Backend.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {

        #region Fields

        private readonly DataContext _context;

        #endregion

        #region Constructor

        public CommentController(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(type: typeof(Result<IList<Comment>>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<IList<Comment>>> GetComments()
        {
            var result = new FluentResults.Result<IList<Comment>>();

            try
            {
                var list = await _context.Comments
                    .Where(c => c.Level == 0)
                    .OrderByDescending(c => c.RegisterDateTime)
                    .Include(c => c.Children.OrderByDescending(c => c.RegisterDateTime))
                    .ThenInclude(c => c.Children.OrderByDescending(c => c.RegisterDateTime))
                    .ThenInclude(c => c.Children.OrderByDescending(c => c.RegisterDateTime))
                    .ToListAsync();
                var returnList = BuildThree(list).ToList();
                result.WithValue(value: returnList);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }

            return result.ConvertToDtxResult();
        }

        [AllowAnonymous]
        [HttpGet("count")]
        [ProducesResponseType(type: typeof(Result<int>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<int>> GetCount()
        {
            var result = new FluentResults.Result<int>();

            try
            {
                var returnList = await _context.Comments.CountAsync();
                result.WithValue(value: returnList);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
            }

            return result.ConvertToDtxResult();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(type: typeof(Result<int>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(type: typeof(Result), statusCode: StatusCodes.Status400BadRequest)]
        public async Task<Result<int>> Register([FromBody] CommentViewModel comment)
        {
            var result = new FluentResults.Result<int>();
            try
            {
                if (string.IsNullOrWhiteSpace(comment.Text))
                {
                    ModelState.AddModelError("Text", "Text is Empty!");
                }
                if (string.IsNullOrWhiteSpace(comment.Username))
                {
                    ModelState.AddModelError("Username", "Username is Empty!");
                }
                else if (comment.Username.Length > 254)
                {
                    ModelState.AddModelError("Username", "Username is greater than 254 characters!");
                }
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        result.WithError(error.ErrorMessage);
                    }

                    return result.ConvertToDtxResult();
                }

                var newComment = new Comment
                {
                    Username = comment.Username,
                    Text = comment.Text,
                    Level = (comment.ParentId != null) ? comment.Level + 1 : 0,
                    ParentId = comment.ParentId,
                    RegisterDateTime = DateTime.Now
                };

                await _context.Comments.AddAsync(newComment);
                await _context.SaveChangesAsync();

                result.WithSuccess(successMessage: "The comment was saved successfully.");
                result.WithValue(value: newComment.Id);
            }
            catch (Exception ex)
            {
                result.WithError(errorMessage: ex.Message);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    result.WithError(ex.InnerException.Message);
                }
            }

            return result.ConvertToDtxResult();
        }

        #endregion

        #region Private Methods

        private IEnumerable<Comment> BuildThree(IEnumerable<Comment> comments, int? parentId = null)
        {
            if (comments == null)
                return null;
            var result = comments.Select(c => new Comment()
            {
                Id = c.Id,
                Text = c.Text,
                ParentId = parentId,
                Username = c.Username,
                RegisterDateTime = c.RegisterDateTime,
                Level = c.Level,
                Children = BuildThree(c.Children, c.Id)
            });
            return result;
        }

        #endregion
    }
}
