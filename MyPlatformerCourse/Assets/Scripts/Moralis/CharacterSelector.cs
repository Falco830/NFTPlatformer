using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using MoralisUnity.Web3Api.Models;
using MoralisUnity.Web3Api.Api;
using MoralisUnity.Platform.Objects;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.UI;

using MoralisUnity;

using Vector2 = UnityEngine.Vector2;
using UnityEngine.SceneManagement;
using MoralisUnity.Kits.AuthenticationKit;

[Serializable]
public class CustomNftMetadata
{
  public string name;
  public string description;
  public string image;
}

public class CharacterSelector : MonoBehaviour
    {
        public static event Action<Texture2D> OnCharacterSelected; //TODO passar la imatge/sprite/textura
        
        [Header("NFT Data")]
        [SerializeField] private string contractAddress;
        [SerializeField] private string[] tokenId;
        
        [Header("UI Elements")]
        [SerializeField] private GameObject characterPanel;
        [SerializeField] private RawImage[] characterImg;
        [SerializeField] private GameObject skipButton;
        [SerializeField] private TextMeshProUGUI debugLabel;
        
        private string _walletAddress;
        private ChainList _deployedChain = ChainList.mumbai;

        [SerializeField] private GameObject character1;
        [SerializeField] private GameObject character2;
        [SerializeField] private GameObject character3;
        
        [SerializeField] private GameObject authenticationKit;
        [SerializeField] private GameObject viewSwitch;

        MoralisUnity.Kits.AuthenticationKit.AuthenticationKitStateObservable _stateObservable = new MoralisUnity.Kits.AuthenticationKit.AuthenticationKitStateObservable();


  public async void GetTransactions()
  {
    // get BSC transactions for a given address
    TransactionCollection BSCtransactions = await Moralis.Web3Api.Account.GetTransactions("0x3d6c0e79a1239df0039ec16Cc80f7A343b6C530e".ToLower(), ChainList.bsc);
    Debug.Log(BSCtransactions.ToJson());
  }


  public async void GetNativeBalance()
  {
    // get BSC native balance for a given address
    NativeBalance BSCbalance = await Moralis.Web3Api.Account.GetNativeBalance("0x4c6Ec2448C243B39Cd1e9E6db0F9bF7436c0c93f".ToLower(), ChainList.bsc);
    Debug.Log(BSCbalance.ToJson());
  }

  public async void fetchTokenBalance()
  {
    List<Erc20TokenBalance> balance = await Moralis.Web3Api.Account.GetTokenBalances("0x3d6c0e79a1239df0039ec16Cc80f7A343b6C530e".ToLower(), ChainList.bsc);
    foreach (Erc20TokenBalance erc20bal in balance)
    {
      Debug.Log(erc20bal.ToJson());
    }
  }

  public async void fetchNFTs()
  {
    NftOwnerCollection polygonNFTs = await Moralis.Web3Api.Account.GetNFTs("0x9512A1Fa0c635F9DF58B3cA17F68Ad3ABf724a4b".ToLower(), ChainList.polygon);
    Debug.Log(polygonNFTs.ToJson());
  }

  #region UNITY_LIFECYCLE

  private void Awake()
  {
      //characterImg.gameObject.SetActive(false);
      if(MoralisUnity.Kits.AuthenticationKit.AuthenticationKitState.Disconnected == _stateObservable.Value || MoralisUnity.Kits.AuthenticationKit.AuthenticationKitState.None == _stateObservable.Value)
    {
      authenticationKit.GetComponent<AuthenticationKit>().Disconnect();
      viewSwitch.GetComponent<ViewSwitchController>().Quit();
    }
      debugLabel.text = "Claiming NFT Metadata...".ToUpper();
  }

  private void OnEnable()
  {

    debugLabel.text = "Enabling...";

    SelectCharacter();
  }

  private void OnDisable()
  {
    debugLabel.text = "Disabling...";
    SelectCharacter();
  }

  public async void fetchNFTsForContract()
  {
    //Moralis.Start();
    //MoralisUser user = await Moralis.GetUserAsync();
    //Debug.Log("address " + user.ethAddress.ToLower());
    NftOwnerCollection polygonNFTs = await Moralis.Web3Api.Account.GetNFTsForContract("0x9512A1Fa0c635F9DF58B3cA17F68Ad3ABf724a4b".ToLower(), "0x15ace62b9f6934211397b802fd5e52eb011a793d", ChainList.polygon);
    //NftOwnerCollection polygonNFTs = await Moralis.Web3Api.Account.GetNFTsForContract("0x9512A1Fa0c635F9DF58B3cA17F68Ad3ABf724a4b".ToLower()/*user.ethAddress.ToLower()*/, "0xf6e7424eeaad84c3c7208030f5241bee657b5338".ToLower(), ChainList.polygon); //0x9512A1Fa0c635F9DF58B3cA17F68Ad3ABf724a4b
    Debug.Log("POLY: " + polygonNFTs.ToJson());
    SelectCharacter();
    Debug.Log("Power ");
  }

  #endregion


  #region PRIVATE_METHODS

  private async void CheckOwnership()
        {
            //We get our wallet address.
            MoralisUser user = await Moralis.GetUserAsync();
            
            _walletAddress = user.ethAddress;
            
            try
            {
              //Moralis.Web3Api.Initialize();
              //Moralis.GetClient().Web3Api.Initialize();
              //NftOwnerCollection polygonNFTs = await Moralis.GetClient().Web3Api.Account.GetNFTsForContract(_walletAddress.ToLower(), "0xf6e7424eeaad84c3c7208030f5241bee657b5338".ToLower(), _deployedChain); //0x9512A1Fa0c635F9DF58B3cA17F68Ad3ABf724a4b
              //Debug.Log("POLLLY: " + polygonNFTs.ToJson());

              debugLabel.text = "Claiming NFT Metadata...".ToUpper();
              NftOwnerCollection noc =
                            await Moralis.Web3Api.Account.GetNFTsForContract(_walletAddress.ToLower(),
                                contractAddress,
                                _deployedChain);
      
              int i = 0;
              Debug.Log("Tokens Size: " + tokenId.Length);
              foreach (String td in tokenId)
              {
                Debug.Log("Id: " + i);
                NftTransferCollection ntc = await Moralis.Web3Api.Account.GetNFTTransfers(_walletAddress, _deployedChain);

                IEnumerable<NftOwner> ownership = from n in noc.Result
                                                  select n;

                IEnumerable<NftTransfer> transfers = from n in ntc.Result
                                                  select n;

                ownership = from n in noc.Result
                                    where n.TokenId.Equals(td)
                                    select n;

                var transferList = transfers.ToList();
                Debug.Log("Transfers: " + transferList.ToList().ToString());//First().Value);
                Debug.Log("ownership.ToList(): " + ownership.ToList());
                var ownershipList = ownership.ToList();
                 Debug.Log("Ownership list" + ownershipList.Any());
                if (ownershipList.Any()) // If I'm the owner :)
                {
                  Debug.Log(ownershipList.First().Metadata);

                  var nftMetaData = ownershipList.First().Metadata;
                  
                  if(nftMetaData != null) 
                  {
                    CustomNftMetadata formattedMetaData = JsonUtility.FromJson<CustomNftMetadata>(nftMetaData);
                    StartCoroutine(GetTexture(formattedMetaData.image, characterImg[i++]));
                  }
                  

                  debugLabel.text = "Success!<br>".ToUpper() + "Select the image to play with the NFT".ToUpper();
                  Debug.Log("Already owns NFT.");
                }
                else
                {
                  debugLabel.text = "You do not own the NFT".ToUpper() + "<br>" + "Press SKIP to play with a local character".ToUpper();
                  Debug.Log("Does not own NFT.");

                  //TODO We could activate a claiming option.
                }
              }
            }
            catch (Exception exp)
            {
                Debug.LogError(exp.Message);
            }
        }

        public void TestCharacterTransaction(int characterLevel, int transaction)
        {
          switch (characterLevel)
          {
            case 1: 
            if(transaction <= 20)
            {
              StaticClass.player.lives = 6;
            }
            break;
          case 2:
            if (transaction <= 30)
            {
              StaticClass.player.lives = 6;
            }
            break;
          case 3:
            if (transaction <= 50)
            {
              StaticClass.player.lives = 6;
            }
            break;
          }
        }
        
        IEnumerator GetTexture(string imageUrl, RawImage ch)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    // Get downloaded asset bundle
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    
                    ch.gameObject.SetActive(true);
                    ch.texture = texture;
                }
            }
        }

        #endregion

        #region EVENT_HANDLERS

        private void SelectCharacter()
        {
          if(characterPanel != null) 
          {
            characterPanel.SetActive(true);
            CheckOwnership();
          }
            
        }

        #endregion
        
        #region PUBLIC_METHODS

        public void NftButtonClicked(int id)
        {
            //TODO
            OnCharacterSelected?.Invoke((Texture2D) characterImg[id].texture);
            Debug.Log("Chosen: " + id);
            switch (id)
            {
              case 1:
                StaticClass.character = character1;
                break;
              case 2:
                StaticClass.character = character2;
                break;
              case 3:
                StaticClass.character = character3;
              break;
            }
            characterPanel.SetActive(false);
            SceneManager.LoadScene("MainMenuSampleScene");
        }
        
        public void SkipButtonClicked()
        {
            //TODO
            OnCharacterSelected?.Invoke(null);
            characterPanel.SetActive(false);
        }

        #endregion
    }

