namespace DML.NET.Tests;

[TestClass]
public class DmlStringTest
{
    //TODO Test
    [TestClass]
    public class Indexer_int : Tester
    {
        //TODO Test
    }

    [TestClass]
    public class Indexer_int_int : Tester
    {
        //TODO Test
    }

    [TestClass]
    public class Substring_Start : Tester
    {
        [TestMethod]
        public void WhenStartingFromMiddle_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(20);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "outskirts of ",
                    Color = dmlString[1].Color
                },
                new()
                {
                    Text = "Behabad",
                    Color = dmlString[2].Color
                }
            }.ToDmlString());
        }
    }

    [TestClass]
    public class Substring_start_length : Tester
    {
        [TestMethod]
        public void WhenStartingIndexIsNegative_Throw()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = -Dummy.Create<int>();
            var length = Dummy.Create<int>();

            //Act
            Action action = () => dmlString.Substring(startingIndex, length);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenLengthIsNegative_Throw()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = Dummy.Create<int>();
            var length = -Dummy.Create<int>();

            //Act
            Action action = () => dmlString.Substring(startingIndex, length);

            //Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenLengthIsZero_ReturnEmpty()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            var startingIndex = Dummy.Create<int>();
            var length = 0;

            //Act
            var result = dmlString.Substring(startingIndex, length);

            //Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenBothStartingIndexAndLengthAreWithinFirstSubstring_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(0, 4);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "That",
                    Color = dmlString[0].Color
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenStartingIndexIsWithinFirstSubstringButLengthGoesIntoSecond_ReturnPartsOfBoth()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(5, 24);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "base is ",
                    Color = dmlString[0].Color
                },
                new()
                {
                    Text = "on the outskirts",
                    Color = dmlString[1].Color
                }
            }.ToDmlString());
        }

        [TestMethod]
        public void WhenStartingFromMiddle_Return()
        {
            //Arrange
            var dmlString = new List<DmlSubstring>
            {
                new()
                {
                    Text = "That base is ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "on the outskirts of ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "Behabad",
                    Color = Dummy.Create<Color>()
                },
            }.ToDmlString();

            //Act
            var result = dmlString.Substring(20, 20);

            //Assert
            result.Should().BeEquivalentTo(new List<DmlSubstring>
            {
                new()
                {
                    Text = "outskirts of ",
                    Color = dmlString[1].Color
                },
                new()
                {
                    Text = "Behabad",
                    Color = dmlString[2].Color
                }
            }.ToDmlString());
        }
    }

    [TestClass]
    public class ToString_Method : Tester
    {
        //TODO Test
    }
}