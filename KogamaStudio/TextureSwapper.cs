using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MelonLoader;

namespace KogamaStudio
{
    internal class TextureSwapper
    {
        public static void SwapFireTexture(string customTexturePath)
        {
            try
            {
                byte[] imageData = File.ReadAllBytes(customTexturePath);
                Texture2D newTexture = new Texture2D(2, 2);
                newTexture.LoadImage(imageData);

                MelonLogger.Msg("Fire texture loaded!");
                
            } 
            catch (Exception ex)
            {
                MelonLogger.Msg($"Failed: {ex.Message}");
            }

        }
    }
}
