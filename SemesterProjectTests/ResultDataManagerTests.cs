using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xUnit;
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
        
    }
}