using System;
using AutoMapper;
using e_commerce.data;
using e_commerce.DTOs.Blogs;
using e_commerce.DTOs.Products;
using e_commerce.Interfaces;
using e_commerce.Models.Blogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static e_commerce.Enums.ProductEnums;

namespace e_commerce.Services.Blogs
{
    public class BlogService: IBlogService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public BlogService(AppDbContext appDbcontext, IMapper mapper) {
            _appDbContext = appDbcontext;
            _mapper = mapper;
        }

      public  async Task<JsonResult> CreateBlogService(BlogCreateDto blogModel)
        {
            try
            {
                BlogModel newblogModel = _mapper.Map<BlogModel>(blogModel);
                newblogModel.BlogId = Guid.NewGuid();
                newblogModel.CreatedAt = DateTime.UtcNow;
                newblogModel.ViewCount = 0;
                await _appDbContext.Blogs.AddAsync(newblogModel);
                await _appDbContext.SaveChangesAsync();

                if (!string.IsNullOrEmpty(blogModel.Message))
                {
                    BlogComment blogComment = _mapper.Map<BlogComment>(blogModel);
                    blogComment.CommentId = Guid.NewGuid();
                    blogComment.BlogId = newblogModel.BlogId;
                    blogComment.CommentedAt = DateTime.UtcNow;

                    await _appDbContext.BlogComments.AddAsync(blogComment);
                    await _appDbContext.SaveChangesAsync();
                }

                return new JsonResult(new { blogId = newblogModel.BlogId, StatusCode = 200 });
            }
            catch (Exception ex)
            {
                return new JsonResult(new {error= ex.Message, StatusCode = 500 });
            }
        }


        public async Task<PaginatedResult<BlogCreateDto>> GetBlogsService(int pageNumber, int pageSize, string search, string catg, string tag)
        {
            // Validate pageNumber and pageSize
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            string searchText = search ?? "";
            var blogsQuery = from blog in _appDbContext.Blogs
                             join comment in _appDbContext.BlogComments
                             on blog.BlogId equals comment.BlogId into blogCommentsGroup
                             from comment in blogCommentsGroup.DefaultIfEmpty()
                             select new BlogCreateDto
                             {
                                 BlogId = blog.BlogId,
                                 BlogTitle = blog.BlogTitle,
                                 Img = blog.Img,
                                 ThumbnailImg = blog.ThumbnailImg,
                                 Summary = blog.Summary,
                                 Author = blog.Author,
                                 Date = blog.Date,
                                 Categories = blog.Categories,
                                 Tags = blog.Tags,
                                 Description = blog.Description,
                                 ViewCount = blog.ViewCount,
                                 CreatedAt = blog.CreatedAt,
                                 UpdatedAt = blog.UpdatedAt,
                                 IsPublished = blog.IsPublished,
                                 MetaTitle = blog.MetaTitle,
                                 MetaDescription = blog.MetaDescription,
                                 CommenterName =  comment.CommenterName,
                                 CommenterEmail = comment.CommenterEmail,
                                 Message = comment.Message
                             };


            // Apply search filters
            if (!string.IsNullOrEmpty(searchText))
            {
                blogsQuery = blogsQuery.Where(p => p.BlogTitle.Contains(searchText) || p.Categories.Contains(searchText));
            }

            // Apply category filter
            if (!string.IsNullOrEmpty(catg))
            {
                blogsQuery = blogsQuery.Where(p => p.Categories.Any(c => EF.Functions.Like(c, "%" + catg + "%")));
            }




            // Apply tag filter
            if (!string.IsNullOrEmpty(tag))
            {
                blogsQuery = blogsQuery.Where(p => p.Tags.Any(tg => EF.Functions.Like(tg, "%" + tag + "%")));
            }

            // Get the total count of products before pagination
            var totalItems = await blogsQuery.CountAsync();

            // Calculate total pages
            var totalPage = (int)Math.Ceiling((double)totalItems / pageSize);

            // Adjust pageNumber to stay within bounds
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPage));

            // Apply pagination (skip and take)
            var skip = (pageNumber - 1) * pageSize;
            var paginatedBlogs = await blogsQuery
                .Skip(skip)
                .Take(pageSize)
                .Select(p => _mapper.Map<BlogCreateDto>(p))
                .ToListAsync();

            // Build the paginated response
            var pager = new PaginatedResult<BlogCreateDto>
            {
                Items = paginatedBlogs,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                StartPage = Math.Max(1, pageNumber - 2),
                EndPage = Math.Min(totalPage, pageNumber + 2)
            };

            return pager;
        }

    }
}
