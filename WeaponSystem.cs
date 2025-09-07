using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class weaponsystem : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
    public float bullet_speed;
    public float firing_speed;
    public float max_bullet;
    bool cooldown = false;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && max_bullet > 0 && cooldown == false)
        {
            GameObject bullet_clone = Instantiate(bullet, transform.position, Quaternion.identity);
            bullet_clone.SetActive(true);
            bullet_clone.AddComponent <deletion>();
            Physics.IgnoreCollision(bullet_clone.GetComponent<Collider>(), player.GetComponent<Collider>());
            Rigidbody rb = bullet_clone.GetComponent<Rigidbody>();
            rb.AddForce(player.transform.forward * bullet_speed);
            cooldown = true;
            StartCoroutine(cooldown_timer());
            max_bullet = max_bullet - 1;
        }
        delete_bullet();
        reload();
    }
    IEnumerator cooldown_timer()
    {
        yield return new WaitForSeconds(firing_speed);
        cooldown = false;
    }
    public void delete_bullet()
    {
        timer = timer + 1;
        if (timer == 30)
        {
            timer = 0;
        }
    }
    public void reload()
    {
        if (Input.GetKey("r"))
        {
            max_bullet = 30;
            Console.WriteLine("something");
        }
    }
}
