using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NFT_Minter
{
    public class MintingPanel : MonoBehaviour
    {
        [Header("Smart Contract Data")]
        [SerializeField] private string contractAddress;
        [SerializeField] private string contractAbi;
        [SerializeField] private string contractFunction;

        private BigInteger _currentTokenId;

        [Header("NFT Metadata")]
        [SerializeField] private string metadataUrl;
        [SerializeField] private Image[] metadataImage;

        [SerializeField] private int selectedNFT;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI metadataUrlText;
        [SerializeField] private Button mintButton;
        [SerializeField] private Button openSeaButton;
        [SerializeField] private TextMeshProUGUI statusText;
        
        private void OnEnable()
        {
            statusText.text = string.Empty;
            metadataUrlText.text = metadataUrl;
        }

        private void OnDisable()
        {
            statusText.text = string.Empty;
            metadataUrlText.text = string.Empty;
        }

        private void OnValidate()
        {
            metadataUrlText.text = metadataUrl;
        }

        #region MINTING_METHODS

        public async void MintNft()
        {
            if (contractAddress == string.Empty || contractAbi == string.Empty || contractFunction == string.Empty)
            {
                Debug.LogError("Contract data is not fully set");
                return;
            }
            long tokenId = DateTime.Now.Ticks;
            string nftName = $"Moralis_Web3_AngryBirds_{tokenId}";
            string nftDescription = "NFT created using Moralis Unity SDK!";

            byte[] fileData = Array.Empty<byte>();
            string filePath = AssetDatabase.GetAssetPath(metadataImage[selectedNFT].sprite);
            string imageName = Path.GetFileName(filePath);

            metadataUrl = Path.GetFileName(filePath);

            if (metadataUrl == string.Empty)
            {
                Debug.LogError("Metadata URL is empty");
                return;
            }
            
            statusText.text = "Please confirm transaction in your wallet";
            mintButton.interactable = false;

            MoralisUser user = await Moralis.GetUserAsync();
            string toWalletAddress = "";
            if (user != null)
            {
              toWalletAddress = user.ethAddress;
            }
              // Same image to IPFS
                string ipfsImagePath = await SaveToIpfs(imageName, fileData);

            if (String.IsNullOrEmpty(ipfsImagePath))
            {
              return;
            }
            object metadata = BuildMetadata(nftName, nftDescription, ipfsImagePath);

            string formatedTokenId = tokenId.ToString().PadLeft(32, '0');
            string metadataName = $"{nftName}_{formatedTokenId}.json";

            string json = JsonConvert.SerializeObject(metadata);
            string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            string ipfsMetadataPath = await SaveToIpfs(metadataName, base64Data);

            var result = await ExecuteMinting(toWalletAddress, new BigInteger(tokenId), ipfsMetadataPath);//(metadataUrl);

            if (result is null)
            {
                statusText.text = "Transaction failed";
                mintButton.interactable = true;
                return;
            }
    
            // We tell the GameManager what we minted the item successfully
            statusText.text = "Transaction completed!";
            Debug.Log($"Token Contract Address: {contractAddress}");
            Debug.Log($"Token ID: {_currentTokenId}");
            
            // Activate OpenSea button
            mintButton.gameObject.SetActive(false);
            openSeaButton.gameObject.SetActive(true);
        }
    
        private async UniTask<string> ExecuteMinting(string toAddress, BigInteger tokenId, string ipfsMetadataPath)//(string tokenUrl)
        {
            // Dummy TokenId based on current time.
            long currentTime = DateTime.Now.Ticks;
            _currentTokenId = new BigInteger(currentTime);
            BigInteger amt = new BigInteger(1);
            //BigInteger data = new BigInteger(0);
            byte[] data = Array.Empty<byte>();

            // These are the parameters that the contract function expects
            /*object[] parameters = {
                _currentTokenId.ToString("x"), // This is the format the contract expects
                tokenUrl
            };*/

            object[] pars = {
                                toAddress,
                                tokenId.ToString("x"),
                                amt.ToString("x"),
                                ipfsMetadataPath,
                                data
                            };

            // Set gas configuration. If you set it at 0, your wallet will use its default gas configuration
            HexBigInteger value = new HexBigInteger(0);
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);

            string resp = await Moralis.ExecuteContractFunction(contractAddress, contractAbi, contractFunction, pars, value, gas, gasPrice);

            return resp;
        }
    private async Task<string> SaveToIpfs(string name, string data)
    {
      string pinPath = null;
      try
      {
        IpfsFileRequest req = new IpfsFileRequest()
        {
          Path = name,
          Content = data
        };

        List<IpfsFileRequest> reqs = new List<IpfsFileRequest>();
        reqs.Add(req);
        List<IpfsFile> resp = await Moralis.GetClient().Web3Api.Storage.UploadFolder(reqs);

        IpfsFile ipfs = resp.FirstOrDefault<IpfsFile>();

        if (ipfs != null)
        {
          pinPath = ipfs.Path;
        }
      }
      catch (Exception exp)
      {
        Debug.LogError($"IPFS Save failed: {exp.Message}");
      }
      return pinPath;
    }

    private async Task<string> SaveToIpfs(string name, byte[] data)
    {
      return await SaveToIpfs(name, Convert.ToBase64String(data));
    }

    private static object BuildMetadata(string name, string desc, string imageUrl)
    {
      object o = new { name = name, description = desc, image = imageUrl };

      return o;
    }
    #endregion


    #region SECONDARY_METHODS

    public void ViewContract()
        {
            if (contractAddress == string.Empty)
            {
                Debug.Log("Contract address is not set");
                return;
            }
            
            MoralisTools.Web3Tools.ViewContractOnPolygonScan(contractAddress);
        }

        public void ViewOnOpenSea()
        {
            MoralisTools.Web3Tools.ViewNftOnTestnetOpenSea(contractAddress, Moralis.CurrentChain.Name, _currentTokenId.ToString());
        }

        #endregion
    }   
}
