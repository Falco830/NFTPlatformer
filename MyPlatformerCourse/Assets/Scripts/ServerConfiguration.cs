using UnityEngine;

public class ServerConfiguration : MonoBehaviour
{
    // Base URL
    public static readonly string URL = "https://moralis-nodejs-app-bpbl5drwdq-uc.a.run.app/";

    // Endpoints
    public static readonly string RequestEndpoint = "/request";
    public static readonly string VerifyEndpoint = "/verify";
    public static readonly string NativeBalanceEndpoint = "/nativeBalance";
}
