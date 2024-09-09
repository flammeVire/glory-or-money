using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Utilitaire
{
    public static Vector3 GetRandomVectorFromOrigin(float range, float hauteur, Vector3 Origin)
    {
       return new Vector3(Origin.x + Random.Range(-range, range), hauteur, Origin.z + Random.Range(-range, range));
    }

    public static string GetRandomName(int length)
    {
        string characterList = "abcdefghijklmnopqrstuvwxyz";
        string name = "";
        for (int i = 0; i < length; i++)
        {
            char c = characterList[Random.Range(0, characterList.Length)];
            name += c;
        }
        
        return name;
    }
    
    public static int GetIntExcluding(int Lenght, List<int> Exclusion)
    {
        int number;

        do
        {
            number = Random.Range(0, Lenght);
        }
        while (Exclusion.Contains(number));

        Debug.Log(number);
        return number;
    }
}
