using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIScript : MonoBehaviour
{
    MenuManager _menuManager;

    void Start()
    {
        _menuManager = MenuManager.Instance;
    }


    public void ChangePageState()
    {
        _menuManager.ChangePageState();   
    }
}
