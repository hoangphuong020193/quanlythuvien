using System.Linq;

namespace HRM.CrossCutting.Command
{
    public static class CommandResultExtensions
    {
        public static int? GetFirstErrorCode(this CommandResult commandResult)
        {
            return commandResult.Errors.FirstOrDefault()?.Code;
        }
    }
}