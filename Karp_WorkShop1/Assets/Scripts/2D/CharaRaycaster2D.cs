using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RayDir { Left, Right, Up, Down }

public class CharaRaycaster2D : MonoBehaviour
{
    [Header("Parameter")]
    public float rayDist = 0.1f;
    [Range(1,3)] public int rayIteration = 2;
    public float skinWidth = 0.02f;

    public Transform self;
    public BoxCollider2D selfBox;

    public bool ThrowRay(RayDir rayDir)
    {
        bool result = false;
        List<Vector2> origins = new List<Vector2>();
        Vector2 startCorner, endingCorner ;

        #region Definning raycast origins
        startCorner = self.position;
        endingCorner = self.position;
        //Add Collider Offset
        startCorner += selfBox.offset;
        endingCorner += selfBox.offset;
        //Define each direction Corner
        switch (rayDir)
        {
            case RayDir.Left:
                //Go to corners
                startCorner += new Vector2(-selfBox.size.x, -selfBox.size.y ) * 0.5f * self.lossyScale;
                endingCorner += new Vector2(-selfBox.size.x,  selfBox.size.y) * 0.5f * self.lossyScale;
                //add skin width
                startCorner += new Vector2(-self.right.x, -self.right.y) * skinWidth;
                endingCorner += new Vector2(-self.right.x, -self.right.y) * skinWidth;
                break;
            case RayDir.Right:
                //Go to corners
                startCorner += new Vector2(selfBox.size.x, -selfBox.size.y) * 0.5f * self.lossyScale;
                endingCorner += new Vector2(selfBox.size.x, selfBox.size.y) * 0.5f * self.lossyScale;
                //add skin width
                startCorner += new Vector2(self.right.x, self.right.y) * skinWidth;
                endingCorner += new Vector2(self.right.x, self.right.y) * skinWidth;
                break;
            case RayDir.Up:
                //Go to corners
                startCorner += new Vector2(selfBox.size.x, selfBox.size.y) * 0.5f * self.lossyScale;
                endingCorner += new Vector2(-selfBox.size.x, selfBox.size.y) * 0.5f * self.lossyScale;
                //add skin width
                startCorner += new Vector2(self.up.x, self.up.y) * skinWidth;
                endingCorner += new Vector2(self.up.x, self.up.y) * skinWidth;
                break;
            case RayDir.Down:
                //Go to corners
                startCorner += new Vector2(-selfBox.size.x, -selfBox.size.y) * 0.5f * self.lossyScale;
                endingCorner += new Vector2(selfBox.size.x, -selfBox.size.y) * 0.5f * self.lossyScale;
                //add skin width
                startCorner += new Vector2(-self.up.x, -self.up.y) * skinWidth;
                endingCorner += new Vector2(-self.up.x, -self.up.y) * skinWidth;
                break;
        }
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

        Vector2 rayDirection = Vector2.zero;
        switch (rayDir)
        {
            case RayDir.Left:
                rayDirection = Vector2.left;
                break;
            case RayDir.Right:
                rayDirection = Vector2.right;
                break;
            case RayDir.Up:
                rayDirection = Vector2.up;
                break;
            case RayDir.Down:
                rayDirection = Vector2.down;
                break;
            default:
                rayDirection = Vector2.zero;
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
