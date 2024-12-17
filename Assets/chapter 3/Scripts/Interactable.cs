using Sirenix.OdinInspector;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [PropertyOrder(1000)] public bool interactable = false;
    [PropertyOrder(1001)] public float ActionDistance = 1;


    public virtual void Update()
    {
        DebugPlus.DrawWireSphere(transform.position, ActionDistance).Color(Color.red);
    }


    public virtual void InteractStart()
    {
        // Runs when E is Pressed on the Object
    }

    public virtual void InteractEnd()
    {
        // Runs when E is Released on the Object
    }

        public virtual void MouseOver()
    {
        // Runs when the mouse Hovers Over this
    }

    public virtual void MouseExit()
    {
        // Runs when the mouse Exits this
    }


    
}
