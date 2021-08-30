using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObjectSigleton<GameSettings>
{
    [SerializeField] private string _gameVersion = "0.0.0";

    public string GameVersion { get { return _gameVersion; } }

    [SerializeField] private string _nickName = "Anonymous";

    public string Nickname 
    { 
        get 
        { 
            int value = Random.Range(0, 9999);
            return _nickName + value.ToString();
        }
    }
}
