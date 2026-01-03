using UnityEngine;

public class Target  : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Dart dart))
        {
            return;
        }
        
        dart.StopDart();
    }
}
