namespace DragonMarkdown.DragonConverter
{
    public class ConverterOptions
    {
        public bool FirstImageIsAlignedRight = true;
        public bool ReplaceImageWithAltWithCaption = true;
        

        public ConverterOptions()
        {
        }

        public ConverterOptions(bool rightAlignFirstImage = true, bool replaceImageWithAlt = true)
        {
            FirstImageIsAlignedRight = rightAlignFirstImage;
            ReplaceImageWithAltWithCaption = replaceImageWithAlt;
        }
    }
}