using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue/DialogueContainer")]
public class DialogueContainer : ScriptableObject
{
    public List<string> lines;
    public Actor actor;
}
