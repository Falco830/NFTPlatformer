﻿using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MoralisUnity;

    public class NftMinter
    {
        private readonly string _contractInstanceName;
        private readonly string _contractAbi;
        private readonly ChainList _deploymentChain;
        private readonly string _contractAddress;
        private bool isInitialized = false;

        private BigInteger _currentTokenId;

  public NftMinter(string instanceName, string abi, ChainList chain, string contractAddress)
        {
            _contractInstanceName = instanceName;
            _contractAbi = abi;
            _deploymentChain = chain;
            _contractAddress = contractAddress;
    //Moralis.(_contractInstanceName, _contractAbi, _deploymentChain.ToString(), _contractAddress);
    //Moralis.Web3Api
    //Moralis.Web3Api.
            //Moralis.ExecuteContractFunction(_contractInstanceName, _contractAbi, _deploymentChain.ToString(), _contractAddress);
          // Moralis.InsertContractInstance(_contractInstanceName, _contractAbi, _deploymentChain.ToString(), _contractAddress);
    //MoralisInterface.InsertContractInstance(_contractInstanceName, _contractAbi, _deploymentChain.ToString(), _contractAddress);
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
          if (!isInitialized)
          {
            MoralisUser user = await Moralis.GetUserAsync();
            if (user != null)
            {
              isInitialized = true;

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
                  object metadata = BuildMetadata(nftName, nftDescription, ipfsImagePath);

                  string formatedTokenId = tokenId.ToString().PadLeft(32, '0');
                  string metadataName = $"{nftName}_{formatedTokenId}.json";

                  // Store metadata to IPFS
                  string json = JsonConvert.SerializeObject(metadata);
                  string base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                  string ipfsMetadataPath = await SaveToIpfs(metadataName, base64Data);

                  Debug.Log($"Metadata file saved to: {ipfsMetadataPath}");

                  if (!String.IsNullOrEmpty(ipfsMetadataPath))
                  {
                    // Execute mint of NFT.
                    string resp = await ExecuteMint(toWalletAddress, tokenId, ipfsMetadataPath);

                    if (resp is { })
                    {
                      result = true;
                    }
                  }
                }
              }
              Debug.Log(result);
              return result;
              }
            }
          return false;
        }

        #endregion


        #region PRIVATE_METHODS

        private async Task<string> ExecuteMint(string toAddress, BigInteger tokenId, string ipfsMetadataPath)
        {
            BigInteger amt = new BigInteger(1);
            //BigInteger data = new BigInteger(0);
            byte[] data = Array.Empty<byte>();
            // Mint Token
            // Convert token id to hex as this is what the contract call expects

            // Set gas estimate
            HexBigInteger gas = new HexBigInteger(0);
            HexBigInteger value = new HexBigInteger("0x5");
            HexBigInteger gasPrice = new HexBigInteger(0);

            object[] pars = {
                                toAddress,
                                tokenId.ToString("x"),
                                amt.ToString("x"),
                                ipfsMetadataPath,
                                data
                            };

              // Call the contract to mint the NFT.
              // IMPORTANT - assumes that mint function is public - this could cause problems so
              // you may want to time box the mint function in your contract or provide other
              // security.
              Debug.Log("To Address " + toAddress);
              string resp = await Moralis.ExecuteContractFunction(_contractAddress, _contractAbi, "mint",pars, value, gas, gasPrice);
              Debug.Log(resp);

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
          if (_contractAddress == string.Empty)
          {
            Debug.Log("Contract address is not set");
            return;
          }

          MoralisTools.Web3Tools.ViewContractOnPolygonScan(_contractAddress);
        }

        public void ViewOnOpenSea()
        {
          MoralisTools.Web3Tools.ViewNftOnTestnetOpenSea(_contractAddress, Moralis.CurrentChain.Name, _currentTokenId.ToString());
        }

        #endregion
}