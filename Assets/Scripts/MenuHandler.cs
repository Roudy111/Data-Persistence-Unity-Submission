using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Security.Policy;


public class MenuHandler : MonoBehaviour
{
    public TMP_InputField TM_PlayeNameInput;

    private void Start()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene(1);
        
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#endif
    }
    public void PlayerName()
    {
        DataManager.Instance.Save_playerName = TM_PlayeNameInput.text;
        


    }




}
