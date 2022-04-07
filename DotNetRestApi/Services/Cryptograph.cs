using Newtonsoft.Json;
using System.Text;
using _SHA256 = System.Security.Cryptography.SHA256;

namespace DotNetRestApi.Services;

public class Cryptograph
{
    public Cryptograph()
    {

    }

    public string CreateHash(object @object)
    {
        var json = JsonConvert.SerializeObject(@object);
        return CreateHash(rawData: json);
    }

    public string CreateHash(string rawData)
    {
        // Create a SHA256   
        // TODO: Could we put this into the pipeline?
        using _SHA256 sha256Hash = _SHA256.Create();
        
        // ComputeHash - returns byte array  
        return new StringBuilder()
            .AppendJoin("", sha256Hash
                .ComputeHash(Encoding.UTF8.GetBytes(rawData))
                    .Select(x => x.ToString("x2")))
            .ToString();        
    }
}

