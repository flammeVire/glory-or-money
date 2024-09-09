using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public Network_Player NetPlayer;


    public bool IsSpell1;
    public bool IsSpell2;
    public bool IsSpell3;

    private void Start()
    {
        StartCoroutine(UsingSpell1());
        StartCoroutine(UsingSpell2());
        StartCoroutine(UsingSpell3());

    }


    // créé fonction pour si changement de class IsSpell == false
    IEnumerator UsingSpell1()
    {
        IsSpell1 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell1);
        IsSpell1 = true;
        Debug.Log("using spell1 : " + NetPlayer.UsingSpell1);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell1);
        StartCoroutine(UsingSpell1());
    }
    IEnumerator UsingSpell2()
    {
        IsSpell2 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell2);
        IsSpell2 = true;
        Debug.Log("using spell2 : " + NetPlayer.UsingSpell2);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell2);

        StartCoroutine(UsingSpell2());
    }
    IEnumerator UsingSpell3()
    {
        IsSpell3 = false;
        yield return new WaitUntil(() => NetPlayer.UsingSpell3);
        IsSpell3 = true;
        Debug.Log("using spell3 : " + NetPlayer.UsingSpell3);
        yield return new WaitForSeconds(NetPlayer.PlayerScriptableClone.DelaySpell3);

        StartCoroutine(UsingSpell3());
    }
}
