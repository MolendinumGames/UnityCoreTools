/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CoreTools
{
    public class UserSettingsController : Singleton<UserSettingsController>
    {
        protected override bool Persistent => true;

        [Header("Audio")]
        [SerializeField]
        AudioMixer masterMixer;

        [SerializeField]
        Vector2Int minMaxVolumeLevel = new Vector2Int(-80, 0);

        private const string volumeId = "volume";
        private const string vSyncId = "vsync";
        private const string fullscreenId = "fullscreen";
        private const string resolutionId = "resolution";

        protected override void Awake()
        {
            base.Awake();
            LoadSettings();
        }
        public void SaveSettings()
        {
            masterMixer.GetFloat("volume", out float volume);
            PlayerPrefs.SetFloat(volumeId, volume);
            PlayerPrefs.SetInt(vSyncId, QualitySettings.vSyncCount);
            PlayerPrefs.SetString(resolutionId, Screen.currentResolution.ToString());
            PlayerPrefs.SetString(fullscreenId, Screen.fullScreen.ToString());
        }
        public void LoadSettings()
        {
            LoadVolume();

            LoadVSync();

            LoadScreenSettings();
        }
        private void LoadVolume()
        {
            if (PlayerPrefs.HasKey(volumeId))
            {
                float volume = PlayerPrefs.GetFloat(volumeId);
                SetVolume(volume);
            }
        }
        private void LoadVSync()
        {
            if (PlayerPrefs.HasKey(vSyncId))
            {
                int vSync = PlayerPrefs.GetInt(vSyncId);
                SetVSync(vSync);
            }
        }
        private void LoadScreenSettings()
        {
            if (PlayerPrefs.HasKey(resolutionId) && PlayerPrefs.HasKey(fullscreenId))
            {
                bool fullScreen = true;
                if (!bool.TryParse(PlayerPrefs.GetString(fullscreenId), out fullScreen))
                    Debug.LogError($"Could not read fullscreen from Playerprefs. Was {PlayerPrefs.GetString(fullscreenId)}");

                string newResText = PlayerPrefs.GetString(resolutionId);
                string[] splitRes = newResText.Split('x');
                int width, height, refreshRate;
                if (!int.TryParse(splitRes[0], out width))
                {
                    Debug.LogError($"Could not read width resolution from Playerprefs. Was: {newResText}");
                    return;
                }
                string[] secondSplitRes = splitRes[1].Split('@');
                if (!int.TryParse(secondSplitRes[0], out height))
                {
                    Debug.LogError($"Could not read height resolution from Playerprefs. Was: {newResText}");
                    return;
                }
                if (!int.TryParse(secondSplitRes[1], out refreshRate))
                {
                    Debug.LogError($"Could not read Hz from Playerprefs. Was: {newResText}");
                    return;
                }

                SetScreen(width, height, fullScreen, refreshRate);
            }
        }

        #region Public Set Methods
        public void SetVSync(int value)
        {
            int vSync = Mathf.Clamp(value, 0, 4);
            QualitySettings.vSyncCount = vSync;
        }
        public void SetFullscreen(bool state)
        {
            Screen.fullScreen = state;
        }
        public void SetScreenresolution(int width, int height)
        {
            bool fullScreen = Screen.fullScreen;
            Screen.SetResolution(width, height, fullScreen);
        }
        public void SetScreen(int width, int height, bool fullScreen, int refreshRate)
        {
            Screen.SetResolution(width, height, fullScreen, refreshRate);
        }
        public void SetRefreshRate(int rate)
        {
            int width = Screen.width;
            int heigth = Screen.height;
            bool fullScreen = Screen.fullScreen;
            rate = Mathf.Abs(rate);
            Screen.SetResolution(width, heigth, fullScreen, rate);
        }
        public void SetVolume(float level)
        {
            int min = minMaxVolumeLevel.x;
            int max = minMaxVolumeLevel.y;
            level = Mathf.Clamp(level, min, max);
            masterMixer.SetFloat("volume", level);
        }
        #endregion
    }
}