
using System.Collections;
/// <summary>
/// The 'Command' abstract class that we will inherit from
/// </summary>
public interface Command
{
    IEnumerator Execute();
    IEnumerator UnExecute();
}
