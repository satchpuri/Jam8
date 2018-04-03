using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Player : MonoBehaviour
{
    [SerializeField]
    private Stat size;

    [SerializeField]
    private Stat spawn;

    [SerializeField]
    private Stat recon;

    [SerializeField]
    private Stat attack;

    [SerializeField]
    private Stat collect;

    [SerializeField]
    private Stat size2;

    [SerializeField]
    private Stat spawn2;

    [SerializeField]
    private Stat recon2;

    [SerializeField]
    private Stat attack2;

    [SerializeField]
    private Stat collect2;

    private void Awake()
    {
        size.Initialize();
        spawn.Initialize();
        recon.Initialize();
        attack.Initialize();
        collect.Initialize();
        size2.Initialize();
        spawn2.Initialize();
        recon2.Initialize();
        attack2.Initialize();
        collect2.Initialize();
    }

    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            size.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            size.CurrentVal += 10;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            spawn.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            spawn.CurrentVal += 10;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            recon.CurrentVal -= 10;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            recon.CurrentVal += 10;
        }
    }
}
