// PlayerPortraitManager class
// ====================================================================================================================
// Handles the generation of player portraits to use in UI elements


using System.Collections;
using UnityEngine;


namespace kaboomcombat
{
    public class PlayerPortraitManager : MonoBehaviour
    {
        // References
        public GameObject playerPortraitRig;
        private PlayerManager playerManager;

        // Array of sprites to store the generated portraits for each player
        public Sprite[] playerPortraits = new Sprite[4];

        private void Start()
        {
            // Get reference
            playerManager = FindObjectOfType<PlayerManager>();
            // Use a coroutine to start the generation function
            StartCoroutine(GeneratePlayerPortraits());
        }

        public IEnumerator GeneratePlayerPortraits()
        {
            // Instantiate the camera rig that is used to capture the portraits of the playermodels at 0, 0, 0
            // Everything in the playerPortraitRig prefab is set to the "PlayerPortrait" Layer, thus the camera won't
            // see anything but the playermodels to capture a clean image
            GameObject playerPRInstance = Instantiate(playerPortraitRig, new Vector3(0f, 0f, 0f), playerPortraitRig.transform.rotation);

            // Get reference to the playermodel container to be used as a parent when instantiating playermodels
            GameObject playermodelContainer = playerPRInstance.transform.GetChild(0).gameObject;
            // Reference to camera
            Camera camera = playerPRInstance.GetComponentInChildren<Camera>();

            // Create a new render texture
            RenderTexture rt = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            // Set the camera's target render texture to the one we just created
            camera.targetTexture = rt;

            // Destroy the placeholder playermodel
            Destroy(playermodelContainer.transform.GetChild(0).gameObject);

            // Repeat for each player
            for (int i = 0; i < playerPortraits.Length; i++)
            {
                // Instantiate the playermodel with playermodelContainer as it's parent
                GameObject playermodelInstance = Instantiate(playerManager.playerModels[i], playermodelContainer.transform);
                // Make sure the playermodel has the right layer assigned, otherwise it won't show up on the camera
                foreach (Transform child in playermodelInstance.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("PlayerPortrait");
                }

                // Wait until the end of the frame to give the camera time to actually see the playermodel we just assigned
                // This is why this function needs to be a coroutine
                yield return new WaitForEndOfFrame();

                // Convert the rendertexture to a texture2d
                Texture2D tex = ToTexture2D(rt);
                // Create a new sprite using the texture2d
                Sprite portrait = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                // Add our finished portrait to the playerPortraits array
                playerPortraits[i] = portrait;
                // Destroy the playermodel once we're done with the portrait capture
                Destroy(playermodelInstance.gameObject);
            }

            // Once every playermodel has been captured, release the render texture
            rt.Release();

            // Assign the values from here to playerManager
            AssignPlayerPortraits();
            // Destroy the playerPortraitRig once we're done with everything
            Destroy(playerPRInstance.gameObject);
        }


        // Kind of reduntant, we could just set the playerManager portraits directly in the function above
        private void AssignPlayerPortraits()
        {
            for(int i = 0; i < playerPortraits.Length; i++)
            {
                playerManager.playerPortraits[i] = playerPortraits[i];
            }
        }


        // Function to convert rendertexture to Texture2D
        private Texture2D ToTexture2D(RenderTexture rTex)
        {
            // Create a new Texture2D of correct size
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.ARGB32, false);

            // Get reference to the current active rendertexture so we can restore it once we're done
            var old_rt = RenderTexture.active;
            // Set our camera's rendertexture as active
            RenderTexture.active = rTex;
            // Conversion
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            
            // Restore the previous active rendertexture
            RenderTexture.active = old_rt;

            return tex;
        }
    }
}

