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
                new DmlSubstring
                {
                    Text = "C'est donc ben ",
                    Color = Fixture.Create<Color>()
                },
                new DmlSubstring()
                {
                    Text = "laid ",
                    Color = Fixture.Create<Color>()

                },
                new DmlSubstring
                {
                    Text = "chez vous!",
                    Color = Fixture.Create<Color>()
                },
            };

            //Act
            var result = source.ToDmlString();

            //Assert
            result.Should().BeEquivalentTo(new DmlString(new List<DmlSubstringEntry>
            {
                new DmlSubstringEntry(source[0])
                {
                    StartIndex = 0
                },
                new DmlSubstringEntry(source[1])
                {
                    StartIndex = 15
                },
                new DmlSubstringEntry(source[2])
                {
                    StartIndex = 20
                },
            }));
        }
    }
}