using NUnit.Framework;
using System.Collections.Generic;
using System;
using MovieAPIs.Common.Helper;

namespace MovieAPIsTest.CommonTest
{
    public class UrlHelperTest
    {
        [TestCase("firstLevel/secondLevel")]
        [TestCase("some string")]
        [TestCase("word")]
        public void GetUrlWithoutQueryTest(string path)
        {
            string url = UrlHelper.GetUrl(path);

            Assert.AreEqual(path, url);
        }

        [TestCase(null)]
        [TestCase("")]
        public void GetUrlInvalidPath(string path)
        {
            Assert.Throws<ArgumentException>(() => UrlHelper.GetUrl(path));
        }

        [TestCaseSource(nameof(getQueryTestSource))]
        public void GetQueryTest(Dictionary<string, string>? queryParams, string expectedQuery)
        {
            string actualQuery = UrlHelper.GetQuery(queryParams);

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [TestCaseSource(nameof(getUrlWithQueryTestSource))]
        public void GetUrlWithQueryTest(Dictionary<string, string> queryParams, string path, string expectedUrl)
        {
            string actualUrl = UrlHelper.GetUrl(path, queryParams);

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [Test]
        public void GetUrlsTest()
        {
            string path = "home/room/bed";
            var queryParams = new Dictionary<string, string>
            {
                ["one"] = "oneValue",
                ["two"] = "twoValue"
            };
            var expectedUrls = new[] { "home/room/bed?one=oneValue&two=twoValue&page=1", "home/room/bed?one=oneValue&two=twoValue&page=2" };

            var actualUrls = UrlHelper.GetUrls(queryParams, path, 1, expectedUrls.Length);

            CollectionAssert.AreEqual(expectedUrls, actualUrls);
            Assert.IsFalse(queryParams.ContainsKey("page"));
        }

        static object[] getQueryTestSource =
        {
            new object[] { new Dictionary<string, string>
            {
                ["firstKey"] = "firstValue",
                ["secondKey"] = "secondValue",
                ["thirdKey"] = "thirdValue"
            }, "firstKey=firstValue&secondKey=secondValue&thirdKey=thirdValue" },

            new object[] { new Dictionary<string, string>
            {
                ["oneKey"] = "oneValue"
            }, "oneKey=oneValue"},

            new object[] { new Dictionary<string, string>(), string.Empty },

            new object[] { null, string.Empty }
        };

        static object[] getUrlWithQueryTestSource =
        {
            new object[] { new Dictionary<string, string>
            {
                ["someKey"] = "someValue",
                ["testKey"] = "testValue"
            }, "home/room/chair", "home/room/chair?someKey=someValue&testKey=testValue" },
            new object[] {new Dictionary<string, string>
            {
                ["oneKey"] = "oneValue"
            }, "school/classroom", "school/classroom?oneKey=oneValue" },
            new object[]{ new Dictionary<string, string>(), "home/room", "home/room"},
            new object[] { null, "home/room", "home/room"}
        };
    }
}