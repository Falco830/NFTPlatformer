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
using Nethereum.Util;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using Pinata.Client;
using Flurl.Http;
using System.Net.Http;
using System.Net.Mime;

//using Task = System.Threading.Tasks;

//using Task = System;

namespace NFT_Minter
{
  public class MintingPanel : MonoBehaviour
    {
        [Header("Smart Contract Data")]
        [SerializeField] private string contractAddress;
        [SerializeField] private string contractAbi;
        [SerializeField] private string contractFunction;
        private const ChainList deploymentChain = ChainList.mumbai;
        private const string contractInstanceName = "MyNftContract";
        [SerializeField]
        private string _characterImgFilePath;

        private BigInteger _currentTokenId;
        //private bool isInitialized = false;

        [Header("NFT Metadata")]
        [SerializeField] private string metadataUrl;
        [SerializeField] private Image[] metadataImage;

        [SerializeField] private int selectedNFT;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI metadataUrlText;
        [SerializeField] private Button[] mintButton;
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
            if(metadataUrlText?.text != null) 
            { 
              metadataUrlText.text = metadataUrl; 
            }            
        }
        public async void Mint(int NFT)
        {
            mintButton[NFT].interactable = false;
            selectedNFT = NFT;
            //_characterImgFilePath = AssetDatabase.GetAssetPath(metadataImage[NFT].sprite);
            statusText.text = "Minting... Check on your wallet to confirm transaction";
            ActivateMintPanel();
            await RunNftMint();
        }
        

        #region PRIVATE_METHODS

        private async Task RunNftMint()
        {
            // Dummy TokenId based on current time.
            long tokenId = DateTime.Now.Ticks;
            
            // Dummy NFT Name.
            string nftName = $"Climbing_Tower_{tokenId}";

            //NftMinter minter = new NftMinter(contractInstanceName, contractAbi, deploymentChain, contractAddress);
            Debug.Log("InstanceName " + contractInstanceName + " Abi " + contractAbi + " DeploymentChain " + deploymentChain + " Address " + contractAddress);
            statusText.text = "Sending Mint Info... ";           
          // Now call the mint process.
          if (await MintNft(nftName, "NFT created using Moralis Unity SDK!", _characterImgFilePath, new BigInteger(tokenId)))
            {
                Debug.Log($"NFT {tokenId} has been minted!");
                statusText.text = $"NFT {tokenId} has been minted!" + 
                                  "<br>" + 
                                  $"<link=\"https://testnets.opensea.io/assets?search[query]={contractAddress}\"><u>Check on OpenSea</u></link>";
            }
            else
            {
                Debug.Log("Failed to mint NFT :-(");
                statusText.text = "Failed to mint NFT. Try again";
                mintButton[selectedNFT].interactable = true;
            }
        }

    #region PUBLIC_METHODS

    /// <summary>
    /// Stores the image to IPFS, generates metadata and store that to IPFS and then calls the NFT Mint 
    /// function on the contract.
    /// </summary>
    /// <param name="nftName">Mapped to 'name' field in the NFT Metadata</param>
    /// <param name="nftDescription">Mapped to 'description' field in the NFT Metadata</param>
    /// <param name="filePath">The full path to the image</param>
    /// <param name="tokenId">What token id to mint this NFT under</param>
    /// <returns>bool</returns>
    public async Task<bool> MintNft(string nftName, string nftDescription, string filePath, BigInteger tokenId)
    {
      /*if (!isInitialized)
      {*/
        MoralisUser user = await Moralis.GetUserAsync();
        if (user != null)
        {
          //isInitialized = true;

          string toWalletAddress = user.ethAddress;
          Debug.Log(toWalletAddress);

          bool result = false;
          byte[] fileData = Array.Empty<byte>();
          string imageName = Path.GetFileName(filePath);


          // Load file data.
          try
          {
            if (!File.Exists(filePath)) throw new IOException($"Could not load file {filePath}");
            fileData = File.ReadAllBytes(filePath);
          }
          catch (IOException exception)
          {
            Debug.LogError(exception.Message);
          }

          if (fileData.Length > 0)
          {
            // Same image to IPFS
            string ipfsImagePath = await SaveToIpfs(imageName, fileData);

            if (!String.IsNullOrEmpty(ipfsImagePath))
            {
              Debug.Log($"NFT Image file saved to: {ipfsImagePath}");

              // Build Metadata
              //object metadata = BuildMetadata(nftName, nftDescription, ipfsImagePath);
              object metadata = MoralisTools.Web3Tools.BuildMetadata(nftName, nftDescription, ipfsImagePath);
              string formatedTokenId = tokenId.ToString().PadLeft(32, '0');
              string metadataName = $"{nftName}_{formatedTokenId}.json";

              // Store metadata to IPFS
              string json = JsonConvert.SerializeObject(metadata);
              string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
              string ipfsMetadataPath = await MoralisTools.Web3Tools.SaveToIpfs(metadataName, base64Data);

              //string ipfsMetadataPath = await SaveToIpfs(metadataName, base64Data);

              Debug.Log($"Metadata file saved to: {ipfsMetadataPath}");

              if (!String.IsNullOrEmpty(ipfsMetadataPath))
              {
                // Execute mint of NFT.
                string resp = await ExecuteMinting(toWalletAddress, tokenId, ipfsMetadataPath);
                  result = true;
                Debug.Log("Response Reaches here: " + resp);
                if (resp is null)
                {
                  Debug.Log("Response is null: " + resp);
                  statusText.text = "Transaction failed";
                  //mintButton.interactable = true;
                  return false;
                }                
                // We tell the GameManager what we minted the item successfully
                statusText.text = "Transaction completed!";
                Debug.Log("Response reaches the Transaction: " + statusText.text);
                Debug.Log($"Token Contract Address: {contractAddress}");
                Debug.Log($"Token ID: {_currentTokenId}");

                // Activate OpenSea button
                //mintButton.gameObject.SetActive(false);
                openSeaButton.gameObject.SetActive(true);
              }
            }
          }
          Debug.Log(result);
          return result;
        }
      /*}*/
      return false;
    }

    #endregion
    #region MINTING_METHODS

    public async void MintNftB()
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
      //string filePath = AssetDatabase.GetAssetPath(metadataImage[selectedNFT].sprite);
      string imageName = "";//Path.GetFileName(filePath);

            metadataUrl = Path.GetFileName(""/*filePath*/);

            if (metadataUrl == string.Empty)
            {
                Debug.LogError("Metadata URL is empty");
                return;
            }
            
            statusText.text = "Please confirm transaction in your wallet";
            //mintButton.interactable = false;

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
                //mintButton.interactable = true;
                return;
            }
    
            // We tell the GameManager what we minted the item successfully
            statusText.text = "Transaction completed!";
            Debug.Log($"Token Contract Address: {contractAddress}");
            Debug.Log($"Token ID: {_currentTokenId}");
            
            // Activate OpenSea button
            //mintButton.gameObject.SetActive(false);
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
      HexBigInteger value = new HexBigInteger(UnitConversion.Convert.ToWei(0.05, 18));
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger gasPrice = new HexBigInteger(0);

            string resp = await Moralis.ExecuteContractFunction(contractAddress, contractAbi, contractFunction, pars, value, gas, gasPrice);

            return resp;
        }

   /* private async string SaveToMongoAtlas(string name, string data)
    {
      var fileName = "D:\\Untitled.png";
      var newFileName = "D:\\new_Untitled.png";
      using (var fs = new FileStream(fileName, FileMode.Open))
      {
        var gridFsInfo = await database.GridFS.Upload(fs, fileName);
        var fileId = gridFsInfo.Id;

        ObjectId oid = new ObjectId(fileId);
        var file = await database.GridFS.FindOne(Query.EQ("_id", oid));

        using (var stream = file.OpenRead())
        {
          var bytes = new byte[stream.Length];
          stream.Read(bytes, 0, (int)stream.Length);
          using (var newFs = new FileStream(newFileName, FileMode.Create))
          {
            newFs.Write(bytes, 0, bytes.Length);
          }
        }
      }
      return null;
    }*/
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

    #region EVENT_HANDLERS

    private void ActivateMintPanel()
    {
        #if UNITY_EDITOR
        _characterImgFilePath = AssetDatabase.GetAssetPath(metadataImage[selectedNFT].sprite);
        #endif
    }

    //Pinata Code
    public async void PinataUpload()
    {
      var config = new Config
      {
        ApiKey = "2981f1eb1813daf...",
        ApiSecret = "42281fa28de32fe3c..."
      };

      var client = new PinataClient(config);

      var html = @"
      <html>
         <head>
            <title>Hello IPFS!</title>
         </head>
         <body>
            <h1>Hello World</h1>
         </body>
      </html>
      ";

      var metadata = new PinataMetadata // optional
      {
        KeyValues =
         {
            {"Author", "Brian Chavez"}
         }
      };

      var options = new PinataOptions(); // optional

      options.CustomPinPolicy.AddOrUpdateRegion("NYC1", desiredReplicationCount: 1);

      var response = await client.Pinning.PinFileToIpfsAsync(content =>
      {
        var file = new StringContent(html, Encoding.UTF8, MediaTypeNames.Text.Html);

        content.AddPinataFile(file, "index.html");
      },
         metadata,
         options);

      if (response.IsSuccess)
      {
        //File uploaded to Pinata Cloud and can be accessed on IPFS!
        var hash = response.IpfsHash; // QmR9HwzakHVr67HFzzgJHoRjwzTTt4wtD6KU4NFe2ArYuj
      }
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
#endregion