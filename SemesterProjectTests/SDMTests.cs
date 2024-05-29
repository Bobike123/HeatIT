using System;
using System.IO;
using xUnit;
using SemesterProject;

namespace SemesterProjectTests
{
    public class SDMTests
    {
        [Fact]
        public void SDM_CSVDisplayGraph()
        {
            // Arrange
            string relativeDirPath = @"SourceDataManager";
            string fileName = "newfile.csv";
            string projectDir = Directory.GetCurrentDirectory(); // or Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string tempDirPath = Path.Combine(projectDir, relativeDirPath);

            // Ensure the directory exists
            if (!Directory.Exists(tempDirPath))
            {
                Directory.CreateDirectory(tempDirPath);
            }

            string tempFilePath = Path.Combine(tempDirPath, fileName);

            int[] columns = new int[] { 1 };

            try
            {
                // Write some sample data to the temporary file
                File.WriteAllLines(tempFilePath, new string[]
                {
                    "Column1,Column2,Column3",
                    "1,2,3",
                    "4,5,6",
                    "7,8,9"
                });

                // Act
                var result = SemesterProject.Views.SourceDataManager.CSVDisplayGraph(tempFilePath, columns);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<string[][]>(result);
                Assert.NotEmpty(result);


                // Assuming you have some expected result to compare against, you can use:
                // var expected = ... // Define your expected result

            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                // Optionally clean up the directory if you created it
                if (Directory.Exists(tempDirPath) && Directory.GetFiles(tempDirPath).Length == 0)
                {
                    Directory.Delete(tempDirPath);
                }
            }
        }
    }
}
