using UnityEngine;

[System.Serializable]
public class Blueprints
{
    public string itemName, Req1, Req2;
    public int Req1Amount, Req2Amount, numOfRequirements;

    public Blueprints(string itemName, string Req1, int Req1Amount, string Req2, int Req2Amount, int numOfRequirements)
    {
        this.itemName = itemName;
        this.Req1 = Req1;
        this.Req1Amount = Req1Amount;
        this.Req2 = Req2;
        this.Req2Amount = Req2Amount;
        this.numOfRequirements = numOfRequirements;
    }
}
