using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.IO;


namespace SemesterProjectTests
{
    public class ResultDataManagerTests
    {
        [Fact]
        public void ResultDataManager_AppendToCSV()
        {
            //Arrange 
            string[][] data = new string[][]
            {
                    new string[] { "", "",},
                    new string[] { "", "",}
            };
            string file = Path.GetTempFileName();


            //Act
            var expected = ",\r\n,\r\n";
            //Assert 
            try
            {
                SemesterProject.ResultDataManager.AppendToCSV(file, data);
                string actual = File.ReadAllText(file);
                Assert.Equal(expected, actual);
            }
            finally
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        [Fact]
        public void ResultDataManager_AppendIntArrayToCSV()
        {
            // Arrange
            string file = Path.GetTempFileName();
            int[][] data = new int[][]
            {
            new int[] { 1, 2, 3 },
            new int[] { 4, 5 }
            };

            string expected = "1,2,3\r\n4,5\r\n";

            // Act
            try
            {
                SemesterProject.ResultDataManager.AppendIntArrayToCSV(file, data);
                string actual = File.ReadAllText(file);

                // Assert
                Assert.Equal(expected, actual);
            }
            finally
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

    }
}