using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class PostService
{
    private readonly ApplicationDbContext _context;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }

   
    public async Task<List<Post>> GetAllIPostsAsync()
    {
        return await _context.Posts.Include(p => p.Category).ToListAsync();
    }

  
    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await _context.Posts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    }

    // Create a new Post (without saving changes)
    public async Task<Post> AddPost(PostEditDto model)
    {
        var categoryRef = await _context.Categories.Include(c => c.Posts).FirstOrDefaultAsync(c => c.Id == model.CategoryId);

        if (categoryRef == null)
        {
            throw new Exception("Category not found");
        }

        var idea = new Post
        {
            Name = model.Name,
            Description = model.Description,
            Details = model.Details,
            CategoryId = model.CategoryId,
        };

        _context.Posts.Add(idea);
        return idea;
    }

    public Post UpdatePost(PostEditDto post)
    {
        var existingPost = _context.Posts.Find(post.Id);
        if (existingPost == null)
        {
            throw new Exception("Post not found");
        }

        existingPost.Name = post.Name;
        existingPost.Description = post.Description;
        existingPost.Details = post.Details;
        existingPost.CategoryId = post.CategoryId;

        return existingPost;
    }

    public bool DeletePost(int id)
    {
        var post = _context.Posts.Find(id);
        if (post == null)
        {
            throw new Exception("Post not found");
        }

        _context.Posts.Remove(post);
        return true;
    }
}
