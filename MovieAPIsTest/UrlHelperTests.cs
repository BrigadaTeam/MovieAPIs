using NUnit.Framework;
using MovieAPIs.Utils;
using System.Collections.Generic;

namespace MovieAPIsTest
{
    public class UrlHelperTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("some string")]
        public void GetUrlWithoutQueryTest(string path)
        {
            string url = UrlHelper.GetUrl(path);

            Assert.AreEqual(path, url);
        }
        [TestCaseSource()]
        public void GetQueryTest()
        {

        }

        static IEnumerable<TestCaseData> TestSource
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

    /*    [Test]
        public void GetPathWithoutQueryTest()
        {
            var pathSegments = new string[] { "home", "room", "bed" };
            string expectedPath = "home/room/bed";

            string pathWithoutQuery = UrlHelper.GetUrl(null, pathSegments);

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

            string queryWithoutPath = UrlHelper.GetUrl(queryParams);

            Assert.AreEqual(expectedQuery, queryWithoutPath);
        }*/

        [Test]
        public void WithoutPathAndQueryTest()
        {
            string path = string.Empty;
            string emptyString = UrlHelper.GetUrl(path);

            Assert.IsEmpty(emptyString);
        }

        [Test]
        public void GetPathWithQueryTest()
        {
            string path = "home/room/bed";
            var queryParams = new Dictionary<string, string>
            {
                ["one"] = "oneValue",
                ["two"] = "twoValue"
            };
            string expectedUrl = "home/room/bed?one=oneValue&two=twoValue";

            string actualUrl = UrlHelper.GetUrl(path, queryParams);

            Assert.AreEqual(expectedUrl, actualUrl);
        }
    }

    internal class TestCase
}