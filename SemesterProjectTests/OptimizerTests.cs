using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SemesterProject;

namespace SemesterProjectTests.OptimizerTests
{
    public class OptimizerTests
    {
        [Fact]
        public void Optimizer_Calculation()
        {
            //Arrange

            List<ProductionUnit> productionUnits =
            [
                new ProductionUnit("GB", "5.0", "0", "500", "215", "gas"),
                new ProductionUnit("OB", "4.0", "0", "700", "265", "oil"),
                new ProductionUnit("GM", "3.6", "2.7", "1100", "640", "gas"),
                new ProductionUnit("EK", "8.0", "-8.0", "50", "0", "electricity"),
            ];
            string[][] olas = new string[][]{
                ["6.62", "800","1.62", "600"]
            };

            //Act
            double[][] expected = [[-1060, 500, 700, 6450]];

            //Assert

            Assert.Equal(expected, SemesterProject.Optimizer.Calculation("summer", olas, productionUnits));
        }
        [Fact]
        public void Optimizer_ChangeToDouble()
        {
            //Arrange
            double[][] doubleValue = new double[][]{
                [2.1,3.2,1.2],
                [3.1,4.5,6.7]
            };
            //Act
            string[][] expected = new string[][]{
                ["2.1","3.2","1.2"],
                ["3.1","4.5","6.7"]
            };
            //Assert
            Assert.Equal(expected, SemesterProject.Optimizer.Convert(doubleValue));
        }
        [Fact]
        public void Optimizer_OperatingPoint()
        {
            //Arrange 
            string[][] data = [
                ["6.62","5.43","9.65","7.42"]
            ];
            List<ProductionUnit> productionUnits =
           [
               new ProductionUnit("GB", "5.0", "0", "500", "215", "gas"),
                new ProductionUnit("OB", "4.0", "0", "700", "265", "oil"),
                new ProductionUnit("GM", "3.6", "2.7", "1100", "640", "gas"),
                new ProductionUnit("EK", "8.0", "-8.0", "50", "0", "electricity"),
            ];
            //act
            double[][] expected = [[1.0859999999999999, 1.3574999999999999, 1.5083333333333333, 0.67874999999999996]];
            //assert 
            Assert.Equal(expected, SemesterProject.Optimizer.OperatingPoint(data, productionUnits, 1));
        }

    }
}