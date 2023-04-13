using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KartGame.KartSystems{
namespace ModScripts
{
    public class DroneScript : MonoBehaviour, IUseItem
    {
        public float moveSpeed = 10f;
        public float rotateSpeed = 100f;
        public float trackingRange = 20f;
        public float explosionRadius = 5f;
        public GameObject explosionEffect;
        private Transform target;

            private void Start()
        {
            // Encontra o jogador e define como alvo do casco azul
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            // Verifica se o alvo est�Edentro do raio de persegui��o
            float distance = Vector3.Distance(transform.position, target.position);
                if (distance <= trackingRange)
                {
                    // Rotaciona o casco azul em dire��o ao alvo
                    Vector3 targetDirection = target.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);

                    // Move o casco azul em dire��o ao alvo
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

                    // Se o casco azul atingir o jogador, explode e causa dano
                }                
        }
            void OnCollisionEnter(Collision other)
            {
                  float distance = Vector3.Distance(transform.position, target.position);
                if (other.gameObject.tag == "Player" && distance <= explosionRadius)
                {
                    Destroy(gameObject);
                    // Causa dano ao jogador aqui
                    other.gameObject.GetComponent<ArcadeKart>().baseStats.Acceleration = 0f;
                    other.gameObject.GetComponent<ArcadeKart>().baseStats.TopSpeed = 0f;
                    // Inicia a coroutine para restaurar a velocidade padr�o depois de 3 segundos
                    StartCoroutine(RestoreSpeed(3f));
                }
            }
            public void UseItem(Transform position, Quaternion rotation)
        {
            Instantiate(gameObject, position.position - (new Vector3(0f, 0f, 0f)), rotation);
        }
            private IEnumerator RestoreSpeed(float delay)
            {
                // Espera por um tempo antes de restaurar a velocidade padr�o
                yield return new WaitForSeconds(delay);

                // Restaura a velocidade padr�o do jogador
                target.GetComponent<ArcadeKart>().baseStats.Acceleration = 10f;
                target.GetComponent<ArcadeKart>().baseStats.TopSpeed = 30f;
            }
        }
}
    }
   

