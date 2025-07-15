using UnityEngine;

[CreateAssetMenu(fileName = "Hole Config", menuName = "Config/HoleConfig")]
public class HoleConfig : ScriptableObject
{
    public int Level;
    public Color HoleColor;
    public int Score;
    public bool IsGolden;
}