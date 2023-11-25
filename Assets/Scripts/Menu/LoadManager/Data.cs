
using System.IO;
using UnityEngine;
[System.Serializable]
public class Data
{
    private float playerPosX;

	public float PlayerPosX
	{
		get{ return playerPosX; }
        set{ playerPosX = value; }
	}

    private float playerPosY;

	public float PlayerPosY
	{
		get{ return playerPosY; }
        set{ playerPosY = value; }

	}

    private float playerPosZ;

	public float PlayerPosZ
	{
		get{ return playerPosZ; }
        set{ playerPosZ = value; }

	}
    private float playerRotX;

	public float PlayerRotX
	{
		get{ return playerRotX; }
        set{ playerRotX = value; }
	}
    
    private float playerRotY;

	public float PlayerRotY
	{
		get{ return playerRotY; }
        set{ playerRotY = value; }
	}

    private float playerRotZ;

	public float PlayerRotZ
	{
		get{ return playerRotZ; }
        set{ playerRotZ = value; }
	}

    private int hour;
    public int Hour{
        get{ return hour; }
        set{ hour = value; }
    }

	private int stage;
    public int Stage{
        get{ return stage; }
        set{ stage = value; }
    }

}
