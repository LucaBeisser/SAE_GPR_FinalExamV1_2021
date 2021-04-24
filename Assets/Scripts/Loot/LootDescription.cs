using UnityEngine;

[CreateAssetMenu]
public class LootDescription : ScriptableObject
{
    [SerializeField] private DropProbabilityPair[] drops;

    public void SetDrops(params DropProbabilityPair[] drops)
    {
        this.drops = drops;
    }

    public Drop SelectDropRandomly()
    {        
        for (int i = 0; i < drops.Length; i++)
        {
            //select an random item out of array, so that the drop rates are correct
            int rndIndex = Random.Range(0, drops.Length);

            float rnd = Random.value;
            DropProbabilityPair pair = drops[rndIndex];

            if (rnd < pair.Probability)
            {
                return pair.Drop;
            }
        }

        //to an ensure that always omething is droped -> drop an random item out of array
        return drops[Random.Range(0, drops.Length)].Drop;
    }
}

[System.Serializable]
public struct DropProbabilityPair
{
    public Drop Drop;

    [Range(0, 1)]
    public float Probability;
}
