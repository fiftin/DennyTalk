using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DennyTalk
{
    public static class ImageHelper2
    {

        public const int AvatarHeight = 50;
        public const int AvatarWidth = 50;

        public static Bitmap DefaultAvatar
        {
            get
            {
                return DennyTalk.Properties.Resources.question_c;
            }
        }

        public static int[] AvatarToIntArray(Bitmap avatar)
        {
            int[] avatarPixels = new int[AvatarWidth * AvatarHeight];
            for (int x = 0; x < AvatarWidth; x++)
            {
                for (int y = 0; y < AvatarHeight; y++)
                {
                    Color pixel = avatar.GetPixel(x, y);
                    avatarPixels[AvatarWidth * y + x] = pixel.ToArgb();
                }
            }
            return avatarPixels;
        }

        public static Bitmap IntArrayToAvatar(int[] avatar)
        {
            Bitmap bitmap = new Bitmap(AvatarWidth, AvatarHeight);
            for (int x = 0; x < AvatarWidth; x++)
            {
                for (int y = 0; y < AvatarHeight; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(avatar[AvatarWidth * y + x]));
                }
            }
            return bitmap;
        }
    }
}
