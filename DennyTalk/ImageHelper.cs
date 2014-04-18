using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DennyTalk
{
    public static class ImageHelper
    {
        public static Bitmap GetUserStatusImage(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Offline:
                    return DennyTalk.Properties.Resources.offline;
                case UserStatus.Online:
                    return DennyTalk.Properties.Resources.comments;
                case UserStatus.Angry:
                    return DennyTalk.Properties.Resources.angry;
                case UserStatus.AtHome:
                    return DennyTalk.Properties.Resources.at_home;
                case UserStatus.AtWork:
                    return DennyTalk.Properties.Resources.at_work;
                case UserStatus.Away:
                    return DennyTalk.Properties.Resources.away;
                case UserStatus.BadMood:
                    return DennyTalk.Properties.Resources.bad_mood;
                case UserStatus.Dns:
                    return DennyTalk.Properties.Resources.dnd;
                case UserStatus.Eating:
                    return DennyTalk.Properties.Resources.eating;
                case UserStatus.FreeForChat:
                    return DennyTalk.Properties.Resources.free_for_chat;
                case UserStatus.NotAvaliable:
                    return DennyTalk.Properties.Resources.not_available;
                case UserStatus.Occupied:
                    return DennyTalk.Properties.Resources.occupied;
                default:
                    return DennyTalk.Properties.Resources.offline;
            }
        }
        /*
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
        */
        public static Bitmap MessageImage
        {
            get
            {
                return DennyTalk.Properties.Resources.email;
            }
        }


        public static Bitmap Tick
        {
            get { return DennyTalk.Properties.Resources.tick; }
        }

        public static Bitmap ContactEditImage
        {
            get
            {
                return DennyTalk.Properties.Resources.pencil;
            }
        }

        public static Bitmap ContactInfoImage
        {
            get
            {
                return DennyTalk.Properties.Resources.user;
            }
        }
        public static Bitmap ContactRemoveImage
        {
            get
            {
                return DennyTalk.Properties.Resources.delete;
            }
        }

        public const int AvatarHeight = 50;
        public const int AvatarWidth = 50;

    }
}
