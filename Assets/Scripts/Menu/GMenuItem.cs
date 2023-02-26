using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMenuItem : MonoBehaviour {
    [SerializeField] private MenuController menuController;
    [SerializeField] private List<GMenuItem> subMenuItems = new List<GMenuItem>();
    private bool subMenuShowed = false;

    public void Back() {
        if(subMenuShowed) {
            subMenuShowed = false;
            menuController.ChangeSubScreen(subMenuItems[0], subMenuItems);
        } else {
            menuController.ChangeToMain();
        }
    }

    public void ShowSub(GMenuItem subMenuItem) {
        subMenuShowed = true;
        menuController.ChangeSubScreen(subMenuItem, subMenuItems);
    }
}