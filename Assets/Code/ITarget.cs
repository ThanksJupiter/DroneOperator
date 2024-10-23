using UnityEngine;

public interface ITarget
{
    public void Hit();
    public Vector3 WorldPosition { get; }
}
