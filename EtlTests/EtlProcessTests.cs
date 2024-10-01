using ETL;
using ETL.Interfaces;
using FakeItEasy;

namespace EtlTests;

public class EtlProcessTests
{
    [Fact]
    public void WhenJiraConnectorMissing_ExceptionThrown()
    {
        // Arrange
        var etlProcess = new EtlProcess();
        etlProcess.ExecutionArguments = new Arguments();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => etlProcess.Run());    
    }

    [Fact]
    public void WhenAllConectorsAreProvided_ExecutionEndsWell()
    {
        var fakeJiraConnector = A.Fake<IJiraConnector>();
        // Arrange
        var etlProcess = new EtlProcess()
        {
            ExecutionArguments = new Arguments(),
            JiraConnector = fakeJiraConnector
        };

        var exception = Record.Exception(() => etlProcess.Run());
        Assert.Null(exception);
    }
}