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
        public void ResultDataManager_AppendDoubleArrayToCSV()
        {
            //Arrange 
            string file = Path.GetTempFileName();
            double[][] data = new double[][]
            {
                new double[] { 1.1, 2.2, 3.3 },
                new double[] { 4.4, 5.5, 6.6 }
            };
            string expectedContent = "1.1,2.2,3.3\r\n4.4,5.5,6.6\r\n";


            //Act
            try
            {
                SemesterProject.ResultDataManager.AppendDoubleArrayToCSV(file, data);
                string actualContent = File.ReadAllText(file);
                Assert.Equal(expectedContent, actualContent);

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