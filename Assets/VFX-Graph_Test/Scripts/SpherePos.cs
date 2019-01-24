using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SpherePos : MonoBehaviour
{
    public float duration;
    public Vector3 minPos;
    public Vector3 maxPos;
    public VisualEffect vfx;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PosUpdate());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PosUpdate()
    {
        int count = 0;
        while (true)
        {
            yield return new WaitForSeconds(duration);

            if (count % 5 == 0)
            {
                vfx.SetVector3("SpherePos", new Vector3(0, 0, 1.21f));
            }
            else
            {
                var x = Random.Range(minPos.x, maxPos.x);
                var y = Random.Range(minPos.y, maxPos.y);
                var z = Random.Range(minPos.z, maxPos.z);
                vfx.SetVector3("SpherePos", new Vector3(x, y, z));
            }
            count ++;
        }
    }
}
