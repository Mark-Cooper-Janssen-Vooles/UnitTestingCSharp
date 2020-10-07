using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class VideoServiceTests
    {
        private VideoService _videoService;
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepository;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new Mock<IFileReader>();
            _videoRepository = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _videoRepository.Object);
        }
        
        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError()
        {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();
            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_WhenCalledWithMultipleUnprocessedVideos_ReturnsCsvOfTitles()
        {
            _videoRepository.Setup(vp => vp.GetUnprocessedVideos())
                .Returns(new List<Video>()
                {
                    new Video() { Id = 1},
                    new Video() { Id = 2}
                });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2"));
        }
        
        [Test]
        public void GetUnprocessedVideosAsCsv_WhenCalledWithOneUnprocessedVideo_ReturnsCsvOfTitles()
        {
            _videoRepository.Setup(vp => vp.GetUnprocessedVideos())
                .Returns(new List<Video>()
                {
                    new Video() { Id = 1},
                });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1"));
        }
        
        [Test]
        public void GetUnprocessedVideosAsCsv_WhenCalledWithZeroUnprocessedVideos_ReturnsCsvOfTitles()
        {
            _videoRepository.Setup(vp => vp.GetUnprocessedVideos())
                .Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }
        
        
    }
}