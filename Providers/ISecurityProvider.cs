using System;

namespace PeerShareV2.Providers
{
    public interface ISecurityProvider
    {
        String GetEncrypt(String clearText);
        String GetDecrypt(String cipherText);
    }
}