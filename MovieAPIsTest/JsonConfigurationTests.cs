﻿using MovieAPIs.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPIsTest
{
    public class JsonConfigurationTests
    {

        IConfiguration jsonConfiguration = null;
        const string pathToFile = "test.json";
        [OneTimeSetUp]
        public void CreateConfigurationFile()
        {
            using (var sw = new StreamWriter(pathToFile))
            {
                string json = @"{
            ""Test"": 
            {
                ""V1"": ""Testversion1"",
                ""V2"": ""Testversion2""
            },
            ""Word"": ""Key""
            }";
                sw.Write(json);
            }
            jsonConfiguration = new JsonConfiguration(pathToFile);
            
        }
        [OneTimeTearDown]
        public void DeleteConfigurationFile()
        {
            File.Delete(pathToFile);
        }

        [TestCase("test:v1", "Testversion1")]
        [TestCase("test:v2", "Testversion2")]
        [TestCase("word", "Key")]
        [TestCase("WORD", "Key")]
        public void IndexerWithValidPathTest(string path, string expected)
        {
            string actual = jsonConfiguration[path];
            Assert.AreEqual(expected, actual);
        }

        [TestCase("Invalid:head")]
        [TestCase("Invalid")]
        [TestCase("")]
        [TestCase(null)]
        public void IndexWithInvalidPathTest(string path)
        {
            Assert.Throws<InvalidPathException>(() => { 
                string actual = jsonConfiguration[path]; 
            });
        }
    }
}
