namespace DragonMarkdown.DragonConverter
{
    public class ConverterOptions
    {
        public bool FirstImageIsAlignedRight = true;

        public ConverterOptions()
        {
        }

        public ConverterOptions(bool rightAlignFirstImage)
        {
            FirstImageIsAlignedRight = rightAlignFirstImage;
        }
    }
}