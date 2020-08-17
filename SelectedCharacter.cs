using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject player_info_pref;
    public void Selected()
    {
        PlayerPrefs.SetString("player_info_pref", this.gameObject.tag);
    }

}
