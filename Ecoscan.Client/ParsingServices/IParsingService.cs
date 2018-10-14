using Ecoscan.Client.Models;

namespace Ecoscan.Client.ParsingServices
{
    public interface IParsingService
    {
        void Parse(ConfigurationModel model, string[] args);
    }
}