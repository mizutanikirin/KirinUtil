using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Blob {
    [Tooltip("Number of pixels in the blob.")]
    public int pixels;

    [Tooltip("Blob limits in X (pixels), Y (pixels) & Z (mm).")]
    public int minx, miny, minz, maxx, maxy, maxz;
    //public long sumx, sumy, sumz;


    /// <summary>
    /// Blob constructor.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Blob(int x, int y) {
        minx = maxx = x;
        //sumx = x;

        miny = maxy = y;
        //sumy = y;

        //minz = maxz = z;
        //sumz = z;

        pixels++;
    }

    /// <summary>
    /// Checks if the current blob is inside the given one.
    /// </summary>
    /// <param name="b">Blob</param>
    /// <returns></returns>
    public bool IsInside(Blob b) {
        if (minx >= b.minx && maxx <= b.maxx && miny >= b.miny && maxy <= b.maxy /*&& minz >= b.minz && maxz <= b.maxz*/) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a pixel is near or inside this blob.
    /// </summary>
    /// <param name="pixX">X</param>
    /// <param name="pixY">Y</param>
    /// <param name="pixZ">Z</param>
    /// <param name="difXY">Tolerance in X- & Y-directions (pixels)</param>
    /// <param name="difZ">Tolerance in Z-direction (mm)</param>
    /// <returns></returns>
    public bool IsNearOrInside(int pixX, int pixY, int difXY) {
        if (pixX >= (minx - difXY) && pixX <= (maxx + difXY) && pixY >= (miny - difXY) && pixY <= (maxy + difXY)) {
            //if (pixZ >= (minz - difZ) && pixZ <= (maxz + difZ)) {
            return true;
            //}
        }

        return false;
    }

    /// <summary>
    /// Adds a pixel to the blob.
    /// </summary>
    /// <param name="pixX">X</param>
    /// <param name="pixY">Y</param>
    /// <param name="pixZ">Z</param>
    public void AddDepthPixel(int pixX, int pixY) {
        pixels++;

        minx = Mathf.Min(minx, pixX);
        miny = Mathf.Min(miny, pixY);
        //minz = Mathf.Min(minz, pixZ);

        maxx = Mathf.Max(maxx, pixX);
        maxy = Mathf.Max(maxy, pixY);
        //maxz = Mathf.Max(maxz, pixZ);

        //sumx += pixX;
        //sumy += pixY;
        //sumz += pixZ;
    }

    /// <summary>
    /// Gets the blob width.
    /// </summary>
    /// <returns>Blob width</returns>
    public float GetWidth() {
        return maxx - minx;
    }

    /// <summary>
    /// Gets the blob height.
    /// </summary>
    /// <returns>Blob height</returns>
    public float GetHeight() {
        return maxy - miny;
    }

    /// <summary>
    /// Gets the blob center.
    /// </summary>
    /// <returns>Blob center</returns>
    public Vector3 GetBlobCenter() {
        return new Vector3((minx + maxx) / 2, (miny + maxy) / 2, 0/*(minz + maxz) / 2*/);
    }

}