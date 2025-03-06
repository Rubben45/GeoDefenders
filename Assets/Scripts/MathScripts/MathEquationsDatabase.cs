using System.Collections.Generic;
using UnityEngine;

// Aici se gaseste lista cu toate ecuatiile din joc
[CreateAssetMenu(fileName = "MathEquationsDatabase", menuName = "Math/MathEquationsDatabase")]
public class MathEquationsDatabase : ScriptableObject
{
    public List<MathEquation> equations;
}
