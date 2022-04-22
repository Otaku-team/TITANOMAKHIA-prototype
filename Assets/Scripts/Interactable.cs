using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    #region variables
    public float radius = .6f;
    public string interactbleText;
    #endregion

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Interact with object");
    }
}
