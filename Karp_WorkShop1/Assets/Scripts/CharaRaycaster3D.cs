using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ray3Dir { Left, Right, Up, Down, Forward, Backward }

public class CharaRaycaster3D : MonoBehaviour
{
    [Header("Parameter")]
    public float rayDist = 0.1f;
    [Range(4,8)] public int rayIteration = 4;
    public float skinWidth = 0.02f;

    public Transform self;
    public BoxCollider selfBox;

    public bool ThrowRay(Ray3Dir rayDir)
    {
        bool result = false;
        List<Vector3> origins = new List<Vector3>();
        Vector3 startCorner, endingCorner ;

        #region Definning raycast origins
        startCorner = self.position;
        endingCorner = self.position;
        //Add Collider Offset
        startCorner += selfBox.center;
        endingCorner += selfBox.center;

        //Define each direction Corner
        switch (rayDir)
        {
            case Ray3Dir.Left:
                break;
            case Ray3Dir.Right:
                break;
            case Ray3Dir.Up:
                break;
            case Ray3Dir.Down:

                break;
            case Ray3Dir.Forward:
                break;
            case Ray3Dir.Backward:
                break;
            default:
                break;
        }

        /*
                case RayDir.Down:
                //Go to corners
                startCorner += new Vector2(-selfBox.size.x, -selfBox.size.y) * 0.5f * self.lossyScale;
                endingCorner += new Vector2(selfBox.size.x, -selfBox.size.y) * 0.5f * self.lossyScale;
                //add skin width
                startCorner += new Vector2(-self.up.x, -self.up.y) * skinWidth;
                endingCorner += new Vector2(-self.up.x, -self.up.y) * skinWidth;
                break;
         */

        //Define ray origin
        for (int i = 0; i < rayIteration; i++)
        {
            float step;
            if (rayIteration <= 1)
            {
                step = 0.5f;
            }
            else
            {
                step = (float)1 / (float)(rayIteration-1);
            }

            //Old
            //Vector2 edge = endingCorner - startCorner;
            //origins.Add(startCorner + (edge * (float)i * step));

            origins.Add(Vector2.Lerp(startCorner, endingCorner, (float)i * step));
        }
        #endregion

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

        for (int i = 0; i < origins.Count; i++)
        {
            if (Physics2D.Raycast(origins[i], rayDirection, rayDist))
            {
                Debug.DrawRay(origins[i], rayDirection * rayDist, Color.green);
                result = true;
            }
            else
            {
                Debug.DrawRay(origins[i], rayDirection * rayDist, Color.red);
            }
        }

        return result;
    }
}
