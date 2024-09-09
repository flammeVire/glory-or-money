using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class Shop : NetworkBehaviour
{
    public ItemScriptable[] item;
    public ItemScriptable[] ItemInShop;

    public Dictionary<GameObject, Network_Player> PlayerList = new Dictionary<GameObject, Network_Player>();

    public TextMeshProUGUI[] NameText;
    public TextMeshProUGUI[] PriceText;
    public GameObject ShopCanva;
    public Transform[] ItemInShopTransform;

    private void Start()
    {
        SelectItem();
    }

    #region Trigger Detection

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7 && other.gameObject.GetComponent<Network_Player>().CurrentClass != PlayerScriptable.PossibleClass.None) 
        {
            PlayerList.Add(other.gameObject,other.gameObject.GetComponent<Network_Player>());
            ShowUI(other.gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 && other.gameObject.GetComponent<Network_Player>().CurrentClass != PlayerScriptable.PossibleClass.None)
        {

            ShowUI(other.gameObject, false);
            PlayerList.Remove(other.gameObject);
            
        }
    }

    #endregion

    #region UI
    void ShowUI(GameObject IsPlayer, bool Show)
    {
        foreach(var player in PlayerList)
        {
            if(player.Key == IsPlayer)
            {
                ShopCanva.SetActive(Show);
            }
        }
            
        
    }

    void InstantiateItemInShop()
    {
        for (int i = 0; i < ItemInShop.Length; i++)
        {
            GameObject itemselect = ItemInShop[i].Image;
            GameObject item = Instantiate(itemselect, ItemInShopTransform[i].position, Quaternion.identity, ItemInShopTransform[i]);
            Button itemButton = item.GetComponent<Button>();
            itemButton.onClick.AddListener(() => OnItemClicked(itemButton));
        }

        ResetUIPostion();


    }

    void ResetUIPostion()
    {
        for(int i = 0; i < 5; i++)
        {
            Transform slot = ItemInShopTransform[i];
            slot.GetChild(0).localPosition = Vector3.zero;
        }
    }

    void SetPrice() 
    { 
        for(int i = 0;i < ItemInShop.Length; i++)
        {
            PriceText[i].text = ItemInShop[i].Price.ToString();
        }
    }

    void SetName() 
    {
        for (int i = 0; i < ItemInShop.Length; i++)
        {
            NameText[i].text = ItemInShop[i].Name;
        }
    }

    #endregion

    #region ItemInShop

    void SelectItem()
    {
        Debug.Log("SelectingItem");
        List<int> index = new List<int>();
        for (int i = 0; i < 5 ; i++)
        {
            int newIndex = Utilitaire.GetIntExcluding(10, index);
            ItemInShop[i] = item[newIndex];
            index.Add(newIndex);
        }
        InstantiateItemInShop();
        SetPrice();
        SetName();
    }

    void ItemBuy(GameObject itemObj)
    {
        Debug.Log(itemObj.name);
        for (int i = 0; i < ItemInShop.Length; i++)
        {
            string SearchingName = ItemInShop[i].Image.name + "(Clone)";
            Debug.Log(SearchingName);
            if (SearchingName == itemObj.name)
            {
                if (PlayerHasMoney(Runner.LocalPlayer, ItemInShop[i]))
                {
                    PlayerPaying(Runner.LocalPlayer, ItemInShop[i]);
                    NameText[i].text = "Sold Out";
                    PriceText[i].text = "";
                    Destroy(itemObj);
                    break;
                }
            }
        }
    }

    
    public void OnItemClicked(Button button)
    {
       
            Debug.Log("Click");
            ItemBuy(button.gameObject);
        
        
    }

    public bool PlayerHasMoney(PlayerRef player, ItemScriptable item)
    {
        NetworkObject playerObject = Runner.GetPlayerObject(player);
        Network_Player NetPlayer = playerObject.GetComponent<Network_Player>();

        Debug.Log("PlayerRef =" + player);
        Debug.Log("PlayerOBJ =" + playerObject);

        Debug.Log("NetPlayer =" + NetPlayer);
        if(NetPlayer.TotalGold < item.Price)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    public void PlayerPaying(PlayerRef player,ItemScriptable item) 
    {
        NetworkObject playerObject = Runner.GetPlayerObject(player);
        Network_Player NetPlayer = playerObject.GetComponent<Network_Player>();

        NetPlayer.TotalGold -= item.Price;
        NetPlayer.GoldToBank();
    }

    #endregion



}
