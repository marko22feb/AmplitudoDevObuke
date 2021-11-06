using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    //public Abilities ability;
    private GameObject bulletObjectToSpawn;
    public bool isPlayer = false;

    private void Start()
    {
        bulletObjectToSpawn = Resources.Load("Abilities/Projectiles/Bullet") as GameObject;
        if (gameObject.tag == "Player") isPlayer = true;
    }

    public void OnAbilityUse(Abilities ability)
    {
        switch (ability)
        {
            case Abilities.bullet:
                SpawnBullet(new Vector3(0, transform.rotation.y, 0));
                break;
            case Abilities.multibullet:
                SpawnBullet(new Vector3(0, transform.rotation.y, 0));
                SpawnBullet(new Vector3(0, transform.rotation.y, 35));
                SpawnBullet(new Vector3(0, transform.rotation.y, -35f));
                break;
            case Abilities.bulletRain:
                for (int i = 0; i < 15; i++)
                {
                    float Zrotation = GameController.MapClampRanged((360 / 15) * i, 0,360, -180,180);
                    SpawnBullet(new Vector3(0, transform.rotation.y, Zrotation));
                }
                break;
            case Abilities.bulletRainWave:
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(BulletRainWave(2f * i));
                }
                break;
            case Abilities.bulletRainWaveRandomized:
                for (int a = 0; a < 3; a++)
                {
                    for (int i = 0; i < 30; i++)
                    {
                        float Zrotation = GameController.MapClampRanged((360 / 30) * i, 0, 360, -180, 180);
                        StartCoroutine(BulletRainWaveRandomized((0.1f * i) + a * 30 * 0.1f, new Vector3(0, transform.rotation.y, Zrotation)));
                    }
                }
                break;
            default:
                break;
        }
    }

    IEnumerator BulletRainWave(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnAbilityUse(Abilities.bulletRain);
    }

    IEnumerator BulletRainWaveRandomized(float delay, Vector3 rotation)
    {
        yield return new WaitForSeconds(delay);
        SpawnBullet(rotation);
    }

    void SpawnBullet(Vector3 rotation)
    {
        Quaternion newrotation = Quaternion.Euler(rotation);

        if (!isPlayer) {
            newrotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y - 180, rotation.z));
        }

        GameObject temp = Instantiate(bulletObjectToSpawn);
        temp.GetComponent<DamageComponent>().owner = gameObject;
        Transform Slot = transform.Find("AbilitySlot");

        Vector3 positionToSpawnAt;

        if (Slot != null)
        {
            positionToSpawnAt = Slot.position;
        }
        else
        {
            positionToSpawnAt = transform.position;
        }

        temp.transform.position = positionToSpawnAt;
        temp.transform.rotation = newrotation;
    }
}
