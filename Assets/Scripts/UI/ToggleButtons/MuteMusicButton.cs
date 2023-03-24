namespace DonBigo.UI.ToggleButtons
{
    public class MuteMusicButton : ToggleImageButton
    {
        public override bool On
        {
            get => Settings.MuteMusic;
            set => Settings.MuteMusic = value;
        }
    }
}