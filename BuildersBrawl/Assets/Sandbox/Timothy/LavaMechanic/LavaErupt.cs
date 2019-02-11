using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaErupt : MonoBehaviour
{
    public Vector3 targetPos;
    public float movePerFrame;
    public float dissolveTime;
    public float cooldownTime;

    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
        StartCoroutine("LavaBoil");
    }

    private IEnumerator LavaBoil()
    {
        bool dissolveLava = false;

        while (true)
        {
            if (!dissolveLava)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, movePerFrame * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPos) <= .2f)
                {
                    dissolveLava = true;
                    yield return new WaitForSeconds(dissolveTime);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPos, movePerFrame * Time.deltaTime);

                if (Vector3.Distance(transform.position, startingPos) <= .2f)
                {
                    dissolveLava = false;
                    yield return new WaitForSeconds(cooldownTime);
                }
            }

            yield return null;
        }
    }
}
