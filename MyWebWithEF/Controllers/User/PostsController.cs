using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MyWebWithEF.Controllers.User.Base
{
    public class PostsController : UserApiController
    {
        private readonly PostService _postService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostsController(PostService postService, ApplicationDbContext context, IMapper mapper)
        {
            _postService = postService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts = await _postService.GetAllIPostsAsync();
            var postDtos = _mapper.Map<List<PostDto>>(posts);
            return Ok(postDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var postDto = _mapper.Map<PostDto>(post);
            return Ok(postDto);
        }
    }
}
