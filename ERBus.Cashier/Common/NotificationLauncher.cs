using System;
using System.Drawing;
using System.IO;
using ERBus.Cashier.ToastNotifications.FormAnimator;
using ToastNotifications;

namespace ERBus.Cashier.Common
{
    public class NotificationLauncher
    {
        public static void ShowNotification(string titleNotify, string bodyNotify, int Duration, string Animation, string AnimationDirection, string Sound)
        {
            var animationMethod = FormAnimator.AnimationMethod.Slide;
            foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            {
                if (string.Equals(method.ToString(), Animation))
                {
                    animationMethod = method;
                    break;
                }
            }
            var animationDirection = FormAnimator.AnimationDirection.Up;
            foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            {
                if (string.Equals(direction.ToString(), AnimationDirection))
                {
                    animationDirection = direction;
                    break;
                }
            }
            var toastNotification = new Notification(titleNotify, bodyNotify, Duration, animationMethod, animationDirection);
            //PlayNotificationSound(Sound);
            toastNotification.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            toastNotification.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Color col = ColorTranslator.FromHtml("#82FA58");
            toastNotification.BackColor = col;
            toastNotification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            toastNotification.ForeColor = System.Drawing.Color.White;
            toastNotification.Font = new Font(toastNotification.Font, FontStyle.Regular);
            toastNotification.Show();
        }



        public static void ShowNotificationWarning(string titleNotify, string bodyNotify, int Duration, string Animation, string AnimationDirection, string Sound)
        {
            var animationMethod = FormAnimator.AnimationMethod.Slide;
            foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            {
                if (string.Equals(method.ToString(), Animation))
                {
                    animationMethod = method;
                    break;
                }
            }
            var animationDirection = FormAnimator.AnimationDirection.Up;
            foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            {
                if (string.Equals(direction.ToString(), AnimationDirection))
                {
                    animationDirection = direction;
                    break;
                }
            }
            var toastNotification = new Notification(titleNotify, bodyNotify, Duration, animationMethod, animationDirection);
            //PlayNotificationSound(Sound);
            toastNotification.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            toastNotification.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            toastNotification.BackColor = System.Drawing.Color.DarkOrange;
            toastNotification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            toastNotification.ForeColor = System.Drawing.Color.Black;
            toastNotification.Font = new Font(toastNotification.Font, FontStyle.Regular);
            toastNotification.Show();
        }

        public static void ShowNotificationError(string titleNotify, string bodyNotify, int Duration, string Animation, string AnimationDirection, string Sound)
        {
            var animationMethod = FormAnimator.AnimationMethod.Slide;
            foreach (FormAnimator.AnimationMethod method in Enum.GetValues(typeof(FormAnimator.AnimationMethod)))
            {
                if (string.Equals(method.ToString(), Animation))
                {
                    animationMethod = method;
                    break;
                }
            }
            var animationDirection = FormAnimator.AnimationDirection.Up;
            foreach (FormAnimator.AnimationDirection direction in Enum.GetValues(typeof(FormAnimator.AnimationDirection)))
            {
                if (string.Equals(direction.ToString(), AnimationDirection))
                {
                    animationDirection = direction;
                    break;
                }
            }
            var toastNotification = new Notification(titleNotify, bodyNotify, Duration, animationMethod, animationDirection);
            //PlayNotificationSound(Sound);
            toastNotification.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            toastNotification.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            toastNotification.BackColor = System.Drawing.Color.Red;
            toastNotification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            toastNotification.ForeColor = System.Drawing.Color.Black;
            toastNotification.Font = new Font(toastNotification.Font, FontStyle.Regular);
            toastNotification.Show();
        }

        private static void PlayNotificationSound(string sound)
        {
            var soundsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds");
            var soundFile = Path.Combine(soundsFolder, sound + ".wav");
            using (var player = new System.Media.SoundPlayer(soundFile))
            {
                player.Play();
            }
        }
    }
}
