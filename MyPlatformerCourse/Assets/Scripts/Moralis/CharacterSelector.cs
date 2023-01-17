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
//[{"inputs":[],"stateMutability":"nonpayable","type":"constructor"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"address","name":"account","type":"address"},{ "indexed":true,"internalType":"address","name":"operator","type":"address"},{ "indexed":false,"internalType":"bool","name":"approved","type":"bool"}],"name":"ApprovalForAll","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"address","name":"previousOwner","type":"address"},{ "indexed":true,"internalType":"address","name":"newOwner","type":"address"}],"name":"OwnershipTransferred","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":false,"internalType":"address","name":"account","type":"address"}],"name":"Paused","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"bytes32","name":"role","type":"bytes32"},{ "indexed":true,"internalType":"bytes32","name":"previousAdminRole","type":"bytes32"},{ "indexed":true,"internalType":"bytes32","name":"newAdminRole","type":"bytes32"}],"name":"RoleAdminChanged","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"bytes32","name":"role","type":"bytes32"},{ "indexed":true,"internalType":"address","name":"account","type":"address"},{ "indexed":true,"internalType":"address","name":"sender","type":"address"}],"name":"RoleGranted","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"bytes32","name":"role","type":"bytes32"},{ "indexed":true,"internalType":"address","name":"account","type":"address"},{ "indexed":true,"internalType":"address","name":"sender","type":"address"}],"name":"RoleRevoked","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"address","name":"operator","type":"address"},{ "indexed":true,"internalType":"address","name":"from","type":"address"},{ "indexed":true,"internalType":"address","name":"to","type":"address"},{ "indexed":false,"internalType":"uint256[]","name":"ids","type":"uint256[]"},{ "indexed":false,"internalType":"uint256[]","name":"values","type":"uint256[]"}],"name":"TransferBatch","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":true,"internalType":"address","name":"operator","type":"address"},{ "indexed":true,"internalType":"address","name":"from","type":"address"},{ "indexed":true,"internalType":"address","name":"to","type":"address"},{ "indexed":false,"internalType":"uint256","name":"id","type":"uint256"},{ "indexed":false,"internalType":"uint256","name":"value","type":"uint256"}],"name":"TransferSingle","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":false,"internalType":"string","name":"value","type":"string"},{ "indexed":true,"internalType":"uint256","name":"id","type":"uint256"}],"name":"URI","type":"event"},{ "anonymous":false,"inputs":[{ "indexed":false,"internalType":"address","name":"account","type":"address"}],"name":"Unpaused","type":"event"},{ "inputs":[],"name":"DEFAULT_ADMIN_ROLE","outputs":[{ "internalType":"bytes32","name":"","type":"bytes32"}],"stateMutability":"view","type":"function"},{ "inputs":[],"name":"MINTER_ROLE","outputs":[{ "internalType":"bytes32","name":"","type":"bytes32"}],"stateMutability":"view","type":"function"},{ "inputs":[],"name":"PAUSER_ROLE","outputs":[{ "internalType":"bytes32","name":"","type":"bytes32"}],"stateMutability":"view","type":"function"},{ "inputs":[],"name":"URI_SETTER_ROLE","outputs":[{ "internalType":"bytes32","name":"","type":"bytes32"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"address","name":"account","type":"address"},{ "internalType":"uint256","name":"id","type":"uint256"}],"name":"balanceOf","outputs":[{ "internalType":"uint256","name":"","type":"uint256"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"address[]","name":"accounts","type":"address[]"},{ "internalType":"uint256[]","name":"ids","type":"uint256[]"}],"name":"balanceOfBatch","outputs":[{ "internalType":"uint256[]","name":"","type":"uint256[]"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"bytes32","name":"role","type":"bytes32"}],"name":"getRoleAdmin","outputs":[{ "internalType":"bytes32","name":"","type":"bytes32"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"bytes32","name":"role","type":"bytes32"},{ "internalType":"address","name":"account","type":"address"}],"name":"grantRole","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"bytes32","name":"role","type":"bytes32"},{ "internalType":"address","name":"account","type":"address"}],"name":"hasRole","outputs":[{ "internalType":"bool","name":"","type":"bool"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"address","name":"account","type":"address"},{ "internalType":"address","name":"operator","type":"address"}],"name":"isApprovedForAll","outputs":[{ "internalType":"bool","name":"","type":"bool"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"address","name":"account","type":"address"},{ "internalType":"uint256","name":"id","type":"uint256"},{ "internalType":"uint256","name":"amount","type":"uint256"},{ "internalType":"string","name":"url","type":"string"},{ "internalType":"bytes","name":"data","type":"bytes"}],"name":"mint","outputs":[],"stateMutability":"payable","type":"function"},{ "inputs":[{ "internalType":"address","name":"to","type":"address"},{ "internalType":"uint256[]","name":"ids","type":"uint256[]"},{ "internalType":"uint256[]","name":"amounts","type":"uint256[]"},{ "internalType":"string[]","name":"urls","type":"string[]"},{ "internalType":"bytes","name":"data","type":"bytes"}],"name":"mintBatch","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"address","name":"","type":"address"},{ "internalType":"address","name":"","type":"address"},{ "internalType":"uint256[]","name":"","type":"uint256[]"},{ "internalType":"uint256[]","name":"","type":"uint256[]"},{ "internalType":"bytes","name":"","type":"bytes"}],"name":"onERC1155BatchReceived","outputs":[{ "internalType":"bytes4","name":"","type":"bytes4"}],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"address","name":"","type":"address"},{ "internalType":"address","name":"","type":"address"},{ "internalType":"uint256","name":"","type":"uint256"},{ "internalType":"uint256","name":"","type":"uint256"},{ "internalType":"bytes","name":"","type":"bytes"}],"name":"onERC1155Received","outputs":[{ "internalType":"bytes4","name":"","type":"bytes4"}],"stateMutability":"nonpayable","type":"function"},{ "inputs":[],"name":"owner","outputs":[{ "internalType":"address","name":"","type":"address"}],"stateMutability":"view","type":"function"},{ "inputs":[],"name":"pause","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[],"name":"paused","outputs":[{ "internalType":"bool","name":"","type":"bool"}],"stateMutability":"view","type":"function"},{ "inputs":[],"name":"renounceOwnership","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"bytes32","name":"role","type":"bytes32"},{ "internalType":"address","name":"account","type":"address"}],"name":"renounceRole","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"bytes32","name":"role","type":"bytes32"},{ "internalType":"address","name":"account","type":"address"}],"name":"revokeRole","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"address","name":"from","type":"address"},{ "internalType":"address","name":"to","type":"address"},{ "internalType":"uint256[]","name":"ids","type":"uint256[]"},{ "internalType":"uint256[]","name":"amounts","type":"uint256[]"},{ "internalType":"bytes","name":"data","type":"bytes"}],"name":"safeBatchTransferFrom","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"address","name":"from","type":"address"},{ "internalType":"address","name":"to","type":"address"},{ "internalType":"uint256","name":"id","type":"uint256"},{ "internalType":"uint256","name":"amount","type":"uint256"},{ "internalType":"bytes","name":"data","type":"bytes"}],"name":"safeTransferFrom","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"address","name":"operator","type":"address"},{ "internalType":"bool","name":"approved","type":"bool"}],"name":"setApprovalForAll","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"string","name":"newuri","type":"string"}],"name":"setURI","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"bytes4","name":"interfaceId","type":"bytes4"}],"name":"supportsInterface","outputs":[{ "internalType":"bool","name":"","type":"bool"}],"stateMutability":"view","type":"function"},{ "inputs":[{ "internalType":"address","name":"newOwner","type":"address"}],"name":"transferOwnership","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[],"name":"unpause","outputs":[],"stateMutability":"nonpayable","type":"function"},{ "inputs":[{ "internalType":"uint256","name":"","type":"uint256"}],"name":"uri","outputs":[{ "internalType":"string","name":"","type":"string"}],"stateMutability":"view","type":"function"}]
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

        private DatabaseAccess databaseAccess;



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

  void Start()
  {
    databaseAccess = GameObject.FindGameObjectWithTag("DatabaseAccess").GetComponent<DatabaseAccess>();
  }
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

    if (!Moralis.Web3Api.IsInitialized){
      Moralis.Start();
    }
      
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
              NftTransferCollection ntc = await Moralis.Web3Api.Account.GetNFTTransfers(_walletAddress, _deployedChain);
              var task = databaseAccess.GetScoresFromDataBase();
              var result = await task;

              //bool validNFT = false;
              Debug.Log("Tokens Size: " + tokenId.Length);
              int i = 0;

            foreach(String td in tokenId)
            {
                Debug.Log("Id: " + td);      
                Debug.Log("OwnerShip " + noc.ToJson());
                Debug.Log("Transfership " + ntc.ToJson());
                IEnumerable<NftOwner> ownership = from n in noc.Result
                                                  where n.TokenId.Equals(td)
                                                  select n;     
                
                var ownershipList = ownership.ToList();
                Debug.Log(ownershipList.LongCount());                
                if(ownershipList.LongCount() == 0)
                {
                  //continue;
                }             

                Debug.Log(ownershipList.First().Metadata);
                IEnumerable<NftTransfer> transfers = from n in ntc.Result
                                                     where n.TokenId.Equals(td) //And newer than last checkup
                                                     select n;
                /*ownership = from n in noc.Result
                                    where n.TokenId.Equals(td)
                                    select n;*/

                //var transferList = transfers.ToList();
                //Debug.Log("Transfers: " + transferList.LongCount());//First().Value);
                if(transfers.LongCount() == 0)
                {
                  //continue;
                }   
                if (ownershipList.Any()) // If I'm the owner :)
                {
                  Debug.Log(ownershipList.First().Metadata);
                  //int i = 0;
                  /*foreach(NftOwner own in ownership)   
                  {
                    /* Debug.Log("Owner: " + own.ToJson());
                    
                    foreach(NftTransfer nft in transfers)
                    {

                      Debug.Log("NFT Transfers: " + nft.ToJson());
              
                      if (DateTime.Compare(DateTime.Parse(nft.BlockTimestamp), DateTime.Parse(result[i].DateOfLastValidTransaction)) > 0)
                      {
                        if (TestCharacterTransaction(GetPlayableCharacter(own.TokenId).GetComponent<Player>().level, double.Parse(nft.Value))) 
                        {
                          databaseAccess.UpdateDateOfLastValidTransaction(nft.BlockTimestamp, Int32.Parse(own.TokenId));
                          validNFT = true;
                        }
                      }
                    }
                    validNFT = true;
                    */
            //String imageData = ownershipList.First().Metadata;
            var nftMetaData = ownershipList.First().Metadata;
                    //var nftMetaData = imageData.Substring(imageData.IndexOf("image") + 7, imageData.IndexOf("}") - (imageData.IndexOf("image") + 7));
                    Debug.Log(nftMetaData);
                    if (nftMetaData != null /*&& validNFT*/)
                    {
                      CustomNftMetadata formattedMetaData = JsonUtility.FromJson<CustomNftMetadata>(nftMetaData);
                      Debug.Log("Image" + formattedMetaData.image);
                      StartCoroutine(GetTexture(formattedMetaData.image, characterImg[i++]));
                    
                      //Enable NFT Button
                      debugLabel.text = "Success!<br>".ToUpper() + "Select the image to play with the NFT".ToUpper();
                      Debug.Log("Already owns NFT.");
                      //When Enabled move onto next tokenID
                      //break;
                    }
                    //debugLabel.text = "Success!<br>".ToUpper() + "Select the image to play with the NFT".ToUpper();
                    //Debug.Log("Already owns NFT.");

                  //}
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

        public GameObject GetPlayableCharacter(string nftID)
        {
          switch (nftID)
          {
            case "637994607936885724":
              return character1;
            case "637994607579971521":
              return character2;
            case "637994592393252661":
              return character3;
            default:
              return null;
          }
        }
        public bool TestCharacterTransaction(int characterLevel, double transaction)
        {
          switch (characterLevel)
          {
            case 1: 
            if(transaction >= 20)
            {
              StaticClass.player.lives = 6;
              return true;
            }
            return false;
            case 2:
            if (transaction >= 30)
            {
              StaticClass.player.lives = 6;
              return true;
            }
            return false;
            case 3:
            if (transaction >= 50)
            {
              StaticClass.player.lives = 6;
              return true;
            }
            return false;
          default:
            return false;
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
                StaticClass.level = 1;
                break;
              case 2:
                StaticClass.character = character2;
                StaticClass.level = 2;
              break;
              case 3:
                StaticClass.character = character3;
                StaticClass.level = 3;
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

