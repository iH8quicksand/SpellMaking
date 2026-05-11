using System;
using System.Collections.Generic;

[Serializable]
public class Spawn
{
    public string enemy;
    public string count;
    public string hp;
    public int delay;
    public string damage; // added this because some of the enemies have damage attribute
    public int[] sequence;
    public string location;
}