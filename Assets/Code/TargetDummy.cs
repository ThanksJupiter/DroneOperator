using UnityEngine;

public class TargetDummy : MonoBehaviour, ITarget
{
    public void Hit()
    {
        Debug.Log("Hit: " + gameObject.name);
    }
}
