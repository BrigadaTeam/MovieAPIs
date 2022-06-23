using NUnit.Framework;
using MovieAPIs.Utils;
using System.Collections.Generic;

namespace MovieAPIsTest
{
    public class UrlHelperTests
    {
        [Test]
        public void GetPathWithNullPathSegmentTest()
        {
            string path = UrlHelper.GetPath(null);

            Assert.IsEmpty(path);
        }

        [Test]
        public void GetPathWithEmptyPathSegmentTest()
        {
            string path = UrlHelper.GetPath();

            Assert.IsEmpty(path);
        }

        [Test]
        public void GetPathWithOnePathSegmentTest()
        {
            string pathSegment = "path segment";

            string path = UrlHelper.GetPath(pathSegment);

            Assert.AreEqual(pathSegment, path);
        }

        [Test]
        public void GetPathWithMultipleSegmentsTest()
        {
            var pathSegments = new string[] { "home", "room", "bed" };
            string expectedPath = "home/room/bed";

            string path = UrlHelper.GetPath(pathSegments);

            Assert.AreEqual(expectedPath, path);

            path = UrlHelper.GetPath(pathSegments[0], pathSegments[1], pathSegments[2]);

            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void GetQueryWithNullQueryTest()
        {
            string query = UrlHelper.GetQuery(null);

            Assert.IsEmpty(query);
        }

        [Test]
        public void GetQueryWithQueryParamsTest()
        {
            var queryParams = new Dictionary<string, string>
            {
                ["type"] = "typeValue",
                ["page"] = "pageValue",
                ["count"] = "countRate"
            };
            string expectedQuery = "type=typeValue&page=pageValue&count=countRate";

            string query = UrlHelper.GetQuery(queryParams);

            Assert.AreEqual(expectedQuery, query);
        }

        [Test]
        public void GetQueryWithOneQueryParamTest()
        {
            var queryParams = new Dictionary<string, string>
            {
                ["one"] = "oneValue"
            };
            string expectedQuery = "one=oneValue";

            string query = UrlHelper.GetQuery(queryParams);

            Assert.AreEqual(expectedQuery, query);
        }

        [Test]
        public void GetPathWithoutQueryTest()
        {
            var pathSegments = new string[] { "home", "room", "bed" };
            string expectedPath = "home/room/bed";

            string pathWithoutQuery = UrlHelper.GetPathWithQuery(null, pathSegments);

            Assert.AreEqual(expectedPath, pathWithoutQuery);
        }

        [Test]
        public void GetQueryWithoutPathTest()
        {
            var queryParams = new Dictionary<string, string>
            {
                ["one"] = "oneValue"
            };
            string expectedQuery = "one=oneValue";

            string queryWithoutPath = UrlHelper.GetPathWithQuery(queryParams);

            Assert.AreEqual(expectedQuery, queryWithoutPath);
        }

        [Test]
        public void WithoutPathAndQueryTest()
        {
            string emptyString = UrlHelper.GetPathWithQuery(null);

            Assert.IsEmpty(emptyString);
        }

        [Test]
        public void GetPathWithQueryTest()
        {
            var pathSegments = new string[] { "home", "room", "bed" };
            var queryParams = new Dictionary<string, string>
            {
                ["one"] = "oneValue",
                ["two"] = "twoValu"
            };
            string expectedPathWithQuery = "home/room/bed?one=oneValue&two=twoValue";

            string pathWithQuery = UrlHelper.GetPathWithQuery(queryParams, pathSegments);

            Assert.AreEqual(expectedPathWithQuery, pathWithQuery);
        }
    }
}