using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    private float _speed = 25;

    void Start()
    {
        gameObject.tag = "Laser";
    }

    void Update()
    {
        PlayerLaser();
    }

    public void PlayerLaser()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
