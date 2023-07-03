//貼上 SilentSigningUseCase.cs

using System;
using MirageSDK.Base;
using MirageSDK.Core.Infrastructure;
using MirageSDK.Data;
using MirageSDK.Provider;
using MirageSDK.WearableNFTExample;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ContractManager : MonoBehaviour
{

    [SerializeField]
    private ContractInformationSO _targetContractInfo;

    private IMirageSDK _sdkInstance;
    private IContract _targetContract;

    [SerializeField] private ContractInformationSO ERC721ContractInformation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            InitCM();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RequestApproveNFTtoNanioContract();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            RequestPlaceNFT();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            RequestPickupNFT();
        }
    }

    public void InitCM()
    {
        _sdkInstance = MirageSDKFactory.GetMirageSDKInstance(NetworkName.Polygon);
        _targetContract = _sdkInstance.GetContract(_targetContractInfo);
        Debug.Log("Init CM");
    }

    public async void RequestApproveNFTtoNanioContract()
    {
        //合約位址
        string _addr = "0x76b7E2DCB365df5F5bf8F7316BA47F0f5f62F00D";
        //NFT ID
        string _tokenId = "0x00000000000000000000000000000000000000000000000000000000000002f8";

        //取得該 NFT 合約
        IContract _targetNFTContract = _sdkInstance.GetContract(_addr, ERC721ContractInformation.ABI);

        var transactionHash = await _targetNFTContract.CallMethod("approve", new object[] { _targetContractInfo.ContractAddress, _tokenId });
        Debug.Log($"Receipt: {transactionHash}");
    }

    private void RequestPlaceNFT()
    {
        string methodName = "placeNFT";

        string _addr = "0x76b7E2DCB365df5F5bf8F7316BA47F0f5f62F00D";
        string _tokenId = "0x00000000000000000000000000000000000000000000000000000000000002f8";
        int _minutes = 300;

        uint minuteParse = uint.Parse(_minutes.ToString());

        UniTask.Create(async () =>
        {
            var defaultAccount = await _sdkInstance.Eth.GetDefaultAccount();
            var transactionHash = await _targetContract.CallMethod(methodName, new object[] { _addr, _tokenId, minuteParse });
            var message = $"放置 NFT Hash : {transactionHash}";
            Debug.Log(message);
        }).Forget();
    }

    private void RequestPickupNFT()
    {
        string methodName = "pickupNFT";
        string _placementId = "0x0000000000000000000000000000000000000000000000000000000000000095";

        UniTask.Create(async () =>
        {
            var defaultAccount = await _sdkInstance.Eth.GetDefaultAccount();
            var transactionHash = await _targetContract.CallMethod(methodName, new object[] { _placementId });
            var message = $"撿起 NFT Hash : {transactionHash}";
            Debug.Log(message);
        }).Forget();
    }
}