using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Defective.JSON;

public class NFTManager : MonoBehaviour
{
    private const string AlchemyBaseURL = "https://polygon-mainnet.g.alchemy.com/v2/";
    private const string AppKey = "nHyImEpBTNe5G1S4F74Gm5cV5gfyUqwj";
    private const string GetNFTsURL = "getNFTs/?owner=";
    private const string OptionURL = "&withMetadata=true&pageSize=100";

    public MirageSDK.UI.WalletManager walletManager;
    public string NFTsURL = "https://api.opensea.io/api/v1/assets?owner=";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNFTs();
        }
    }

    public async void GetNFTs()
    {
        var address = walletManager.GetAddress();
        var url = AlchemyBaseURL + AppKey + "/" + GetNFTsURL + address + OptionURL;
        Debug.Log(url);

        var jsonString = await GetNFTsData(url);
        JSONObject nftJson = new JSONObject(jsonString);

        Debug.Log("==================================");
        foreach (var nft in nftJson["ownedNfts"])
        {
            if (!nft.HasField("spamInfo"))
            {
                Debug.Log("Title:" + nft["title"].stringValue);
                Debug.Log("Address:" + nft["contract"]["address"].stringValue);
            }
            Debug.Log(nft["metadata"]["name"]);
        }
    }


    public async UniTask<string> GetNFTsData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return "ERROR";
        }
        return request.downloadHandler.text;
    }
}
