using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary
{
  public Dictionary<string, Vector3> itemDictionary = new Dictionary<string, Vector3>() {
    {"Food Tray", new Vector3(-4.34f, 4.73f, -9.7f)},
    {"Pill Bottle Pickup", new Vector3(-7f, 1.4f, -10.3f)}
  };
}
