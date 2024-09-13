using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DataManager : singleton<DataManager>
{
    /// <summary>
    /// This script is singlethon to manage the saving between scene and session
    /// The Data between the scene should be saved here 
    /// The Data Between Session are also added in SaveData Class
    /// </summary>
    /// 
    
    // Id of who is playing game
    public string playerId;

    // Varibales for highscore profile
    public string FirstPlayer;
    public int HighScore;

   






    [System.Serializable]
    class SaveDate
    {
        public int HighScore;
        public string FirstPlayer;

    }

    public void SaveHighscore()
    {

    }

   
}
