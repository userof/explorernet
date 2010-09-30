using ExplorerNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.VisualBasic.FileIO;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for CopyOrMoveWindowTest and is intended
    ///to contain all CopyOrMoveWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CopyOrMoveWindowTest
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
        ///A test for CopyOrMoveWindow Constructor
        ///</summary>
        //[TestMethod()]
        public void CopyOrMoveWindowConstructorTest()
        {
            string localPath = @"F:\Projects\C#2pr\Explorer.NET\1\ExplorerNet\out";

            List<FileSystemInfo> sourceList = new List<FileSystemInfo>();

            DirectoryInfo dir1 = new DirectoryInfo(localPath + @"\temp\source\mob");
            FileSystem.CopyDirectory(dir1.FullName, localPath + @"\temp\mob", true);
               
            FileInfo file1 = new FileInfo(localPath + @"\temp\source\dir.ico");
            FileSystem.CopyFile(file1.FullName, localPath + @"\temp\dir.ico", true);
                
            FileInfo file2 = new FileInfo(localPath + @"\temp\source\FCommander.exe");
            FileSystem.CopyFile(file2.FullName, localPath + @"\temp\FCommander.exe", true);
               
            FileInfo file3 = new FileInfo(localPath + @"\temp\source\LastSeans.fcm");
            FileSystem.CopyFile(file3.FullName, localPath + @"\temp\LastSeans.fcm", true);
               
            FileInfo file4 = new FileInfo(localPath + @"\temp\source\LF.ico");
            FileSystem.CopyFile(file4.FullName, localPath + @"\temp\LF.ico", true);
               
            FileInfo file5 = new FileInfo(localPath + @"\temp\source\set.ini");
            FileSystem.CopyFile(file5.FullName, localPath + @"\temp\set.ini", true);


            sourceList.Add(dir1);
            sourceList.Add(file1);
            sourceList.Add(file2);
            sourceList.Add(file3);
            sourceList.Add(file4);
            sourceList.Add(file5);


            CopyOrMoveWindow target = new CopyOrMoveWindow(sourceList, new DirectoryInfo(localPath + @"\temp"));
            target.ShowDialog();
            target.Close();

        }
    }
}
