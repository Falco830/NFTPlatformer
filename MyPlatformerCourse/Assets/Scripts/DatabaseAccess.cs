using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using UnityImage = UnityEngine.UI.Image;
using Image = System.Drawing.Image;
using UnityEditor;
using System.Text;

public class DatabaseAccess : MonoBehaviour
{

  MongoClient client = new MongoClient("mongodb+srv://Foxo940:MultiShine@cluster0.o04ygfi.mongodb.net/?retryWrites=true&w=majority");

  IMongoDatabase database;
  //IMongoCollection<BsonDocument> collection;
  IMongoCollection<BsonDocument> collection;
  // Start is called before the first frame update

  [SerializeField] private UnityImage champImage;
  void Start()
    {
    database = client.GetDatabase("CastleClimb");
    collection = database.GetCollection<BsonDocument>("CharacterCollection");
    //SaveScoreToDataBase("Shire", 90, 30, 634, 5, 1, 2, 331, false, ImageToBytes(champImage), DateTime.Today.ToString(), 5);
    //SaveScoreToDataBase("Rohan", 300, 3, 6, 5, 10);
    //SaveScoreToDataBase("SpoungeBob", 800);
  }
  public byte[] ImageToBytes(UnityImage unityImage)
  {
    Texture2D textureByte = (Texture2D)unityImage.mainTexture;
    byte[] pngData = textureByte.EncodeToPNG();
    //byte[] temp = File.ReadAllBytes(AssetDatabase.GetAssetPath(unityImage.sprite));
    Debug.Log("Temp it" + pngData);

    //Image image = unityImage.GetComponent<UnityImage>();
    return pngData;
    /*using (var ms = new MemoryStream())
    {
      image.Save(ms, image.RawFormat);
      return ms.ToArray();
    }*/
  }
  public void UpdateDateOfLastValidTransaction(string date, int championID)
  {

    var filter = Builders<BsonDocument>.Filter.Eq("ChampionID", championID);
    var update = Builders<BsonDocument>.Update.Set("DateOfLastValidTransaction", date);
    collection.UpdateOne(filter, update);
  }

  public async void SaveScoreToDataBase(string characterName, int score, int lives, int health, int damage, int worldLevel, int playerLevel, int championID, bool dirty, byte[] championImage, string dateOfLastValidTransaction, int jumpingPower)
      {

        var characterCollection = new CharacterCollection
        {
          CharacterName = characterName,
          Score = score,
          Lives = lives,
          Health = health,
          Damage = damage,
          WorldLevel = worldLevel,
          PlayerLevel = playerLevel,
          ChampionID = championID,
          ChampionImage = championImage,
          DateOfLastValidTransaction = dateOfLastValidTransaction,
          Dirty = dirty,
          JumpingPower = jumpingPower
        };

        var document = BsonSerializer.Deserialize<BsonDocument>(characterCollection.ToJson());

        //var document = new BsonDocument { { userName, score }, { "Lives", lives }, { "Health", health }, { "Damage", damage }, {"Jumping Power",jumpingPower } };
        await collection.InsertOneAsync(document);
      }

      public async Task<List<CharacterCollection>> GetScoresFromDataBase()
      {
        
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;
        //;
        List<CharacterCollection> highscores = new List<CharacterCollection>();
        foreach(var score in scoresAwaited.ToList())
        {
          highscores.Add(Deserialize(score.ToString()));
        }
        return highscores;
  }

      private CharacterCollection Deserialize(string rawJson)
      {
        var highScore = new CharacterCollection();

        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4); // gets rid of ID

        var username = stringWithoutID.Substring(stringWithoutID.IndexOf("CharacterName") + 18, stringWithoutID.IndexOf("Score") - (stringWithoutID.IndexOf("CharacterName") + 22)); //grabs first index of username
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf("Score") + 9, stringWithoutID.IndexOf("Lives") - (stringWithoutID.IndexOf("Score") + 12)); // grabse second index of score
        var lives = stringWithoutID.Substring(stringWithoutID.IndexOf("Lives") + 9, stringWithoutID.IndexOf("Health") - (stringWithoutID.IndexOf("Lives") + 12));
        var health = stringWithoutID.Substring(stringWithoutID.IndexOf("Health") + 10, stringWithoutID.IndexOf("Damage") - (stringWithoutID.IndexOf("Health") + 13));
        var Damage = stringWithoutID.Substring(stringWithoutID.IndexOf("Damage") + 10, stringWithoutID.IndexOf("WorldLevel") - (stringWithoutID.IndexOf("Damage") + 13));
        var WorldLevel = stringWithoutID.Substring(stringWithoutID.IndexOf("WorldLevel") + 14, stringWithoutID.IndexOf("PlayerLevel") - (stringWithoutID.IndexOf("WorldLevel") + 17));
        var PlayerLevel = stringWithoutID.Substring(stringWithoutID.IndexOf("PlayerLevel") + 15, stringWithoutID.IndexOf("ChampionID") - (stringWithoutID.IndexOf("PlayerLevel") + 18));

        var ChampionID = stringWithoutID.Substring(stringWithoutID.IndexOf("ChampionID") + 14, stringWithoutID.IndexOf("Dirty") - (stringWithoutID.IndexOf("ChampionID") + 17));
        var Dirty = stringWithoutID.Substring(stringWithoutID.IndexOf("Dirty") + 9, stringWithoutID.IndexOf("ChampionImage") - (stringWithoutID.IndexOf("Dirty") + 12));
        var ChampionImage = stringWithoutID.Substring(stringWithoutID.IndexOf("ChampionImage") + 17, stringWithoutID.IndexOf("DateOfLastValidTransaction") - (stringWithoutID.IndexOf("ChampionImage") + 20));
        var dateOfLastValidTransaction = stringWithoutID.Substring(stringWithoutID.IndexOf("DateOfLastValidTransaction") + 31, stringWithoutID.IndexOf("JumpingPower") - (stringWithoutID.IndexOf("DateOfLastValidTransaction") + 35));
        var JumpingPower = stringWithoutID.Substring(stringWithoutID.IndexOf("JumpingPower") + 16, stringWithoutID.IndexOf("}") - (stringWithoutID.IndexOf("JumpingPower") + 17));

          highScore.CharacterName = username;
          highScore.Score = Convert.ToInt32(score);
          highScore.Lives = Convert.ToInt32(lives);
          highScore.Health = Convert.ToInt32(health);
          highScore.Damage = Convert.ToInt32(Damage);
          highScore.WorldLevel = Convert.ToInt32(WorldLevel);
          highScore.PlayerLevel = Convert.ToInt32(PlayerLevel);
          highScore.Dirty = Convert.ToBoolean(Dirty);
          highScore.ChampionImage = Encoding.ASCII.GetBytes(ChampionImage);
          highScore.DateOfLastValidTransaction = dateOfLastValidTransaction;
          highScore.JumpingPower = Convert.ToInt32(JumpingPower);

          return highScore;
      }
      /*private async Task<string> SaveToMongoAtlas(string name, string data)
      {
        var fileName = "D:\\Untitled.png";
        var newFileName = "D:\\new_Untitled.png";
        using (var fs = new FileStream(fileName, FileMode.Open))
        {
          var gridFsInfo = database.GridFS.Upload(fs, fileName);
          var fileId = gridFsInfo.Id;

          ObjectId oid = new ObjectId(fileId);
          var file = database.GridFS.FindOne(Query.EQ("_id", oid));

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
        return 
      }*/

  public async void CreateCharacterToDataBase(string userName, int score, int lives, int health, int damage, int worldLevel, int playerLevel, int championID, bool dirty, byte[] championImage, string dateOfLastValidTransaction, int jumpingPower)
  {

    var characterCollection = new CharacterCollection
    {
      CharacterName = userName,
      Score = score,
      Lives = lives,
      Health = health,
      Damage = damage,
      WorldLevel = worldLevel,
      PlayerLevel = playerLevel,
      ChampionID = championID,
      Dirty = dirty,
      ChampionImage = championImage,
      DateOfLastValidTransaction = dateOfLastValidTransaction,
      JumpingPower = jumpingPower
    };

    var document = BsonSerializer.Deserialize<BsonDocument>(characterCollection.ToJson());

    //var document = new BsonDocument { { userName, score }, { "Lives", lives }, { "Health", health }, { "Damage", damage }, {"Jumping Power",jumpingPower } };
    await collection.InsertOneAsync(document);
  }

  public class CharacterCollection
  {
        public string CharacterName { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }
        public int WorldLevel { get; set; }
        public int PlayerLevel { get; set; }
        public int ChampionID { get; set; }
        public bool Dirty { get; set; }
        public byte[] ChampionImage { get; set; }
        public string DateOfLastValidTransaction { get; set; }
        public int JumpingPower { get; set; }
  }

}
