using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Generator : NetworkBehaviour
{
    public NetworkObject[] NoneExitRoom;
    public NetworkObject[] OneExitRoom;
    public NetworkObject[] TwoExitRoom;
    public NetworkObject[] ThreeExitRoom;

    public NetworkObject TemplateMap1;

    public int NumberOfRoom;
    int NumberOfExits;

    public Direction entranceOfRoom;
    public Direction exitOfRoom;

    [SerializeField] Vector3 Start;
    [SerializeField] Vector3 Exit;
    public enum Direction
    {
        left,
        right,
        top,
        bottom,
    }



    


    
    public void Generating()
    {
       Runner.Spawn(TemplateMap1,Start);
    }




    /*
        recupere le point de start
        spawn une tiles a partir du point de start
        random un nombre de sortie
        Start = sortie
        recommence
        
        Si nb de room >= a nombre de room spawn, 
            Stop Spawning de nouvelle salle, Spawn "Boss room" a une salle
        Si Salle.exit = salle déjà créé, change le mur pour que ça soit entré
     */



    /*
     
    spawn un sol 
     
     */
}
