using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopOpen : MonoBehaviour
{
    public GameObject mainpanel;
    

    public GameObject Storebutton;

    public GameObject CloseButton;

    public GameObject Store;

    public void shopOpens()
    {
        mainpanel.SetActive(false);
        CloseButton.SetActive(true);
        Storebutton.SetActive(true);
        Store.SetActive(false);
    }
    
    public void shopOff()
    {
        mainpanel.SetActive(true);
        CloseButton.SetActive(false);
        Storebutton.SetActive(false);
        Store.SetActive(true);
    }
    
    


}
