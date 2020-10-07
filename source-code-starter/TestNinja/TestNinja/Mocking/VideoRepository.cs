using System.Collections.Generic;
using System.Linq;

namespace TestNinja.Mocking
{
    public interface IVideoRepository
    {
        List<Video> GetUnprocessedVideos();
    }

    public class VideoRepository : IVideoRepository
    {
        public List<Video> GetUnprocessedVideos()
        {
            using (var context = new VideoContext())
            {
                return (from video in context.Videos
                    where !video.IsProcessed
                    select video).ToList();
            }
        }
    }
}