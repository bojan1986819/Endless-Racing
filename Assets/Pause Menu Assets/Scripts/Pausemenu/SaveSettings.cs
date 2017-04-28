using System;
using System.IO;
using UnityEngine;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{
    [System.Serializable]
    public class SaveSettings
    {
        /// <summary>
        /// Change the file name if something else floats your boat
        /// </summary>
        public string fileName = "GameSettings.json";
        public float musicVolume;
        public float effectsVolume;
        public float masterVolume;
        public float shadowDistINI;
        public float renderDistINI;
        public float aaQualINI;
        public float densityINI;
        public float treeMeshAmtINI;
        public float fovINI;
        public float terrainHeightMapLOD;
        public int msaaINI;
        public int vsyncINI;
        public int textureLimit;
        public int curQualityLevel;
        public int lastShadowCascade;
        public int anisoLevel;
        public bool aoBool;
        public bool dofBool;
        public bool useSimpleTerrain;
        public bool fullscreenBool;
        public int resHeight;
        public int resWidth;
        /// <summary>
        /// The string that will be saved.
        /// </summary>
        static string jsonString;
        /// <summary>
        /// Load the same settings
        /// </summary>
        public static object createJSONOBJ(string jsonString)
        {
            return JsonUtility.FromJson<SaveSettings>(jsonString);

        }
        public void LoadGameSettings(String readString)
        {
            try
            {

                SaveSettings read = (SaveSettings)createJSONOBJ(readString);
                QualitySettings.antiAliasing = (int)read.aaQualINI;
                PMANAGEROLD.densityINI = read.densityINI;
                QualitySettings.shadowDistance = read.shadowDistINI;
                PMANAGEROLD.mainCamShared.farClipPlane = read.renderDistINI;
                PMANAGEROLD.treeMeshAmtINI = read.treeMeshAmtINI;
                PMANAGEROLD.mainCamShared.fieldOfView = read.fovINI;
                QualitySettings.antiAliasing = read.msaaINI;
                QualitySettings.vSyncCount = read.vsyncINI;
                PMANAGEROLD.lastTexLimit = read.textureLimit;
                QualitySettings.masterTextureLimit = read.textureLimit;
                AudioListener.volume = read.masterVolume;
                PMANAGEROLD.lastAudioMult = read.effectsVolume;
                PMANAGEROLD.lastMusicMult = read.musicVolume;
                PMANAGEROLD.dofBool = read.dofBool;
                PMANAGEROLD.aoBool = read.aoBool;
                QualitySettings.SetQualityLevel(read.curQualityLevel);
                QualitySettings.shadowCascades = read.lastShadowCascade;
                Screen.SetResolution(read.resWidth, read.resHeight, read.fullscreenBool);
                if (read.anisoLevel == 0)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                }
                else if (read.anisoLevel == 1)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                }
                else if (read.anisoLevel == 2)
                {
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                }
                try
                {
                    if (read.useSimpleTerrain)
                    {
                        PMANAGEROLD.readTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                    }
                    else
                    {
                        PMANAGEROLD.readSimpleTerrain.heightmapMaximumLOD = (int)read.terrainHeightMapLOD;
                    }
                    PMANAGEROLD.readUseSimpleTerrain = read.useSimpleTerrain;
                }
                catch
                {
                    Debug.Log("Cannot read terain heightmap LOD because the terrain was not assigned.");
                }
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Game settings not found in: " + Application.persistentDataPath + "/" + fileName);
            }

        }
        /// <summary>
        /// Get the quality/music settings before saving 
        /// </summary>
        public void SaveGameSettings()
        {
            if (File.Exists(Application.persistentDataPath + "/" + fileName))
            {
                File.Delete(Application.persistentDataPath + "/" + fileName);
            }
            aaQualINI = QualitySettings.antiAliasing;
            densityINI = PMANAGEROLD.densityINI;
            shadowDistINI = PMANAGEROLD.shadowDistINI;
            renderDistINI = PMANAGEROLD.mainCamShared.farClipPlane;
            treeMeshAmtINI = PMANAGEROLD.treeMeshAmtINI;
            fovINI = PMANAGEROLD.mainCamShared.fieldOfView;
            msaaINI = QualitySettings.antiAliasing;
            vsyncINI = PMANAGEROLD.vsyncINI;
            textureLimit = PMANAGEROLD.lastTexLimit;
            masterVolume = PMANAGEROLD.beforeMaster;
            effectsVolume = PMANAGEROLD.lastAudioMult;
            musicVolume = PMANAGEROLD.lastMusicMult;
            aoBool = PMANAGEROLD.aoBool;
            dofBool = PMANAGEROLD.dofBool;
            curQualityLevel = QualitySettings.GetQualityLevel();
            lastShadowCascade = PMANAGEROLD.lastShadowCascade;
            resHeight = Screen.currentResolution.height;
            resWidth = Screen.currentResolution.width;
            fullscreenBool = Screen.fullScreen;
            if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable)
            {
                anisoLevel = 0;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.ForceEnable)
            {
                anisoLevel = 1;
            }
            else if (QualitySettings.anisotropicFiltering == AnisotropicFiltering.Enable)
            {
                anisoLevel = 2;
            }
            try
            {
                if (PMANAGEROLD.readUseSimpleTerrain)
                {
                    terrainHeightMapLOD = PMANAGEROLD.readTerrain.heightmapMaximumLOD;
                }
                else
                {
                    terrainHeightMapLOD = PMANAGEROLD.readSimpleTerrain.heightmapMaximumLOD;
                }
            }
            catch
            {
                Debug.Log("Cannot save terain heightmap LOD because the terrain was not assigned.");
            }
            useSimpleTerrain = PMANAGEROLD.readUseSimpleTerrain;
            jsonString = JsonUtility.ToJson(this);
            Debug.Log(jsonString);
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, jsonString);
        }


    }
}