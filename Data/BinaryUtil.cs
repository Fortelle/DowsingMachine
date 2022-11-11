namespace PBT.DowsingMachine.Data;

public class BinaryUtil
{

    public static bool CheckSignature(byte[] arr, string signature)
    {
        if(arr.Length < signature.Length)
        {
            return false;
        }

        for(var i = 0; i < signature.Length; i++)
        {
            if (arr[i] != signature[i])
            {
                return false;
            }
        }

        return true;
    }

}
