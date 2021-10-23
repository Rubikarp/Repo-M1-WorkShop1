using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using System.Diagnostics;

public enum Ray3Dir { Left, Right, Up, Down, Forward, Backward }

public class CharaRaycaster3D : MonoBehaviour
{
    [Header("Parameter")]
    public float rayDist = 0.1f;

    public LayerMask mask;
    public Transform self;
    public CapsuleCollider selfCaps;

    public bool CastCapsule(Ray3Dir rayDir)
    {
        bool result = false;

        Vector3 footPos = self.position + selfCaps.center + Vector3.down * selfCaps.height * 0.5f, headPos = self.position + selfCaps.center + Vector3.up * selfCaps.height * 0.5f;

        Vector3 rayDirection = Vector3.zero;
        switch (rayDir)
        {
            case Ray3Dir.Left:
                rayDirection = Vector3.left;
                break;
            case Ray3Dir.Right:
                rayDirection = Vector3.right;
                break;
            case Ray3Dir.Up:
                rayDirection = Vector3.up;
                break;
            case Ray3Dir.Down:
                rayDirection = Vector3.down;
                break;
            case Ray3Dir.Forward:
                rayDirection = Vector3.forward;
                break;
            case Ray3Dir.Backward:
                rayDirection = Vector3.back;
                break;
            default:
                rayDirection = Vector3.zero;
                break;
        }

        RaycastHit hitInfo;

        if (Physics.SphereCast(footPos + (rayDirection * rayDist), selfCaps.radius, rayDirection, out hitInfo, selfCaps.radius, mask, QueryTriggerInteraction.Ignore))
        {
            /*
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(footPos + (rayDirection * rayDist), selfCaps.radius);
            Gizmos.DrawWireSphere(headPos + (rayDirection * rayDist), selfCaps.radius);
            Gizmos.color = Color.white;
            */
            result = true;
        }
        else
        {
            /*
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(footPos + (rayDirection * rayDist), selfCaps.radius);
            Gizmos.DrawWireSphere(headPos + (rayDirection * rayDist), selfCaps.radius);
            Gizmos.color = Color.white;
            */
        }

        return result;
    }
    
    /*public void TestPerf(System.Action act, string debufText)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        //Calcul
        act();

        sw.Stop();
        //Draw perf
        UnityEngine.Debug.Log(debufText + "  " + sw.ElapsedTicks.ToString());
        
        sw.Reset();
    }

    void TestTest()
    {
        //https://docs.microsoft.com/fr-fr/dotnet/csharp/language-reference/operators/lambda-expressions
        TestPerf(() => CastCapsule(Ray3Dir.Forward), "debug perf :");
    }
    */
}
