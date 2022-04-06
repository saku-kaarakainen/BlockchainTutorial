namespace DotNetRestApi.Services;
public class ConsensusMechanism
{
    public ConsensusMechanism()
    {

    }

    /// <summary>
    /// <para>Simple Proof of Work Algorithm:</para>
    /// <para>- Find a number p' such that hash(pp') contains leading 4 zeroes, where p is the previous p'</para>
    /// <para>- p is the previous proof, and p' is the new proof</para>
    /// </summary>
    public int ProofOfWork(int lastProof)
    {
        int proof = 0;

        while (!IsValidProof(lastProof, proof))
        {
            proof++;
        }

        return proof;
    }

    /// <summary>
    ///  Validates the Proof: Does hash(last_proof, proof) contain 4 leading zeroes?
    /// </summary>
    /// <param name="lastProof">Previous Proof</param>
    /// <param name="proof">Current Proof</param
    /// <returns>True if correct, False if not.</returns>
    private bool IsValidProof(int lastProof, int proof)
    {
        string guess = new System.Text
            .StringBuilder(lastProof)
            .Append(proof)
            .ToString();

        string hash = SHA256.CreateHash(rawData: guess);
        bool isCorrect = hash.StartsWith("0000");
        return isCorrect;
    }
}

