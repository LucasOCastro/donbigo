namespace DonBigo.UI.ToggleButtons
{
    public class MuteSfxButton : ToggleImageButton
    {
        public override bool On
        {
            get => Settings.MuteSfx;
            set => Settings.MuteSfx = value;
        }
    }
}