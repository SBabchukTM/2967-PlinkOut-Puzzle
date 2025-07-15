using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoleSetup", menuName = "Config/HoleSetup")]
public class HoleSetup : ScriptableObject
{
    public List<HoleConfig> HoleConfigs;
}