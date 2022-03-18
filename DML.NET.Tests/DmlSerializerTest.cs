using ToolBX.DML.NET.Conversion;

namespace DML.NET.Tests;

[TestClass]
public class DmlSerializerTest
{
    [TestClass]
    public class Deserialize : Tester<DmlSerializer>
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void WhenTextIsEmpty_Throw(string text)
        {
            //Arrange

            //Act
            Action action = () => Instance.Deserialize(text);

            //Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Always_ParseMarkupAndReturnConverted()
        {
            //Arrange
            var text = Fixture.Create<string>();

            var metaStrings = Fixture.Create<List<MetaString>>();
            GetMock<IMarkupParser>().Setup(x => x.Parse(text, null)).Returns(metaStrings);

            var dml = Fixture.Create<DmlString>();
            GetMock<IDmlConverter>().Setup(x => x.Convert(metaStrings)).Returns(dml);

            //Act
            var result = Instance.Deserialize(text);

            //Assert
            result.Should().BeEquivalentTo(dml);
        }
    }
}