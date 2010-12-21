using ExplorerNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for DeleteWindowTest and is intended
    ///to contain all DeleteWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DeleteWindowTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for DeleteWindow Constructor
        ///</summary>
        //[TestMethod()]
        public void DeleteWindowConstructorTest()
        {
            string localPath = @"F:\Projects\C#2pr\Explorer.NET\1\ExplorerNet\out";

            List<FileSystemInfo> deleteFSIList = new List<FileSystemInfo>();

            DirectoryInfo dir1 = new DirectoryInfo(localPath + @"\temp\mob");
            FileInfo file1 = new FileInfo(localPath + @"\temp\dir.ico");
            FileInfo file2 = new FileInfo(localPath + @"\temp\FCommander.exe");
            FileInfo file3 = new FileInfo(localPath + @"\temp\LastSeans.fcm");
            FileInfo file4 = new FileInfo(localPath + @"\temp\LF.ico");
            FileInfo file5 = new FileInfo(localPath + @"\temp\set.ini");

            deleteFSIList.Add(dir1);
            deleteFSIList.Add(file1);
            deleteFSIList.Add(file2);
            deleteFSIList.Add(file3);
            deleteFSIList.Add(file4);
            deleteFSIList.Add(file5);
           


            DeleteWindow target = new DeleteWindow(deleteFSIList);

            target.ShowDialog();
            //target.Close();

            //Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
