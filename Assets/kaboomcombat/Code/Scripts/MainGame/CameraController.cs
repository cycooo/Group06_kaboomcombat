// CameraController class
// ====================================================================================================================
// Handles everything to do with camera movements and zooming in the main game


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kaboomcombat
{
    public class CameraController : MonoBehaviour
    {
        // References
        private SessionManager sessionManager;

        // List of objects camera will follow
        public List<Transform> targets;
        public Camera cam;

        // Parameters
        public Vector3 offset;
        public Vector3 velocity;

        private float smoothTime = 0.7f;
        // Zoom parameters
        private float minZoom = 26f;
        private float maxZoom = 20f;
        private float zoomLimiter = 16f;


        private void Start()
        {
            // Get references
            sessionManager = FindObjectOfType<SessionManager>();
            cam = GetComponentInChildren<Camera>();
        }


        private void LateUpdate()
        {
            // Only move the camera if the game is active
            if(DataManager.gameState == GameState.PLAYING)
            {
                if(sessionManager.playerList.Count == 1) 
                {
                    Vector3 newPosition = sessionManager.playerList[0].transform.position + offset;
                    transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
                }
                else
                {
                    Move();
                    Zoom();
                }
            }
        }


        // Function that smoothly moves the camera to a certain position over a given time
        public void MoveTo(Vector3 targetPos, float moveTime)
        {
            // Add the offset to the position so that the camera's y position doesn't change
            Vector3 newPos = targetPos + offset;
            LeanTween.move(gameObject, newPos, moveTime).setEaseOutSine();
        }


        // Function that smoothly zooms the camera to a certain FOV value over a given time
        public void ZoomTo(float targetZoom, float zoomTime)
        {
            LTDescr zoomTween = LeanTween.value(cam.gameObject, cam.fieldOfView, targetZoom, zoomTime);
            zoomTween.setEaseOutSine();
            zoomTween.setOnUpdate( (float val) => { cam.fieldOfView = val; });
        }


        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 origPos = transform.position;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float z = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition += new Vector3(x, origPos.y, z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = origPos;
        }


        // Function used during normal gameplay to move the camera according to the players' positions
        private void Move()
        {
            // Calculate the center point of all the players
            Vector3 centerPoint = GetCenterPoint();
            // Add the offset so that the camera's y position doesn't change
            Vector3 newPosition = centerPoint + offset;

            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }

        
        // Function used during normal gameplay to zoom the camera according to the distance between players
        private void Zoom()
        {
            // Divide by zoomLimiter to scale the value
            float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        }


        // Function that gets the greatest distance between all players
        private float GetGreatestDistance()
        {
            List<float> distances = new List<float>();

            for (int i = 0; i < sessionManager.playerList.Count; i++)
            {
                foreach(GameObject player in sessionManager.playerList)
                {
                    if(player != sessionManager.playerList[i])
                    {
                        float dist = Vector3.Distance(player.transform.position, sessionManager.playerList[i].transform.position);

                        distances.Add(dist);
                    }
                }
            }

            distances.Sort();

            if(distances.Count > 1)
            {
                return distances[distances.Count - 1];
            }
            else
            {
                return distances[0];
            }
        }


        // Function that calculates the center point of all the players
        private Vector3 GetCenterPoint()
        {
            if (sessionManager.playerList.Count == 1)
            {
                return sessionManager.playerList[0].transform.position;
            }

            var bounds = new Bounds(sessionManager.playerList[0].transform.position, Vector3.zero);

            for (int i = 0; i < sessionManager.playerList.Count; i++)
            {
                bounds.Encapsulate(sessionManager.playerList[i].transform.position);
            }

            // Add some points in the inner corners of the level so the camera doesn't pan too far away from the level
            bounds.Encapsulate(new Vector3(3f, 0f, 7f));
            bounds.Encapsulate(new Vector3(9f, 0f, 7f));
            bounds.Encapsulate(new Vector3(3f, 0f, 3f));
            bounds.Encapsulate(new Vector3(9f, 0f, 3f));

            return bounds.center;
        }
    }
}
