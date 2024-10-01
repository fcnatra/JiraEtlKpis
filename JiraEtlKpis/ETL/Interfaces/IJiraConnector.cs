using Interfaces;

namespace ETL.Interfaces;
public interface IJiraConnector
{
    int BlockSize { get; set; }
    IJiraArguments? JiraArguments { get; set; }
    string GetIssuesSince(DateTime dateTime);
}
