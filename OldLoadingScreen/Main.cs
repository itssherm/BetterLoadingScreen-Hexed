using CoreRuntime.Interfaces;
using CoreRuntime.Manager;
using Il2CppInterop.Runtime;
using System;
using System.Collections;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using HexedBase;
using Object = UnityEngine.Object;

namespace HexedBase
{
    public class OldLoadingScreenMod : HexedCheat
    {
        private GameObject loadScreenPrefab;

        private GameObject loginPrefab;

        private AssetBundle assets;

        public override void OnLoad(string[] args)
        {
            
            Console.WriteLine("Hexed Base Cheat successfully loaded!");
            MonoManager.PatchUpdate(typeof(VRCApplication).GetMethod(nameof(VRCApplication.Update)));
            CoroutineManager.RunCoroutine(WaitForUiManagerInit());
            Console.WriteLine("OldLoadingScreenMod intiallized");
        }

        private IEnumerator WaitForUiManagerInit()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
            {
                yield return null;
            }
            this.OnUiManagerInit();
            yield break;
        }

        public void OnUiManagerInit()
        {

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OldLoadingScreen.loading.assetbundle"))
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);

                assets = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                assets.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }

            loadScreenPrefab = assets.LoadAsset_Internal("Assets/Bundle/LoadingBackground.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
            loadScreenPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            loginPrefab = assets.LoadAsset_Internal("Assets/Bundle/Login.prefab", Il2CppType.Of<GameObject>()).Cast<GameObject>();
            loginPrefab.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            CreateGameObjects();
        }

        public void OnPreferencesSaved()
        {
            Console.WriteLine("Applying Preferences");
            this.loadScreenPrefab = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/LoadingBackground(Clone)").gameObject;
            Transform transform = this.loadScreenPrefab.transform.Find("MenuMusic");
            Transform transform2 = this.loadScreenPrefab.transform.Find("SpaceSound");
            Transform transform3 = this.loadScreenPrefab.transform.Find("SkyCube");
            Transform transform4 = this.loadScreenPrefab.transform.Find("Stars");
            Transform transform5 = this.loadScreenPrefab.transform.Find("Tunnel");
            Transform transform6 = this.loadScreenPrefab.transform.Find("VRCLogo");
            GameObject gameObject = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel").gameObject;
            GameObject gameObject2 = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").gameObject;
            Transform transform7 = this.loadScreenPrefab.transform.Find("meme");
            transform.gameObject.SetActive(true);
            transform2.gameObject.SetActive(true);
            gameObject2.SetActive(false);
            transform5.gameObject.SetActive(true);
            transform6.gameObject.SetActive(true);
            gameObject.SetActive(true);
            bool flag = DateTime.Today.Month == 4 && DateTime.Now.Day == 1;
            if (flag)
            {
                transform6.gameObject.SetActive(false);
                transform7.gameObject.SetActive(true);
            }
        }

        internal static GameObject UserInterface => VRCUiManager.field_Private_Static_VRCUiManager_0?.gameObject; //credits: _1254


        private void CreateGameObjects()
        {
            Console.WriteLine("Finding original GameObjects");
            GameObject gameObject = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup").gameObject;
            GameObject gameObject2 = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingInfoPanel").gameObject;
            GameObject gameObject3 = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").gameObject;
            GameObject gameObject4 = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/_FX_ParticleBubbles").gameObject;
            GameObject gameObject5 = OldLoadingScreenMod.UserInterface.transform.Find("LoadingBackground_TealGradient_Music/_FX_ParticleBubbles").gameObject;
            GameObject gameObject6 = OldLoadingScreenMod.UserInterface.transform.Find("LoadingBackground_TealGradient_Music/").gameObject;
            GameObject gameObject7 = OldLoadingScreenMod.UserInterface.transform.Find("LoadingBackground_TealGradient_Music/LoadingSound").gameObject;
            GameObject gameObject8 = OldLoadingScreenMod.UserInterface.transform.Find("LoadingBackground_TealGradient_Music/SkyCube_Baked").gameObject;
            GameObject gameObject9 = OldLoadingScreenMod.UserInterface.transform.Find("MenuContent/Popups/LoadingPopup/LoadingSound").gameObject;
            Console.WriteLine("Creating new GameObjects");
            this.loadScreenPrefab = this.CreateGameObject(this.loadScreenPrefab, new Vector3(400f, 400f, 400f), OldLoadingScreenMod.UserInterface.gameObject.name + "/MenuContent/Popups/", "LoadingPopup");
            this.loginPrefab = this.CreateGameObject(this.loginPrefab, new Vector3(0.5f, 0.5f, 0.5f), OldLoadingScreenMod.UserInterface.gameObject.name, "LoadingBackground_TealGradient_Music");
            Console.WriteLine("Disabling original GameObjects");
            gameObject3.active = false;
            gameObject4.active = false;
            gameObject9.active = false;
            gameObject7.active = false;
            gameObject8.active = false;
            gameObject5.active = false;
            this.OnPreferencesSaved();
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002524 File Offset: 0x00000724
        private GameObject CreateGameObject(GameObject obj, Vector3 scale, string rootDest, string parent)
        {
            Console.WriteLine("Creating " + obj.name);
            GameObject gameObject = GameObject.Find(rootDest);
            Transform transform = gameObject.transform.Find(parent);
            GameObject gameObject2 = Object.Instantiate<GameObject>(obj, transform, false).Cast<GameObject>();
            gameObject2.transform.parent = transform;
            gameObject2.transform.localScale = scale;
            return gameObject2;
        }
    }
}
