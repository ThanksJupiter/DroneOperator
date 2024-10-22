using UnityEngine;

namespace Operator
{
    public class AimLaser : MonoBehaviour
    {
        public Transform laserEnd;
        public LineRenderer laser;

        public void UpdatePosition()
        {
            laser.SetPosition(0, transform.position);
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                laser.SetPosition(1, hit.point);
                laserEnd.position = hit.point + hit.normal * .1f;
            }
            else
            {
                laser.SetPosition(1, transform.forward * 100f);
                laserEnd.position = transform.position + transform.forward * 100f;
            }
        }
    }
}