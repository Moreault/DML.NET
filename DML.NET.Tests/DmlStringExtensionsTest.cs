namespace DML.NET.Tests;

[TestClass]
public class DmlStringExtensionsTest
{
    [TestClass]
    public class ToDmlString : Tester
    {
        [TestMethod]
        public void WhenSourceIsNull_Throw()
        {
            //Arrange
            IEnumerable<DmlSubstring> source = null!;

            //Act
            Action action = () => source.ToDmlString();

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_ReturnDmlStringWithCorrectIndexes()
        {
            //Arrange
            var source = new List<DmlSubstring>
            {
                new()
                {
                    Text = "C'est donc ben ",
                    Color = Dummy.Create<Color>()
                },
                new()
                {
                    Text = "laid ",
                    Color = Dummy.Create<Color>()

                },
                new()
                {
                    Text = "chez vous!",
                    Color = Dummy.Create<Color>()
                },
            };

            //Act
            var result = source.ToDmlString();

            //Assert
            result.Should().BeEquivalentTo(new DmlString(new List<DmlSubstringEntry>
            {
                new(source[0])
                {
                    StartIndex = 0
                },
                new(source[1])
                {
                    StartIndex = 15
                },
                new(source[2])
                {
                    StartIndex = 20
                },
            }));
        }
    }
}